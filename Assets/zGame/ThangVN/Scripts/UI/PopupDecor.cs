using ntDev;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupDecor : Popup
{
    [SerializeField] EasyButton btnBack, btnGallery, btnPlusColorPlate;
    [SerializeField] RectTransform select;
    [SerializeField] List<GameObject> listSelect;

    private void Awake()
    {
        btnBack.OnClick(() => BackHome());

        btnGallery.OnClick(() =>
        {
            PopupGallery.Show();
        });

        btnPlusColorPlate.OnClick(() => PopupGoToLevel.Show());
    }

    public static async void Show()
    {
        PopupDecor pop = await ManagerPopup.ShowPopup<PopupDecor>();
        HomeUI.Instance.animator.Play("Hide");
        pop.Init();
    }

    public override void Init()
    {
        base.Init();
        ManagerPopup.Instance.nShadow.GetComponent<Image>().enabled = false;

        for (int i = 0; i < listSelect.Count; i++)
        {
            listSelect[i].SetActive(false);
        }
        listSelect[SaveGame.CurrentObject].SetActive(true);

        SaveGame.CanShow = true;
    }

    public override void Hide()
    {
        base.Hide();
        ManagerPopup.Instance.nShadow.GetComponent<Image>().enabled = true;
    }

    public void BackHome()
    {
        HomeUI.Instance.animator.Play("Show");
        base.Hide();
    }


}
