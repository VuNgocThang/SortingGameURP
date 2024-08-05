﻿using BaseGame;
using DG.Tweening;
using ntDev;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities.Common;
using Color = UnityEngine.Color;
public enum GameMode
{
    Edit,
    Play
}

public class LogicGame : MonoBehaviour
{
    public static LogicGame Instance;
    public GameMode gameMode;

    public const int RULE_COMPLETE = 10;
    [SerializeField] Camera cam;
    [SerializeField] Transform holder;
    [SerializeField] public Transform targetUIPosition;

    [SerializeField] public ColorPlateData colorPlateData;
    [SerializeField] ColorPlate colorPLatePrefab;

    [SerializeField] public List<ColorPlate> listNextPlate;
    [SerializeField] public List<ColorPlate> listSpawnNew;
    [SerializeField] public List<ColorPlate> ListColorPlate;
    [SerializeField] public List<ColorPlate> ListArrowPlate;
    [HideInInspector] public int rows;
    [HideInInspector] public int cols;
    [SerializeField] public PopupHome homeInGame;

    [SerializeField] LayerMask layerArrow;
    [SerializeField] LayerMask layerPlateSpawn;
    [SerializeField] LayerMask layerUsingItem;
    [SerializeField] List<LogicColor> listColors;
    [SerializeField] private ColorPlate colorRoot;

    [SerializeField] private ParticleSystem clickParticle;
    [SerializeField] private ParticleSystem eatParticle;
    [SerializeField] private ParticleSystem unlockParticle;
    [SerializeField] private ParticleSystem specialParticle;

    public CustomPool<ParticleSystem> clickParticlePool;
    public CustomPool<ParticleSystem> eatParticlePool;
    public CustomPool<ParticleSystem> unlockParticlePool;
    public CustomPool<ParticleSystem> specialParticlePool;

    public bool isMergeing;
    Tweener tweenerMove;

    public bool isLose = false;
    public bool isWin = false;
    public bool isPauseGame = false;
    public static bool isContiuneMerge = false;
    public int point;
    public int maxPoint;
    public int gold;
    public int pigment;

    [SerializeField] int pointSpawnSpecial = 100;
    [SerializeField] RectTransform slot_1;
    [SerializeField] RectTransform slot_2;
    [SerializeField] RectTransform slot_3;
    [SerializeField] RectTransform slot_4;

    public AnimationCurve curveMove;
    [SerializeField] SetMapManager setMapManager;
    [HideInInspector] public int countMove;
    public int countDiff;
    public int countDiffMax;
    public bool isUsingHammer;
    public bool isUsingHand;

    int countToMove = 0;
    int countSpawnSpecial = 0;
    [SerializeField] bool isHadSpawnSpecial = false;
    LogicColor GetColorNew()
    {
        return listColors.GetClone();
    }
    private void Awake()
    {
        Instance = this;
        ManagerEvent.RegEvent(EventCMD.EVENT_SWITCH, SwitchNextPlate);

    }

    void Start()
    {
        Debug.Log("Load scene Game: " + SaveGame.Challenges);
        Debug.Log(SaveGame.Heart + " heart");
        Refresh();
        InitPlateSpawn(false);

        LoadData();
    }

    private void Refresh()
    {
        DOTween.KillAll();
        isWin = false;
        isLose = false;
        isMergeing = false;
        isPauseGame = false;
        listColors.Refresh();
        countDiff = 2;
        countDiffMax = 5;
        countMove = 2;
        point = 0;
        ManagerEvent.RaiseEvent(EventCMD.EVENT_COUNT, countMove);
        clickParticlePool = new CustomPool<ParticleSystem>(clickParticle, 5, transform, false);
        eatParticlePool = new CustomPool<ParticleSystem>(eatParticle, 5, transform, false);
        unlockParticlePool = new CustomPool<ParticleSystem>(unlockParticle, 5, transform, false);
        specialParticlePool = new CustomPool<ParticleSystem>(specialParticle, 2, transform, false);
        Application.targetFrameRate = 60;

        ResetPosSpawn();
    }

    void ResetPosSpawn()
    {
        Vector3 screenPos1 = RectTransformUtility.WorldToScreenPoint(cam, slot_1.position);
        Vector3 screenPos2 = RectTransformUtility.WorldToScreenPoint(cam, slot_2.position);
        Vector3 screenPos3 = RectTransformUtility.WorldToScreenPoint(cam, slot_3.position);
        Vector3 screenPos4 = RectTransformUtility.WorldToScreenPoint(cam, slot_4.position);

        Vector3 worldPos1;
        Vector3 worldPos2;
        Vector3 worldPos3;
        Vector3 worldPos4;

        RectTransformUtility.ScreenPointToWorldPointInRectangle(slot_1, screenPos1, cam, out worldPos1);
        RectTransformUtility.ScreenPointToWorldPointInRectangle(slot_2, screenPos2, cam, out worldPos2);
        RectTransformUtility.ScreenPointToWorldPointInRectangle(slot_3, screenPos3, cam, out worldPos3);
        RectTransformUtility.ScreenPointToWorldPointInRectangle(slot_4, screenPos4, cam, out worldPos4);

        listSpawnNew[0].transform.position = worldPos1;
        listSpawnNew[1].transform.position = worldPos2;
        listSpawnNew[2].transform.position = worldPos3;
        listNextPlate[0].transform.position = worldPos4;
    }

    void LoadData()
    {
        setMapManager.LoadData();
        colorPlateData = setMapManager.colorPlateData;

        rows = colorPlateData.rows;
        cols = colorPlateData.cols;
        setMapManager.Init(rows, cols, holder, ListColorPlate, colorPLatePrefab);

        if (SaveGame.Challenges)
        {
            LoadLevelChallenges();
        }
        else
        {
            LoadLevelNormal();
        }
        DataLevel dataLevel = DataLevel.GetData(SaveGame.currentLevel + 1);

        Debug.Log(dataLevel.ID + " Level ");
    }

    void LoadLevelChallenges()
    {
        for (int i = 0; i < colorPlateData.listArrowData.Count; i++)
        {
            int index = colorPlateData.listArrowData[i].Row * cols + colorPlateData.listArrowData[i].Col;

            ListArrowPlate.Add(ListColorPlate[index]);

            if (ListColorPlate[index].isLocked)
            {
                ListColorPlate[index].anim.enabled = false;
            }
            else
            {
                ListColorPlate[index].anim.enabled = true;
            }

            ListColorPlate[index].status = (Status)colorPlateData.listArrowData[i].type;
            ListColorPlate[index].gameObject.layer = 6;
            ListColorPlate[index].logicVisual.SetDirectionArrow(ListColorPlate[index].status, ListColorPlate[index].isLocked);
        }

        for (int i = 0; i < colorPlateData.listEmptyData.Count; i++)
        {
            int index = colorPlateData.listEmptyData[i].Row * cols + colorPlateData.listEmptyData[i].Col;
            ListColorPlate[index].logicVisual.DeletePlate();
        }
    }


    private void LoadLevelNormal()
    {
        for (int i = 0; i < colorPlateData.listSpecialData.Count; i++)
        {
            int index = colorPlateData.listSpecialData[i].Row * cols + colorPlateData.listSpecialData[i].Col;

            ListColorPlate[index].status = (Status)colorPlateData.listSpecialData[i].type;
            ListColorPlate[index].logicVisual.SetSpecialSquare(ListColorPlate[index].status);

            if (ListColorPlate[index].status == Status.Frozen)
            {
                ListColorPlate[index].countFrozen = 3;
            }

            if (ListColorPlate[index].status == Status.LockCoin)
            {
                ListColorPlate[index].isLocked = true;
                ListColorPlate[index].txtPointUnlock.gameObject.SetActive(true);
                ListColorPlate[index].pointToUnLock = colorPlateData.listSpecialData[i].pointUnlock;
                ListColorPlate[index].txtPointUnlock.text = ListColorPlate[index].pointToUnLock.ToString();
            }
        }

        for (int i = 0; i < colorPlateData.listExistedData.Count; i++)
        {
            int index = colorPlateData.listExistedData[i].Row * cols + colorPlateData.listExistedData[i].Col;
            ListColorPlate[index].Init(GetColorNew);
            ListColorPlate[index].InitColor();

        }

        for (int i = 0; i < colorPlateData.listArrowData.Count; i++)
        {
            int index = colorPlateData.listArrowData[i].Row * cols + colorPlateData.listArrowData[i].Col;

            ListArrowPlate.Add(ListColorPlate[index]);

            if (ListColorPlate[index].isLocked)
            {
                ListColorPlate[index].anim.enabled = false;
            }
            else
            {
                ListColorPlate[index].anim.enabled = true;
            }

            ListColorPlate[index].status = (Status)colorPlateData.listArrowData[i].type;
            ListColorPlate[index].gameObject.layer = 6;
            ListColorPlate[index].logicVisual.SetDirectionArrow(ListColorPlate[index].status, ListColorPlate[index].isLocked);
        }

        for (int i = 0; i < colorPlateData.listEmptyData.Count; i++)
        {
            int index = colorPlateData.listEmptyData[i].Row * cols + colorPlateData.listEmptyData[i].Col;
            ListColorPlate[index].logicVisual.DeletePlate();
        }

        maxPoint = colorPlateData.goalScore;
        gold = colorPlateData.gold;
        pigment = colorPlateData.pigment;
    }

    #region InitNextPlate
    public void InitPlateSpawn(bool isShuffle = false)
    {
        for (int i = 0; i < listSpawnNew.Count; i++)
        {
            if (listSpawnNew[i].ListValue.Count == 0 || isShuffle)
            {
                listSpawnNew[i].Init(GetColorNew);
                listSpawnNew[i].InitColor();
            }
        }
    }
    #endregion

    void Swap(List<ColorPlate> a)
    {
        ColorPlate temp = a[0];
        a[0] = a[1];
        a[1] = temp;
    }

    void SwitchNextPlate(object e)
    {
        listNextPlate[0].transform.DOLocalMove(listNextPlate[1].transform.localPosition, 0.2f).SetEase(Ease.OutCirc);
        listNextPlate[1].transform.DOLocalMove(listNextPlate[0].transform.localPosition, 0.2f).SetEase(Ease.InCirc);
        foreach (LogicColor c in listNextPlate[0].ListColor)
        {
            c.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.2f);
        }

        foreach (LogicColor c in listNextPlate[1].ListColor)
        {
            c.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
        }

        Swap(listNextPlate);
    }

    float timerMove = -1;
    float timerClear = -1;
    float timeClick = -1;

    RaycastHit raycastHit;
    [SerializeField] float timerRun = -1;
    ColorPlate previousArrowPlate = null;
    ColorPlate previousHolder = null;
    void Update()
    {
        if (timeClick >= 0)
        {
            timeClick -= Ez.TimeMod;
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hit, 100f, layerArrow) && !isPauseGame)
                {
                    if (countMove == 0)
                    {
                        ColorPlate arrowPlate = hit.collider.GetComponent<ColorPlate>();

                        if (arrowPlate.isLocked) return;


                        if (arrowPlate != previousArrowPlate)
                        {
                            if (previousArrowPlate != null)
                            {
                                //previousArrowPlate.logicVisual.PlayNormal();
                            }

                            previousArrowPlate = arrowPlate;
                        }

                        ICheckStatus checkStatusHolder = new CheckGetHolderStatus();
                        ColorPlate holder = checkStatusHolder.CheckHolder(arrowPlate);

                        if (holder != previousHolder)
                        {
                            if (previousHolder != null)
                            {
                                if (IsInLayerMask(previousHolder.gameObject, layerArrow))
                                    previousHolder.logicVisual.PlayNormal(true);
                                else
                                    previousHolder.logicVisual.PlayNormal(false);
                            }

                            previousHolder = holder;
                        }


                        if (holder != null)
                        {
                            holder.logicVisual.PlayTarget();
                        }
                    }
                }
                else
                {
                    if (previousHolder != null)
                    {
                        if (IsInLayerMask(previousHolder.gameObject, layerArrow))
                            previousHolder.logicVisual.PlayNormal(true);
                        else
                            previousHolder.logicVisual.PlayNormal(false);
                    }
                }
            }

            if (Input.GetMouseButtonUp(0) && !isLose && !isWin && !isPauseGame)
            {
                //ManagerAudio.PlaySound(ManagerAudio.Data.soundClick);
                timeClick = .1f;
                Vector3 spawnPosition = GetMouseWorldPosition();
                clickParticlePool.Spawn(spawnPosition, true);
                OnClick();
            }
        }

        if (timerRun >= 0)
        {
            timerRun -= Ez.TimeMod;
            if (timerRun < 0)
            {
                RecursiveMerge();
            }
        }

        if (point >= maxPoint && !isWin && !isContiuneMerge && !SaveGame.Challenges)
        {
            CheckWin();
            StartCoroutine(RaiseEventWin());
        };

        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    if (countDiff < 7)
        //        countDiff++;
        //}

        if (Input.GetKeyDown(KeyCode.Z))
        {
            countSpawnSpecial = 1;
            isHadSpawnSpecial = true;
        }

        //Debug.Log("isHadSpawnSpecial: " + isHadSpawnSpecial);

        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    isLose = true;
        //    Debug.Log("You lose");
        //    StartCoroutine(RaiseEventLose());
        //}

        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    PopupEndChallenges.Show();
        //}
    }


    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cam.nearClipPlane;
        return cam.ScreenToWorldPoint(mousePos);
    }

    void RecursiveMerge()
    {
        if (listSteps.Count > 0)
        {
            isContiuneMerge = true;

            Merge(listSteps[listSteps.Count - 1].nearByColorPlate, listSteps[listSteps.Count - 1].rootColorPlate);
        }
        else
        {
            isContiuneMerge = false;

            CheckClear();

            if (isWin) return;

            ProcessRemainingPlates();

            if (listSteps.Count > 0)
            {
                RecursiveMerge();
            }
            else
            {
                colorRoot = null;
                isMergeing = false;
            }

        }

        if (!isLose)
        {
            CheckLose();
        }
    }

    void OnClick()
    {
        if (gameMode == GameMode.Play)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            // click from start to grid
            if (Physics.Raycast(ray, out var hit, 100f, layerArrow) && !isPauseGame)
            {
                if (countMove == 0)
                {
                    ColorPlate arrowPlate = hit.collider.GetComponent<ColorPlate>();

                    if (arrowPlate.isLocked) return;

                    ICheckStatus checkStatusHolder = new CheckGetHolderStatus();
                    ColorPlate holder = checkStatusHolder.CheckHolder(arrowPlate);

                    arrowPlate.PlayAnimOnClick();
                    ManagerAudio.PlaySound(ManagerAudio.Data.soundArrowButton);

                    if (holder != null)
                    {

                        SetColor(arrowPlate, holder);

                        if (previousHolder != null)
                        {
                            if (IsInLayerMask(previousHolder.gameObject, layerArrow))
                                previousHolder.logicVisual.PlayNormal(true);
                            else
                                previousHolder.logicVisual.PlayNormal(false);
                        }
                    }


                    if (isHadSpawnSpecial)
                    {
                        countMove = 0;
                        ManagerEvent.RaiseEvent(EventCMD.EVENT_COUNT, countMove);
                        //SetColorIntoStartPlate(listSpawnNew[0], listNextPlate[0]);
                        Debug.Log("Play Effect Has Special");
                        //StartCoroutine(homeInGame.PlayEffectSpecial());
                        listNextPlate[0].SpawnSpecialColor(GetColorNew);
                        Vector3 spawnPos = listNextPlate[0].transform.position;
                        specialParticlePool.Spawn(spawnPos, true);
                        isHadSpawnSpecial = false;
                    }
                    else
                    {
                        countMove = UnityEngine.Random.Range(2, 4);
                        ManagerEvent.RaiseEvent(EventCMD.EVENT_COUNT, countMove);
                    }
                }
            }


            // click from spawn to start
            if (Physics.Raycast(ray, out var hitPlate, 100f, layerPlateSpawn) && !isPauseGame)
            {
                ColorPlate plateSpawn = hitPlate.collider.GetComponent<ColorPlate>();
                if (plateSpawn.ListValue.Count == 0) return;

                if (countMove > 0)
                {
                    SetColorIntoStartPlate(plateSpawn, listNextPlate[0]);
                }
            }

            // using Item Hammer
            if (isUsingHammer)
            {
                if (Physics.Raycast(ray, out var plate, 100f, layerUsingItem))
                {
                    ColorPlate plateSelect = plate.collider.GetComponent<ColorPlate>();

                    if (plateSelect.ListValue.Count == 0) return;

                    //animation effect hammer here... done ==> ()
                    // boool
                    plateSelect.Init(GetColorNew);
                    //homeInGame.ExitUsingItem();
                }
            }
        }
    }

    void SetColorIntoStartPlate(ColorPlate startColorPlate, ColorPlate endColorPlate)
    {
        countMove--;
        ManagerEvent.RaiseEvent(EventCMD.EVENT_COUNT, countMove);

        endColorPlate.listTypes.AddRange(startColorPlate.listTypes);

        int count = startColorPlate.listTypes[startColorPlate.listTypes.Count - 1].listPlates.Count;

        startColorPlate.listTypes.Clear();

        Sequence sq = DOTween.Sequence();


        for (int i = count - 1; i >= 0; i--)
        {
            startColorPlate.TopColor.transform.SetParent(endColorPlate.transform);
            startColorPlate.TopColor.transform.localScale = Vector3.one;
            endColorPlate.ListValue.Add(startColorPlate.TopValue);
            endColorPlate.ListColor.Add(startColorPlate.TopColor);

            startColorPlate.ListValue.RemoveAt(startColorPlate.ListValue.Count - 1);
            startColorPlate.ListColor.RemoveAt(startColorPlate.ListColor.Count - 1);

            if (endColorPlate.Col == startColorPlate.Col && endColorPlate.Row > startColorPlate.Row) timerMove = endColorPlate.InitValue(endColorPlate.transform, false, 0) / 10;
            else if (endColorPlate.Col < startColorPlate.Col && endColorPlate.Row == startColorPlate.Row) timerMove = endColorPlate.InitValue(endColorPlate.transform, false, 1) / 10;
            else if (endColorPlate.Col > startColorPlate.Col && endColorPlate.Row == startColorPlate.Row) timerMove = endColorPlate.InitValue(endColorPlate.transform, false, 2) / 10;
            else timerMove = endColorPlate.InitValue(endColorPlate.transform, false, 3) / 10;
        }
        sq.AppendInterval(0.3f);

        sq.AppendCallback(() =>
        {
            endColorPlate.PlayAnimScale();
        });


        if (endColorPlate.listTypes.Count >= 2)
        {
            if (endColorPlate.listTypes[endColorPlate.listTypes.Count - 1].type == endColorPlate.listTypes[endColorPlate.listTypes.Count - 2].type)
            {
                //Debug.Log("same type. MERGE");
                endColorPlate.listTypes[endColorPlate.listTypes.Count - 1].listPlates.AddRange(endColorPlate.listTypes[endColorPlate.listTypes.Count - 2].listPlates);
                endColorPlate.listTypes.RemoveAt(endColorPlate.listTypes.Count - 2);
            }
            else
            {
                //Debug.Log("not same type. DONT NEED MERGE");
            }
        }
        else
        {
            //Debug.Log("Only 1 type");
        }
        StartCoroutine(WaitForInitNextPlateSpawn(startColorPlate));
    }
    IEnumerator WaitForInitNextPlateSpawn(ColorPlate colorPlate)
    {
        yield return new WaitForSeconds(1f);
        InitPlateSpawn(false);
        //colorPlate.Init(GetColorNew);
        //colorPlate.InitColor();

        foreach (LogicColor c in colorPlate.ListColor)
        {
            c.transform.DOLocalJump(c.transform.localPosition, 2, 1, 0.3f);
        }
    }
    void SetColor(ColorPlate startColorPlate, ColorPlate endColorPlate)
    {
        if (startColorPlate.ListValue.Count == 0)
        {
            if (listNextPlate[0].ListValue.Count == 0) return;

            foreach (LogicColor renderer in listNextPlate[0].ListColor)
            {
                renderer.transform.SetParent(startColorPlate.transform);
                renderer.transform.localPosition = new Vector3(0, renderer.transform.localPosition.y, 0);
                renderer.transform.localRotation = Quaternion.identity;
                renderer.transform.localScale = Vector3.one;
            }

            startColorPlate.ListValue.AddRange(listNextPlate[0].ListValue);
            startColorPlate.ListColor.AddRange(listNextPlate[0].ListColor);
            startColorPlate.listTypes.AddRange(listNextPlate[0].listTypes);

            startColorPlate.InitValue(startColorPlate.transform);
            listNextPlate[0].ListValue.Clear();
            listNextPlate[0].ListColor.Clear();
            listNextPlate[0].listTypes.Clear();

            float delay = 0f;

            DG.Tweening.Sequence sq = DOTween.Sequence();
            foreach (LogicColor renderer in startColorPlate.ListColor)
            {
                renderer.transform.SetParent(endColorPlate.transform);
                sq.Insert(delay, renderer.transform.DOLocalMove(new Vector3(0, renderer.transform.localPosition.y, 0), 0.4f).SetEase(curveMove));
                renderer.transform.localRotation = Quaternion.identity;
                renderer.transform.localScale = Vector3.one;
                delay += 0.01f;
            }

            //for (int i = startColorPlate.ListColor.Count - 1; i >= 0; i--)
            //{
            //    LogicColor renderer = startColorPlate.ListColor[i];
            //    renderer.transform.SetParent(endColorPlate.transform);
            //    sq.Insert(delay, renderer.transform.DOLocalMove(new Vector3(0, renderer.transform.localPosition.y, 0), 1f).SetEase(curveMove));
            //    renderer.transform.localRotation = Quaternion.identity;
            //    renderer.transform.localScale = Vector3.one;
            //    delay += 0.2f;
            //}


            List<ColorEnum> listValueMid = new List<ColorEnum>();
            List<LogicColor> ListColorMid = new List<LogicColor>();
            List<GroupEnum> listTypes = new List<GroupEnum>();

            listValueMid.AddRange(startColorPlate.ListValue);
            ListColorMid.AddRange(startColorPlate.ListColor);
            listTypes.AddRange(startColorPlate.listTypes);


            startColorPlate.ListValue.Clear();
            startColorPlate.ListColor.Clear();
            startColorPlate.listTypes.Clear();


            endColorPlate.ListValue.AddRange(listValueMid);
            endColorPlate.ListColor.AddRange(ListColorMid);
            endColorPlate.listTypes.AddRange(listTypes);


            sq.OnComplete(() =>
            {
                if ((int)endColorPlate.TopValue != (int)ColorEnum.Random)
                {
                    if (!isMergeing)
                    {
                        List<ColorPlate> listDataConnect = new List<ColorPlate>();
                        CheckNearByRecursive(listDataConnect, endColorPlate);

                        if (listDataConnect.Count <= 1)
                        {
                            if (!isLose) CheckLose();
                        }
                        else
                        {
                            FindTarget findTarget = new FindTarget();
                            if (colorRoot == null) colorRoot = findTarget.FindTargetRoot(listDataConnect);
                            Debug.Log("listDataConnect.Count: " + listDataConnect.Count);
                            Debug.Log(" Color Root:" + colorRoot.name);

                            HashSet<ColorPlate> processedNearBy = new HashSet<ColorPlate>();
                            HashSet<ColorPlate> processedRoot = new HashSet<ColorPlate>();
                            AddStepRecursivelyOtherRoot(colorRoot, listDataConnect, processedRoot, processedNearBy);
                            RecursiveMerge();
                        }
                    }
                }
                else
                {
                    FindColorEnum findColorEnum = new FindColorEnum();
                    ColorEnum cEnum = findColorEnum.FindTargetColorEnum(endColorPlate.ListConnect);

                    endColorPlate.Init(GetColorNew);
                    endColorPlate.ChangeSpecialColorPLate(cEnum);
                    RecursiveMerge();
                }
            });
        }
    }

    public void SetColorUsingSwapItem(ColorPlate startColorPlate, ColorPlate endColorPlate)
    {
        if (endColorPlate.ListValue.Count == 0)
        {
            for (int i = 0; i < startColorPlate.ListColor.Count; i++)
            {
                LogicColor c = startColorPlate.ListColor[i];
                c.transform.SetParent(endColorPlate.transform);
                c.transform.localPosition = new Vector3(0, 0.2f + (i + 1) * GameConfig.OFFSET_PLATE, 0);
                c.transform.localRotation = Quaternion.identity;
                c.transform.localScale = Vector3.one;
            }

            List<ColorEnum> listValueMid = new List<ColorEnum>();
            List<LogicColor> ListColorMid = new List<LogicColor>();
            List<GroupEnum> listTypes = new List<GroupEnum>();

            listValueMid.AddRange(startColorPlate.ListValue);
            ListColorMid.AddRange(startColorPlate.ListColor);
            listTypes.AddRange(startColorPlate.listTypes);


            startColorPlate.ListValue.Clear();
            startColorPlate.ListColor.Clear();
            startColorPlate.listTypes.Clear();


            endColorPlate.ListValue.AddRange(listValueMid);
            endColorPlate.ListColor.AddRange(ListColorMid);
            endColorPlate.listTypes.AddRange(listTypes);
        }
        else
        {
            for (int i = 0; i < startColorPlate.ListColor.Count; i++)
            {
                LogicColor c = startColorPlate.ListColor[i];
                c.transform.SetParent(endColorPlate.transform);
                c.transform.localPosition = new Vector3(0, 0.2f + (i + 1) * GameConfig.OFFSET_PLATE, 0);
                c.transform.localRotation = Quaternion.identity;
                c.transform.localScale = Vector3.one;
            }

            for (int i = 0; i < endColorPlate.ListColor.Count; i++)
            {
                LogicColor c = endColorPlate.ListColor[i];
                c.transform.SetParent(startColorPlate.transform);
                c.transform.localPosition = new Vector3(0, 0.2f + (i + 1) * GameConfig.OFFSET_PLATE, 0);
                c.transform.localRotation = Quaternion.identity;
                c.transform.localScale = Vector3.one;
            }

            List<ColorEnum> listValueMid = new List<ColorEnum>();
            List<LogicColor> ListColorMid = new List<LogicColor>();
            List<GroupEnum> listTypes = new List<GroupEnum>();

            listValueMid.AddRange(startColorPlate.ListValue);
            ListColorMid.AddRange(startColorPlate.ListColor);
            listTypes.AddRange(startColorPlate.listTypes);

            startColorPlate.ListValue.Clear();
            startColorPlate.ListColor.Clear();
            startColorPlate.listTypes.Clear();

            startColorPlate.ListValue.AddRange(endColorPlate.ListValue);
            startColorPlate.ListColor.AddRange(endColorPlate.ListColor);
            startColorPlate.listTypes.AddRange(endColorPlate.listTypes);

            endColorPlate.ListValue.Clear();
            endColorPlate.ListColor.Clear();
            endColorPlate.listTypes.Clear();

            endColorPlate.ListValue.AddRange(listValueMid);
            endColorPlate.ListColor.AddRange(ListColorMid);
            endColorPlate.listTypes.AddRange(listTypes);
        }
    }

    void ProcessRemainingPlates()
    {
        colorRoot = null;
        isMergeing = false;
        Debug.Log(" _________________________________________ ");
        foreach (ColorPlate c in ListColorPlate)
        {
            if (c.ListValue.Count == 0 || c.status == Status.Empty || c.countFrozen != 0) continue;
            List<ColorPlate> listDataConnect = new List<ColorPlate>();
            CheckNearByRecursive(listDataConnect, c);
            if (listDataConnect.Count <= 1) continue;

            FindTarget findTarget = new FindTarget();
            if (colorRoot == null) colorRoot = findTarget.FindTargetRoot(listDataConnect);
            Debug.Log("Root : " + colorRoot.name);

            HashSet<ColorPlate> processedNearBy = new HashSet<ColorPlate>();
            HashSet<ColorPlate> processedRoot = new HashSet<ColorPlate>();
            AddStepRecursivelyOtherRoot(colorRoot, listDataConnect, processedRoot, processedNearBy);
            break;
        }
    }

    public List<Step> listSteps = new List<Step>();
    public void AddStepRecursively(ColorPlate colorRoot, List<ColorPlate> listDataConnect, HashSet<ColorPlate> processedNearBy)
    {
        ColorPlate colorNearBy = new Step().ColorNearByColorRoot(colorRoot, listDataConnect, processedNearBy);
        if (colorNearBy == null || processedNearBy.Contains(colorNearBy))
        {
            return;
        }

        processedNearBy.Add(colorNearBy);
        processedNearBy.Add(colorRoot);

        listSteps.Add(new Step
        {
            rootColorPlate = colorRoot,
            nearByColorPlate = colorNearBy
        });

        AddStepRecursively(colorRoot, listDataConnect, processedNearBy);
    }
    public void AddStepRecursivelyOtherRoot(ColorPlate colorRoot, List<ColorPlate> listDataConnect, HashSet<ColorPlate> processedRoot, HashSet<ColorPlate> processedNearBy)
    {
        if (processedRoot.Contains(colorRoot))
        {
            return;
        }
        processedRoot.Add(colorRoot);

        foreach (var p in processedRoot.ToList())
        {
            AddStepRecursively(p, listDataConnect, processedNearBy);
        }

        foreach (var p in processedNearBy.ToList())
        {
            AddStepRecursivelyOtherRoot(p, listDataConnect, processedRoot, processedNearBy);
        }
    }

    public List<ColorPlate> listNearByCanConnect = new List<ColorPlate>();
    public void CheckNearByRecursive(List<ColorPlate> listDataConnect, ColorPlate colorPlate)
    {
        List<ColorPlate> listNearBySame = colorPlate.CheckNearByCanConnect(/*colorPlate*/);

        foreach (ColorPlate c in listNearBySame)
        {
            if (c.ListValue.Count == 0 || c.countFrozen != 0) continue;

            if (c.TopValue == colorPlate.TopValue)
            {
                if (!listDataConnect.Contains(colorPlate))
                {
                    listDataConnect.Add(colorPlate);

                }
                if (!listDataConnect.Contains(c))
                {
                    listDataConnect.Add(c);
                    CheckNearByRecursive(listDataConnect, c);
                }
            }
        }
    }
    void CheckClear()
    {
        foreach (ColorPlate colorPlate in ListColorPlate)
        {
            if (colorPlate.listTypes.Count <= 0 || colorPlate.status == Status.Empty) continue;
            int count = colorPlate.listTypes[colorPlate.listTypes.Count - 1].listPlates.Count;
            if (count < RULE_COMPLETE) continue;

            if (count >= RULE_COMPLETE)
            {
                colorPlate.InitClear();
                colorPlate.DecreaseCountFrozenNearBy();
                colorPlate.InitValue();
                //ManagerEvent.RaiseEvent(EventCMD.EVENT_POINT, count);
            }
        }
        Debug.Log("point: " + point);
        //IncreaseCountDiff();
    }

    public void IncreaseCountDiff()
    {
        if (point >= 20) countDiff = 3;
        if (point >= 50) countDiff = 4;
        if (point >= 100) countDiff = 5;
        if (point >= 150) countDiff = 6;
        if (point >= 180) countDiff = 7;

        if (countDiff > countDiffMax) countDiff = countDiffMax;
    }

    public void SpawnSpecialColor()
    {
        if (point >= pointSpawnSpecial)
        {
            Debug.Log("spawn special color");
            isHadSpawnSpecial = true;
            pointSpawnSpecial += 50;
        }
    }

    public void ExecuteLockCoin(int point)
    {
        foreach (ColorPlate c in ListColorPlate)
        {
            if (c.status == Status.Empty) continue;
            c.UnlockedLockCoin(point);
        }
    }

    public GameObject obj;
    void Merge(ColorPlate startColorPlate, ColorPlate endColorPlate)
    {
        timerRun = 0;
        isMergeing = true;
        for (int i = listSteps.Count - 1; i >= 0; i--)
        {
            Debug.Log(listSteps[i].nearByColorPlate + " to " + listSteps[i].rootColorPlate);
        }
        int count = startColorPlate.listTypes[startColorPlate.listTypes.Count - 1].listPlates.Count;
        Sequence sequence = DOTween.Sequence();

        for (int i = count - 1; i >= 0; i--)
        {
            sequence.AppendCallback(() =>
            {
                if (startColorPlate.TopValue == endColorPlate.TopValue)
                {
                    startColorPlate.TopColor.transform.SetParent(endColorPlate.transform);

                    ManagerAudio.PlaySound(ManagerAudio.Data.soundMerge);

                    endColorPlate.listTypes[endColorPlate.listTypes.Count - 1].listPlates.Add(startColorPlate.TopValue);
                    startColorPlate.listTypes[startColorPlate.listTypes.Count - 1].listPlates.Remove(startColorPlate.TopValue);

                    startColorPlate.ClearLastType();

                    endColorPlate.ListValue.Add(startColorPlate.TopValue);
                    endColorPlate.ListColor.Add(startColorPlate.TopColor);

                    startColorPlate.ListValue.RemoveAt(startColorPlate.ListValue.Count - 1);
                    startColorPlate.ListColor.RemoveAt(startColorPlate.ListColor.Count - 1);

                    if (endColorPlate.Col == startColorPlate.Col && endColorPlate.Row > startColorPlate.Row)
                        timerMove = endColorPlate.InitValue(endColorPlate.transform, false, 0) / 10;
                    else if (endColorPlate.Col < startColorPlate.Col && endColorPlate.Row == startColorPlate.Row)
                        timerMove = endColorPlate.InitValue(endColorPlate.transform, false, 1) / 10;
                    else if (endColorPlate.Col > startColorPlate.Col && endColorPlate.Row == startColorPlate.Row)
                        timerMove = endColorPlate.InitValue(endColorPlate.transform, false, 2) / 10;
                    else timerMove = endColorPlate.InitValue(endColorPlate.transform, false, 3) / 10;
                }
            });


            sequence.AppendInterval(0.08f);
            timerRun += 0.13f;
        }
        sequence.Play();

        if (listSteps.Count > 0) listSteps.RemoveAt(listSteps.Count - 1);
    }
    void CheckLose()
    {
        bool allPlaced = true;

        for (int i = 0; i < ListArrowPlate.Count; i++)
        {
            if (ListArrowPlate[i].ListValue.Count == 0)
            {
                allPlaced = false;
                break;
            }
        }

        if (allPlaced && !isMergeing && !isLose)
        {
            isLose = true;
            Debug.Log("You lose");
            StartCoroutine(RaiseEventLose());
        }
    }
    IEnumerator RaiseEventLose()
    {
        yield return new WaitForSeconds(2f);
        if (!SaveGame.Challenges)
            ManagerEvent.RaiseEvent(EventCMD.EVENT_LOSE);
        else ManagerEvent.RaiseEvent(EventCMD.EVENT_CHALLENGES);
    }
    void CheckWin()
    {
        isWin = true;
        Debug.Log(point + " __ " + gold + " __ " + pigment);
        Debug.Log("check win");
        SaveGame.Level++;
        foreach (ColorPlate c in ListColorPlate)
        {
            if (c.ListValue.Count == 0 || c.status == Status.Empty) continue;

            c.ClearAll();
        }
    }
    IEnumerator RaiseEventWin()
    {
        yield return new WaitForSeconds(1.5f);
        ManagerEvent.RaiseEvent(EventCMD.EVENT_WIN);
    }
    public bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return ((layerMask.value & (1 << obj.layer)) > 0);
    }

}