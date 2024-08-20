using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ToolSpawnQueueData : MonoBehaviour
{

    public int level;
    [HideInInspector] public QueueData queueData;

    public List<WaveData> listWaveData = new List<WaveData>()
    {
        new WaveData()
    };

    public void SaveData()
    {
        Debug.Log("Save Data");
        queueData.level = level;
        queueData.data = listWaveData;

        string data = JsonUtility.ToJson(queueData);
        File.WriteAllText($"Assets/Resources/QueueData/Queue_{level}.json", data);
    }
}
