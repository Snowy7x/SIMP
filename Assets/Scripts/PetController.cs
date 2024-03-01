using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetController : MonoBehaviour
{
    public Transform player; //drag and stop player object in the inspector
    public Transform target; //drag and stop player object in the inspector
    public float speed;
    public float smoothTime = 0.3F;
    public Vector3 offset;
    public Vector3 velocity;

    private void Update()
    {
        try
        {
            float dis = Vector3.Distance(transform.position, target.position);
            if (dis > 0.1f)
            {
                velocity = (target.position + offset) - transform.position;
                // move towards player
                transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref velocity,
                    speed * Time.deltaTime);
                transform.localScale = player.localScale;
            }
        }
        catch (Exception e)
        {
            Destroy(gameObject);
        }
        
    }
}
