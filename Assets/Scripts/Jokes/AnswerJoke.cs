using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(JokeManager))]
public class AnswerJoke : MonoBehaviour
{
    [SerializeField] private Image buttonImg;
    [SerializeField] private Sprite[] directionSpr;

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
        buttonImg.sprite = directionSpr[index];
        jokeManager.RevealAnswer(index);

        StartCoroutine(CheckAnswer());
    }

    private IEnumerator CheckAnswer()
    {
        yield return new WaitForSeconds(3);
        jokeManager.ChangeJoke();
        buttonImg.sprite = directionSpr[4];
    }
}
