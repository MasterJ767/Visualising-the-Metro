using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path
{
    [SerializeField] 
    public List<Vector3> points;

    public Path(Vector3 centre)
    {
        points = new List<Vector3>
        {
            centre + Vector3.left,
            centre + ((Vector3.left + Vector3.up) * 0.5f),
            centre + Vector3.forward + ((Vector3.right + Vector3.down) * 0.5f),
            centre + Vector3.forward + Vector3.right
        };
    }

    public int NumPoints => points.Count;
    
    public int NumSegments => points.Count / 3;

    public Vector3 this[int index] => points[index];

    public int NumPathPoints(float spacing, float resolution)
    {
        return CalculateIntervals(spacing, resolution).Length;
    }

    public void AddSegment(Vector3 anchorPoint)
    {
        points.Add((2 * points[points.Count - 1]) - points[points.Count - 2]);
        points.Add((points[points.Count - 1] + anchorPoint) * 0.5f);
        points.Add(anchorPoint);
    }

    public Vector3[] GetPointsInSegment(int index)
    {
        if (index < 0 || index >= NumSegments)
        {
            throw new IndexOutOfRangeException(index + "is outside the range of the segment array");
        }
        
        return new Vector3[] { points[index * 3], points[(index * 3) + 1], points[(index * 3) + 2], points[(index * 3) + 3] };
    }

    public void MovePoint(int index, Vector3 newPosition)
    {
        Vector3 displacement = newPosition - points[index];
        points[index] = newPosition;

        // If anchor point
        if (index % 3 == 0)
        {
            if (index + 1 < points.Count)
            {
                points[index + 1] += displacement;
            }

            if (index - 1 >= 0)
            {
                points[index - 1] += displacement;
            }
        }
        // If control point
        else
        {
            int anchorIndex = index % 3 == 2 ? index + 1 : index - 1;
            Vector3 anchor = points[anchorIndex];
            int otherControlPointIndex = anchorIndex == index + 1 ? index + 2 : index - 2;

            if (otherControlPointIndex >= 0 && otherControlPointIndex < points.Count)
            {
                float distance = (anchor - points[otherControlPointIndex]).magnitude;
                Vector3 direction = (anchor - newPosition).normalized;
                points[otherControlPointIndex] = anchor + (direction * distance);
            }
        }
    }

    public Vector3[] CalculateIntervals(float spacing, float resolution = 1)
    {
        List<Vector3> evenlySpacedPoints = new List<Vector3>();
        evenlySpacedPoints.Add(points[0]);
        Vector3 previousPoint = points[0];
        float distanceSinceLastPoint = 0;

        for (int segmentIndex = 0; segmentIndex < NumSegments; segmentIndex++)
        {
            Vector3[] p = GetPointsInSegment(segmentIndex);
            float controlNetLength = Vector3.Distance(p[0], p[1]) + Vector3.Distance(p[1], p[2]) + Vector3.Distance(p[2], p[3]);
            float estimatedCurveLength = Vector3.Distance(p[0], p[3]) + controlNetLength / 2f;
            int divisions = Mathf.CeilToInt(estimatedCurveLength * resolution * 10);
            float t = 0;
            while (t <= 1)
            {
                t += 1f/divisions;
                Vector3 pointOnCurve = Bezier.EvaluateCubic(p[0], p[1], p[2], p[3], t);
                distanceSinceLastPoint += Vector3.Distance(previousPoint, pointOnCurve);

                while (distanceSinceLastPoint >= spacing)
                {
                    float overshootDistance = distanceSinceLastPoint - spacing;
                    Vector3 newSpacedPoint = pointOnCurve + (previousPoint - pointOnCurve).normalized * overshootDistance;
                    evenlySpacedPoints.Add(newSpacedPoint);
                    distanceSinceLastPoint = overshootDistance;
                    previousPoint = newSpacedPoint;
                }
                
                previousPoint = pointOnCurve;
            }
        }

        return evenlySpacedPoints.ToArray();
    }
}
