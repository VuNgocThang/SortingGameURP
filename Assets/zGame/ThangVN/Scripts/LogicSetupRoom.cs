using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using Color = UnityEngine.Color;

public class LogicSetupRoom : MonoBehaviour
{
    public static LogicSetupRoom instance;
    private void Awake()
    {
        instance = this;
    }
    public List<RoomObject> listRoomObject;
    public List<GameObject> listGameObject;
    [SerializeField] LayerMask layerRoom;
    [SerializeField] LayerMask layerNoRaycast;
    [SerializeField] Camera cam;
    [SerializeField] List<ListObjetRoomPainted> listRoomPainted;
    [SerializeField] EventSystem currentEvent;


    private void Start()
    {
        Application.targetFrameRate = 60;

        SaveGame.CanShow = false;
        listRoomPainted = SaveGame.ListRoomPainted.listRoomPainted;


        for (int i = 0; i < listRoomPainted.Count; i++)
        {
            int idRoom = listRoomPainted[i].idRoom;

            if (SaveGame.CurrentRoom == idRoom)
            {
                for (int j = 0; j < listRoomPainted[i].listObjectPainted.Count; j++)
                {
                    int idObject = listRoomPainted[i].listObjectPainted[j].idObject;
                    int currentSprite = listRoomPainted[i].listObjectPainted[j].currentSprite;
                    bool isPainted = listRoomPainted[i].listObjectPainted[j].isPainted;

                    Debug.Log("idObject: " + idObject + "___ " + " currentSprite: " + currentSprite + "___ " + "isPainted: " + isPainted);

                    for (int k = 0; k < listRoomObject.Count; k++)
                    {
                        if (listRoomObject[k].id == idObject)
                        {
                            Debug.Log(listRoomObject[k].name);
                            if (listRoomObject[k].id == 3) listGameObject[3].SetActive(true);
                            if (listRoomObject[k].id == 4) listGameObject[4].SetActive(true);

                            listRoomObject[k].gameObject.SetActive(true);
                            listRoomObject[k].SetUpMaterial(currentSprite + GameConfig.ROW_COUNT * idObject);
                            listRoomObject[k].isPainted = isPainted;
                            listRoomObject[k].gameObject.layer = 11;
                        }
                    }
                }
            }
        }
    }

    private Ray ray;
    private RaycastHit hit;


    public void OnPointerClick()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 500f, layerRoom) && SaveGame.CanShow/* && !IsPointerOverUIObject()*/)
        {
            RoomObject objRoom = hit.collider.GetComponent<RoomObject>();
            if (SaveGame.CurrentObject != objRoom.id) return;

            Debug.Log(objRoom.name);

            if (!objRoom.isPainted)
            {
                PopupSetupRoom.Show(SaveGame.CurrentRoom, objRoom.id);
            }
        }
    }
    void OnDrawGizmos()
    {
        if (ray.direction != Vector3.zero)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(ray.origin, ray.direction * 50f);
            if (hit.collider != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(hit.point, 0.5f);
            }
        }
    }

    public List<RoomObject> SelectedRoomObject(int index)
    {
        List<RoomObject> listRoom = new List<RoomObject>();
        for (int i = 0; i < listRoomObject.Count; i++)
        {
            if (listRoomObject[i].id == index)
            {
                listRoom.Add(listRoomObject[i]);
            }
        }
        return listRoom;
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(currentEvent);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        currentEvent.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
