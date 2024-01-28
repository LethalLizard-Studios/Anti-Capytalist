using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeSceneTimer : MonoBehaviour
{
    [SerializeField] private int sceneIndex;
    [SerializeField] private float time = 6.0f;
    [Space(8)]
    [SerializeField] private Image countdownCircle;
    [SerializeField] private TextMeshProUGUI countdownTxt;


    private void Start()
    {
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        float timer = 0;
        countdownCircle.fillAmount = 0;

        do
        {
            yield return new WaitForSeconds(0.01f);
            timer += 0.01f;
            countdownCircle.fillAmount = (timer / time);
            countdownTxt.text = Mathf.RoundToInt(Mathf.Abs(timer - time) + 1).ToString();
        }
        while (countdownCircle.fillAmount <= 0.98f);

        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
    }
}
