using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Pieces
{
    public int type;
    public int countPiece;

    public bool NoCount()
    {
        return countPiece == 0;
    }
}
