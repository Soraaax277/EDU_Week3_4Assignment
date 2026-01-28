using UnityEngine;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    [Header("References")]
    public GameObject enemyPrefab;
    public Transform targetLocation;
    public Transform spawnQuad;
    public Transform spawnCubic;

    [Header("Custom Path Controls")]
    public List<Transform> quadControls = new List<Transform>();
    public List<Transform> cubicControls = new List<Transform>();

    [Header("Settings")]
    public int enemiesPerSpawn = 5;
    public float spawnInterval = 1.5f;

    private float timer;
    private int spawnedCount = 0;
    private bool waveFinished = false;

    void Update()
    {
        if (waveFinished) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval && spawnedCount < enemiesPerSpawn)
        {
            timer = 0;
            SpawnBatch();
            spawnedCount++;
        }

        if (spawnedCount >= enemiesPerSpawn)
        {
            waveFinished = true;
        }
    }

    void SpawnBatch()
    {
        if (spawnQuad != null)
        {
            SpawnEnemy(spawnQuad.position, MovementType.Quadratic, quadControls);
        }
        if (spawnCubic != null)
        {
            SpawnEnemy(spawnCubic.position, MovementType.Cubic, cubicControls);
        }
    }

    void SpawnEnemy(Vector3 pos, MovementType type, List<Transform> controls)
    {
        if (enemyPrefab == null || targetLocation == null) return;

        GameObject go = Instantiate(enemyPrefab, pos, Quaternion.identity);
        Enemy enemy = go.GetComponent<Enemy>();
        if (enemy != null)
        {
            List<Vector3> controlPositions = new List<Vector3>();
            foreach (var t in controls) if (t != null) controlPositions.Add(t.position);
            
            enemy.Setup(pos, targetLocation.position, type, controlPositions);
        }
    }

    void OnDrawGizmos()
    {
        if (targetLocation == null) return;

        if (spawnQuad != null)
        {
            Gizmos.color = Color.yellow;
            DrawPath(spawnQuad.position, targetLocation.position, MovementType.Quadratic, quadControls);
        }

        if (spawnCubic != null)
        {
            Gizmos.color = Color.cyan;
            DrawPath(spawnCubic.position, targetLocation.position, MovementType.Cubic, cubicControls);
        }
    }

    void DrawPath(Vector3 start, Vector3 end, MovementType type, List<Transform> controls)
    {
        Vector3 prevPoint = start;
        int segments = 20;

        Vector3 q1 = Vector3.zero, c1 = Vector3.zero, c2 = Vector3.zero;
        
        List<Vector3> manual = new List<Vector3>();
        foreach (var t in controls) if (t != null) manual.Add(t.position);

        if (manual.Count > 0)
        {
            if (type == MovementType.Quadratic) 
            {
                q1 = manual[0];
            }
            else 
            {
                c1 = manual[0];
                c2 = (manual.Count > 1) ? manual[1] : Vector3.Lerp(c1, end, 0.5f);
            }
        }
        else
        {
            if (type == MovementType.Quadratic) q1 = LerpMovement.GetControlPoint(start, end);
            else LerpMovement.GetCubicControlPoints(start, end, out c1, out c2);
        }

        for (int i = 1; i <= segments; i++)
        {
            float t = i / (float)segments;
            Vector3 point = (type == MovementType.Quadratic) 
                ? LerpMovement.QuadraticBezier(start, q1, end, t) 
                : LerpMovement.CubicBezier(start, c1, c2, end, t);
            
            Gizmos.DrawLine(prevPoint, point);
            prevPoint = point;
        }
    }
}
