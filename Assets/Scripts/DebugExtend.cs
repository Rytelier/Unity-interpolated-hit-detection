using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugExtend
{
    /// <summary>
    /// Debug draw spherecast... it's about time! Almost accurate (tip should be rounded, maybe I'll work on it later)
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="radius"></param>
    /// <param name="direction"></param>
    /// <param name="distance"></param>
    /// <param name="color"></param>
    /// <param name="time"></param>
    public static void DrawSphereCast(Vector3 origin, float radius, Vector3 direction, float distance, Color? color = null, float time = 0)
    {
        Color col = color == null ? Color.white : (Color)color;

        Vector3 axis = Vector3.Cross(direction, Vector3.up);
        Vector3 axisR = Vector3.Cross(direction, Vector3.forward);
        Vector3 dirCircle = Quaternion.AngleAxis(90, axis) * direction.normalized;
        Vector3 dirCircleR = Quaternion.AngleAxis(90, axisR) * direction.normalized;
        Vector3 axisLoop = Vector3.Cross(dirCircle, dirCircleR);

        Vector3 tip = origin - direction.normalized * radius;
        Vector3 tip2 = origin + direction.normalized * distance + direction.normalized * radius;

        for (int i = 0; i < 8; i++)
        {
            Vector3 dirCircleLoop = Quaternion.AngleAxis(i * (360 / 8), axisLoop) * dirCircle;
            Vector3 dirCircleLoopPrev = Quaternion.AngleAxis((i - 1) * (360 / 8), axisLoop) * dirCircle;

            Vector3 loopEnd = origin + dirCircleLoop * radius;
            Vector3 loopEndPrev = origin + dirCircleLoopPrev * radius;

            //Rays along cylinder
            Debug.DrawRay(loopEnd, direction.normalized * distance, col, time);

            //Rays connecting cylinder cap
            Vector3 toPrev = MathExtend.RayTarget(loopEnd, loopEndPrev);
            Debug.DrawRay(loopEnd, toPrev, col, time);
            Debug.DrawRay(origin + direction.normalized * distance + dirCircleLoop * radius, toPrev, col, time);

            //Rays to the tip
            Debug.DrawLine(loopEnd, tip, col, time);
            Debug.DrawLine(origin + direction.normalized * distance + dirCircleLoop * radius, tip2, col, time);
        }
    }
}
