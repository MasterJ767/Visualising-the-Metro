using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TreePlacer : MonoBehaviour
{
    public GameObject[] treePrefabs;
    
    private void Start()
    {
        InstantiateTrees();
    }
    
    private void InstantiateTrees()
    {
        Vector3[] treeLocations = FileReader.ReadCSVFile(Application.dataPath + "/Ordnance Survey/TreePositions.txt");

        foreach (Vector3 position in treeLocations)
        {
            int index = Random.Range(0, treePrefabs.Length - 1);
            Vector3 offset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            float scale = Random.Range(0.625f, 0.875f);
            
            GameObject treeGameObject = Instantiate(treePrefabs[index], position + offset, Quaternion.identity, transform);
            treeGameObject.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
