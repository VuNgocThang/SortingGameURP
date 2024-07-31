using ntDev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupGoToLevel : Popup
{
    [SerializeField] EasyButton btnGoToLevel;

    private void Awake()
    {
        btnGoToLevel.OnClick(() =>
        {
            ManagerEvent.ClearEvent();
            SceneManager.LoadScene("SceneGame");
        });
    }

    public static async void Show()
    {
        PopupGoToLevel pop = await ManagerPopup.ShowPopup<PopupGoToLevel>();

        pop.Init();
    }

    public override void Init()
    {
        base.Init();
    }

    public override void Hide()
    {
        base.Hide();
    }
}