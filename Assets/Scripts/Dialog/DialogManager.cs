using Assets.Scripts.Dialog;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DialogManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogText;

    public float disableInputTime;

    public Queue<string> sentences;

    public static DialogManager instance;

    public GameObject dialogUI;

    public bool IsDialogShowing;

    private ControllerMapping controls;
    private InputAction nextAction;

    private Sound textScrollSound;

    private bool endGame;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        sentences = new Queue<string>();

        controls = new ControllerMapping();

        textScrollSound = AudioManager.instance.GetSound("TextScroll");
    }

    private void OnEnable()
    {
        nextAction = controls.Player.Jump;
        nextAction.started += ctx => DisplayNextSentence();
        nextAction.Enable();
    }

    private void OnDisable()
    {
        nextAction.Disable();
    }

    public void StartDialog(Dialog dialog)
    {
        endGame = dialog.endGame;

        IsDialogShowing = true;

        Resources.FindObjectsOfTypeAll<ImpulseJump>().First().KillJump();

        dialogUI.SetActive(true);

        nameText.text = dialog.name;

        sentences.Clear();

        foreach(var sentence in dialog.sentences)
            sentences.Enqueue(sentence);

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (!IsDialogShowing)
            return; 

        if(sentences.Count == 0)
        {
            EndDialog();
            return;
        }

        var sentence = sentences.Dequeue();
        dialogText.text = sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentece(sentence));
    }

    private IEnumerator TypeSentece(string sentece)
    {
        textScrollSound.source.Play();

        dialogText.text = "";
        foreach(var letter in sentece.ToCharArray())
        {
            dialogText.text += letter;
            yield return null;
        }

        yield return new WaitForSeconds(0.05f);

        textScrollSound.source.Stop();
    }

    private void EndDialog()
    {
        dialogUI.SetActive(false);


        if (endGame)
        {
            GameObject.FindObjectOfType<PromptCanvasController>().gameObject.SetActive(false);

            AudioManager.instance.StopSound("MainLoop");
            AudioManager.instance.StopSound("Fight");

            GameObject.FindObjectOfType<LevelLoader>().LoadNextLevel("Credits");
        }
        else
            Invoke("AllowInput", disableInputTime);
    }

    private void AllowInput()
    {
        IsDialogShowing = false;
    }
}
