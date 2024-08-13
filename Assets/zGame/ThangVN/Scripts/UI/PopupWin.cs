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
    public Transform vfx;

    // effect coin
    public GameObject pileCoin;
    public List<GameObject> pileOfCoins;
    public Vector3[] initPosCoin;
    public Quaternion[] initRotCoin;
    public Transform endPosCoin;

    //effect pigment
    public GameObject pilePigment;
    public List<GameObject> pileOfPigment;
    public Vector3[] initPosPigment;
    public Quaternion[] initRotPigment;
    public Transform endPosPigment;

    //
    int currentCoin;
    int currentPigment;
    float duration = 1f;

    public static async void Show()
    {
        PopupWin pop = await ManagerPopup.ShowPopup<PopupWin>();
        pop.Init();
    }

    private void Awake()
    {

        btnContinue.OnClick(() =>
        {
            InitPile();
            ReceiveReward();
            ManagerEvent.ClearEvent();
            //StartCoroutine(LoadScene("SceneGame"));
        });

        btnHome.OnClick(() =>
        {
            InitPile();

            ReceiveReward();

            ManagerEvent.ClearEvent();
            //StartCoroutine(LoadScene("SceneHome"));

        });

        btnClaimX2.OnClick(() =>
        {
            InitPile();

            ReceiveReward();

            ManagerEvent.ClearEvent();
            //StartCoroutine(LoadScene("SceneGame"));

        });
    }

    public override void Init()
    {
        base.Init();
        ManagerAudio.PlaySound(ManagerAudio.Data.soundPaperFireWorks);
        ManagerAudio.PlaySound(ManagerAudio.Data.soundPopupWin);

        Debug.Log("init popup win");

        currentCoin = SaveGame.Coin;
        currentPigment = SaveGame.Pigment;

        txtGold.text = SaveGame.Coin.ToString();
        txtPigment.text = SaveGame.Pigment.ToString();

        txtGoldReward.text = LogicGame.Instance.gold.ToString();
        txtPigmentReward.text = LogicGame.Instance.pigment.ToString();

        SaveGame.Coin += LogicGame.Instance.gold;
        SaveGame.Pigment += LogicGame.Instance.pigment;
    }

    private void Update()
    {
        if (vfx != null)
            vfx.Rotate(new Vector3(0, 0, 1) * -20f * Time.deltaTime);
    }

    IEnumerator LoadScene(string sceneName)
    {
        //base.Hide();
        yield return null;
        SceneManager.LoadScene(sceneName);
    }




    void InitPile()
    {
        for (int i = 0; i < pileOfCoins.Count; i++)
        {
            initPosCoin[i] = pileOfCoins[i].transform.position;
            initRotCoin[i] = Quaternion.Euler(67.407f, 0, 0);
        }

        for (int i = 0; i < pileOfPigment.Count; i++)
        {
            initPosPigment[i] = pileOfPigment[i].transform.position;
            initRotPigment[i] = Quaternion.Euler(67.407f, 0, 0);
        }
    }
    public void Reset()
    {
        for (int i = 0; i < pileOfCoins.Count; i++)
        {
            pileOfCoins[i].transform.position = initPosCoin[i];
            pileOfCoins[i].transform.rotation = initRotCoin[i];
        }

        for (int i = 0; i < pileOfPigment.Count; i++)
        {
            pileOfPigment[i].transform.position = initPosPigment[i];
            pileOfPigment[i].transform.rotation = initRotPigment[i];
        }
    }

    public void ReceiveReward()
    {
        Reset();

        var delayCoin = 0f;
        var delayPigment = 0f;

        for (int i = 0; i < pileOfCoins.Count; i++)
        {
            pileOfCoins[i].transform
                .DOScale(1f, 0.3f)
                .SetDelay(delayCoin)
                .SetEase(Ease.InOutCirc);

            if (endPosCoin != null)
            {
                pileOfCoins[i].GetComponent<RectTransform>()
                    .DOMove(endPosCoin.position, 0.5f)
                    .SetDelay(delayCoin + 0.2f)
                    .SetEase(Ease.InOutCirc);
            }

            pileOfCoins[i].transform
                .DORotate(Vector3.zero, 0.5f)
                .SetDelay(delayCoin + 0.2f)
                .SetEase(Ease.InOutCirc);

            pileOfCoins[i].transform
                .DOScale(0f, 0.3f)
                .SetDelay(delayCoin + 1f)
                .SetEase(Ease.InOutCirc);

            delayCoin += 0.1f;
        }


        for (int i = 0; i < pileOfPigment.Count; i++)
        {
            pileOfPigment[i].transform
                .DOScale(1f, 0.3f)
                .SetDelay(delayPigment)
                .SetEase(Ease.InOutCirc);

            if (endPosPigment != null)
            {
                pileOfPigment[i].GetComponent<RectTransform>()
                    .DOMove(endPosPigment.position, 0.5f)
                    .SetDelay(delayPigment + 0.2f)
                    .SetEase(Ease.InOutCirc);
            }

            pileOfPigment[i].transform
                .DORotate(Vector3.zero, 0.5f)
                .SetDelay(delayPigment + 0.2f)
                .SetEase(Ease.InOutCirc);

            pileOfPigment[i].transform
                .DOScale(0f, 0.3f)
                .SetDelay(delayPigment + 1f)
                .SetEase(Ease.InOutCirc);

            delayPigment += 0.1f;

        }


    }



    public void UpdateMoney(int targetMoney)
    {
        StartCoroutine(CountMoney(currentCoin, targetMoney, duration));
    }

    private IEnumerator CountMoney(int start, int end, float duration)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            //currentCoin = Mathf.Lerp(start, end, elapsed / duration);
            txtGold.text = currentCoin.ToString();
            yield return null;
        }

        currentCoin = end;
        txtGold.text = currentCoin.ToString();
    }
}
