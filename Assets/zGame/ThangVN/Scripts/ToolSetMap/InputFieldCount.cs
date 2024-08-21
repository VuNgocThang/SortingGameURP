using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ntDev;
using Utilities.Common;

public class InputFieldCount : MonoBehaviour
{
    [SerializeField] EasyButton btnConfirm, btnExit;
    [SerializeField] LogicDropdown dropdownPrefab;
    [SerializeField] RectTransform nParent;
    public TMP_InputField inputField;
    public List<LogicDropdown> listPieces;

    private void Awake()
    {
        btnConfirm.OnClick(() =>
        {
            SpawnDropDown();
        });

        btnExit.OnClick(() =>
        {
            ResetData();
        });
    }

    public void SpawnDropDown()
    {
        foreach (var dropdown in listPieces)
        {
            dropdown.gameObject.SetActive(false);
        }
        listPieces.Clear();

        int count = int.Parse(inputField.text);

        for (int i = 0; i < count; i++)
        {
            LogicDropdown dropdown = LogicDropdownPool.Instance.GetPooledObject();
            dropdown.SetActive(true);
            dropdown.SetParent(nParent);
            dropdown.Init(0);
            listPieces.Add(dropdown);
        }
        Debug.Log(count);
    }

    public void LoadText(int count)
    {
        inputField.text = count.ToString();
    }

    public void LoadExistedPlate(/*int count,*/ int index)
    {
        //for (int i = 0; i < count; i++)
        //{
        LogicDropdown dropdown = LogicDropdownPool.Instance.GetPooledObject();
        dropdown.SetActive(true);
        dropdown.SetParent(nParent);
        dropdown.Init(index);
        listPieces.Add(dropdown);
        //}
    }

    public void ResetData()
    {
        //Debug.Log("Clear Data");
        inputField.text = "0";
        foreach (var dropdown in listPieces)
        {
            dropdown.gameObject.SetActive(false);
        }
        listPieces.Clear();
    }
}
