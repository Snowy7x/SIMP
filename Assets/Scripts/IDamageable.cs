using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IDamageable : MonoBehaviour
{
    [Header("General Entity Settings")]
    public float health = 100f;
    public float maxHealth = 100f;
    public float armor;
    public float maxArmor;
    public float regen;
    public float maxRegen;

    public abstract void TakeDamage(float damage);

    public abstract void Die(bool isInstant = true);
}
