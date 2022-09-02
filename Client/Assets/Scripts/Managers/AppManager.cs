using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
}
