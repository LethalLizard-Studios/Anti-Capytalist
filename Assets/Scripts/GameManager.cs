using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool controller = true;

    public int[] botDefense = new int[2];

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

    private const int COLLECT_TIME = 30;

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
        lumberTxt.text = lumberAmount.ToString();
        stoneTxt.text = stoneAmount.ToString();
        foodTxt.text = foodAmount.ToString();
        royalDeedsTxt.text = royalDeedsAmount.ToString();
        militaryTxt.text = militaryStrength.ToString();
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

        foreach(Island island in collectIslands)
        {
            island.resourceAmount += island.resourceMultiplier;
        }

        StartCoroutine(CollectTimer());
    }
}
