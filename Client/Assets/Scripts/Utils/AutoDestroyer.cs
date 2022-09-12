using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyer : MonoBehaviour
{
    public float destroyDelay = 4f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyObj());
    }

    IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(destroyDelay);

        Destroy(this.gameObject);
    }
}
