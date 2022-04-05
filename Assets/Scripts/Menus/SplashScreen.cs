using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ReturnMainMenu());
    }

    private IEnumerator ReturnMainMenu()
    {
        yield return new WaitForSeconds(10f);
        GameObject.FindObjectOfType<LevelLoader>().LoadNextLevel("MainMenu");
    }
}
