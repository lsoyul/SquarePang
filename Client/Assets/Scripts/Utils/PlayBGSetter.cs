using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBGSetter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (var renderer in transform.GetComponentsInChildren<MeshRenderer>())
        {
            renderer.sortingOrder = -5;
        }   
    }
}
