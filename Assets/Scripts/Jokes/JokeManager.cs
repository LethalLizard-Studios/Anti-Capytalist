using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JokeManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] laughTracks;
    private AudioSource laughSource;
    private int laughIndex = 0;

    [SerializeField] private TextAsset knockKnockJokes;
    [Space(8)]
    [SerializeField] private TextMeshProUGUI dialogue;
    [SerializeField] private TextMeshProUGUI[] answers = new TextMeshProUGUI[4];
    [SerializeField] private TextMeshProUGUI[] answersKeyboard = new TextMeshProUGUI[4];
    [Space(8)]
    [SerializeField] private GameObject OutOfJokesMsg;

    private int correctAnswer = 0;

    public int GetCorrectAnswer() { return correctAnswer; }

    private void Start()
    {
        laughSource = GetComponent<AudioSource>();

        ParseTextAsset(knockKnockJokes);
    }

    private List<KnockKnockJoke> sortedKnockJokes = new List<KnockKnockJoke>();
    private int currentJoke = 0;

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
                    sortedKnockJokes.Add(tempJoke);

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
                    break;
                case 'C':
                    tempJoke.correctAnswer = str.Remove(0,2).Replace("\r", "");
                    break;
                case '-':
                    tempJoke.wrongAnswers[tempJoke.wrongAmount] = str.Remove(0, 2).Replace("\r", "");
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

        KnockKnockJoke joke = sortedKnockJokes[currentJoke];

        dialogue.text = "Knock, knock\n" +
            "Who's there?\n" +
            joke.joke +
            " \n" + joke.joke + " who?"; ;

        correctAnswer = Random.Range(0, 4);
        answers[correctAnswer].text = joke.correctAnswer;
        answersKeyboard[correctAnswer].text = joke.correctAnswer;

        int wrongJoke = 0;

        for (int i = 0; i < answers.Length; i++)
        {
            answers[i].color = Color.white;
            answersKeyboard[i].color = Color.white;

            if (wrongJoke > joke.wrongAmount)
                break;

            if (i != correctAnswer)
            {
                answers[i].text = joke.wrongAnswers[wrongJoke];
                answersKeyboard[i].text = joke.wrongAnswers[wrongJoke];
                wrongJoke++;
            }
        }

        currentJoke++;

        if (currentJoke >= sortedKnockJokes.Count)
            currentJoke = 0;
    }

    public void RevealAnswer(int guessIndex)
    {
        if (guessIndex == correctAnswer)
        {
            GameManager.Instance.AddResource(3, 1);
            answers[correctAnswer].color = Color.green;
            answersKeyboard[correctAnswer].color = Color.green;

            laughSource.clip = laughTracks[laughIndex];
            laughSource.Play();

            if (laughIndex >= laughTracks.Length-1)
                laughIndex = 0;
            else
                laughIndex++;
        }
        else
        {
            GameManager.Instance.DenyClickSFX();

            answers[correctAnswer].color = Color.red;
            answersKeyboard[correctAnswer].color = Color.red;
        }
    }

    public void OutOfJokes()
    {
        OutOfJokesMsg.SetActive(true);
        gameObject.SetActive(false);
    }
}
