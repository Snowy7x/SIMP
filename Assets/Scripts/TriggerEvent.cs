using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    public bool isOnce = false;
    
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;
    public UnityEvent onTriggerStay;
    
    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnter.Invoke();
        if (isOnce)
        {
            Destroy(this);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        onTriggerExit.Invoke();
        if (isOnce)
        {
            Destroy(this);
        }
    }
    
    
    private void OnTriggerStay(Collider other)
    {
        onTriggerStay.Invoke();
        if (isOnce)
        {
            Destroy(this);
        }
    }
}
