using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[Serializable]
//public class ColorPlateInTable
//{
//    public int typeColorPLate;
//    public List<CurrentEnum> listEnum;
//}

[Serializable]
public class SaveCurrentDataGame
{
    public List<ColorPlateInTable> ListColorPLate = new List<ColorPlateInTable>();
    public int currentPoint;
    public float timePlayed;
    public int currentCombo;
}
