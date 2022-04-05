using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator animator;
    public float transitionTime;

    public void LoadNextLevel(string name)
    {
        StartCoroutine(LoadLevel(name));
    }

    private IEnumerator LoadLevel(string name)
    {
        animator.SetTrigger("Start");
        
        yield return new WaitForSeconds(transitionTime); ;

        SceneManager.LoadScene(name);
    }
}
