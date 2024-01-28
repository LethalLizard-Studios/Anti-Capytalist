using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsSelection : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void ToggleControls()
    {
        GameManager.Instance.controller = !GameManager.Instance.controller;

        if (!GameManager.Instance.controller)
            text.text = "Using\nMouse & Keyboard";
        else
            text.text = "Using\nController";
    }

    public void StartGame()
    {
        GameManager.Instance.transform.GetChild(0).gameObject.SetActive(true);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
