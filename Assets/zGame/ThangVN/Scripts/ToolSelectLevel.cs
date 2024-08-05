using ntDev;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToolSelectLevel : MonoBehaviour
{
    public EasyButton btnOke;
    public TMP_InputField inputField;

    private void Start()
    {
        btnOke.OnClick(MoveToLevel);
    }

    void MoveToLevel()
    {
        Debug.Log(inputField.text);
        ManagerEvent.ClearEvent();
        SaveGame.Level = int.Parse(inputField.text) - 1;
        SceneManager.LoadScene("SceneGame");
    }
}