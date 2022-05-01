using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FileReader
{
    public static Vector3[] ReadXYZFile(string filename)
    {
        string contents = File.ReadAllText(filename);

        string[] entries = contents.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

        List<Vector3> outputs = new List<Vector3>();

        foreach (string entry in entries)
        {
            string[] coordinates = entry.Split(',', StringSplitOptions.None);
            outputs.Add(new Vector3(float.Parse(coordinates[0]) - Config.xOffset, float.Parse(coordinates[2]), float.Parse(coordinates[1]) - Config.zOffset));
            Debug.Log((float.Parse(coordinates[0]) - Config.xOffset) + ", " + float.Parse(coordinates[2]) + ", " + (float.Parse(coordinates[1]) - Config.zOffset));
        }

        return outputs.ToArray();
    }

    public static Vector3[] ReadCSVFile(string filename)
    {
        string contents = File.ReadAllText(filename);
        Debug.Log(contents);
        
        string[] entries = contents.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

        List<Vector3> outputs = new List<Vector3>();
        
        foreach (string entry in entries)
        {
            Debug.Log(entry);
            string[] coordinates = entry.Split(',', StringSplitOptions.None);
            outputs.Add(new Vector3(float.Parse(coordinates[2]), 74.05f - float.Parse(coordinates[1]), 2000 - float.Parse(coordinates[0])));
        }

        return outputs.ToArray();
    }
}
