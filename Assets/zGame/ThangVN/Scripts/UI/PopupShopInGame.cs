using ntDev;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupShopInGame : Popup
{
    [SerializeField] EasyButton btnPrev, btnNext, btnBuyUseCoin, btnBuyAds;
    [SerializeField] BoosterData boosterData;
    public Image imgIcon, imgIconBuyUseIcon, imgIconBuyAds;
    public TextMeshProUGUI txtNameBooster;
    public BoosterEnum boosterEnum;
    public CanvasGroup canvasGroup;

    private void Awake()
    {
    }

    public static async void Show(int index)
    {
        PopupShopInGame pop = await ManagerPopup.ShowPopup<PopupShopInGame>();

        pop.Initialized(index);
    }


    public void Initialized(int index)
    {
        base.Init();
        canvasGroup.blocksRaycasts = true;
        boosterEnum = (BoosterEnum)index;

        btnBuyUseCoin.OnClick(() =>
        {
            BuyUseCoin(boosterEnum, false);
            canvasGroup.blocksRaycasts = false;
            Hide();
        });
        btnBuyAds.OnClick(() =>
        {
            BuyUseCoin(boosterEnum, true);
            Hide();
        });


        for (int i = 0; i < boosterData.listBooster.Count; i++)
        {
            if (index == (int)boosterData.listBooster[i].boosterEnum)
            {
                imgIcon.sprite = boosterData.listBooster[i].spriteBooster;
                imgIconBuyUseIcon.sprite = boosterData.listBooster[i].spriteIcon;
                imgIconBuyAds.sprite = boosterData.listBooster[i].spriteIcon;
                txtNameBooster.text = boosterData.listBooster[i].nameBooster;
            }
        }
    }

    public override void Hide()
    {
        canvasGroup.blocksRaycasts = false;
        base.Hide();

        //ManagerPopup.HidePopup<PopupShopInGame>();
    }

    public void BuyUseCoin(BoosterEnum boosterEnum, bool useAds)
    {
        switch (boosterEnum)
        {
            case BoosterEnum.BoosterSwap:
                if (useAds)
                {
                    SaveGame.Swap++;
                }
                else
                {
                    if (GameConfig.EnoughCoinBuySwap)
                    {
                        Debug.Log("swap");
                        SaveGame.Coin -= GameConfig.COIN_SWAP;
                        SaveGame.Swap++;
                    }
                }
                break;
            case BoosterEnum.BoosterHammer:
                if (useAds)
                {
                    SaveGame.Hammer++;
                }
                else
                {
                    if (GameConfig.EnoughCoinBuyHammer)
                    {
                        Debug.Log("Hammer");
                        SaveGame.Coin -= GameConfig.COIN_HAMMER;
                        SaveGame.Hammer++;
                    }
                }

                break;

            case BoosterEnum.BoosterRefresh:
                if (useAds)
                {
                    SaveGame.Refresh++;

                }
                else
                {
                    if (GameConfig.EnoughCoinBuyRefresh)
                    {
                        Debug.Log("Refresh");
                        SaveGame.Coin -= GameConfig.COIN_REFRESH;
                        SaveGame.Refresh++;
                    }
                }
                break;
        }
    }

}