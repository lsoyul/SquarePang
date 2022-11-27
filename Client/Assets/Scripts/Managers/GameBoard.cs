using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using static GameStatics;

/*
 * Block size : 1 unity unit
 */

public class GameBoard : MonoBehaviour
{
    public GameObject slotRoot;
    public BlockSlot blockSlotBase;
    public GameObject backGround;
    
    public Canvas UIcanvas;

    public static int CurBoardWidth = BoardStatics.BOARD_WIDTH_MIN;
    public static int CurBoardHeight = BoardStatics.BOARD_HEIGHT_MIN;

    public static int ResultScore = 0;
    public static int RemainBreakerCount = 0;
    public const int RemainBreakerMaxCount = 15;

    public static bool useBreakerScoreMultiply = true;

    public static Action<int, int> onChangeScore;   // getScore, totalScore
    public static Action onInitBoard;
    public static Action<List<List<BlockSlot>>, int, bool> onMadeSquare;  // madelist, breakerCount, isCleanBoard
    public static Action<GameEndType> onGameOver;
    public static Action onReviveGameBoard;

    public static GameMode CurGameMode = GameMode.Sprint;

    public static int SprintModeMaxTargetSquareCount = 10;
    public static int sprintModeCurMadeSquareCount = 0;

    public static float ValidReleaseMaxDist = 0f;
    public static float BoardScreenCoordBottomY = 0f;
    public static float BoardScreenCoordTopY = 0f;
    public static float BoardScreenWidthHeight = 1500f;

    public const int ReviveMaxCount = 1;
    public static int ReviveRemainCount = ReviveMaxCount;

    // =============== private
    private static List<List<BlockSlot>> blockSlots = new List<List<BlockSlot>>();
    private static List<int> matchSquareSizes = new List<int>();


    [Header("====TEST====")]
    public int boardWidth = 7;
    public int boardHeight = 7;

    public float boardXOffset = 0.5f;
    public float boardYOffset = 1f;

    [ContextMenu("InitGameBoardBase")]
    public void TESTInitGameBoardBase()
    {
        InitGameBoardBase(boardWidth, boardHeight);
    }

    private void Awake()
    {
        BlockControl.onFinishReleasePolyomino += OnFinishReleasePolyomino;
        BlockControl.onSuccessReleaseOnGameBoard += OnSuccessReleaseOnGameBoard;

        AdsManager.onEarnedByRewardAd += OnEarnedByRewardAd;
    }

    private void OnDestroy()
    {
        BlockControl.onFinishReleasePolyomino -= OnFinishReleasePolyomino;
        BlockControl.onSuccessReleaseOnGameBoard -= OnSuccessReleaseOnGameBoard;

        AdsManager.onEarnedByRewardAd -= OnEarnedByRewardAd;
    }

    private void Start()
    {
        matchSquareSizes.Add(3);
        matchSquareSizes.Add(4);
        matchSquareSizes.Add(5);
        matchSquareSizes.Add(6);
        matchSquareSizes.Add(7);
    }

    void OnSuccessReleaseOnGameBoard(List<BlockSlot> releaseSlotList, int putCount, int breakCount)
    {
        int curScore = 0;
        curScore += putCount;
        curScore += breakCount * 4;

        ResultScore += curScore;

        onChangeScore(curScore, ResultScore);
    }

    void OnFinishReleasePolyomino()
    {
        List<List<BlockSlot>> madeSlotList = GetMatchSqares();

        int curScore = 0;
        foreach (List<BlockSlot> madeSlots in madeSlotList)
        {
            foreach (BlockSlot blockSlot in madeSlots)
            {
                // Scoring!
                blockSlot.RemoveBlock();
                curScore++;
            }
        }

        if (useBreakerScoreMultiply) curScore *= (RemainBreakerCount + 1);

        bool isCleanBoard = IsCleanBoard();

        if (isCleanBoard) curScore *= 2;

        ResultScore += curScore;

        if (madeSlotList.Count > 0)
        {
            sprintModeCurMadeSquareCount += madeSlotList.Count;

            onMadeSquare?.Invoke(madeSlotList, RemainBreakerCount, isCleanBoard);

            RemainBreakerCount = 0;
            //RemainBreakerCount = GetAfterBreakerCount(madeSlotList);
            //if (useBreakerScoreMultiply) RemainBreakerCount = 1;
        }


        onChangeScore?.Invoke(curScore, ResultScore);

        // Check Game Finish (if sprintMode)
        if (CurGameMode == GameMode.Sprint)
        {
            if (sprintModeCurMadeSquareCount >= SprintModeMaxTargetSquareCount)
            {
                onGameOver?.Invoke(GameEndType.SprintFinish);
            }
        }

        // Check Game Over
        if (IsGameOver() == true) onGameOver?.Invoke(GameEndType.GameOver);
    }

    public static List<List<BlockSlot>> GetBlockSlotsOnBoard()
    {
        return blockSlots;
    }

    bool IsCleanBoard()
    {
        foreach (List<BlockSlot> slots in blockSlots)
        {
            foreach (BlockSlot slot in slots)
            {
                if (slot.curBlock != null) return false;
            }
        }

        return true;
    }


    bool IsGameOver()
    {
        List<PolyominoBase> possiblePolyominos = new List<PolyominoBase>();

        // 1. Add First polyomino
        possiblePolyominos.Add(BlockControl.NextBlocks[0].GetComponentInChildren<PolyominoBase>());

        // 2. Add Second
        if (BlockControl.StashBlock != null) possiblePolyominos.Add(BlockControl.StashBlock.GetComponentInChildren<PolyominoBase>());
        else possiblePolyominos.Add(BlockControl.NextBlocks[1].GetComponentInChildren<PolyominoBase>());


        for (int i = 0; i < blockSlots.Count; i++)
        {
            for (int j = 0; j < blockSlots[i].Count; j++)
            {
                foreach (PolyominoBase polyomino in possiblePolyominos)
                {
                    foreach (var relativeIndices in polyomino.AllRelativePosIndicesList)
                    {
                        List<BlockSlot> fitSlots = GetFitSlotList(blockSlots[i][j], relativeIndices);

                        if (fitSlots != null)
                        {
                            int emptySlotcount = 0;
                            foreach (BlockSlot slot in fitSlots)
                            {
                                if (slot.curBlock == null) emptySlotcount++;
                            }

                            if (emptySlotcount + RemainBreakerCount >= polyomino.blocks.Count) return false;
                        }
                    }

                }
            }
        }


        return true;
    }

    int GetAfterBreakerCount(List<List<BlockSlot>> madeSlotList)
    {
        int res = 0;
        foreach (List<BlockSlot> slotList in madeSlotList)
        {
            switch (slotList.Count)
            {
                case 9:
                    res += 1;
                    break;
                case 16:
                    res += 2;
                    break;
                case 25:
                    res += 3;
                    break;
                case 36:
                    res += 4;
                    break;
                case 49:
                    res += 5;
                    break;
                default:
                    break;
            }
        }

        return res;
    }

    public float backGroundScaleOffset = 0.5f;
    public float backGroundZOffset = 0f;

    public void InitGameBoardBase(int boardWidth, int boardHeight)
    {
        sprintModeCurMadeSquareCount = 0;
        ResultScore = 0;
        RemainBreakerCount = 0;
        ReviveRemainCount = ReviveMaxCount;

        CurBoardWidth = boardWidth;
        CurBoardHeight = boardHeight;

        backGround.transform.localScale = new Vector3(boardWidth + backGroundScaleOffset, boardHeight + backGroundScaleOffset);
        backGround.transform.localPosition = new Vector3(0, boardYOffset + 0.5f, backGroundZOffset);
        backGround.SetActive(true);

        if (blockSlots.Count > 0) ClearGameBoardBase();

        blockSlots = new List<List<BlockSlot>>();

        for (int i = 0; i < boardHeight; i++)
        {
            List<BlockSlot> newBlockSlotList = new List<BlockSlot>();

            for (int j = 0; j < boardWidth; j++)
            {
                BlockSlot newBlockSlot = Instantiate(blockSlotBase, slotRoot.transform);
                newBlockSlot.name = string.Format("Slot{0}.{1}", i, j);
                float posX = j - boardWidth / 2f + boardXOffset;
                float posY = -(i - boardHeight / 2f)  + boardYOffset;
                newBlockSlot.transform.localPosition = new Vector3(posX, posY, 0f);
                newBlockSlot.InitBlockSlot(j, i);

                newBlockSlotList.Add(newBlockSlot);
            }

            blockSlots.Add(newBlockSlotList);
        }

        Vector3 firstColBlockVertexPos = blockSlots[0][0].transform.position + Vector3.left * 0.5f;
        Vector3 lastColBlockVertexPos = blockSlots[0][blockSlots[0].Count - 1].transform.position + Vector3.right * 0.5f;

        float canvasWidthSize = UIcanvas.GetComponent<RectTransform>().rect.width;
        BoardScreenWidthHeight = Vector2.Distance(CameraManager.GameCamera.WorldToScreenPoint(firstColBlockVertexPos), CameraManager.GameCamera.WorldToScreenPoint(lastColBlockVertexPos));

        float widthRatio = BoardScreenWidthHeight / CameraManager.GameCamera.pixelWidth;

        BoardScreenWidthHeight = canvasWidthSize * widthRatio;

        ValidReleaseMaxDist = Vector2.Distance(CameraManager.GameCamera.WorldToScreenPoint(blockSlots[0][1].transform.position), CameraManager.GameCamera.WorldToScreenPoint(blockSlots[0][2].transform.position));

        BlockSlot lastSlot = blockSlots[blockSlots.Count - 1][blockSlots[blockSlots.Count - 1].Count - 1];
        Vector3 targetWorldPos = lastSlot.transform.position + Vector3.down * 1.3f;
        Vector2 lastSlotScreenPos = CameraManager.GameCamera.WorldToScreenPoint(targetWorldPos);

        BlockSlot firstSlot = blockSlots[0][0];
        Vector3 firstSlotWorldPos = firstSlot.transform.position + Vector3.up * 0.6f;
        Vector2 firstSlotScreenPos = CameraManager.GameCamera.WorldToScreenPoint(firstSlotWorldPos);

        BoardScreenCoordBottomY = lastSlotScreenPos.y;
        BoardScreenCoordTopY = firstSlotScreenPos.y;

        onChangeScore?.Invoke(0, ResultScore);
        onInitBoard?.Invoke();
    }

    [ContextMenu("ClearGameBoardBase")]
    public void ClearGameBoardBase()
    {
        if (blockSlots == null) return;

        foreach (List<BlockSlot> blockSlotList in blockSlots)
        {
            foreach (BlockSlot blockSlot in blockSlotList)
            {
                Destroy(blockSlot.gameObject);
            }
            blockSlotList.Clear();
        }
        blockSlots.Clear();
    }

    public static List<BlockSlot> GetOnBoardSlots(PolyominoBase polyomino)
    {
        BlockSlot firstOnSlot = IsThisPosOnSlot(polyomino.blocks[0].transform.position);
        if (firstOnSlot != null)
        {
            //Debug.Log("FirstOnSlot: " + firstOnSlot.iy + "." + firstOnSlot.ix);
            return GetFitSlotList(firstOnSlot, polyomino.GetRelativePosIndices());
        }

        return null;
    }

    static BlockSlot IsThisPosOnSlot(Vector3 targetWPos)
    {
        Vector2 targetSreenPos = Camera.main.WorldToScreenPoint(targetWPos);

        BlockSlot targetSlot = null;

        foreach (var widthList in blockSlots)
        {
            float minDist = 9999f;
            foreach (BlockSlot slot in widthList)
            {
                float dist = Vector2.Distance(Camera.main.WorldToScreenPoint(slot.transform.position), targetSreenPos);
                if (dist < minDist && dist < ValidReleaseMaxDist/2f)
                {
                    minDist = dist;
                    targetSlot = slot;
                }
            }
        }

        return targetSlot;
    }

    static List<BlockSlot> GetFitSlotList(BlockSlot firstOnSlot, List<PolyominoBase.relativePosIndex> relativePosList)
    {
        int pivotX = firstOnSlot.ix;
        int pivotY = firstOnSlot.iy;

        List<BlockSlot> resultSlots = new List<BlockSlot>();

        foreach (PolyominoBase.relativePosIndex relativePos in relativePosList)
        {
            int addedX = pivotX + relativePos.x;
            int addedY = pivotY - relativePos.y;

            // If Over the Board
            if (addedX < 0 || addedX >= CurBoardWidth) return null;
            if (addedY < 0 || addedY >= CurBoardHeight) return null;

            resultSlots.Add(blockSlots[addedY][addedX]);
        }

        return resultSlots;
    }

    public static List<List<BlockSlot>> GetMatchSqares()
    {
        List<List<BlockSlot>> madeSlotList = new List<List<BlockSlot>>();
        foreach (int squareSize in matchSquareSizes)
        {
            for (int y = 0; y < blockSlots.Count; y++)
            {
                if (y + squareSize > blockSlots.Count) continue;

                for (int x = 0; x < blockSlots[y].Count; x++)
                {
                    if (x + squareSize > blockSlots[y].Count) continue;

                    // Check Square from (x, y)

                    List<BlockSlot> madeSlots = GetMatchSlots(x, y, squareSize);
                    if (madeSlots != null)
                    {
                        madeSlotList.Add(madeSlots);
                    }
                }
            }
        }

        return madeSlotList;
    }

    static List<BlockSlot> GetMatchSlots(int x, int y, int squareSize)
    {
        List<BlockSlot> madeSlots = new List<BlockSlot>();
        for (int checkY = y; checkY < y+squareSize; checkY++)
        {
            for (int checkX = x; checkX < x+squareSize; checkX++)
            {
                if (blockSlots[checkY][checkX].curBlock == null) return null;

                madeSlots.Add(blockSlots[checkY][checkX]);
            }
        }

        if (CheckEdgeOfSquare(x, y, squareSize) == true) return madeSlots;
        else return null;

    }

    static bool CheckEdgeOfSquare(int x, int y, int squareSize)
    {
        // Above of Square
        for (int checkX = x; checkX < x + squareSize; checkX++)
        {
            if (y - 1 < 0) break;

            if (blockSlots[y - 1][checkX].curBlock != null)
            {
                Debug.Log("==Above Fail");
                return false;
            }
        }

        // Below of Square
        for (int checkX = x; checkX < x + squareSize; checkX++)
        {
            if (y + squareSize + 1 > blockSlots.Count) break;

            if (blockSlots[y + squareSize][checkX].curBlock != null)
            {
                Debug.Log("==Below Fail");
                return false;
            }
        }

        // Left of Square
        for (int checkY = y; checkY < y + squareSize; checkY++)
        {
            if (x - 1 < 0) break;

            if (blockSlots[checkY][x - 1].curBlock != null)
            {
                Debug.Log("==Left Fail");
                return false;
            }
        }

        // Right of Square
        for (int checkY = y; checkY < y + squareSize; checkY++)
        {
            if (x + squareSize + 1 > blockSlots[y].Count) break;

            if (blockSlots[checkY][x + squareSize].curBlock != null)
            {
                Debug.Log("==Right Fail");
                return false;
            }
        }

        return true;
    }


    public void ShowAdsToRevive()
    {
        if (ReviveRemainCount > 0)
        {
            ReviveRemainCount--;
            AdsManager.Instance.ShowAds_Rewarded_AfterGameOver();
        }
    }

    void OnEarnedByRewardAd()
    {
        foreach (List<BlockSlot> slotList in blockSlots)
        {
            foreach (BlockSlot slot in slotList)
            {
                slot.RemoveBlock();
            }
        }

        RemainBreakerCount = 0;

        onReviveGameBoard?.Invoke();
    }
}
