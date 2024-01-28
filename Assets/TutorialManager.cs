using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject[] pages;
    [SerializeField] private Image[] progress;
    [SerializeField] private Color progressColor;
    [SerializeField] private TextMeshProUGUI buttonTxt;

    private int currentPage = 0;

    public void Start()
    {
        GameManager.Instance.isInGame = false;
    }

    public void ButtonB(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (transform.GetChild(0).gameObject.activeSelf)
                NextPage();
        }
    }

    public void NextPage()
    {
        Debug.Log("NEXT!");
        GameManager.Instance.PlayClickSFX();

        currentPage++;

        if (currentPage >= pages.Length - 1)
            buttonTxt.text = "Close";
        else
            buttonTxt.text = "Next";

        if (currentPage >= pages.Length)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            return;
        }

        progress[currentPage].color = progressColor;
        pages[currentPage-1].SetActive(false);
        pages[currentPage].SetActive(true);
    }
}
