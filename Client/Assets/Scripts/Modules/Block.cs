using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public GameObject HardBlock;

    Renderer hardMatRenderer;

    Color baseMatColor;

    private void Awake()
    {
        SpriteRenderer spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = 1;
        }


        hardMatRenderer = HardBlock.GetComponent<Renderer>();

        baseMatColor = hardMatRenderer.material.color;
    }

    public void SetBlockVisual()
    {
        HardBlock.SetActive(true);
    }


    public void SetBlockTransparent(float alphaValue)
    {
        if (hardMatRenderer != null)
        {
            Color hardMatColor = hardMatRenderer.material.color;
            hardMatColor.a = alphaValue;
            hardMatRenderer.material.color = hardMatColor;
        }

    }

    public void SetEmission(float emissionValue)
    {
        Color baseColor = hardMatRenderer.material.color;
        //hardMatRenderer.material.SetVector("_EmissionColor", baseColor * emissionValue);
        hardMatRenderer.material.SetVector("_EmissionColor", Color.white * emissionValue);
    }

    public GameObject GetBlockObj()
    {
        return HardBlock;
    }
}
