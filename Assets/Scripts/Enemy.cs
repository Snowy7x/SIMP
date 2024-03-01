    using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
    using UnityEngine.Events;

    public class Enemy : IDamageable
{
    [Header("Enemy Settings")]
    public float speed;
    public float damage;
    public float dirCooldown = 0.2f;
    
    public GameObject DeathEffect;
    /*
    public GameObject HitEffect;
    */

    public bool IsDead;
    public bool damageOnHit;
    
    private Animator animator;
    private Rigidbody2D rb;
    private int direction;
    private bool canMove = true;
    private bool isFacingRight = true;
    private float dirCooldownTimer;
    public bool canChange = true;

    public bool isDialogueAfterDeath = false;
    public TextAsset dialogAfterDeath;
    public UnityEvent onDialogueStart;
    public UnityEvent onDialogueEnd;
    public UnityEvent onDeath;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        direction = 1;
        dirCooldownTimer = 0;
    }

    private void Update()
    {
        if (dirCooldownTimer > 0)
        {
            canChange = false;
            dirCooldownTimer -= Time.deltaTime;
        }
        else
        {
            canChange = true;
        }
    }


    private void FixedUpdate()
    {
        if (IsDead)
            return;
        Move();
        Flip();
    }

    // Move in one direction
    private void Move()
    {
        if (!canMove || IsDead) return;
        rb.velocity = new Vector2(speed * direction, rb.velocity.y);
    }
    
    public override void TakeDamage(float dam)
    {
        health -= dam;
        if (!(health <= 0)) return;
        health = 0;
        Die();
    }

    public override void Die(bool isInstant = true)
    {
        if (IsDead) return;
        canMove = false;
        IsDead = true;
        rb.velocity = Vector2.zero;
        onDeath?.Invoke();
        if (isDialogueAfterDeath)
        {
            DialogueManager.GetInstance().EnterDialogueMode(dialogAfterDeath, onDialogueStart, onDialogueEnd);
        }
        if (isInstant)
        {
            animator.Play("Death");
            Instantiate(DeathEffect, transform.position, Quaternion.identity);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsDead)
            return;
        if (collision.gameObject.CompareTag("Player"))
        {
            if (damageOnHit)
            {
                collision.gameObject.GetComponent<Player2DMovement>().TakeDamage(damage);
            }
        }
        else if (collision.gameObject.CompareTag("Ground") && canChange)
        {
            Flip();
            direction *= -1;
            dirCooldownTimer = dirCooldown;
        }
    }
    
    private void Flip()
    {
        if (isFacingRight && direction < 0f || !isFacingRight && direction > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
