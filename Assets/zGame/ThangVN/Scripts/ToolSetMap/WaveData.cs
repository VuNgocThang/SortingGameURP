using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveData
{
    public int indexLevel;

    public int indexWave;

    public List<PlateData> listPlateData = new List<PlateData>()
    {
        new PlateData(),
        new PlateData(),
        new PlateData(),
    };
}
