using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class Fun_Effect : MonoBehaviour
{
    public List<Rigidbody> fun1ObjList;
    public List<Transform> madeTarget1;

    public float effectTimer = 7f;
    float curTimer = 0f;

    float effectInterval = 1f;

    bool isEffecting = false;

    // Start is called before the first frame update
    void Start()
    {
        curTimer = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isEffecting == false)
        {
            curTimer += Time.deltaTime;
            Debug.Log("curTimer: " + curTimer);
            if (curTimer > effectTimer)
            {
                StartCoroutine(effector1());
                isEffecting = true;
                curTimer = 0f;
            }
        }
    }

    IEnumerator effector1()
    {
        for (int i = 0; i < fun1ObjList.Count; i++)
        {
            fun1ObjList[i].isKinematic = true;
            fun1ObjList[i].transform.DOMove(madeTarget1[i].position, 0.7f).SetEase(Ease.InOutBack);
            fun1ObjList[i].transform.DORotate(madeTarget1[i].rotation.eulerAngles, 0.7f).SetEase(Ease.InOutBack);

            yield return new WaitForSeconds(effectInterval);
        }

        for (int i = 0; i < fun1ObjList.Count; i++)
        {
            fun1ObjList[i].isKinematic = false;
            Vector3 randForce = new Vector3(Random.Range(-50000f, 50000f), Random.Range(20000f, 70000f), 0f);
            fun1ObjList[i].AddForce(randForce, ForceMode.Impulse);
        }

        isEffecting = false;
        curTimer = 0f;
    }
}
