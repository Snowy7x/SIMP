using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckerTP : MonoBehaviour
{
    public Collider2D collider;
    //public Collider2D collider2;
    
    private void OnTriggerExit2D(Collider2D col)
    {
        //collider.isTrigger = true;
        Physics2D.IgnoreCollision(collider, col, false);

    }
}