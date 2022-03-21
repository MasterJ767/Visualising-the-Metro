using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path
{
    [SerializeField] 
    private List<Vector3> points;

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
}
