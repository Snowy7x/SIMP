using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorObject : MonoBehaviour
{
    public void Rotate(int dir = 1)
    {
        transform.Rotate(0, 0, 90 * dir);
    }
    
    public void Move(int dir = 1){
        transform.position = new Vector3(transform.position.x + 2 * dir, transform.position.y, transform.position.z);
    }
}
