using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class AppManager : MonoBehaviour
{
    public static bool IsSettingOn = false;

    public DOTweenAnimation tween_SettingPopup;

    public static SystemLanguage deviceLanguage = SystemLanguage.English;

    private void Awake()
    {
        DOTween.SetTweensCapacity(500, 50);
        Application.targetFrameRate = 60;

        deviceLanguage = Application.systemLanguage;
    }

    public void OnClickSettingButton()
    {
        tween_SettingPopup.DOPlayForward();
        IsSettingOn = true;
    }

    public void OnClickSettingExitButton()
    {
        tween_SettingPopup.DOPlayBackwards();
        IsSettingOn = false;
    }
}
