using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathCreator))]
public class PathEditor : Editor
{
    private PathCreator creator;
    private Path path;

    private void OnSceneGUI()
    {
        Input();
        Draw();
    }

    private void OnEnable()
    {
        creator = (PathCreator)target;
        if (creator.path == null)
        {
            creator.CreatePath();
        }
        path = creator.path;
    }

    private void Input()
    {
        Event guiEvent = Event.current;
        Vector3 mousePosition = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
        {
            Undo.RecordObject(creator, "Add Segment");
            path.AddSegment(mousePosition);
        }
    }

    private void Draw()
    {
        for (int i = 0; i < path.NumSegments; i++)
        {
            Vector3[] points = path.GetPointsInSegment(i);
            Handles.color = Color.black;
            Handles.DrawLine(points[1], points[0]);
            Handles.DrawLine(points[2], points[3]);
            Handles.DrawBezier(points[0], points[3], points[1], points[2], creator.segmentColour, null, 3);
        }

        for (int i = 0; i < path.NumPoints; i++)
        {
            if (i % 3 == 0)
            {
                Handles.color = creator.anchorColour;
            }
            else
            {
                Handles.color = creator.controlColour;
            }
            
            Vector3 newPosition = Handles.FreeMoveHandle(path[i], Quaternion.identity, 1.5f, Vector3.zero, Handles.SphereHandleCap);
            
            Event guiEvent = Event.current;
            if (guiEvent.control)
            {
                newPosition.y = path[i].y;
            }

            if (path[i] != newPosition)
            {
                Undo.RecordObject(creator, "Move Point");
                path.MovePoint(i, newPosition);
            }
        }
    }
}
