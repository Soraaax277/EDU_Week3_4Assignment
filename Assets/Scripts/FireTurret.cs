using UnityEngine;

public class FireTurret : BaseTurret
{
    [Header("Fire Settings")]
    public float coneAngle = 45f;
    public float particleSpeed = 10f;
    public float flamethrowerFireRate = 0.05f;

    protected override void Awake()
    {
        base.Awake();
        fireRate = flamethrowerFireRate;
    }

    protected override void SetupLineRenderer()
    {
        rangeRenderer.startWidth = 0.1f;
        rangeRenderer.endWidth = 0.1f;
        rangeRenderer.loop = false;
    }

    public override void UpdateTurret(Vector3 playerPos, float deltaTime)
    {
        if (!canFire) return;

        Transform target = GetClosestEnemy();
        
        if (target == null) 
        {
            rangeRenderer.positionCount = 0; 
            return;
        }

        Vector3 targetPos = target.position;
        DrawCone(transform.position, transform.forward, range, coneAngle);
        
        TrackTarget(targetPos, deltaTime);

        bool enemyInCone = MathCollision.IsPointInCone(targetPos, transform.position, transform.forward, range, coneAngle);

        if (enemyInCone)
        {
            fireTimer -= deltaTime;
            if (fireTimer <= 0)
            {
                Fire();
                fireTimer = fireRate;
            }
        }
    }

    protected override void Fire()
    {
        if (projectilePrefab == null) return;
        
        GameObject projObj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        projObj.SetActive(true);
        Projectile proj = projObj.GetComponent<Projectile>();
        if (proj != null)
        {
            float randomAngle = Random.Range(-coneAngle * 0.5f, coneAngle * 0.5f);
            Vector3 fireDir = Quaternion.AngleAxis(randomAngle, Vector3.up) * transform.forward;
            proj.velocity = fireDir * particleSpeed;
            GameManager.Instance.RegisterProjectile(proj);
            Debug.Log($"{gameObject.name} FIRED!");
        }
    }
}
