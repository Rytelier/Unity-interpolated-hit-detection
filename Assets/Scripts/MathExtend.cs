using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathExtend
{
    /// <summary>
    /// Turn segment into a curve.
    /// 0 - point previous, 1 - point middle, 2 - point next.
    /// Goes from middle to previous, using next as tangent.
    /// </summary>
    /// <param name="points"></param>
    /// <param name="subdivs"></param>
    /// <returns></returns>
    public static Vector3[] SegmentInterpolate(Vector3[] points, int subdivs = 2)
    {
        Vector3[] subPoints = new Vector3[subdivs + 2];
        Vector3 previousPoint = points[1];
        Vector3 targetFinal = RayTargetInv(points[1], points[0], points[2]);
        for (int s = 0; s < subPoints.Length; s++)
        {
            float inter = (float)s / (float)subPoints.Length;
            float interp = (float)s / (float)(subPoints.Length - 1);
            subPoints[s] = Vector3.Lerp(previousPoint, previousPoint + targetFinal, inter);
            subPoints[s] = Vector3.Lerp(subPoints[s], points[2], interp);
        }

        return subPoints;
    }

    /// <summary>
    /// Raycast direction from origin to target
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="target"></param>
    /// <param name="mult"></param>
    /// <returns></returns>
    public static Vector3 RayTarget(Vector3 origin, Vector3 target, float mult = 1)
    {
        return (target - origin).normalized * (Vector3.Distance(origin, target) * mult);
    }

    public static Vector3 RayTargetInv(Vector3 origin, Vector3 target, Vector3 lTarget, float mult = 1)
    {
        return (origin - target).normalized * (Vector3.Distance(origin, lTarget) * mult);
    }
}