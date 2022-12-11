using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using DG.Tweening;

public class BlockEffector : MonoBehaviour
{
    public static BlockEffector Instance;

    public static bool isEffectTime = false;

    public static Action<List<BlockSlot>, PolyominoBase> onStartReleasePolyomino;
    public static Action onEndReleasePolyomino;

    public static Action<List<List<BlockSlot>>> onStartMadeSquareEffect;
    public static Action<List<List<BlockSlot>>> onEndMadeSquareEffect;

    [Header("== Release On Board ==")]
    public AnimationCurve ReleaseOnBoard_MoveToPos;
    public float ReleaseOnBoard_MoveToPos_Duration = 0.5f;
    public AnimationCurve ReleaseOnBoard_FallDown;
    public float ReleaseOnBoard_FallDown_Duration = 0.5f;

    [Header("== Made Square Effect ==")]
    public Light ingameDirectionalLight;
    public AnimationCurve MadeSquareEffect;
    public float MadeSquareEffect_Duration = 0.4f;
    public float MadeSquareEffect_TargetEmission = 2f;
    public float MadeSquareEffect_DefaultLightIntensity = 1.5f;

    IEnumerator releaseIE;
    IEnumerator madeSquareIE;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void StartReleasePolyomino(List<BlockSlot> realSlots, List<BlockSlot> fakeSlots, GameObject polyominoForEffect)
    {
        PolyominoBase targetPolyomino = polyominoForEffect.GetComponent<PolyominoBase>();
        if (targetPolyomino == null) return;

        if (releaseIE != null) StopCoroutine(releaseIE);

        releaseIE = IEReleasePolyomino(realSlots, fakeSlots, targetPolyomino);

        StartCoroutine(releaseIE);
    }

    IEnumerator IEReleasePolyomino(List<BlockSlot> realSlots, List<BlockSlot> fakeSlots, PolyominoBase effectObj)
    {
        SPSoundManager.PlayEffect(SPSoundManager.Sound_EFFECT.MoveStart);
        isEffectTime = true;
        onStartReleasePolyomino?.Invoke(realSlots, effectObj);

        float timer = 0f;

        List<Block> effectBlocks = effectObj.blocks;
        List<Vector3> moveTarget = new List<Vector3>();
        List<Vector3> fallTarget = new List<Vector3>();

        foreach (BlockSlot slot in realSlots)
        {
            if (slot.curBlock != null) slot.curBlock.gameObject.SetActive(false);

            moveTarget.Add(new Vector3(slot.AttachRoot.transform.position.x, slot.AttachRoot.transform.position.y, effectObj.transform.position.z));
            fallTarget.Add(slot.AttachRoot.transform.position);
        }

        foreach (BlockSlot fakeSlot in fakeSlots)
        {
            fakeSlot.gameObject.SetActive(true);
        }

        while (timer <= ReleaseOnBoard_MoveToPos_Duration)
        {
            timer += Time.deltaTime;

            for (int i = 0; i < realSlots.Count; i++)
            {
                effectBlocks[i].transform.position =
                    Vector3.Lerp(effectBlocks[i].transform.position, moveTarget[i], ReleaseOnBoard_MoveToPos.Evaluate(timer / ReleaseOnBoard_MoveToPos_Duration));
            }

            yield return null;
        }

        for (int i = 0; i < realSlots.Count; i++)
        {
            effectBlocks[i].transform.position = moveTarget[i];
        }

        yield return null;

        timer = 0f;

        while (timer <= ReleaseOnBoard_FallDown_Duration)
        {
            timer += Time.deltaTime;

            for (int i = 0; i < realSlots.Count; i++)
            {
                effectBlocks[i].transform.position =
                    Vector3.Lerp(effectBlocks[i].transform.position, fallTarget[i], ReleaseOnBoard_FallDown.Evaluate(timer / ReleaseOnBoard_FallDown_Duration));
            }

            yield return null;
        }

        for (int i = 0; i < realSlots.Count; i++)
        {
            effectBlocks[i].transform.position = fallTarget[i];
        }

        yield return null;

        Destroy(effectObj.gameObject);

        bool isCrack = false;
        for (int i = 0; i < realSlots.Count; i++)
        {
            if (realSlots[i].curBlock != null)
            {
                realSlots[i].curBlock.gameObject.SetActive(true);
                realSlots[i].ShakeBoard();
            }

            if (realSlots[i].curBlock == null && fakeSlots[i].curBlock != null)
            {
                isCrack = true;
                // Block Crash!
                ParticleEffector.PlayEffect(ParticleEffector.EffectName.Crack, realSlots[i].AttachRoot.transform.position, Quaternion.identity);
            }
        }

        if (isCrack) SPSoundManager.PlayEffect(SPSoundManager.Sound_EFFECT.StompBreaker);
        else SPSoundManager.PlayEffect(SPSoundManager.Sound_EFFECT.StompBoard);

        foreach (BlockSlot fakeSlot in fakeSlots)
        {
            fakeSlot.gameObject.SetActive(false);
            Destroy(fakeSlot.gameObject);
        }

        isEffectTime = false;
        onEndReleasePolyomino?.Invoke();
    }

    public void StartMadeSquareEffect(List<List<BlockSlot>> madeSlotList)
    {
        if (madeSquareIE != null) StopCoroutine(madeSquareIE);
        madeSquareIE = IEMadeSquareEffect(madeSlotList);

        StartCoroutine(madeSquareIE);
    }

    IEnumerator IEMadeSquareEffect(List<List<BlockSlot>> madeSlotList)
    {
        isEffectTime = true;
        onStartMadeSquareEffect?.Invoke(madeSlotList);

        foreach (List<BlockSlot> slotList in madeSlotList)
        {
            foreach (BlockSlot slot in slotList)
            {
                if (slot.curBlock != null)
                {
                    DOTweenAnimation tweener = slot.curBlock.GetBlockObj().GetComponent<DOTweenAnimation>();
                    if (tweener != null)
                    {
                        tweener.duration = MadeSquareEffect_Duration;
                        tweener.DORestart();
                    }

                }
            }
        }

        SPSoundManager.PlayEffect(SPSoundManager.Sound_EFFECT.MadeStart);

        float timer = 0f;

        while (timer <= MadeSquareEffect_Duration)
        {
            timer += Time.deltaTime;

            foreach (List<BlockSlot> slotList in madeSlotList)
            {
                foreach (BlockSlot slot in slotList)
                {
                    if (slot.curBlock != null)
                    {
                        slot.curBlock.SetEmission(MadeSquareEffect_TargetEmission * MadeSquareEffect.Evaluate(timer / MadeSquareEffect_Duration));
                    }
                }
            }
            // 1 -> 0
            // 1.5f --> 0.75f
            float intensityRatio = 1f - MadeSquareEffect.Evaluate(timer / MadeSquareEffect_Duration);
            intensityRatio = MadeSquareEffect_DefaultLightIntensity / 2f * intensityRatio;

            ingameDirectionalLight.intensity = MadeSquareEffect_DefaultLightIntensity / 2f + intensityRatio;

            yield return null;
        }

        SPSoundManager.Sound_EFFECT madeSoundType  = SPSoundManager.Sound_EFFECT.MadeFinish1;
        foreach (List<BlockSlot> slotList in madeSlotList)
        {
            foreach (BlockSlot slot in slotList)
            {
                if (slot.curBlock != null)
                {
                    slot.curBlock.SetEmission(MadeSquareEffect_TargetEmission);

                    ParticleEffector.PlayEffect(ParticleEffector.EffectName.BlockMade, slot.curBlock.transform.position, Quaternion.identity);
                }
            }

            // 3 4 5 6 7
            switch (slotList.Count)
            {
                case 9:
                case 16:
                    madeSoundType = SPSoundManager.Sound_EFFECT.MadeFinish1;
                    break;
                case 25:
                    madeSoundType = SPSoundManager.Sound_EFFECT.MadeFinish2;
                    break;
                case 36:
                    madeSoundType = SPSoundManager.Sound_EFFECT.MadeFinish3;
                    break;
                case 49:
                    madeSoundType = SPSoundManager.Sound_EFFECT.MadeFinish4;
                    break;

                default:
                    break;
            }

            SPSoundManager.PlayEffect(madeSoundType);
        }

        ingameDirectionalLight.intensity = MadeSquareEffect_DefaultLightIntensity;

        isEffectTime = false;
        onEndMadeSquareEffect?.Invoke(madeSlotList);
    }
}
