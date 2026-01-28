using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 velocity;
    public float radius = 0.2f;
    public bool isActive = true;

    public void UpdateProjectile(float deltaTime)
    {
        if (!isActive) return;
        transform.position += velocity * deltaTime;

        Enemy[] enemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (var enemy in enemies)
        {
            Vector2 p1 = new Vector2(transform.position.x, transform.position.z);
            Vector2 p2 = new Vector2(enemy.transform.position.x, enemy.transform.position.z);
            float distXZ = (p1 - p2).magnitude;
            
            float distY = Mathf.Abs(transform.position.y - enemy.transform.position.y);

            if (distXZ <= (radius + enemy.radius) && distY <= 1.5f)
            {
                enemy.TakeDamage();
                Destroy(gameObject);
                break;
            }
        }
    }
}
