using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


#if UNITY_EDITOR
[CustomEditor(typeof(ToolSpawnQueueData))]
public class ToolSpawnQueueDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ToolSpawnQueueData toolSpawnQueueData = (ToolSpawnQueueData)target;

        if (GUILayout.Button("Save Data"))
        {
            toolSpawnQueueData.SaveData();
        }
    }
}
#endif
