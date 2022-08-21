using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_Firebase : MonoBehaviour
{
    public GameObject gamePage;
    private bool initOn = false;

    // Start is called before the first frame update
    void Awake()
    {
        FB.onReadyFireBase += OnReadyFireBase;
    }

    private void OnDestroy()
    {
        FB.onReadyFireBase -= OnReadyFireBase;
    }

    private void Update()
    {
        if (initOn && gamePage.activeSelf == false) gamePage.SetActive(true);
    }

    void OnReadyFireBase(Firebase.FirebaseApp app)
    {
        Debug.Log("OnReadyFirebase: " + app.Name);
        initOn = true;
    }
}
