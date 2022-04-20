using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PathMesh : MonoBehaviour
{
    public PathCreator route;
    public PathPlacer placer;
    private float spacing;
    private float resolution;
    private float width;

    private void Start()
    {
        UpdateRoad();
    }

    public void Update()
    {
        if (spacing != placer.spacing || resolution != placer.resolution || width != placer.roadWidth)
        {
            UpdateRoad();
        }
    }

    private void UpdateRoad()
    {
        spacing = placer.spacing;
        resolution = placer.resolution;
        width = placer.roadWidth;
        
        Path path = route.path;
        Vector3[] pathPoints = path.CalculateIntervals(placer.spacing, placer.resolution);
        GetComponent<MeshFilter>().mesh = CreateRoadMesh(pathPoints);
    }

    private Mesh CreateRoadMesh(Vector3[] points)
    {
        Vector3[] vertices = new Vector3[points.Length * 2];
        int[] triangles = new int[2 * (points.Length - 1) * 3];
        int vertexIndex = 0;
        int triangleIndex = 0;

        for (int i = 0; i < points.Length; i++)
        {
            Vector3 forward = Vector3.zero;
            
            if (i < points.Length - 1)
            {
                forward += points[i + 1] - points[i];
            }

            if (i > 0)
            {
                forward += points[i] - points[i - 1];
            }

            forward.Normalize();

            Vector3 left = new Vector3(-forward.z, forward.y, forward.x);

            vertices[vertexIndex] = points[i] + (left * placer.roadWidth * 0.5f);
            vertices[vertexIndex + 1] = points[i] - (left * placer.roadWidth * 0.5f);

            if (i < points.Length - 1)
            {
                triangles[triangleIndex] = vertexIndex;
                triangles[triangleIndex + 1] = vertexIndex + 2;
                triangles[triangleIndex + 2] = vertexIndex + 1;
                triangles[triangleIndex + 3] = vertexIndex + 1;
                triangles[triangleIndex + 4] = vertexIndex + 2;
                triangles[triangleIndex + 5] = vertexIndex + 3;
            }
            
            vertexIndex += 2;
            triangleIndex += 6;
        }

        return new Mesh{ vertices = vertices, triangles = triangles};
    }
}
