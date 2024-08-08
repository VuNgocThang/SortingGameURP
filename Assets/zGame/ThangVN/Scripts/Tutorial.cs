using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject imgCoverTut;
    public List<GameObject> listSteps;
    public int currentIndex;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if (SaveGame.Level == 0 && !SaveGame.IsDoneTutorial)
        {
            imgCoverTut.SetActive(true);

            PlayProgressTut(0);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (currentIndex < listSteps.Count - 1)
            {
                currentIndex++;
                PlayProgressTut(currentIndex);
            }
        }
    }

    public void PlayProgressTut(int index)
    {
        for (int i = 0; i < listSteps.Count; i++)
        {
            listSteps[i].SetActive(false);
        }

        listSteps[index].SetActive(true);
    }
}
