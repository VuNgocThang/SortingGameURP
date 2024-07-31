using ntDev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelectRoom : MonoBehaviour
{
    public EasyButton btnSelect;
    public int id;
    public Image icon;
    [SerializeField] ListRoomData listRoomData;


    private void Awake()
    {
        btnSelect.OnClick(SetCurrentIndex);
    }
    public void Init(int idRoom, int idObject, int indexSprite)
    {
        Debug.Log("Init At: " + idObject);
        //icon.sprite = listRoomData.listRoom[idRoom].listObjectPainted[idObject].sprite[indexSprite];
    }

    void SetCurrentIndex()
    {
        ManagerEvent.RaiseEvent(EventCMD.EVENT_SELECT_ROOM, id);
    }

    void Show(int index)
    {

    }
}
