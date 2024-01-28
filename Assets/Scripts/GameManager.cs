using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public AudioClip[] musicTracks;

    public bool controller = true;

    public int[] botDefense = new int[2];

    [SerializeField] private AudioSource clickSource;
    [SerializeField] private AudioSource deniedSource;

    public int lumberAmount = 0;
    [SerializeField] private TextMeshProUGUI lumberTxt;
    public int stoneAmount = 0;
    [SerializeField] private TextMeshProUGUI stoneTxt;
    public int foodAmount = 0;
    [SerializeField] private TextMeshProUGUI foodTxt;
    public int royalDeedsAmount = 0;
    [SerializeField] private TextMeshProUGUI royalDeedsTxt;
    public int militaryStrength = 0;
    [SerializeField] private TextMeshProUGUI militaryTxt;

    public List<Island> collectIslands = new List<Island>();

    private AudioSource musicSource;

    private const int COLLECT_TIME = 30;
    public bool isInGame = false;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(CollectTimer());

        musicSource = GetComponent<AudioSource>();

        lumberTxt.text = lumberAmount.ToString();
        stoneTxt.text = stoneAmount.ToString();
        foodTxt.text = foodAmount.ToString();
        royalDeedsTxt.text = royalDeedsAmount.ToString();
        militaryTxt.text = militaryStrength.ToString();
    }

    public void SwitchMusic()
    {
        if (musicSource.clip == musicTracks[0])
            musicSource.clip = musicTracks[1];
        else
            musicSource.clip = musicTracks[0];

        musicSource.Play();
    }

    public void PlayClickSFX()
    {
        clickSource.pitch = Random.Range(0.92f, 1.08f);
        clickSource.Play();
    }

    public void DenyClickSFX()
    {
        deniedSource.pitch = Random.Range(0.92f, 1.08f);
        deniedSource.Play();
    }

    public void Reset()
    {
        isInGame = false;

        lumberAmount = 0;
        stoneAmount = 0;
        foodAmount = 0;
        militaryStrength = 0;
        royalDeedsAmount = 0;

        lumberTxt.text = lumberAmount.ToString();
        stoneTxt.text = stoneAmount.ToString();
        foodTxt.text = foodAmount.ToString();
        royalDeedsTxt.text = royalDeedsAmount.ToString();
        militaryTxt.text = militaryStrength.ToString();

        collectIslands.Clear();
    }

    public void AddResource(int index, int amount)
    {
        switch (index)
        {
            case 0:
                lumberAmount += amount;
                lumberTxt.text = lumberAmount.ToString();
                break;
            case 1:
                stoneAmount += amount;
                stoneTxt.text = stoneAmount.ToString();
                break;
            case 2:
                foodAmount += amount;
                foodTxt.text = foodAmount.ToString();
                break;
            case 3:
                royalDeedsAmount += amount;
                royalDeedsTxt.text = royalDeedsAmount.ToString();
                break;
            case 4:
                militaryStrength += amount;
                militaryTxt.text = militaryStrength.ToString();
                break;
        }
    }

    IEnumerator CollectTimer()
    {
        yield return new WaitForSeconds(COLLECT_TIME);

        if (isInGame)
        {
            foreach (Island island in collectIslands)
            {
                island.resourceAmount += island.resourceMultiplier;
            }

            InteractionInterface.Instance.UpdateHarvestAmount();
        }

        StartCoroutine(CollectTimer());
    }
}
