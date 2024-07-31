using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObject : MonoBehaviour
{
    public int id;
    public List<Material> listMaterials;
    [SerializeField] MeshRenderer mesh;

    public void SetUpMaterial(int index)
    {
        mesh.material = listMaterials[index];
    }
}
