using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Fun_forcer : MonoBehaviour, IPointerDownHandler
{
    public Rigidbody rb;

    private float force = 80000f;

    public void OnPointerDown(PointerEventData eventData)
    {
        rb.AddForce(Vector3.up * force, ForceMode.Impulse);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
