using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ControllerScreen : MonoBehaviour
{
    public ControllerMapping mapping;
    private InputAction next;

    private void OnEnable()
    {
        mapping = new ControllerMapping();

        next = mapping.Player.Jump;
        next.started += ctx => Play();
        next.Enable();
    }

    private void OnDisable()
    {
        next.Disable();
    }

    private void Play()
    {
        SceneManager.LoadScene("GameScene");
    }
}
