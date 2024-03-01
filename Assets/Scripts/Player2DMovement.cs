using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2DMovement : IDamageable
{
    [Header("Player Settings")]
    private float horizontal;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpingPower = 16f;
    private bool isFacingRight = true;
    private bool isJumping;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private Animator splashAnimation;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject landDust;
    [SerializeField] private Transform landDustPosition;
    [SerializeField] private GameObject deathParticles;

    public bool canMove = true;

    void Update()
    {

        if (Mathf.Abs(rb.velocity.x) > 0.2 && IsGrounded())
        {
            CinemachineShake.Instance.ShakeCamera(0.5f, .1f, 0.05f);
            anim.SetBool("Moving", true);
        }
        else
        {
            anim.SetBool("Moving", false);
        }
        //Attack();

        horizontal = InputManager.GetInstance().GetMoveDirection().x;

        if (InputManager.GetInstance().GetJumpPressed() && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            isJumping = true;
        }

        if (InputManager.GetInstance().GetJumpPressed() && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        
        if (!IsGrounded())
        {
            isJumping = true;
        }

        Flip();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Attacking...");
            splashAnimation.gameObject.SetActive(true);
            anim.Play("Attack");
            CinemachineShake.Instance.ShakeCamera(2, .1f, 1);
            splashAnimation.Play("Slash");
        }
    }

    private void Move()
    {
        if (!canMove) return;
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts.Length > 0)
        {
            
                ContactPoint2D contact = collision.contacts[0];
                if (Vector3.Dot(contact.normal, Vector3.up) > 0.5)
                {
                    //collision was from below
                    if (isJumping)
                    {
                        isJumping = false;
                        GameObject particles = Instantiate(landDust, landDustPosition.position, Quaternion.identity);
                        Destroy(particles, 2f);
                        CinemachineShake.Instance.ShakeCamera(2, .1f, 0.1f);
                    }
                }
        }
    }

    public override void TakeDamage(float damage)
    {
        health -= damage;
        StartCoroutine(TookDamage());
        CinemachineShake.Instance.ShakeCamera(3, .1f, 2);
        if (!(health <= 0)) return;
        health = 0;
        Die();
    }

    IEnumerator TookDamage()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
        yield return new WaitForSeconds(.1f);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    public override void Die(bool isInstant = true)
    {
        canMove = false;
        rb.velocity = Vector2.zero;
        if (isInstant)
        {
            anim.Play("Death2");
            CinemachineShake.Instance.ShakeCamera(3, .2f, 2);
        }
        else
        {
            StartCoroutine(Death());
        }

    }

    public void DeathParticles()
    {
        Destroy(gameObject);
        GameObject particles = Instantiate(deathParticles, transform.position, Quaternion.identity);
        Destroy(particles, 2f);
    }

    IEnumerator Death()
    {
        anim.Play("Death");
        CinemachineShake.Instance.ShakeCamera(3, 1.2f, 2);
        yield return new WaitForSeconds(1f);
        DeathParticles();
    }
}