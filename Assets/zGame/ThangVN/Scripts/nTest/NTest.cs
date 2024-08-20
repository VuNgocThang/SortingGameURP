using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTest : MonoBehaviour
{
    private void Start()
    {
        Test();
    }

    void Test()
    {
        Sequence sequence = DOTween.Sequence();

        //for (int i = 0; i < 10; i++)
        //{
        //    DOVirtual.DelayedCall(() =>
        //    {
        //        float delay = 0.06f * i;

        //        TestCount();
        //    });
            
        //}
    }

    void TestCount()
    {
        for (int i = 0; i < 3; i++)
        {
            Debug.Log("i: " + i);
        }
    }
}
