using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;
using UnityEngine.SceneManagement;
using TMPro;

public class PopupEndless : Popup
{
    [SerializeField] EasyButton btnContinue;
    [SerializeField] TextMeshProUGUI txtBestScore;

    private void Awake()
    {
        btnContinue.OnClick(() =>
        {
            SaveGame.Challenges = true;
            ManagerEvent.ClearEvent();
            SceneManager.LoadScene("SceneGame");
        });
    }

    public static async void Show()
    {
        PopupEndless pop = await ManagerPopup.ShowPopup<PopupEndless>();

        pop.Init();
    }

    public override void Init()
    {
        base.Init();
        txtBestScore.text = SaveGame.BestScore.ToString();
    }

    public override void Hide()
    {
        base.Hide();
    }
}
