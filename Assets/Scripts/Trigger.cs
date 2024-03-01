using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] private Sprite triggered;
    [SerializeField] private Sprite defaultSprite;
    private List<Collider2D> _colliders = new List<Collider2D>();
    public bool delayAfterFirstTrigger = false;
    private bool doneFirst = false;
    
    [SerializeField] UnityEvent onTriggerEnterFirst;
    [SerializeField] UnityEvent onTriggerEnter;
    [SerializeField] UnityEvent onTriggerExit;
    
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        defaultSprite = GetComponent<SpriteRenderer>().sprite;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        AudioManager.Instance.Play("Tick", _audioSource);
        if (!_colliders.Contains(col))
            _colliders.Add(col);
        GetComponent<SpriteRenderer>().sprite = triggered;

        if (!doneFirst)
        {
            onTriggerEnterFirst?.Invoke();
            doneFirst = true;
            if (delayAfterFirstTrigger)
                StartCoroutine(Delay());
            else
            {
                onTriggerEnter?.Invoke();
            }
        }
        else
        {
            onTriggerEnter?.Invoke();
        }
    }
    
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);
        onTriggerEnter?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (_colliders.Contains(col))
            _colliders.Remove(col);
        if (_colliders.Count == 0)
        {
            AudioManager.Instance.Play("Tick2", _audioSource);
            GetComponent<SpriteRenderer>().sprite = defaultSprite;
            onTriggerExit?.Invoke();
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        GetComponent<SpriteRenderer>().sprite = triggered;
    }
}
