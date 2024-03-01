using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] private int leftCam;
    [SerializeField] private int rightCam;
    [SerializeField] private bool isFollow;
    
    [SerializeField] private UnityEvent onEnterRyt;
    [SerializeField] private UnityEvent onEnterLft;
    [SerializeField] private UnityEvent onExitRyt;
    [SerializeField] private UnityEvent onExitLft;

    public bool isUpDown = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isFollow)
            {
                if (!isUpDown)
                {
                    if (other.transform.position.x < transform.position.x)
                    {
                        onEnterRyt?.Invoke();
                        CameraController.Instance.ChangeCamera(leftCam);
                    }

                    if (other.transform.position.x > transform.position.x)
                    {
                        onEnterLft?.Invoke();
                        CameraController.Instance.ChangeCamera(rightCam);
                    }
                }
                else
                {
                    if (other.transform.position.y < transform.position.y)
                    {
                        CameraController.Instance.ChangeCamera(leftCam);
                    }
                    if (other.transform.position.y > transform.position.y)
                    {
                        CameraController.Instance.ChangeCamera(rightCam);
                    }
                }
            }
            else
            {
                CameraController.Instance.Follow(other.transform);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isFollow)
            {
                CameraController.Instance.Follow(other.transform);
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isFollow)
            {
                if (!isUpDown)
                {
                    if (other.transform.position.x < transform.position.x)
                    {
                        CameraController.Instance.ChangeCamera(leftCam);
                    }

                    if (other.transform.position.x > transform.position.x)
                    {
                        CameraController.Instance.ChangeCamera(rightCam);
                    }
                }
                else
                {
                    if (other.transform.position.y < transform.position.y)
                    {
                        onExitRyt?.Invoke();
                        CameraController.Instance.ChangeCamera(leftCam);
                    }
                    if (other.transform.position.y > transform.position.y)
                    {
                        onExitLft?.Invoke();
                        CameraController.Instance.ChangeCamera(rightCam);
                    }
                }
            }
            
        }
    }
}
