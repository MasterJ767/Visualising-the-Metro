using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPlacer : MonoBehaviour
{
    public Color pathColour = Color.black;
    public float spacing = 0.1f;
    public float resolution = 1;

    private void OnDrawGizmos()
    {
        Vector3[] pathPoints = FindObjectOfType<PathCreator>().path.CalculateIntervals(spacing, resolution);
        foreach (Vector3 pathPoint in pathPoints)
        {
            Gizmos.color = pathColour;
            Gizmos.DrawSphere(pathPoint, 0.2f);
        }
    }
}
