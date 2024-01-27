using UnityEngine;

public class ControlsToggle : MonoBehaviour
{
    [SerializeField] private bool isController = false;

    private void Start()
    {
        gameObject.SetActive((isController) ? GameManager.Instance.controller : !GameManager.Instance.controller);
    }
}
