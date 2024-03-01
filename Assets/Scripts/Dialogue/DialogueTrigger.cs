using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Ink JSON")]
    private bool gotCalled = false;

    public bool isOnce = true;
    [SerializeField] private TextAsset inkJSON;
    public UnityEvent onDialogueStart;
    public UnityEvent onDialogueEnd;

    private bool playerInRange;

    private void Awake() 
    {
        playerInRange = false;
    }

    private void OnTriggerEnter2D(Collider2D collider) 
    {
        if (isOnce && gotCalled)
            return;
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON, onDialogueStart, onDialogueEnd);
        gotCalled = true;
    }
    
}