using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PageControl : MonoBehaviour
{
    public GameObject TitlePage;
    public GameObject GamePagee;

    bool isInit = false;

    void Awake()
    {
        FB.onReadyFireBase += OnReadyFireBase;
    }

    private void OnDestroy()
    {
        FB.onReadyFireBase -= OnReadyFireBase;
    }

    void OnReadyFireBase(Firebase.FirebaseApp app)
    {
        isInit = true;
    }

    public void OnClickGameStart()
    {
        TitlePage.SetActive(false);
        GamePagee.SetActive(true);
    }
}
