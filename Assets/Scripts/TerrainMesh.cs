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

        int[] indices = new int[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            indices[i] = i;
        }
        
        Mesh mesh = new Mesh();
        mesh.SetVertices(vertices);
        mesh.SetIndices(indices, MeshTopology.Points, 0);

        meshFilter.mesh = mesh;
    }
}
