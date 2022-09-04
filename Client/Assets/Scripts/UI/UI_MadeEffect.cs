using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MadeEffect : MonoBehaviour
{
    public GameObject madeTextRoot;
    public TMPro.TMP_Text madeText;
    public TMPro.TMP_Text madeScore;
    public TMPro.TMP_Text madeBonus;

    public TMPro.TMP_Text cleanText;

    private void Awake()
    {
        madeTextRoot.SetActive(false);
        GameBoard.onMadeSquare += OnMadeSquare;
    }

    private void OnDestroy()
    {
        GameBoard.onMadeSquare -= OnMadeSquare;
    }

    void OnMadeSquare(List<List<BlockSlot>> madeSlots, int breakerCount, bool isCleanBoard)
    {
        Vector3 worldCenterPos = Vector3.zero;

        if (breakerCount > 0)
        {
            madeBonus.gameObject.SetActive(true);
            madeBonus.text = string.Format("Bonus x{0}", breakerCount + 1);
        }
        else
        {
            madeBonus.gameObject.SetActive(false);
        }
        foreach (List<BlockSlot> made in madeSlots)
        {
            madeScore.text = "+" + made.Count.ToString();
            switch (made.Count)
            {
                case 9:
                    madeText.text = "Great!";
                    break;
                case 16:
                    madeText.text = "Super!";
                    break;
                case 25:
                    madeText.text = "Fantastic!";
                    break;
                case 36:
                    madeText.text = "Splendid!";
                    break;
                case 49:
                    madeText.text = "Unbelievable!";
                    break;
            }

            worldCenterPos = Vector3.zero;
            foreach (BlockSlot slot in made)
            {
                worldCenterPos += slot.transform.position;
            }

            worldCenterPos /= made.Count;
        }



        Vector2 textPos = Camera.main.WorldToScreenPoint(worldCenterPos);

        madeTextRoot.transform.position = textPos;
        madeTextRoot.SetActive(true);
        madeText.GetComponent<TMPro.TextMeshProEffect>().Play();
        madeScore.GetComponent<TMPro.TextMeshProEffect>().Play();
        madeBonus.GetComponent<TMPro.TextMeshProEffect>().Play();

        if (isCleanBoard)
        {
            cleanText.gameObject.SetActive(true);
            cleanText.GetComponent<TMPro.TextMeshProEffect>().Play();

            StartCoroutine(DelayedDisable_Clean());
        }

        StartCoroutine(DelayedDisable());
        
    }

    IEnumerator DelayedDisable()
    {

        yield return new WaitForSeconds(1.5f);

        madeTextRoot.SetActive(false);
    }

    IEnumerator DelayedDisable_Clean()
    {
        yield return new WaitForSeconds(2f);

        cleanText.gameObject.SetActive(false);
    }
}
