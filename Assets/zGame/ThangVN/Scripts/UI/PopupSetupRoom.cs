using ntDev;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupSetupRoom : Popup
{
    [SerializeField] EasyButton btnTick;
    [SerializeField] int currentSprite;
    [SerializeField] int idObjectToAdd;
    [SerializeField] ListRoomData listRoomData;
    [SerializeField] ButtonSelectRoom btnSelectRoom1, btnSelectRoom2, btnSelectRoom3, btnSelectRoom4;
    public bool HasObjectRoom(int idObjectRoom)
    {
        for (int i = 0; i < SaveGame.ListRoomPainted.listRoomPainted.Count; i++)
        {
            for (int j = 0; j < SaveGame.ListRoomPainted.listRoomPainted[i].listObjectPainted.Count; j++)
            {
                if (idObjectRoom == SaveGame.ListRoomPainted.listRoomPainted[i].listObjectPainted[j].idObject)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void Awake()
    {
        ManagerEvent.RegEvent(EventCMD.EVENT_SELECT_ROOM, SelectRoomIndex);

        btnTick.OnClick(() =>
        {

            if (!HasObjectRoom(idObjectToAdd))
            {
                //Todo Chua co room thi ad data room moi vao
                AddNewRoom();
            }
            else
            {
                //todo lam gi khi da co room
                UpdateExistedRoom();
            }
        });
    }
    public static async void Show(int idRoom, int idObj)
    {
        Debug.Log("SHow");
        PopupSetupRoom pop = await ManagerPopup.ShowPopup<PopupSetupRoom>();
        pop.Initialized(idRoom, idObj);
    }

    public override void Hide()
    {
        base.Hide();
        SaveGame.IsShow = false;
    }

    public void Initialized(int idRoom, int idObj)
    {
        idObjectToAdd = idObj;
        ManagerPopup.Instance.nShadow.GetComponent<Image>().enabled = false;
        btnSelectRoom1.Init(idRoom, idObj, 0);
        btnSelectRoom2.Init(idRoom, idObj, 1);
        btnSelectRoom3.Init(idRoom, idObj, 2);
        btnSelectRoom4.Init(idRoom, idObj, 3);
        SaveGame.IsShow = true;
        //LoadExistedData();
        base.Init();
    }

    public void SelectRoomIndex(object e)
    {
        currentSprite = (int)e;
        Debug.Log(currentSprite + idObjectToAdd * GameConfig.ROW_COUNT + 1 + " index");
        //LogicSetupRoom.instance.listRoomObject[idObjectToAdd].SetUpMaterial(currentSprite + idObjectToAdd * GameConfig.ROW_COUNT);
        RoomObject roomObj = LogicSetupRoom.instance.SelectedRoom(idObjectToAdd);
        roomObj.SetUpMaterial(currentSprite + idObjectToAdd * GameConfig.ROW_COUNT);
        Debug.Log($"show room {e}");
    }

    void AddNewRoom()
    {
        ListObjetRoomPainted listObjectRoom = new ListObjetRoomPainted();
        ObjectRoomPainted objectRoom = new ObjectRoomPainted();
        List<ObjectRoomPainted> listObjectPaintedCache = new List<ObjectRoomPainted>();

        objectRoom.idObject = idObjectToAdd;
        objectRoom.isPainted = true;
        objectRoom.currentSprite = currentSprite;
        listObjectPaintedCache.Add(objectRoom);
        listObjectRoom.listObjectPainted = listObjectPaintedCache;

        //Todo Set data

        ListRoomPainted dataCache = SaveGame.ListRoomPainted;
        dataCache.listRoomPainted.Add(listObjectRoom);
        SaveGame.ListRoomPainted = dataCache;
        Debug.Log("Save Data: " + idObjectToAdd);
    }

    void UpdateExistedRoom()
    {
        ListRoomPainted dataCache = SaveGame.ListRoomPainted;

        for (int i = 0; i < dataCache.listRoomPainted.Count; i++)
        {
            int idRoom = dataCache.listRoomPainted[i].idRoom;

            if (SaveGame.CurrentRoom == idRoom)
            {

                for (int j = 0; j < dataCache.listRoomPainted[i].listObjectPainted.Count; j++)
                {
                    int idObject = dataCache.listRoomPainted[i].listObjectPainted[j].idObject;

                    if (idObjectToAdd == idObject)
                    {
                        dataCache.listRoomPainted[i].listObjectPainted[j].currentSprite = currentSprite;
                        Debug.Log("wtf update????");
                    }
                }
            }
        }

        SaveGame.ListRoomPainted = dataCache;
    }


    //void LoadExistedData()
    //{
    //    for (int i = 0; i < SaveGame.ListRoomPainted.listRoomPainted.Count; i++)
    //    {
    //        int idRoom = SaveGame.ListRoomPainted.listRoomPainted[i].idRoom;
    //        if (SaveGame.CurrentRoom == idRoom)
    //        {
    //            for (int j = 0; j < SaveGame.ListRoomPainted.listRoomPainted[i].listObjectPainted.Count; j++)
    //            {
    //                int idObject = SaveGame.ListRoomPainted.listRoomPainted[i].listObjectPainted[j].idObject;
    //                int currentSprite = SaveGame.ListRoomPainted.listRoomPainted[i].listObjectPainted[j].currentSprite;
    //                if (idObjectToAdd == idObject)
    //                {
    //                    LogicSetupRoom.instance.listRoomObject[idObjectToAdd].SetUpMaterial(currentSprite + GameConfig.ROW_COUNT * idObject);
    //                }
    //            }
    //        }
    //    }
    //}

}
