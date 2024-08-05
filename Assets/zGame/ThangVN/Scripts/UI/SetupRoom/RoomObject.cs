using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObject : MonoBehaviour
{
    public int id;
    //public List<Material> listMaterials;
    [SerializeField] MeshRenderer mesh;
    public List<GameObject> listObjects;
    public bool isPainted;

    //private void Awake()
    //{
    //    mesh = GetComponent<MeshRenderer>();
    //}

    public void SetUpMaterial(int index)
    {
        mesh.enabled = true;
        foreach (Material mat in mesh.materials)
        {
            mat.SetFloat("_Index", index + 1);
        }

        if (listObjects != null)
        {
            for (int i = 0; i < listObjects.Count; i++)
            {
                listObjects[i].SetActive(true);
            }
        }
    }
}
