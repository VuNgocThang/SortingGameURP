using ntDev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public EasyButton btn;
    public int index;

    private void Awake()
    {
        btn.OnClick(() =>
        {
            SaveGame.CurrentRoom = index;
            //Show Room with index
            Debug.Log(" SaveGame.CurrentRoom: " + SaveGame.CurrentRoom);
        });
    }
}
