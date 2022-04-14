using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;

public class PathObject : MonoBehaviour
{
    public PathCreator route;
    public PathPlacer placer;
    public bool reverseDirection;

    private int lastControlPointIndex = 0;
    private int currentPathPointIndex = 0;
    private bool waiting = false;

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
        if (!waiting)
        {
            Vector3[] shapePoints = route.path.points.ToArray();
            Vector3[] pathPoints = route.path.CalculateIntervals(placer.spacing, placer.resolution);

            if (reverseDirection)
            {
                Array.Reverse(shapePoints);
                Array.Reverse(pathPoints);
            }

            if (currentPathPointIndex >= pathPoints.Length - 1)
            {
                StartCoroutine(Wait());
            }
            else
            {
                if (currentPathPointIndex < pathPoints.Length - 1)
                {
                    float speed = Mathf.Lerp(placer.speeds[lastControlPointIndex], placer.speeds[lastControlPointIndex + 1], Vector3.Distance(transform.position, shapePoints[lastControlPointIndex]) / Vector3.Distance(shapePoints[lastControlPointIndex], shapePoints[(3 * lastControlPointIndex) + 3]));
                    Vector3 movement = (pathPoints[currentPathPointIndex + 1] - transform.position).normalized;
                    transform.position = Vector3.MoveTowards(transform.position,pathPoints[currentPathPointIndex + 1],speed * Time.deltaTime);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(movement, Vector3.up), speed * Time.deltaTime);
                }

                if (Vector3.Distance(transform.position, pathPoints[currentPathPointIndex + 1]) < 0.1f)
                {
                    transform.position = pathPoints[currentPathPointIndex + 1];
                    currentPathPointIndex++;
                }

                if (Vector3.Distance(transform.position, shapePoints[(3 * lastControlPointIndex) + 3]) < 0.11f)
                {
                    lastControlPointIndex++;
                }
            }
        }
    }

    private IEnumerator Wait()
    {
        waiting = true;
        yield return new WaitForSeconds(5);
        currentPathPointIndex = 0;
        lastControlPointIndex = 0;
        
        if (reverseDirection)
        {
            transform.position = route.path[route.path.NumPoints - 1];
        }
        else
        {
            transform.position = route.path[0];
        }
        waiting = false;
    }
}
