using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
     public enum Visual
    {
        Hard = 0,
        Soft = 1,
        Round = 2,
        Paper = 3,
        END = 4,
    }

    public GameObject HardBlock;
    public GameObject SoftBlock;
    public GameObject RoundBlock;
    public GameObject PaperBlock;

    public Visual CurVisual;


    Renderer hardMatRenderer;
    Renderer softMatRenderer;
    Renderer paperMatRenderer;
    Renderer roundMatRenderer;

    Color baseMatColor;

    private void Start()
    {
        SpriteRenderer spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = 1;
        }


        hardMatRenderer = HardBlock.GetComponent<Renderer>();
        softMatRenderer = SoftBlock.GetComponent<Renderer>();
        paperMatRenderer = PaperBlock.GetComponent<Renderer>();
        roundMatRenderer = RoundBlock.GetComponent<Renderer>();

        baseMatColor = hardMatRenderer.material.color;
    }

    public void SetBlockVisual(Visual targetVisual)
    {
        CurVisual = targetVisual;

        HardBlock.SetActive(false);
        
        SoftBlock.SetActive(false);
        
        RoundBlock.SetActive(false);
        
        PaperBlock.SetActive(false);
        

        switch (targetVisual)
        {
            case Visual.Hard:
                HardBlock.SetActive(true);
                break;
            case Visual.Soft:
                SoftBlock.SetActive(true);
                break;
            case Visual.Round:
                RoundBlock.SetActive(true);
                break;
            case Visual.Paper:
                PaperBlock.SetActive(true);
                break;
            default:
                break;
        }
    }


    public void SetBlockTransparent(float alphaValue)
    {
        Color hardMatColor = hardMatRenderer.material.color;
        hardMatColor.a = alphaValue;
        hardMatRenderer.material.color = hardMatColor;

        Color softMatColor = softMatRenderer.material.color;
        softMatColor.a = alphaValue;
        softMatRenderer.material.color = softMatColor;

        Color paperMatColor = paperMatRenderer.material.color;
        paperMatColor.a = alphaValue;
        paperMatRenderer.material.color = paperMatColor;

        Color roundMatColor = roundMatRenderer.material.color;
        roundMatColor.a = alphaValue;
        roundMatRenderer.material.color = roundMatColor;


        //hardMatRenderer.material.SetVector("_EmissionColor", hardMatColor * 10f);
    }

    public void SetEmission(float emissionValue)
    {
        Color baseColor = hardMatRenderer.material.color;
        //hardMatRenderer.material.SetVector("_EmissionColor", baseColor * emissionValue);
        hardMatRenderer.material.SetVector("_EmissionColor", Color.white * emissionValue);
    }

    public GameObject GetBlockObj()
    {
        switch (CurVisual)
        {
            case Visual.Hard: return HardBlock;
            case Visual.Paper: return PaperBlock;
            case Visual.Round: return RoundBlock;
            case Visual.Soft: return SoftBlock;
            default:
                return HardBlock;
        }
    }
}
