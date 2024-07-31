using ntDev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupDecor : Popup
{
    [SerializeField] EasyButton btnBack, btnGallery, btnPlusColorPlate;

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
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void BackHome()
    {
        HomeUI.Instance.animator.Play("Show");
        base.Hide();
    }
}
