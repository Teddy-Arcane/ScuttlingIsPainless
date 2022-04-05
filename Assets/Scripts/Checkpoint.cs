using Assets.Scripts.NameGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Checkpoint : MonoBehaviour
{
    public Light2D light;

    public bool isAIRoom;

    private Sound fightTheme;
    private Sound evaTheme;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().checkpoint =
                gameObject.transform.position;

            light.pointLightOuterRadius = 1.25f;

            PromptCanvasController.instance.SetText("Checkpoint!", false);
            PromptCanvasController.instance.ToggleCanvas(true);

            AudioManager.instance.PlaySound("Checkpoint");

            if (isAIRoom)
            {
                fightTheme = AudioManager.instance.GetSound("Fight");
                evaTheme = AudioManager.instance.GetSound("EVA");

                evaTheme?.source?.Stop();
                fightTheme?.source?.Play();

                NameGenerator.instance.KillFaster();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
            PromptCanvasController.instance.ToggleCanvas(false);
    }
}
