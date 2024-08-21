using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlateData
{
    public List<Pieces> listPieces = new List<Pieces>()
    {
        new Pieces()
    };

    public bool NoPieces()
    {
        return listPieces.Count == 0;
    }
}
