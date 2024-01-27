using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JokeManager : MonoBehaviour
{
    [SerializeField] private TextAsset knockKnockJokes;
    [Space(8)]
    [SerializeField] private TextMeshProUGUI dialogue;
    [SerializeField] private TextMeshProUGUI[] answers = new TextMeshProUGUI[4];
    [Space(8)]
    [SerializeField] private GameObject OutOfJokesMsg;

    private int correctAnswer = 0;

    public int GetCorrectAnswer() { return correctAnswer; }

    private void Start()
    {
        ParseTextAsset(knockKnockJokes);
    }

    private Stack<KnockKnockJoke> sortedKnockJokes = new Stack<KnockKnockJoke>();

    private void ParseTextAsset(TextAsset textAsset)
    {
        if (textAsset == null)
            return;

        string text = textAsset.text;
        string[] seperated = text.Split("\n"[0]);

        int i = 0;

        KnockKnockJoke tempJoke = new KnockKnockJoke();
        tempJoke.wrongAnswers = new string[4];
        tempJoke.wrongAmount = 0;

        foreach (string str in seperated)
        {
            char startChar = str[0];

            //Add joke to stack when all variables are read.
            if (i > 4 || startChar.Equals('E'))
            {
                //Make sure to only push if joke exists!S
                if (!string.IsNullOrEmpty(tempJoke.joke))
                    sortedKnockJokes.Push(tempJoke);

                Debug.Log("R");

                tempJoke = new KnockKnockJoke();
                tempJoke.wrongAnswers = new string[4];
                tempJoke.wrongAmount = 0;
                i = 0;

                if (startChar.Equals('E'))
                    break;
            }

            switch (startChar)
            {
                case 'S':
                    tempJoke.joke = str.Remove(0, 2).Replace("\r", "");
                    Debug.Log(tempJoke.joke);
                    break;
                case 'C':
                    tempJoke.correctAnswer = str.Remove(0,2).Replace("\r", "");
                    Debug.Log(tempJoke.correctAnswer);
                    break;
                case '-':
                    tempJoke.wrongAnswers[tempJoke.wrongAmount] = str.Remove(0, 2).Replace("\r", "");
                    Debug.Log(tempJoke.wrongAnswers[tempJoke.wrongAmount]);
                    tempJoke.wrongAmount++;
                    break;
            }

            i++;
        }

        ChangeJoke();
    }

    public void ChangeJoke()
    {
        if (sortedKnockJokes.Count <= 0)
            OutOfJokes();

        KnockKnockJoke joke = sortedKnockJokes.Pop();

        dialogue.text = "Knock, knock\n" +
            "Who's there?\n" +
            joke.joke +
            " \n" + joke.joke + " who?"; ;

        correctAnswer = Random.Range(0, 4);
        answers[correctAnswer].text = joke.correctAnswer;

        int wrongJoke = 0;

        for (int i = 0; i < answers.Length; i++)
        {
            answers[i].color = Color.white;

            if (wrongJoke > joke.wrongAmount)
                break;

            if (i != correctAnswer)
            {
                Debug.Log(i);
                answers[i].text = joke.wrongAnswers[wrongJoke];
                wrongJoke++;
            }
        }
    }

    public void RevealAnswer(int guessIndex)
    {
        if (guessIndex == correctAnswer)
        {
            GameManager.Instance.royalDeedsAmount++;
            answers[correctAnswer].color = Color.green;
        }
        else
            answers[correctAnswer].color = Color.red;
    }

    public void OutOfJokes()
    {
        OutOfJokesMsg.SetActive(true);
        gameObject.SetActive(false);
    }
}
