using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera gameCamera;
    public static Camera GameCamera;

    private void Awake()
    {
        if (gameCamera != null && GameCamera == null)
        {
            GameCamera = gameCamera;
        }
    }
}
