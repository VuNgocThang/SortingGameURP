using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogicDropdown : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TMP_Dropdown tmpDropdown;
    public TMP_InputField nCount;
    public List<Sprite> listIcons;
    public int indexType;

    private void Awake()
    {
        tmpDropdown.onValueChanged.AddListener(delegate
        {
            DropdownItemSelected(tmpDropdown);
        });
    }

    public void Init(int index)
    {
        icon.sprite = listIcons[index];
    }

    void DropdownItemSelected(TMP_Dropdown dropdown)
    {
        int index = dropdown.value;
        indexType = index;
        string selectedItem = dropdown.options[index].text;

        icon.sprite = listIcons[index];
        Debug.Log("index: " + index);
        Debug.Log("Selected item: " + selectedItem);
    }

    public void LoadText(int count)
    {
        nCount.text = count.ToString();
    }

}
