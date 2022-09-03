using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class AppManager : MonoBehaviour
{
    bool isSettingOn = false;

    public DOTweenAnimation tween_SettingPopup;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    public void OnClickSettingButton()
    {
        tween_SettingPopup.DOPlayForward();
        isSettingOn = true;
    }

    public void OnClickSettingExitButton()
    {
        tween_SettingPopup.DOPlayBackwards();
        isSettingOn = false;
    }
}
