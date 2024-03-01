using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeEvent : MonoBehaviour
{
    public float time;
    public bool isOnce = false;
    
    private bool _isDone = false;
    public UnityEvent onTime;
    
    public void DoTheThing()
    {
        if (_isDone) return;
        StartCoroutine(TimeEventCoroutine());
    }
    
    private IEnumerator TimeEventCoroutine()
    {
        yield return new WaitForSeconds(time);
        onTime.Invoke();
        _isDone = true;
    }
}
