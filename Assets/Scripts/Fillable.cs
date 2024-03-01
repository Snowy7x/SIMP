using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Fillable : MonoBehaviour
{
    public int fillLevel;
    public int maxFillLevel;
    public int fillRate;
    public float fillDelay;
    public float unFillDelay;
    public float unFilltime;
    public float time;
    
    public Sprite[] fillSprites;
    public SpriteRenderer spriteRenderer;

    public bool isOnce = false;
    private bool _isDone = false;

    public bool isFilling;
    public bool isFilled = false;
    public bool isEmpty = false;

    [SerializeField] UnityEvent onFull;
    [SerializeField] UnityEvent onEmpty;
    
    
    void Start()
    {
        unFilltime = unFillDelay;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isFilling)
        {
            if (isFilled)
            {
                isFilling = false;
                return;
            }

            if (time > 0)
            {
                time -= Time.deltaTime;
            }
            else
            {
                time = 0;
                _Fill();
            }
            isFilling = false;
        }
        else
        {
            if (isEmpty)
                return;
            time = fillDelay;
            if (unFilltime > 0)
            {
                unFilltime -= Time.deltaTime;
            }
            else
            {
                unFilltime = 0;
                unFill();
            }
        }
        
        /*if (unFilltime <= 0)
        {
            unFill();
            unFilltime = 0;
        }
        else
        {
            if (time == 0)
            {
                unFilltime -= Time.deltaTime;
            }
        }
        if (time > 0)
        {
            time -= Time.deltaTime;
            canFill = false;
        }
        else
        {
            canFill = true;
        }*/
    }

    private void unFill()
    {
        if (isFilled)
            isFilled = false;
        unFilltime = unFillDelay;
        fillLevel--;
        if (fillLevel < 0)
        {
            isEmpty = true;
            fillLevel = 0;
            onEmpty?.Invoke();
        }

        spriteRenderer.sprite = fillSprites[fillLevel];
    }

    private void _Fill()
    {
        if (isEmpty)
            isEmpty = false;
        time = fillDelay;
        fillLevel += fillRate;
        if (fillLevel > maxFillLevel)
        {
            spriteRenderer.sprite = fillSprites[fillSprites.Length - 1];
            fillLevel = maxFillLevel;
            isFilled = true;
            if (isOnce && !_isDone){
                onFull?.Invoke();
                _isDone = true;
            }else if (!isOnce){
                onFull?.Invoke();
            }
        }
        else
        {
            spriteRenderer.sprite = fillSprites[fillLevel];
            isFilled = false;
        }
    }

    public void Fill()
    {
        isFilling = true;
    }
}
