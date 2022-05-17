using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    public float radius = 0.25f; //Radius for sphere cast
    public List<Transform> hitPoints = new List<Transform>(); //Points used for hit detection
    public LayerMask hitLayers; //Layers for hittable objects
    public float hitDetectionSegment = 1.2f; //Max lengths of the interpolated hit detection segment

    Vector3[] hitPointsFirst;
    Vector3[] hitPoints0;
    Vector3[] hitPoints1;
    Vector3[] hitPoints2;
    bool hitpointsEnabled;
    int frameCurrent;

    List<GameObject> hitObjects = new List<GameObject>();

    public void Init()
    {
        hitPoints0 = new Vector3[hitPoints.Count];
        hitPoints1 = new Vector3[hitPoints.Count];
        hitPoints2 = new Vector3[hitPoints.Count];
    }

    //Called in animation event
    public void OpenHitbox()
    {
        frameCurrent = 0;

        hitPoints0 = hitPoints.Select(x => x.position).ToArray();
        hitPoints1 = hitPoints.Select(x => x.position).ToArray();
        hitPoints2 = hitPoints.Select(x => x.position).ToArray();

        hitpointsEnabled = true;
    }

    //Called in animation event
    public void CloseHitbox()
    {
        frameCurrent = 0;
        hitObjects.Clear();

        hitpointsEnabled = false;
    }

    //Hit detection code
    private void FixedUpdate()
    {
        if (!hitpointsEnabled) return;

        hitPoints0 = hitPoints1;
        hitPoints1 = hitPoints2;
        hitPoints2 = hitPoints.Select(x => x.position).ToArray();

        Color colRayOrig = new Color(1, 0, 0, 0.3f);
        Color colRayOrig2 = new Color(1, 1, 0, 0.3f);
        Color colSphereCastFirst = new Color(0.5f, 1, 0, 0.5f);
        Color colSphereCast = new Color(0, 1, 0, 0.5f);

        frameCurrent++;
        for (int i = 0; i < hitPoints2.Length; i++)
        {
            if (frameCurrent == 1)
            {
                hitPointsFirst = hitPoints0;
                return;
            }
            float dist = Vector3.Distance(hitPoints1[i], hitPoints2[i]);
            int subs = Mathf.FloorToInt(dist / hitDetectionSegment);
            Debug.DrawRay(hitPoints2[i], MathExtend.RayTarget(hitPoints2[i], hitPoints1[i]), colRayOrig, 3);
            Debug.DrawRay(hitPoints1[i], MathExtend.RayTarget(hitPoints1[i], hitPoints0[i]), colRayOrig2, 3);

            Vector3[] interpPoints = MathExtend.SegmentInterpolate(new Vector3[] { hitPoints0[i], hitPoints1[i], hitPoints2[i] }, subs);


            if (frameCurrent == 2)
            {
                int subsp = Mathf.FloorToInt(Vector3.Distance(hitPointsFirst[i], hitPoints1[i]) / hitDetectionSegment);
                Vector3[] interpPointsPrev = MathExtend.SegmentInterpolate(new Vector3[] { hitPoints2[i], hitPoints1[i], hitPointsFirst[i] }, subsp);
                interpPoints = interpPoints.Reverse().ToArray();
                interpPoints = interpPoints.Concat(interpPointsPrev).ToArray();
            }

            for (int t = 0; t < interpPoints.Length - 1; t++)
            {
                Vector3 direction = (interpPoints[t + 1] - interpPoints[t]).normalized;
                float distance = Vector3.Distance(interpPoints[t + 1], interpPoints[t]);
                DebugExtend.DrawSphereCast(interpPoints[t], radius, direction, distance,
                                           frameCurrent == 2 ? colSphereCastFirst : colSphereCast, 3);

                RaycastHit[] hits = new RaycastHit[10];
                int hitsAmount = Physics.SphereCastNonAlloc(interpPoints[t], radius, direction, hits, distance, ~(1 << hitLayers));

                //Main hit detection function, replate with your methods
                for (int h = 0; h < hitsAmount; h++)
                {
                    DetectHit(hits[h].collider);
                }
            }
        }
    }

    //Do stuff on hit object
    void DetectHit(Collider other)
    {
        if (hitObjects.Contains(other.gameObject)) return;

        other.GetComponent<Renderer>().material.SetColor("_Color", new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
        hitObjects.Add(other.gameObject);
    }
}
