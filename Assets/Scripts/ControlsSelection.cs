using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ControlsSelection : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject[] controlsPreview;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ToggleControls()
    {
        GameManager.Instance.controller = !GameManager.Instance.controller;

        GameManager.Instance.PlayClickSFX();

        if (!GameManager.Instance.controller)
        {
            foreach (GameObject control in controlsPreview)
                control.SetActive(false);
            text.text = "Using\nMouse & Keyboard";
        }
        else
        {
            foreach (GameObject control in controlsPreview)
                control.SetActive(true);
            text.text = "Using\nController";
        }
    }

    public void StartGame()
    {
        GameManager.Instance.PlayClickSFX();
        GameManager.Instance.transform.GetChild(0).gameObject.SetActive(true);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void QuitDesktop()
    {
        Application.Quit();
    }

    public void ButtonA(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            StartGame();
        }
    }

    public void ButtonX(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            ToggleControls();
        }
    }

    public void ButtonB(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            QuitDesktop();
        }
    }
}
