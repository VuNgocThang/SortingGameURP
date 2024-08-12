using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;
using UnityEngine.SceneManagement;
using DG.Tweening;
using BaseGame;
using TMPro;

public class PopupWin : Popup
{
    public EasyButton btnContinue, btnClaimX2, btnHome;
    public TextMeshProUGUI txtGoldReward, txtPigmentReward, txtGold, txtPigment;
    public static async void Show()
    {
        PopupWin pop = await ManagerPopup.ShowPopup<PopupWin>();
        pop.Init();
    }

    private void Awake()
    {
        btnContinue.OnClick(() =>
        {
            ManagerEvent.ClearEvent();
            //ManagerPopup.HidePopup<PopupWin>();
            StartCoroutine(LoadScene("SceneGame"));
        });

        btnHome.OnClick(() =>
        {
            ManagerEvent.ClearEvent();
            //ManagerPopup.HidePopup<PopupWin>();
            StartCoroutine(LoadScene("SceneHome"));

        });

        btnClaimX2.OnClick(() =>
        {
            ManagerEvent.ClearEvent();
            //ManagerPopup.HidePopup<PopupWin>();
            StartCoroutine(LoadScene("SceneGame"));

        });



    }

    public override void Init()
    {
        base.Init();
        ManagerAudio.PlaySound(ManagerAudio.Data.soundPaperFireWorks);
        ManagerAudio.PlaySound(ManagerAudio.Data.soundPopupWin);
        Debug.Log("init popup win");
        txtGold.text = SaveGame.Coin.ToString();
        txtPigment.text = SaveGame.Pigment.ToString();

        txtGoldReward.text = LogicGame.Instance.gold.ToString();
        txtPigmentReward.text = LogicGame.Instance.pigment.ToString();

        SaveGame.Coin += LogicGame.Instance.gold;
        SaveGame.Pigment += LogicGame.Instance.pigment;
    }

    //private void Update()
    //{
    //    if (vfx != null)
    //        vfx.transform.Rotate(new Vector3(0, 0, 1) * -20f * Time.deltaTime);
    //}

    IEnumerator LoadScene(string sceneName)
    {
        base.Hide();
        yield return null;
        SceneManager.LoadScene(sceneName);
    }
}
