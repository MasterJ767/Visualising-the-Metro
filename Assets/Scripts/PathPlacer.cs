using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPlacer : MonoBehaviour
{
    public Color pathColour = Color.black;
    public float spacing = 0.5f;
    public float resolution = 1;
    public float[] speeds;

    private void Start()
    {
        if (speeds.Length != FindObjectOfType<PathCreator>().path.NumSegments + 1)
        {
            Debug.LogError("Length of speed array must match number of control points in path");
        }
    }

    private void OnDrawGizmos()
    {
        Vector3[] pathPoints = FindObjectOfType<PathCreator>().path.CalculateIntervals(spacing, resolution);
        foreach (Vector3 pathPoint in pathPoints)
        {
            Gizmos.color = pathColour;
            Gizmos.DrawSphere(pathPoint, 0.1f);
        }
    }
}
