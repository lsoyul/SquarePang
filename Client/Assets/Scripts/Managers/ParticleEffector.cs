using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffector : MonoBehaviour
{
    public List<GameObject> effectList;
    static Dictionary<string, GameObject> effectDic = new Dictionary<string, GameObject>();

    public enum EffectName
    {
        Crack,
    }

    private void Awake()
    {
        foreach (GameObject effectGO in effectList)
        {
            effectDic.Add(effectGO.name, effectGO);
        }
    }


    public static void PlayEffect(EffectName effectName, Vector3 worldPos, Quaternion rotation)
    {
        string effectKey = effectName.ToString();
        if (effectDic.ContainsKey(effectKey))
        {
            GameObject newEffect = Instantiate(effectDic[effectKey], worldPos, rotation);

            newEffect.GetComponent<ParticleSystem>().Play();
        }
    }
}
