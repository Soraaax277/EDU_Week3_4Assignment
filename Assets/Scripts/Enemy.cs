using UnityEngine;
using System.Collections.Generic;

public enum MovementType { Quadratic, Cubic }

public class Enemy : MonoBehaviour
{
    public MovementType movementType;
    public float speed = 0.2f; 
    public float radius = 0.5f;
    public GameObject coinPrefab;

    private Vector3 p0, p1, p2, p3;
    private float t = 0f;
    private bool isDead = false;

    public void Setup(Vector3 start, Vector3 end, MovementType type, List<Vector3> manualControls = null)
    {
        p0 = start;
        p3 = end;
        movementType = type;

        if (manualControls != null && manualControls.Count > 0)
        {
            if (movementType == MovementType.Quadratic)
            {
                p1 = manualControls[0];
            }
            else 
            {
                p1 = manualControls[0];
                if (manualControls.Count > 1) p2 = manualControls[1];
                else p2 = Vector3.Lerp(p1, p3, 0.5f); 
            }
        }
        else
        {
            if (movementType == MovementType.Quadratic)
            {
                p1 = LerpMovement.GetControlPoint(start, end);
            }
            else
            {
                LerpMovement.GetCubicControlPoints(start, end, out p1, out p2);
            }
        }

        transform.position = start;
    }

    void Update()
    {
        if (isDead) return;

        t += speed * Time.deltaTime;
        
        if (movementType == MovementType.Quadratic)
        {
            transform.position = LerpMovement.QuadraticBezier(p0, p1, p3, t);
        }
        else
        {
            transform.position = LerpMovement.CubicBezier(p0, p1, p2, p3, t);
        }

        if (t >= 1f)
        {
            ReachTarget();
        }
    }

    private void ReachTarget()
    {
        PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(1);
            Debug.Log("Enemy reached target - 1 HP deducted!");
        }
        else
        {
            Debug.LogWarning("No PlayerHealth found in scene to damage!");
        }
        Destroy(gameObject);
    }

    public void TakeDamage()
    {
        if (isDead) return;
        isDead = true;
        
        Debug.Log($"Enemy {gameObject.name} killed!");
        SpawnCoin();
        Destroy(gameObject);
    }

    private void SpawnCoin()
    {
        if (coinPrefab != null)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Enemy killed but CoinPrefab is missing on Enemy script!");
        }
    }
}
