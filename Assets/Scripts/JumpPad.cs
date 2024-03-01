using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    private List<Rigidbody2D> _rbs = new List<Rigidbody2D>();
    public Animator animator;
    
    public float jumpForce = 10f;
    public Sprite[] TimerSprites;
    public SpriteRenderer TimerSpriteRenderer;
    public bool isActive = false;
    public bool canPush = false;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PushUp()
    {
        if (isActive) return;
        AudioManager.Instance.Play("jumppad", _audioSource);
        isActive = true;
        TimerSpriteRenderer.sprite = TimerSprites[0];
        StartCoroutine(TimerCoroutine());
    }
    
    private void DoPushUp()
    {
        if (canPush)
        {
            animator.Play("Push");
            foreach (Rigidbody2D rb in _rbs)
            {
                rb.mass = 1f;
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
            canPush = true;
        }
    }
    
    IEnumerator TimerCoroutine()
    {
        foreach (var sprite in TimerSprites)
        {
            TimerSpriteRenderer.sprite = sprite;
            yield return new WaitForSeconds(0.5f);
        }
        isActive = false;
        canPush = true;
        DoPushUp();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Entered");
        if (col.gameObject.GetComponent<Rigidbody2D>())
        {
            _rbs.Add(col.gameObject.GetComponent<Rigidbody2D>());
        }
    }
    
    private void OnCollisionExit2D(Collision2D col)
    {
        Debug.Log("Exited");
        if (col.gameObject.GetComponent<Rigidbody2D>())
        {
            _rbs.Remove(col.gameObject.GetComponent<Rigidbody2D>());
        }
    }
}
