using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using System;

public class PointerManager : MonoBehaviour
{
    public static PolyominoBase CurGrabbingPolyomino;

    public static bool isGrabbingPolyomino = false;

    public static Action<PolyominoBase, PointerEventData> onGrabPolyomino;
    public static Action<PolyominoBase> onReleasePolyomino;
    public static Action<Vector2> onMovePolyomino;

    private void Awake()
    {
        PolyominoBase.onPointerDown += OnPointerDown;
    }

    private void OnDestroy()
    {
        PolyominoBase.onPointerDown -= OnPointerDown;
    }

    void OnPointerDown(PolyominoBase targetPolyomino, PointerEventData eventData)
    {
        if (targetPolyomino.IsGrabable == false) return;
        if (BlockEffector.isEffectTime == true) return;

        CurGrabbingPolyomino = targetPolyomino;
        isGrabbingPolyomino = true;
        onGrabPolyomino?.Invoke(targetPolyomino, eventData);
    }

    private void Update()
    {

#if UNITY_STANDALONE || UNITY_EDITOR

        if (Input.GetMouseButtonUp(0))
        {
            // Release
            isGrabbingPolyomino = false;
            if (CurGrabbingPolyomino != null)
                onReleasePolyomino?.Invoke(CurGrabbingPolyomino);
            CurGrabbingPolyomino = null;
        }
#else
if (Input.touchCount < 1 && isGrabbingPolyomino)
        {
            // Release
            isGrabbingPolyomino = false;
            onReleasePolyomino?.Invoke(CurGrabbingPolyomino);
            CurGrabbingPolyomino = null;
        }
#endif

        if (isGrabbingPolyomino)
        {
            if (Input.touchCount > 0)
                onMovePolyomino?.Invoke(Input.touches[0].position);
            else
            {
                onMovePolyomino?.Invoke(Input.mousePosition);
            }
        }
    }
}
