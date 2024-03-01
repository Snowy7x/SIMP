using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour
{
    public int currentSWlevel = 0;
    public int maxSWLevel;
    public Sprite[] switchSprites;
    public SpriteRenderer spriteRenderer;
    
    [SerializeField] UnityEvent switchPosUnityEvent;
    [SerializeField] UnityEvent switchNegUnityEvent;
    
    private bool _inRange = false;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = switchSprites[currentSWlevel];
        
        maxSWLevel = switchSprites.Length - 1;
        // Setup initial switch level
        if (currentSWlevel < 0)
        {
            currentSWlevel = 0;
        }
        if (currentSWlevel > (maxSWLevel - 1))
        {
            currentSWlevel = maxSWLevel - 1;
        }
        spriteRenderer.sprite = switchSprites[currentSWlevel];
    }

    private void Update()
    {
        if (_inRange)
        {
            if (InputManager.GetInstance().GetSwitchRight())
            {
                SwitchLevel();
            }
            if (InputManager.GetInstance().GetSwitchLeft())
            {
                SwitchLevel(-1);
            }
        }
    }

    public void SwitchLevel(int dir = 1)
    {
        currentSWlevel += dir;
        if (dir > 0 && currentSWlevel <= maxSWLevel)
        {
            switchPosUnityEvent?.Invoke();
        }
        else if (dir < 0 && currentSWlevel >= 0)
        {
            switchNegUnityEvent?.Invoke();
        }
        if (currentSWlevel < 0)
        {
            currentSWlevel = 0;
        }
        if (currentSWlevel > maxSWLevel)
        {
            currentSWlevel = maxSWLevel;
        }
        AudioManager.Instance.Play("Tick", _audioSource);
        spriteRenderer.sprite = switchSprites[currentSWlevel];
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _inRange = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("OnTriggerStay2D");
        if (other.CompareTag("Player"))
        {
            _inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _inRange = false;
        }
    }
}
