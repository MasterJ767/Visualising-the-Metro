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
    public float speed;
    public float startDelay;

    float[] speeds;
    private int lastControlPointIndex = 0;
    private int speedIndex = 0;
    private int currentPathPointIndex = 0;
    private bool waiting = true;
    private Quaternion defaultRot;

    private void Start()
    {
        speeds = placer.speeds;
        
        if (reverseDirection)
        {
            Array.Reverse(speeds);
            transform.position = route.path[route.path.NumPoints - 1];
        }
        else
        {
            transform.position = route.path[0];
        }

        defaultRot = transform.rotation;

        StartCoroutine(Wait());
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
                //Reset();
            }
            else
            {
                Vector3 position = transform.position;
                speed = Mathf.Lerp(speeds[speedIndex], speeds[speedIndex + 1], Vector3.Distance(position, shapePoints[lastControlPointIndex]) / Vector3.Distance(shapePoints[lastControlPointIndex], shapePoints[lastControlPointIndex + 3]));
                Vector3 movement = (pathPoints[currentPathPointIndex + 1] - position).normalized;
                transform.position = Vector3.MoveTowards(position,pathPoints[currentPathPointIndex + 1],speed * Time.deltaTime);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(movement, Vector3.up), speed * Time.deltaTime);

                if (Vector3.Distance(transform.position, pathPoints[currentPathPointIndex + 1]) < 0.26f)
                {
                    transform.position = pathPoints[currentPathPointIndex + 1];
                    currentPathPointIndex++;
                }

                if (Vector3.Distance(transform.position, shapePoints[lastControlPointIndex + 3]) < 0.26f)
                {
                    lastControlPointIndex += 3;
                    speedIndex++;
                }
            }
        }
    }

    private void Reset()
    {
        currentPathPointIndex = 0;
        lastControlPointIndex = 0;
        speedIndex = 0;

        transform.rotation = defaultRot;
        
        if (reverseDirection)
        {
            transform.position = route.path[route.path.NumPoints - 1];
        }
        else
        {
            transform.position = route.path[0];
        }

        waiting = true;
        
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(startDelay);
        waiting = false;
    }
}
