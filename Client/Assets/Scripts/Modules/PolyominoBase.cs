using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using System;

using static GameStatics;

public class PolyominoBase : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public PolyominoType polyominoType;
    public List<Block> blocks;
    public GameObject blocksRoot;

    public PolyominoRot Rotation = PolyominoRot.Rot0;

    public bool IsGrabable = false;

    // Define from blocks[0] - (0, 0)
    public List<relativePosIndex> ShapeByRelative0;
    public List<relativePosIndex> ShapeByRelative90;
    public List<relativePosIndex> ShapeByRelative180;
    public List<relativePosIndex> ShapeByRelative270;

    public List<List<relativePosIndex>> AllRelativePosIndicesList = new List<List<relativePosIndex>>();

    [System.Serializable]
    public struct relativePosIndex
    {
        public int x;
        public int y;
    }

    #region PC Only

    public static Action<PolyominoBase, PointerEventData> onPointerEnter;
    public static Action<PolyominoBase, PointerEventData> onPointerExit;

    #endregion

    public static Action<PolyominoBase, PointerEventData> onPointerClick;
    public static Action<PolyominoBase, PointerEventData> onPointerDown;
    public static Action<PolyominoBase, PointerEventData> onPointerUp;

    private void Start()
    {
        AllRelativePosIndicesList.Clear();
        AllRelativePosIndicesList.Add(ShapeByRelative0);
        AllRelativePosIndicesList.Add(ShapeByRelative90);
        AllRelativePosIndicesList.Add(ShapeByRelative180);
        AllRelativePosIndicesList.Add(ShapeByRelative270);

    }

    public List<relativePosIndex> GetRelativePosIndices()
    {
        switch (Rotation)
        {
            case PolyominoRot.Rot0:
                return ShapeByRelative0;
            case PolyominoRot.Rot90:
                return ShapeByRelative90;
            case PolyominoRot.Rot180:
                return ShapeByRelative180;
            case PolyominoRot.Rot270:
                return ShapeByRelative270;
            default:
                return null;
        }
    }


    public void RotateShape()
    {
        float rot = this.transform.localRotation.eulerAngles.z;
        rot += 90f;
        rot %= 360f;
        this.transform.localRotation = Quaternion.Euler(0f, 0f, rot);

        Rotation = GetRotation();
    }

    public PolyominoRot GetRotation()
    {
        if (Mathf.Approximately(this.transform.localRotation.eulerAngles.z, 0f)) return PolyominoRot.Rot0;
        else if (Mathf.Approximately(this.transform.localRotation.eulerAngles.z, 90f)) return PolyominoRot.Rot90;
        else if (Mathf.Approximately(this.transform.localRotation.eulerAngles.z, 180f)) return PolyominoRot.Rot180;
        else if (Mathf.Approximately(this.transform.localRotation.eulerAngles.z, 270f)) return PolyominoRot.Rot270;

        return PolyominoRot.Rot0;
    }

    public GameObject GetRootGO()
    {
        return transform.parent.gameObject;
    }

    #region Click Handler
    public void OnPointerClick(PointerEventData eventData)
    {
        onPointerClick?.Invoke(this, eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onPointerDown?.Invoke(this, eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onPointerUp?.Invoke(this, eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter?.Invoke(this, eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExit?.Invoke(this, eventData);
    }

    #endregion

}
