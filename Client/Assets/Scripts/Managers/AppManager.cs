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
        if (isSettingOn == false) tween_SettingPopup.DOPlayForward();
        else tween_SettingPopup.DOPlayBackwards();
        

        isSettingOn = !isSettingOn;

    }
}
