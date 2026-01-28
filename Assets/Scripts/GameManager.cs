using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game References")]
    public PlayerController player;
    public PlayerHealth playerHealth;
    public GameObject failUI;

    [Header("Turret References")]
    public List<BaseTurret> turrets = new List<BaseTurret>();
    
    private List<Projectile> activeTDProjectiles = new List<Projectile>();
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (player == null) player = FindFirstObjectByType<PlayerController>();
    }

    void Start()
    {
        if (playerHealth == null) playerHealth = FindFirstObjectByType<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.OnPlayerDeath -= OnPlayerDeath; 
            playerHealth.OnPlayerDeath += OnPlayerDeath;
        }

        if (failUI == null)
        {
            GameObject found = GameObject.Find("FailPanel") ?? GameObject.Find("FailCanvas") ?? GameObject.Find("GameOverUI");
            if (found != null) failUI = found;
        }
        
        if (turrets == null || turrets.Count == 0)
        {
            turrets = new List<BaseTurret>(FindObjectsByType<BaseTurret>(FindObjectsSortMode.None));
            Debug.Log($"Auto-found {turrets.Count} turrets.");
        }
    }

    private void OnPlayerDeath()
    {
        GameOver(false);
    }

    void Update()
    {
        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            return;
        }

        float deltaTime = Time.deltaTime;
        Enemy[] allEnemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        foreach (var turret in turrets)
        {
            Enemy closest = null;
            float minDist = turret.range;
            foreach (var enemy in allEnemies)
            {
                float dist = (turret.transform.position - enemy.transform.position).magnitude;
                if (dist < minDist) { minDist = dist; closest = enemy; }
            }

            Vector3 targetPos = closest != null ? closest.transform.position : player.transform.position;
            turret.UpdateTurret(targetPos, deltaTime);
        }

        for (int i = activeTDProjectiles.Count - 1; i >= 0; i--)
        {
            var p = activeTDProjectiles[i];
            if (p == null) { activeTDProjectiles.RemoveAt(i); continue; }
            p.UpdateProjectile(deltaTime);
            if (p.transform.position.magnitude > 100f)
            {
                Destroy(p.gameObject);
                activeTDProjectiles.RemoveAt(i);
            }
        }
    }

    public void RegisterProjectile(Projectile proj)
    {
        activeTDProjectiles.Add(proj);
    }

    void GameOver(bool win)
    {
        if (isGameOver) return;
        isGameOver = true;

        if (win)
        {
            Debug.Log("YOU WIN!");
            foreach (var turret in turrets) turret.SetTurretActive(false);
        }
        else
        {
            Debug.Log("GAME OVER");
            if (failUI != null) 
            {
                failUI.SetActive(true);
                Canvas parentCanvas = failUI.GetComponentInParent<Canvas>(true);
                if (parentCanvas != null) parentCanvas.gameObject.SetActive(true);
            }
            Time.timeScale = 0; 
        }
    }
}
