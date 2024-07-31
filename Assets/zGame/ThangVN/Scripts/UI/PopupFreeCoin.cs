using DG.Tweening;
using ntDev;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PopupFreeCoin : Popup
{
    public FreeCoinData freeCoinData;
    public ItemFreeCoin itemFreeCoin;
    public Transform nContent;
    public List<ItemFreeCoin> listItem;
    private void Awake()
    {
        ManagerEvent.RegEvent(EventCMD.EVENT_FREECOIN, UpdateListContent);
    }

    public static async void Show()
    {
        PopupFreeCoin pop = await ManagerPopup.ShowPopup<PopupFreeCoin>();
        pop.Init();
    }

    public override void Init()
    {
        base.Init();
        listItem.Clear();

        for (int i = 0; i < freeCoinData.listDataFreeCoin.Count; i++)
        {
            ItemFreeCoin item = Instantiate(itemFreeCoin, nContent);
            item.isClaimed = freeCoinData.listDataFreeCoin[i].isClaimed;
            item.countCoin = freeCoinData.listDataFreeCoin[i].countCoin;
            item.index = freeCoinData.listDataFreeCoin[i].index;
            item.Show(SaveGame.DataFreeCoin.currentIndex);
            listItem.Add(item);
        }

    }

    public override void Hide()
    {
        transform.localScale = Vector3.one;

        transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            for (int i = 0; i < listItem.Count; i++)
            {
                listItem[i].gameObject.SetActive(false);
            }

            gameObject.SetActive(false);
            ManagerEvent.RaiseEvent(EventCMD.EVENT_POPUP_CLOSE, this);
        });
    }

    public void UpdateListContent(object e)
    {
        for (int i = 0; i < listItem.Count; i++)
        {
            if (listItem[i].index == SaveGame.DataFreeCoin.currentIndex)
            {
                listItem[i].imgActive.SetActive(true);
                listItem[i].imgInActive.SetActive(false);
            }
        }
    }

}
