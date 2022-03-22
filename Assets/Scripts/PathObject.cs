using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathObject : MonoBehaviour
{
    public PathCreator route;
    public PathPlacer placer;
    public bool reverseDirection;

    private int lastControlPointIndex = 0;
    private int currentPathPointIndex = 0;

    private void Start()
    {
        if (reverseDirection)
        {
            transform.position = route.path[route.path.NumPoints - 1];
        }
        else
        {
            transform.position = route.path[0];
        }
    }

    private void Update()
    {
        Vector3[] shapePoints = route.path.points.ToArray();
        Vector3[] pathPoints = route.path.CalculateIntervals(placer.spacing, placer.resolution);

        if (reverseDirection)
        {
            Array.Reverse(shapePoints);
            Array.Reverse(pathPoints);
        }

        float speed = 0;

        if (currentPathPointIndex < pathPoints.Length - 1)
        {
            speed = Mathf.Lerp(placer.speeds[lastControlPointIndex], placer.speeds[lastControlPointIndex + 1], Vector3.Distance(transform.position, shapePoints[lastControlPointIndex]) / Vector3.Distance(shapePoints[lastControlPointIndex], shapePoints[lastControlPointIndex + 1]));
            transform.position = Vector3.MoveTowards(transform.position, pathPoints[currentPathPointIndex + 1], speed * 3.6f * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.RotateTowards(transform.forward, pathPoints[currentPathPointIndex + 1] - transform.forward, Mathf.PI, speed * 3.6f * Time.deltaTime));
        }

        if (transform.position == pathPoints[currentPathPointIndex + 1])
        {
            currentPathPointIndex++;
        }

        if (Vector3.Distance(transform.position, shapePoints[lastControlPointIndex + 3]) < 0.11f)
        {
            lastControlPointIndex++;
        }
    }
}
