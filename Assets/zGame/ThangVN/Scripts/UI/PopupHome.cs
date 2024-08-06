using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ntDev;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using BaseGame;
using ThangVN;
using System;

public class PopupHome : MonoBehaviour
{
    [SerializeField] Camera cam;
    public EasyButton btnSetting, btnCloseItem;
    public TextMeshProUGUI txtPoint, txtLevel, txtCount, txtCurrentScore, txtBestScore, txtTargetPigment, txtLevelInTarget;
    public Image imgFill;
    [SerializeField] Animator animBtnSwitch;
    [SerializeField] GameObject imgSpecial, top, bot;
    [SerializeField] GameObject itemObj, nLevel, nBar, nScoreChallenges, nTargetPigment, nChallenges;
    bool canClick = true;

    [SerializeField] Image iconItem, imgTextName;
    [SerializeField] TextMeshProUGUI txtExplain;
    [SerializeField] HandDrag handDrag;
    [SerializeField] ButtonBoosterHammer btnHammer;
    [SerializeField] ButtonBoosterRefresh btnRefresh;
    [SerializeField] ButtonBoosterSwap btnSwap;
    [SerializeField] BoosterData boosterData;
    [SerializeField] Animator animBar, animPigment, animChallenges;
    [SerializeField] RectTransform rectTransformTarget, rectTransformChallenges;
    [SerializeField] Transform iconFake, iconTargetPigment, txtFake, txtChallengesObj;
    public GameObject imgDanger;
    public GameObject UiEffect;
    public GameObject UiEffect2;


    private void Awake()
    {
        btnSetting.OnClick(() => PopupSetting.Show());


        btnCloseItem.OnClick(ExitUsingItem);
        btnHammer.Init();
        btnSwap.Init();
        btnRefresh.Init();

        btnRefresh.button.OnClick(() =>
        {
            if (SaveGame.Level >= btnRefresh.indexLevelUnlock)
            {
                if (SaveGame.Refresh > 0)
                {
                    SaveGame.Refresh--;
                    ShuffleRandomColorSpawn();
                }
                else PopupShopInGame.Show((int)BoosterEnum.BoosterRefresh);
            }
        });

        btnHammer.button.OnClick(() =>
        {
            if (SaveGame.Level >= btnHammer.indexLevelUnlock)
            {
                if (SaveGame.Hammer > 0)
                {
                    UsingItemHammer();
                }
                else PopupShopInGame.Show((int)BoosterEnum.BoosterHammer);
            }
        });

        btnSwap.button.OnClick(() =>
        {
            if (SaveGame.Level >= btnSwap.indexLevelUnlock)
            {
                if (SaveGame.Swap > 0)
                {
                    UsingItemSwap();
                }
                else PopupShopInGame.Show((int)BoosterEnum.BoosterSwap);
            }
        });

        ManagerEvent.RegEvent(EventCMD.EVENT_POINT, UpdatePoint);
        ManagerEvent.RegEvent(EventCMD.EVENT_WIN, ShowPopupWin);
        ManagerEvent.RegEvent(EventCMD.EVENT_LOSE, ShowPopupLose);
        ManagerEvent.RegEvent(EventCMD.EVENT_COUNT, UpdateCount);
        ManagerEvent.RegEvent(EventCMD.EVENT_CHALLENGES, ShowPopupEndChallenges);
    }

    void Start()
    {
        if (SaveGame.Music) ManagerAudio.PlayMusic(ManagerAudio.Data.musicInGame);
        else ManagerAudio.PauseMusic();

        txtLevel.text = $"Level {SaveGame.Level + 1}";

        btnHammer.Init();
        btnSwap.Init();
        btnRefresh.Init();

        if (!SaveGame.Challenges)
        {
            nLevel.SetActive(true);
            nScoreChallenges.SetActive(false);
            StartCoroutine(ShowTarget());
        }
        else
        {
            nLevel.SetActive(false);
            nBar.SetActive(false);
            //nScoreChallenges.SetActive(true);
            StartCoroutine(ShowText());
        }

        iconFake.DOMove(rectTransformTarget.position, 1f);
        // Reach Level Show Popup UnlockBooster
        //PopupUnlockBooster.Show(index);
    }

    private void Update()
    {
        if (!SaveGame.Challenges)
        {
            imgFill.fillAmount = (float)LogicGame.Instance.point / (float)LogicGame.Instance.maxPoint;
            txtPoint.text = $"{LogicGame.Instance.point}/{LogicGame.Instance.maxPoint}";
        }
        else
        {
            txtCurrentScore.text = $"{LogicGame.Instance.point}";

            if (LogicGame.Instance.point >= SaveGame.BestScore) SaveGame.BestScore = LogicGame.Instance.point;
            txtBestScore.text = $"Best score: {SaveGame.BestScore}";
        }


        if (Input.GetKeyDown(KeyCode.V))
        {
            SaveGame.Level++;
        }
    }

    void UpdatePoint(object e)
    {
        animBar.Play("AnimBar", 0, 0);
        LogicGame.Instance.point += (int)e;

        if (!SaveGame.Challenges)
        {
            if (LogicGame.Instance.point >= LogicGame.Instance.maxPoint) LogicGame.Instance.point = LogicGame.Instance.maxPoint;

            imgFill.fillAmount = (float)LogicGame.Instance.point / (float)LogicGame.Instance.maxPoint;
            txtPoint.text = $"{LogicGame.Instance.point} / {LogicGame.Instance.maxPoint}";

        }
        else
        {
            txtCurrentScore.text = $"{LogicGame.Instance.point}";

            if (LogicGame.Instance.point >= SaveGame.BestScore) SaveGame.BestScore = LogicGame.Instance.point;
            txtBestScore.text = $"Best score: {SaveGame.BestScore}";
        }
    }

    void UpdateCount(object e)
    {
        txtCount.text = $"{e}";
    }

    void ShowPopupWin(object e)
    {
        PopupWin.Show();
    }

    void ShowPopupLose(object e)
    {
        PopupLose.Show();
    }

    void ShowPopupEndChallenges(object e)
    {
        PopupEndChallenges.Show();
    }

    IEnumerator OnClickSwitch()
    {
        canClick = false;
        if (animBtnSwitch != null) animBtnSwitch.Play("Click");
        yield return new WaitForSeconds(0.2f);
        canClick = true;
    }

    public IEnumerator PlayEffectSpecial()
    {
        imgSpecial.SetActive(true);
        yield return new WaitForSeconds(1f);
        imgSpecial.SetActive(false);
    }

    void ShuffleRandomColorSpawn()
    {
        //LogicGame.Instance.InitPlateSpawn(true);
        LogicGame.Instance.ShufflePlateSpawn();
    }

    void DestroyColorPlate()
    {
        // Change focus camera
        // Set active false some element UI
        // 

    }
    void ActiveUsingItem(bool isHammer)
    {
        btnHammer.Init();
    }

    void UsingItemHammer()
    {
        itemObj.SetActive(true);
        top.SetActive(false);
        bot.SetActive(false);
        LogicGame.Instance.isPauseGame = true;

        CameraChange.Ins.ChangeCameraUsingItem();

        LogicGame.Instance.isUsingHammer = true;
        SetupImgItem((int)BoosterEnum.BoosterHammer);
        Debug.Log("use hammer");
    }


    void UsingItemSwap()
    {
        itemObj.SetActive(true);
        top.SetActive(false);
        bot.SetActive(false);
        LogicGame.Instance.isPauseGame = true;

        CameraChange.Ins.ChangeCameraUsingItem();

        LogicGame.Instance.isUsingHand = true;
        SetupImgItem((int)BoosterEnum.BoosterSwap);

        Debug.Log("use swap");
    }

    public void ExitUsingItem()
    {
        LogicGame.Instance.isUsingHammer = false;
        LogicGame.Instance.isUsingHand = false;
        LogicGame.Instance.isPauseGame = false;
        handDrag.selectingPlate = null;

        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.2f);
        CameraChange.Ins.ExitUsingItemCamera();

        itemObj.SetActive(false);
        top.SetActive(true);
        bot.SetActive(true);
    }

    void SetupImgItem(int index)
    {
        for (int i = 0; i < boosterData.listBooster.Count; i++)
        {
            if (index == (int)boosterData.listBooster[i].boosterEnum)
            {
                imgTextName.sprite = boosterData.listBooster[i].spriteText;
                iconItem.sprite = boosterData.listBooster[i].spriteIcon;
                txtExplain.text = boosterData.listBooster[i].textExplain;
            }
        }
    }

    IEnumerator ShowTarget()
    {
        LogicGame.Instance.isPauseGame = true;
        nTargetPigment.SetActive(true);
        txtLevelInTarget.text = $"Level {SaveGame.Level + 1}";
        txtTargetPigment.text = LogicGame.Instance.colorPlateData.pigment.ToString();
        animPigment.Play("Show");

        yield return new WaitForSeconds(1f);

        Vector3 targetWorldPosition = cam.ScreenToWorldPoint(rectTransformTarget.position);
        targetWorldPosition.z = 0;

        iconFake.gameObject.SetActive(true);
        iconFake.position = iconTargetPigment.position;
        iconTargetPigment.gameObject.SetActive(false);

        iconFake.transform.DOMove(rectTransformTarget.position, 0.3f)
            .OnComplete(() =>
            {
                iconFake.gameObject.SetActive(false);
                nBar.SetActive(true);
                nTargetPigment.SetActive(false);
                TutorialManager.ShowPopup(SaveGame.Level);
            });
    }


    IEnumerator ShowText()
    {
        LogicGame.Instance.isPauseGame = true;
        nChallenges.SetActive(true);
        animChallenges.Play("Show");

        yield return new WaitForSeconds(1f);

        Vector3 targetWorldPosition = cam.ScreenToWorldPoint(rectTransformChallenges.position);
        targetWorldPosition.z = 0;

        txtFake.gameObject.SetActive(true);
        txtFake.position = txtChallengesObj.position;
        txtChallengesObj.gameObject.SetActive(false);

        txtFake.transform.DOMove(rectTransformChallenges.position, 0.3f)
           .OnComplete(() =>
           {
               txtFake.gameObject.SetActive(false);
               nScoreChallenges.SetActive(true);
               nChallenges.SetActive(false);
           });

    }

    private void OnApplicationQuit()
    {
        if (SaveGame.Heart <= GameConfig.MAX_HEART)
        {
            PlayerPrefs.SetString(GameConfig.LAST_HEART_LOSS, DateTime.Now.ToString());
            PlayerPrefs.Save();
        }
    }
}
