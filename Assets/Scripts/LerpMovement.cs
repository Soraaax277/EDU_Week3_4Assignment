using UnityEngine;

public static class LerpMovement
{
    public static Vector3 QuadraticBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return oneMinusT * oneMinusT * p0 + 2f * oneMinusT * t * p1 + t * t * p2;
    }

    public static Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return oneMinusT * oneMinusT * oneMinusT * p0 + 
               3f * oneMinusT * oneMinusT * t * p1 + 
               3f * oneMinusT * t * t * p2 + 
               t * t * t * p3;
    }

    public static Vector3 GetControlPoint(Vector3 start, Vector3 end)
    {
        Vector3 mid = (start + end) / 2f;
        return mid + Vector3.up * 5f + Vector3.Cross((end - start).normalized, Vector3.up) * 5f;
    }

    public static void GetCubicControlPoints(Vector3 start, Vector3 end, out Vector3 p1, out Vector3 p2)
    {
        Vector3 dir = (end - start).normalized;
        p1 = start + dir * 5f + Vector3.up * 3f;
        p2 = end - dir * 5f + Vector3.up * 3f;
    }
}
