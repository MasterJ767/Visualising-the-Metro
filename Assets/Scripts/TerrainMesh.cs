using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class TerrainMesh : MonoBehaviour
{
    private MeshFilter meshFilter;

    private void Start()
    {
        meshFilter = gameObject.GetComponent<MeshFilter>();

        CreateMesh();
    }

    private void CreateMesh()
    {
        Vector3[] verticesNW = FileReader.ReadXYZFile(Application.dataPath + "/Ordnance Survey/terrain-5-dtm-NZ26NW.txt");
        Vector3[] verticesNE = FileReader.ReadXYZFile(Application.dataPath + "/Ordnance Survey/terrain-5-dtm-NZ26NE.txt");
        Vector3[] vertices = new Vector3[verticesNW.Length + verticesNE.Length];
        verticesNW.CopyTo(vertices, 0);
        verticesNE.CopyTo(vertices, verticesNW.Length);
        Debug.Log(vertices.Length);
        List<Vector3> verticesFiltered = new List<Vector3>();

        float x_centre = 5000f;
        float z_centre = 3000f;
        float scale = 1000f;

        float min_x = x_centre - scale - 1;
        float max_x = x_centre + scale + 1;
        
        float min_z = z_centre - scale - 1;
        float max_z = z_centre + scale + 1;

        foreach (Vector3 vertex in vertices)
        {
            if (min_x < vertex.x && vertex.x < max_x && min_z < vertex.z && vertex.z < max_z)
            {
                verticesFiltered.Add(vertex);
            }
        }
        
        Vector3[] vertexArray = verticesFiltered.ToArray();
        
        Debug.Log(vertexArray.Length);

        int[] indices = new int[vertexArray.Length];
        for (int i = 0; i < vertexArray.Length; i++)
        {
            indices[i] = i;
        }
        
        Mesh mesh = new Mesh();
        mesh.SetVertices(vertexArray);
        mesh.SetIndices(indices, MeshTopology.Points, 0);

        meshFilter.mesh = mesh;
    }
}
