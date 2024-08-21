using ntDev;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utilities.Common;
using System.IO;

public class LogicUISetMap : MonoBehaviour
{
    [SerializeField] int currentLevel, currentWave;
    [SerializeField] TextMeshProUGUI txtWave, txtLevel;
    [SerializeField] EasyButton btnNextWave, btnPrevWave, btnNextLevel, btnPrevLevel;
    [SerializeField] EasyButton btnSaveWave, btnSaveLevel;
    [SerializeField] List<InputFieldCount> listPlate;

    [HideInInspector] public QueueData queueData;
    public List<WaveData> listWaveData = new List<WaveData>();

    private void Awake()
    {
        btnNextWave.OnClick(() =>
        {
            ShowNextWave();
        });

        btnPrevWave.OnClick(() =>
        {
            ShowPrevWave();
        });

        btnNextLevel.OnClick(() =>
        {
            ShowNextLevel();
        });

        btnPrevLevel.OnClick(() =>
        {
            ShowPrevLevel();
        });

        btnSaveWave.OnClick(() =>
        {
            SaveWave();
        });

        btnSaveLevel.OnClick(() =>
        {
            SaveLevel();
        });

    }

    private void Start()
    {
        UpdateText();
        LoadQueueData(currentLevel);
        LoadWave();
    }

    void LoadQueueData(int level)
    {
        string filePath = $"QueueData/Queue_{level}";
        TextAsset textAsset = Resources.Load<TextAsset>(filePath);

        Debug.Log(filePath);
        if (textAsset != null)
        {
            Debug.Log(textAsset);
            queueData = JsonUtility.FromJson<QueueData>(textAsset.ToString());
            if (listWaveData.Count == 0)
            {
                listWaveData = queueData.data;
            }
            else
            {
                Debug.Log("ClearData");
                listWaveData.Clear();

                Debug.Log("CreateData");
                listWaveData = queueData.data;
            }
        }
        else
        {
            Debug.Log("empty data");
            return;
        }
    }

    void ShowNextWave()
    {
        currentWave++;
        UpdateText();

        LoadWave();
    }

    void ShowPrevWave()
    {
        if (currentWave > 0)
            currentWave--;
        UpdateText();

        LoadWave();
    }

    void ShowNextLevel()
    {
        currentLevel++;
        UpdateText();
        LoadLevel(currentLevel);
    }

    void ShowPrevLevel()
    {
        if (currentLevel > 0)
            currentLevel--;

        UpdateText();
        LoadLevel(currentLevel);
    }

    void UpdateText()
    {
        txtLevel.text = currentLevel.ToString();
        txtWave.text = currentWave.ToString();
    }

    void SaveWave()
    {
        WaveData waveData = new WaveData();
        waveData.listPlateData.Clear();
        waveData.indexWave = currentWave;
        //mỗi wave có 3 plates
        for (int i = 0; i < listPlate.Count; i++)
        {
            PlateData plate = new PlateData();
            plate.listPieces.Clear();
            // mỗi plate có n pieces
            // mỗi pieces có n count
            for (int j = 0; j < listPlate[i].listPieces.Count; j++)
            {
                Pieces piece = new Pieces();
                piece.type = listPlate[i].listPieces[j].indexType;

                if (string.IsNullOrEmpty(listPlate[i].listPieces[j].nCount.text))
                    piece.countPiece = 0;
                else
                    piece.countPiece = int.Parse(listPlate[i].listPieces[j].nCount.text);

                if (piece.NoCount())
                {
                    EasyUI.Toast.Toast.Show("Fill quantity Pieces", 0.3f);
                }
                else
                {
                    plate.listPieces.Add(piece);
                }

            }
            if (plate.NoPieces())
                EasyUI.Toast.Toast.Show("Fill All Plates Empty", 0.3f);
            else
                waveData.listPlateData.Add(plate);
        }

        for (int i = 0; i < listWaveData.Count; i++)
        {
            if (listWaveData[i].indexWave == currentWave)
            {
                listWaveData.RemoveAt(i);
                break;
            }
        }

        if (waveData.listPlateData.Count == 3)
        {
            EasyUI.Toast.Toast.Show($"SAVE WAVE{currentWave} COMPLETED", 0.3f);
            listWaveData.Add(waveData);
        }
    }

    void LoadWave()
    {
        // Check indexWave
        if (currentLevel == queueData.level)
        {
            for (int i = 0; i < listWaveData.Count; i++)
            {
                if (listWaveData[i].indexWave == currentWave)
                {
                    for (int j = 0; j < listPlate.Count; j++)
                    {
                        listPlate[j].ResetData();
                    }
                }
                else
                {
                    for (int j = 0; j < listPlate.Count; j++)
                    {
                        listPlate[j].ResetData();
                    }
                }
            }

            LoadWaveCoroutine();
        }
    }
    void LoadWaveCoroutine()
    {
        for (int i = 0; i < listWaveData.Count; i++)
        {
            if (listWaveData[i].indexWave == currentWave)
            {
                Debug.Log("Load Wave");
                for (int j = 0; j < listWaveData[i].listPlateData.Count; j++)
                {
                    listPlate[j].LoadText(listWaveData[i].listPlateData[j].listPieces.Count);

                    for (int k = 0; k < listWaveData[i].listPlateData[j].listPieces.Count; k++)
                    {
                        int type = listWaveData[i].listPlateData[j].listPieces[k].type;
                        int count = listWaveData[i].listPlateData[j].listPieces[k].countPiece;

                        listPlate[j].LoadExistedPlate(type);
                        listPlate[j].listPieces[k].LoadText(count);
                    }
                }
            }
        }
    }

    void SaveLevel()
    {
        Debug.Log("Save Json");

        queueData.level = currentLevel;
        Debug.Log(listWaveData.Count + " sddadasd");
        queueData.data = listWaveData;
        Debug.Log(queueData.data.Count + " sdadadasdasdsad");

        string data = JsonUtility.ToJson(queueData);
        File.WriteAllText($"Assets/Resources/QueueData/Queue_{currentLevel}.json", data);
    }

    void LoadLevel(int index)
    {
        LoadQueueData(index);
        LoadWave();
    }
}
