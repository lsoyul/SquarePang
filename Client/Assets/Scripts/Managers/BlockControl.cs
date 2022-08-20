    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using System;

public class BlockControl : MonoBehaviour
{
    public List<GameObject> polyominoBaseList;

    public static List<GameObject> NextBlocks = new List<GameObject>();

    public GameObject nextBlockRoot;
    public static GameObject StashBlock = null;
    public Transform stashBlockPos;
    public List<Transform> nextBlockPos;

    public float grabMoveLerp = 0f;

    private static List<GameObject> prepareBlocks = new List<GameObject>();


    public static Action onFinishReleasePolyomino;
    public static Action<List<BlockSlot>> onImpossiblePutBlockByBreakerCount;

    private bool isGrabbing = false;

    Vector3 moveTargetPos;
    private Vector3 grabOffsetPosition = Vector3.zero;

    public static int beforeUseBreakerCount = 0;

    private void Awake()
    {
        PointerManager.onGrabPolyomino += OnGrabPolyomino;
        PointerManager.onReleasePolyomino += OnReleasePolyomino;
        PointerManager.onMovePolyomino += OnMovePolyomino;

        GameBoard.onInitBoard += OnInitBoard;
    }

    private void OnDestroy()
    {
        PointerManager.onGrabPolyomino -= OnGrabPolyomino;
        PointerManager.onReleasePolyomino -= OnReleasePolyomino;
        PointerManager.onMovePolyomino -= OnMovePolyomino;

        GameBoard.onInitBoard -= OnInitBoard;
    }

    private void Start()
    {
        OnInitBoard();
    }

    void OnInitBoard()
    {
        InitializeBlocks();
        UpdatePolyominoPos();
    }


    void OnGrabPolyomino(PolyominoBase targetPolyomino, PointerEventData eventData)
    {
        if (PointerManager.CurGrabbingPolyomino != null)
        {
            Vector3 screenPos = eventData.position;
            screenPos.z = -(Camera.main.transform.position.z + 0.3f);   // 0.3 for duplicate

            grabOffsetPosition = Camera.main.ScreenToWorldPoint(screenPos) - targetPolyomino.transform.position;
            //grabOffsetPosition.z = 0.3f;

            moveTargetPos = Camera.main.ScreenToWorldPoint(screenPos) - grabOffsetPosition;
            //Debug.Log("==OnMovePolyomino: " + targetWorldPos);
            isGrabbing = true;
        }
    }

    void OnReleasePolyomino(PolyominoBase targetPolyomino)
    {
        if (CheckIsOnGameBoard(targetPolyomino))
        {
            SpendSelectedBlock(targetPolyomino);

        }
        else
        {
            //Debug.Log("==OnReleasePolyomino: " + targetPolyomino);
            PointerManager.CurGrabbingPolyomino.transform.localPosition = Vector3.zero;
        }

        isGrabbing = false;
        onFinishReleasePolyomino?.Invoke();
    }

    void SpendSelectedBlock(PolyominoBase selectedBlock)
    {
        NextBlocks.Remove(selectedBlock.GetRootGO());
        //NextBlocks.RemoveAt(0);
        NextBlocks.Add(prepareBlocks[0]);
        prepareBlocks.RemoveAt(0);

        UpdatePolyominoPos();

        CheckPrepareBlocksEmpty();

    }

    void CheckPrepareBlocksEmpty()
    {
        if (prepareBlocks.Count == 0)
        {
            foreach (GameObject go in polyominoBaseList)
            {
                GameObject newGO = Instantiate(go, nextBlockRoot.transform);
                newGO.SetActive(false);

                prepareBlocks.Add(newGO);
            }

            prepareBlocks.Shuffle();
        }
    }

    void OnMovePolyomino(Vector2 moveScreenPos)
    {
        if (PointerManager.CurGrabbingPolyomino != null)
        {
            Vector3 screenPos = moveScreenPos;
            screenPos.z = -(Camera.main.transform.position.z + 0.3f);
            
            moveTargetPos = Camera.main.ScreenToWorldPoint(screenPos) - grabOffsetPosition;
            //Debug.Log("==OnMovePolyomino: " + targetWorldPos);
            
        }
    }

    private void Update()
    {
        if (PointerManager.CurGrabbingPolyomino != null && isGrabbing)
            PointerManager.CurGrabbingPolyomino.transform.position = Vector3.Lerp(PointerManager.CurGrabbingPolyomino.transform.position, moveTargetPos, grabMoveLerp);
    }

    private void InitializeBlocks()
    {
        if (StashBlock != null) Destroy(StashBlock.gameObject);
        StashBlock = null;
        
        foreach (GameObject nextblock in NextBlocks)
        {
            Destroy(nextblock);
        }

        foreach (GameObject prepareBlock in prepareBlocks)
        {
            Destroy(prepareBlock);
        }

        NextBlocks.Clear();
        prepareBlocks.Clear();

        foreach (GameObject go in polyominoBaseList)
        {
            GameObject newGO = Instantiate(go, nextBlockRoot.transform);
            newGO.SetActive(false);
            NextBlocks.Add(newGO);
        }

        foreach (GameObject go in polyominoBaseList)
        {
            GameObject newGO = Instantiate(go, nextBlockRoot.transform);
            newGO.SetActive(false);

            prepareBlocks.Add(newGO);
        }

        NextBlocks.Shuffle();
        prepareBlocks.Shuffle();

    }

    public static void OnClickRotate()
    {
        NextBlocks[0].GetComponentInChildren<PolyominoBase>().RotateShape();
    }


    public void OnClickStash()
    {
        if (StashBlock == null)
        {
            StashBlock = NextBlocks[0];
            NextBlocks.RemoveAt(0);
        }
        else
        {
            GameObject t = NextBlocks[0];
            NextBlocks[0] = StashBlock;
            StashBlock = t;
        }

        UpdatePolyominoPos();
    }

    void UpdatePolyominoPos()
    {
        foreach (var go in NextBlocks)
        {
            go.GetComponentInChildren<PolyominoBase>().IsGrabable = false;
        }

        for (int i = 0; i < nextBlockPos.Count; i++)
        {
            NextBlocks[i].SetActive(true);
            NextBlocks[i].transform.position = nextBlockPos[i].position;
        }

        if (StashBlock != null)
        {
            StashBlock.transform.position = stashBlockPos.position;
            StashBlock.GetComponentInChildren<PolyominoBase>().IsGrabable = false;
        }

        NextBlocks[0].GetComponentInChildren<PolyominoBase>().IsGrabable = true;
        //NextBlocks[1].GetComponentInChildren<PolyominoBase>().IsGrabable = true;
    }



    static bool CheckIsOnGameBoard(PolyominoBase targetPolyomino)
    {
        List<BlockSlot> fitSlots = GameBoard.GetOnBoardSlots(targetPolyomino);
        if (fitSlots != null)
        {
            // Check Breaker
            bool haveToUseBreaker = false;
            int haveToUseBreakerCount = 0;
            foreach (BlockSlot blockSlot in fitSlots)
            {
                if (blockSlot.curBlock != null)
                {
                    if (GameBoard.RemainBreakerCount < 1)
                    {
                        onImpossiblePutBlockByBreakerCount?.Invoke(fitSlots);
                        return false;
                    }

                    haveToUseBreaker = true;
                    haveToUseBreakerCount++;
                }
            }

            if (haveToUseBreaker)
            {
                if (haveToUseBreakerCount > GameBoard.RemainBreakerCount) return false;
                else
                {
                    GameBoard.RemainBreakerCount -= haveToUseBreakerCount;

                    beforeUseBreakerCount = haveToUseBreakerCount;
                }
            }
            else GameBoard.RemainBreakerCount++;

            int index = 0;
            foreach (BlockSlot blockSlot in fitSlots)
            {
                blockSlot.PutBlock(targetPolyomino.blocks[index++]);
            }

            Destroy(targetPolyomino.transform.parent.gameObject);

            return true;
        }

        return false;
    }

}
