using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using Color = UnityEngine.Color;

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
        //Application.targetFrameRate = 60;

        SaveGame.IsShow = false;
        listRoomPainted = SaveGame.ListRoomPainted.listRoomPainted;

        //for (int i = 0; i < SaveGame.ListRoomPainted.listRoomPainted.Count; i++)
        //{
        //    Debug.Log(SaveGame.ListRoomPainted.listRoomPainted[i].idRoom);

        //    for (int j = 0; j < SaveGame.ListRoomPainted.listRoomPainted[i].listObjectPainted.Count; j++)
        //    {
        //        Debug.Log(SaveGame.ListRoomPainted.listRoomPainted[i].listObjectPainted[j].idObject);
        //        Debug.Log(SaveGame.ListRoomPainted.listRoomPainted[i].listObjectPainted[j].currentSprite);
        //        Debug.Log(SaveGame.ListRoomPainted.listRoomPainted[i].listObjectPainted[j].isPainted);
        //    }
        //}

        for (int i = 0; i < SaveGame.ListRoomPainted.listRoomPainted.Count; i++)
        {
            int idRoom = SaveGame.ListRoomPainted.listRoomPainted[i].idRoom;
            if (SaveGame.CurrentRoom == idRoom)
            {
                for (int j = 0; j < SaveGame.ListRoomPainted.listRoomPainted[i].listObjectPainted.Count; j++)
                {
                    int idObject = SaveGame.ListRoomPainted.listRoomPainted[i].listObjectPainted[j].idObject;
                    int currentSprite = SaveGame.ListRoomPainted.listRoomPainted[i].listObjectPainted[j].currentSprite;
                    bool isPainted = SaveGame.ListRoomPainted.listRoomPainted[i].listObjectPainted[j].isPainted;

                    Debug.Log(listRoomObject[idObject].listObjects.Count + " count");
                    listRoomObject[idObject].isPainted = isPainted;
                    listRoomObject[idObject].SetUpMaterial(currentSprite + GameConfig.ROW_COUNT * idObject);
                }
            }
        }
    }

    private Ray ray;
    private RaycastHit hit;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 50f, layerRoom) && !SaveGame.IsShow)
            {
                RoomObject objRoom = hit.collider.GetComponent<RoomObject>();
                //if (!objRoom.isPainted)
                //{
                PopupSetupRoom.Show(SaveGame.CurrentRoom, objRoom.id);
                //}
            }
        }
    }
    //void OnDrawGizmos()
    //{
    //    if (ray.direction != Vector3.zero) 
    //    {
    //        Gizmos.color = Color.red;

    //        Gizmos.DrawRay(ray.origin, ray.direction * 50f);

    //        if (hit.collider != null)
    //        {
    //            Gizmos.color = Color.green;
    //            Gizmos.DrawSphere(hit.point, 0.5f);
    //        }
    //    }
    //}

    public RoomObject SelectedRoom(int index)
    {
        RoomObject roomObject = null;
        for (int i = 0; i < listRoomObject.Count; i++)
        {
            if (listRoomObject[i].id == index)
            {
                roomObject = listRoomObject[i];
                break;
            }
        }
        return roomObject;
    }
}
