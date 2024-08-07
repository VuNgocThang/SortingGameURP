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
    public Animator anim;


    //private void Awake()
    //{
    //    mesh = GetComponent<MeshRenderer>();
    //}

    public void SetUpMaterial(int index)
    {
        Debug.Log("isPainted: " + isPainted);
        mesh.enabled = true;
        if (anim != null)
        {
            if (!isPainted) anim.Play("Show");
        }


        foreach (Material mat in mesh.materials)
        {
            mat.SetFloat("_Index", index + 1);
        }

        if (listObjects != null)
        {
            for (int i = 0; i < listObjects.Count; i++)
            {
                listObjects[i].SetActive(true);
                if (!isPainted)
                    listObjects[i].GetComponent<Animator>().Play("Show");
            }
        }
    }
}
