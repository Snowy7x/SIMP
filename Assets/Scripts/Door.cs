using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    [SerializeField] Collider2D _collider2D;
    private Animator _animator;
    public bool startOpened = false;
    private static readonly int Opened = Animator.StringToHash("Opened");
    
    public bool disableBottomCollision = false;
    public GameObject[] checkers;
    
    public UnityEvent onOpen;
    public UnityEvent onClose;
    private bool canDo = true;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        canDo = false;
        if (startOpened)
        {
            if (_animator.GetBool(Opened))
            {
                canDo = true;
            }
            else
            {
                Open();
            }
        }
        else
        {
            if (!_animator.GetBool(Opened))
            {
                canDo = true;
            }
            else
            {
                Close();
            }
        }

        foreach (GameObject checker in checkers)
        {
            checker.SetActive(disableBottomCollision);
        }
    }
    
    public void Open()
    {
        if (canDo)
        {
            AudioManager.Instance.Play("open", _audioSource);
        }
        _collider2D.enabled = false;
        _animator.SetBool(Opened, true);
    }
    
    public void Close()
    {
        if (canDo)
        {
            AudioManager.Instance.Play("close", _audioSource);
        }
        _collider2D.enabled = true;
        _animator.SetBool(Opened, false);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //_collider2D.isTrigger = false;
        if (disableBottomCollision)
            Physics2D.IgnoreCollision(_collider2D, _collider2D, false);
    }

    public void OnClose()
    {
        if (!canDo)
        {
            canDo = true;
            return;
        }
        onClose?.Invoke();
    }

    public void OnOpen()
    {
        if (!canDo)
        {
            canDo = true;
            return;
        }
        onOpen?.Invoke();
    }
    
}
