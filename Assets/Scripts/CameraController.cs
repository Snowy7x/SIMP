using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    public Player2DMovement player;
    public Transform[] poses;
    public Transform camPos;
    public int currentPose = 0;
    public int startPos = 0;
    public float camWait = 3f;
    public bool canChange = true;
    private bool _isFollowingPlayer;

    private void Awake()
    {
        Instance = this;
        Debug.Log("CameraController Awake: startPos = " + startPos);
        ChangeCamera(startPos);
        if (!player)
            player = FindObjectOfType<Player2DMovement>();
    }

    public void ChangeCamera(int pose, bool force = false)
    {
        if (!canChange && !force) return;
        currentPose = pose;
        camPos.position = poses[pose].position;
    }

    public void ChangeCameraWait(int pose)
    {
        canChange = false;
        int oldPos = currentPose;
        player.canMove = false;
        ChangeCamera(pose, true);
        StartCoroutine(ChangeCameraBack(oldPos));
    }

    private IEnumerator ChangeCameraBack(int pose)
    {
        yield return new WaitForSeconds(camWait);
        ChangeCamera(pose, true);
        canChange = true;
        player.canMove = true;
    }

    public void Follow(Transform target)
    {
        if (!canChange)
            return;
        Debug.Log("Following..");
        Vector3 targetPos = camPos.position;
        targetPos.x = target.position.x;
        camPos.position = targetPos;
    }

    public void FollowPlayer(bool follow)
    {
        _isFollowingPlayer = follow;
    }

    private void Update()
    {
        if (_isFollowingPlayer)
        {
            Vector3 targetPos = camPos.position;
            targetPos.x = player.transform.position.x;
            targetPos.y = player.transform.position.y;
            transform.position = targetPos;
        }
    }
}