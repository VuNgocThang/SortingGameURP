using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LogicSetupRoom : MonoBehaviour
{
    public static LogicSetupRoom instance;
    private void Awake()
    {
        instance = this;
    }
    public List<RoomObject> listRoomObject;
    [SerializeField] LayerMask layerRoom;
    [SerializeField] Camera cam;
    [SerializeField] List<ListObjetRoomPainted> listRoomPainted;

    private void Start()
    {
        SaveGame.IsShow = false;
        listRoomPainted = SaveGame.ListRoomPainted.listRoomPainted;

        for (int i = 0; i < SaveGame.ListRoomPainted.listRoomPainted.Count; i++)
        {
            Debug.Log(SaveGame.ListRoomPainted.listRoomPainted[i].idRoom);

            for (int j = 0; j < SaveGame.ListRoomPainted.listRoomPainted[i].listObjectPainted.Count; j++)
            {
                Debug.Log(SaveGame.ListRoomPainted.listRoomPainted[i].listObjectPainted[j].idObject);
                Debug.Log(SaveGame.ListRoomPainted.listRoomPainted[i].listObjectPainted[j].currentSprite);
                Debug.Log(SaveGame.ListRoomPainted.listRoomPainted[i].listObjectPainted[j].isPainted);
            }
        }

        for (int i = 0; i < SaveGame.ListRoomPainted.listRoomPainted.Count; i++)
        {
            int idRoom = SaveGame.ListRoomPainted.listRoomPainted[i].idRoom;
            if (SaveGame.CurrentRoom == idRoom)
            {
                for (int j = 0; j < SaveGame.ListRoomPainted.listRoomPainted[i].listObjectPainted.Count; j++)
                {
                    int idObject = SaveGame.ListRoomPainted.listRoomPainted[i].listObjectPainted[j].idObject;
                    int currentSprite = SaveGame.ListRoomPainted.listRoomPainted[i].listObjectPainted[j].currentSprite;

                    listRoomObject[idObject].SetUpMaterial(currentSprite);
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 100f, layerRoom) && !SaveGame.IsShow)
            {
                RoomObject objRoom = hit.collider.GetComponent<RoomObject>();
                PopupSetupRoom.Show(SaveGame.CurrentRoom, objRoom.id);
            }
        }
    }
}
