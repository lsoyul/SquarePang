using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class Tutorial : MonoBehaviour
{
    public DOTweenAnimation tutorialTweener;
    public List<GameObject> pages;

    public List<GameObject> desc_kor;
    public List<GameObject> desc_eng;

    private int totalPageCount;
    private int curPage = 0;

    public static bool IsOnTutorial = false;

    void CheckHaveToShowFirstTutorial()
    {
        int watched = PlayerPrefs.GetInt("WatchedTutorial_t1", 0);
        if (watched == 0)
        {
            // Show Once
            PlayerPrefs.SetInt("WatchedTutorial_t1", 1);
            StartTutorial();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (pages != null)
        {
            totalPageCount = pages.Count;
        }

        //if (true)
        if (Application.systemLanguage == SystemLanguage.Korean)
        {
            foreach (var item in desc_eng)
            {
                item.SetActive(false);
            }

            foreach (var item in desc_kor)
            {
                item.SetActive(true);
            }
        }
        else
        {
            foreach (var item in desc_eng)
            {
                item.SetActive(true);
            }

            foreach (var item in desc_kor)
            {
                item.SetActive(false);
            }
        }

        CheckHaveToShowFirstTutorial();
    }

    public void StartTutorial()
    {
        curPage = 0;
        SetPage(curPage);
        tutorialTweener.DOPlayForward();
        IsOnTutorial = true;
    }

    public void ExitTutorial()
    {
        tutorialTweener.DOPlayBackwards();
        IsOnTutorial = false;
    }

    public void OnClickLeft()
    {
        curPage--;
        if (curPage < 0) curPage = 0;

        SetPage(curPage);
    }

    public void OnClickRight()
    {
        curPage++;
        if (curPage >= pages.Count) curPage = pages.Count - 1;

        SetPage(curPage);
    }

    void SetPage(int pageIndex)
    {
        if (pages == null) return;

        for (int i = 0; i < pages.Count; i++)
        {
            if (i == pageIndex) pages[i].SetActive(true);
            else pages[i].SetActive(false);
        }
    }

}
