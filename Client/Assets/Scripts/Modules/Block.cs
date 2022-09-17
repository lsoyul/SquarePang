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

    public GameObject HardBlocks;
    public GameObject SoftBlocks;
    public GameObject RoundBlocks;
    public GameObject PaperBlocks;

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


        hardMatRenderer = HardBlocks.GetComponent<Renderer>();
        softMatRenderer = SoftBlocks.GetComponent<Renderer>();
        paperMatRenderer = PaperBlocks.GetComponent<Renderer>();
        roundMatRenderer = RoundBlocks.GetComponent<Renderer>();

        baseMatColor = hardMatRenderer.material.color;
    }

    public void SetBlockVisual(Visual targetVisual)
    {
        CurVisual = targetVisual;

        HardBlocks.SetActive(false);
        
        SoftBlocks.SetActive(false);
        
        RoundBlocks.SetActive(false);
        
        PaperBlocks.SetActive(false);
        

        switch (targetVisual)
        {
            case Visual.Hard:
                HardBlocks.SetActive(true);
                break;
            case Visual.Soft:
                SoftBlocks.SetActive(true);
                break;
            case Visual.Round:
                RoundBlocks.SetActive(true);
                break;
            case Visual.Paper:
                PaperBlocks.SetActive(true);
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
        hardMatRenderer.material.SetVector("_EmissionColor", baseColor * emissionValue);
    }

}
