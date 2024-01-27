using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(JokeManager))]
public class AnswerJoke : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI amountDoneTxt;
    [SerializeField] private TextMeshProUGUI timerTxt;
    [SerializeField] private Image timerImg;
    [Space(8)]
    [SerializeField] private Image buttonImg;
    [SerializeField] private Sprite[] directionSpr;

    private bool hasAnswered = false;

    private int amountCompleted = 1;

    private JokeManager jokeManager;
    private Controls _input;

    private void Start()
    {
        jokeManager = GetComponent<JokeManager>();

        _input = new Controls();
        _input.Player.Enable();

        _input.Player.Answer1.performed += obj => Answer(0);
        _input.Player.Answer2.performed += obj => Answer(1);
        _input.Player.Answer3.performed += obj => Answer(2);
        _input.Player.Answer4.performed += obj => Answer(3);
    }

    public void Answer(int index)
    {
        if (hasAnswered)
            return;

        buttonImg.sprite = directionSpr[index];
        jokeManager.RevealAnswer(index);

        StartCoroutine(CheckAnswer());
    }

    private IEnumerator CheckAnswer()
    {
        hasAnswered = true;

        timerImg.gameObject.SetActive(true);

        float timer = 0;
        timerImg.fillAmount = 0;

        do
        {
            yield return new WaitForSeconds(0.01f);
            timer += 0.01f;
            timerImg.fillAmount = (timer/3);
            timerTxt.text = Mathf.RoundToInt(Mathf.Abs(timer-3)).ToString();
        }
        while (timerImg.fillAmount <= 0.98f);

        timerImg.gameObject.SetActive(false);

        amountCompleted++;
        amountDoneTxt.text = amountCompleted+"/3";

        jokeManager.ChangeJoke();
        buttonImg.sprite = directionSpr[4];

        hasAnswered = false;
    }
}
