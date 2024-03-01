using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public Animator transition2;
    public float transitionTime = 1f;
    private static readonly int Start = Animator.StringToHash("Start");

    public void LoadLevel(int index)
    {
        StartCoroutine(LoadLevelAsync(index));
    }

    public void LoadPreviousLevel()
    {
        StartCoroutine(LoadLevelAsync(SceneManager.GetActiveScene().buildIndex - 1));
    }
    
    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevelAsync(SceneManager.GetActiveScene().buildIndex + 1));
    }
    
    IEnumerator LoadLevelAsync(int index)
    {
        // Play Animation
        transition.SetTrigger(Start);
        // Wait for animation to finish
        yield return new WaitForSeconds(transitionTime);
        // Load level
        SceneManager.LoadScene(index);
    }
    
    public void PlayTransition()
    {
        if (!transition2) return;
        transition2.SetTrigger(Start);
    }
}
