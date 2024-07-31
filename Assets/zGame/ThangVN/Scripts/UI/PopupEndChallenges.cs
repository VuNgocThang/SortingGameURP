using ntDev;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupEndChallenges : Popup
{
    [SerializeField] EasyButton btnHome;
    [SerializeField] TextMeshProUGUI txtCoin, txtColorPlate, txtScore;
    [SerializeField] GameObject imgBest;

    [SerializeField] int score;
    public static async void Show()
    {
        PopupEndChallenges pop = await ManagerPopup.ShowPopup<PopupEndChallenges>();
        pop.Init();
    }
    public override void Init()
    {
        base.Init();

        txtCoin.text = SaveGame.Coin.ToString();

        txtColorPlate.text = SaveGame.Pigment.ToString();

        if (score > SaveGame.BestScore) imgBest.SetActive(true);
        else imgBest.SetActive(false);

        txtScore.text = SaveGame.BestScore.ToString();
    }
}