using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    public float damage = 10f;
    public float coolDown = 1;
    private float _timer = 0;

    private void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        _timer = coolDown;
        if (col.gameObject.GetComponent<IDamageable>())
        {
            col.gameObject.GetComponent<IDamageable>().TakeDamage(damage);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_timer <= 0)
        {
            if (other.gameObject.GetComponent<IDamageable>())
            {
                other.gameObject.GetComponent<IDamageable>().TakeDamage(damage);
            }
        }
    }
}
