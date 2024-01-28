using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryConditions : MonoBehaviour
{
    public static VictoryConditions Instance;

    [SerializeField] private Animation victoryCutscene;

    private int kingdomsDefeated = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    public void DefeatedKingdom()
    {
        kingdomsDefeated++;

        if (kingdomsDefeated >= 2)
            StartCoroutine(Victory());
    }

    private IEnumerator Victory()
    {
        victoryCutscene.Play();

        do
        {
            yield return null;
        }
        while (victoryCutscene.isPlaying);

        GameManager.Instance.transform.GetChild(0).gameObject.SetActive(false);
        GameManager.Instance.Reset();

        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }
}
