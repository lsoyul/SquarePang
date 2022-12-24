using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class Tutorial : MonoBehaviour
{
    public DOTweenAnimation tutorialTweener;
    public List<GameObject> pages;

    private int totalPageCount;
    private int curPage = 0;

    public static bool IsOnTutorial = false;

    // Start is called before the first frame update
    void Start()
    {
        if (pages != null)
        {
            totalPageCount = pages.Count;
        }

        if (AppManager.deviceLanguage == SystemLanguage.Korean)
        {

        }
        else
        {

        }
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
