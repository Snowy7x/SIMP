using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassResetter : MonoBehaviour
{
    public bool reset = true;
    private float mass = 0;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mass = rb.mass;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (reset)
            rb.mass = mass;
    }
}
