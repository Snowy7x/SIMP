using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FinalStage : MonoBehaviour
{
    [Header("Utilities")]
    [SerializeField] private GameObject blocker;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] lasers;
    [Header("First Stage")]
    public float delay = 1f;
    public TextAsset firstStageText;
    [SerializeField] UnityEvent firstStageTextEndEvent;
    [SerializeField] UnityEvent firstStageTextStartEvent;

    [Header("Second Stage")]
    public Material worldMat;
    public Collider2D worldCollider;
    public TextAsset secondStageText;
    [SerializeField] UnityEvent secondStageTextEndEvent;
    [SerializeField] UnityEvent secondStageTextStartEvent;
    public bool isDisolving = false;
    public float disolveSpeed = 0.1f;
    public float disolveAmount = 1.0f;
    private static readonly int Fade = Shader.PropertyToID("_Fade");

    [Header("Third Stage")]
    public TextAsset thirdStageText;
    [SerializeField] UnityEvent thirdStageTextEndEvent;
    [SerializeField] UnityEvent thirdStageTextStartEvent;

    void Start()
    {
        worldMat.SetFloat(Fade, 1);
    }

    private void Update()
    {
        if (isDisolving && disolveAmount > 0.0f)
        {
            disolveAmount -= disolveSpeed * Time.deltaTime;
            worldMat.SetFloat(Fade, disolveAmount);
        }
        else if (isDisolving)
        {
            isDisolving = false;
        }
    }

    #region First Stage

    public void FirstStage()
    {
        StartCoroutine(FirstStageCoroutine());
    }
    
    IEnumerator FirstStageCoroutine()
    {
        yield return new WaitForSeconds(delay);
        DialogueManager.GetInstance().EnterDialogueMode(firstStageText, firstStageTextStartEvent, firstStageTextEndEvent);    
    }
    #endregion

    #region SecondStage
    
    public void SecondStage()
    {
        if (isDisolving) return;
        disolveAmount = 1.0f;
        isDisolving = true;
        CameraController.Instance.FollowPlayer(true);
        StartCoroutine(Disolve());
    }
    
    IEnumerator Disolve()
    {
        DialogueManager.GetInstance().EnterDialogueMode(secondStageText, secondStageTextStartEvent, secondStageTextEndEvent);
        while (disolveAmount > 0.0f)
        {
            disolveAmount -= 0.1f * Time.deltaTime;
            worldMat.SetFloat(Fade, disolveAmount);
            yield return new WaitForSeconds(disolveSpeed);
        }
        isDisolving = false;
        foreach (var laser in lasers)
        {
            laser.SetActive(false);
        }
        foreach (var variable in GameObject.FindObjectsOfType<Collider2D>())
        {
            if (variable.gameObject != blocker && variable.gameObject != player)
                variable.enabled = false;
        }
        worldCollider.enabled = false;
        ThirdStage();
    }
    

    #endregion

    #region ThirdStage

    public void ThirdStage()
    {
        DialogueManager.GetInstance().EnterDialogueMode(thirdStageText, thirdStageTextStartEvent, thirdStageTextEndEvent);
    }

    #endregion
}
