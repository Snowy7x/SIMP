using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using JetBrains.Annotations;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private AudioClip[] sounds;
    
    [Header("Params")]
    [SerializeField] private float typingSpeed = 0.04f;
    [SerializeField] private float delayBeforeNextLine = 1f;
    

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject continueIcon;
    [SerializeField] private TextMeshProUGUI dialogueText;
     private string displayNameText;
    [SerializeField] private Animator portraitAnimator;
    private Animator layoutAnimator;

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private bool canContinueToNextLine = false;

    private Coroutine displayLineCoroutine;

    private static DialogueManager instance;

    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";

    private UnityEvent onDialogueEnd;
    private AudioSource audioSource;
    
    private void Awake() 
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public static DialogueManager GetInstance() 
    {
        return instance;
    }

    private void Start() 
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        // get the layout animator
        layoutAnimator = dialoguePanel.GetComponent<Animator>();

    }

    private void Update() 
    {
        // return right away if dialogue isn't playing
        if (!dialogueIsPlaying) 
        {
            return;
        }

        // handle continuing to the next line in the dialogue when submit is pressed
        // NOTE: The 'currentStory.currentChoiecs.Count == 0' part was to fix a bug after the Youtube video was made
        if (InputManager.GetInstance().GetSubmitPressed())
        {
            ContinueStory();
        }
    }
    

    IEnumerator WaitForTime(float time, TextAsset ink, Action<TextAsset> callback)
    {
        yield return new WaitForSeconds(time);
        callback(ink);
    }

    public void EnterDialogueMode(TextAsset inkJSON) 
    {
        // resetting the events system
        onDialogueEnd = new UnityEvent();
        
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        
        // reset portrait, layout, and speaker
        displayNameText = "???";
//        portraitAnimator.Play("default");
//        layoutAnimator.Play("right");

        ContinueStory();
    }
    
    public void EnterDialogueMode(TextAsset inkJSON, [CanBeNull] UnityEvent onDialogueStart, [CanBeNull] UnityEvent onDialogueEnd) 
    {
        this.onDialogueEnd = onDialogueEnd;
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        
        // reset portrait, layout, and speaker
        displayNameText = "???";
//        portraitAnimator.Play("default");
//        layoutAnimator.Play("right");
        ContinueStory();
        onDialogueStart?.Invoke();
    }

    private IEnumerator ExitDialogueMode() 
    {
        yield return new WaitForSeconds(0.2f);
        onDialogueEnd?.Invoke();
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory() 
    {
        if (currentStory.canContinue)
        {
            // set text for the current dialogue line
            if (displayLineCoroutine != null)
            { 
                StopCoroutine(displayLineCoroutine);
            }

            string text = currentStory.Continue();
            List<string> tags = currentStory.currentTags;
            if (tags.Count > 0)
            {
                // Run the voice
                int voiceIndex = int.Parse(tags[0]) - 1;
                    Debug.Log("Playing voice: " + voiceIndex);
                    audioSource.clip = sounds[voiceIndex];
                    audioSource.Play();
            }

            displayLineCoroutine = StartCoroutine(DisplayLine(text));
            // handle tags
        }
        else 
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private IEnumerator DisplayLine(string line) 
    {
        // set the text to the full line, but set the visible characters to 0
        //line = displayNameText + ": " + line;
        dialogueText.text = line;

        dialogueText.maxVisibleCharacters = 0;
        // hide items while text is typing
        continueIcon.SetActive(false);

        canContinueToNextLine = false;

        bool isAddingRichTextTag = false;

        // display each letter one at a time
        while (dialogueText.maxVisibleCharacters < line.Length)
        {
            // add the next letter
            dialogueText.maxVisibleCharacters++;
            // wait for the next frame
            yield return new WaitForSeconds(typingSpeed);
            // check if the next letter is a rich text tag
            if (line[dialogueText.maxVisibleCharacters - 1] == '<') 
            {
                isAddingRichTextTag = true;
            }
            // if the next letter is a closing tag, stop adding rich text tags
            else if (line[dialogueText.maxVisibleCharacters - 1] == '>') 
            {
                isAddingRichTextTag = false;
            }
            // if the next letter is a space and we're not adding rich text tags, stop adding rich text tags
            else if (line[dialogueText.maxVisibleCharacters - 1] == ' ' && !isAddingRichTextTag) 
            {
                isAddingRichTextTag = false;
            }
            // if we're adding rich text tags, don't show the continue icon
            else if (isAddingRichTextTag) 
            {
                continueIcon.SetActive(false);
            }
            // if we're not adding rich text tags, show the continue icon
            else 
            {
                continueIcon.SetActive(true);
            }
            
        }

        // actions to take after the entire line has finished displaying
        continueIcon.SetActive(true);
        //DisplayChoices();
        StartCoroutine(DisplayNextLine());
        canContinueToNextLine = true;
    }
    

    IEnumerator DisplayNextLine()
    {
        // wait for the player to press the continue button
        //yield return new WaitForSeconds(delayBeforeNextLine);
        yield return new WaitUntil(() => !audioSource.isPlaying);
        yield return new WaitForSeconds(0.5f);
        // display the next line
        // if the player hasn't pressed the continue button, skip the next line
        ContinueStory();
    }
}