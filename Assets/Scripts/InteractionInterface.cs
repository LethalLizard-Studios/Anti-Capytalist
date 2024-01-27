using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractionInterface : MonoBehaviour
{
    public static InteractionInterface Instance;

    private Island currentIsland;

    [SerializeField] private TeamController team;

    [SerializeField] private GameObject performUI;

    [SerializeField] private GameObject harvestUI;
    [SerializeField] private Image harvestImg;
    [SerializeField] private Sprite[] resourceSprites;
    [SerializeField] private TextMeshProUGUI resourceAmount;

    [SerializeField] private GameObject startWarUI;
    [SerializeField] private TextMeshProUGUI warStrengthAmount;

    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject claimLandUI;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Show(Island island)
    {
        currentIsland = island;

        harvestUI.SetActive(false);
        startWarUI.SetActive(false);
        performUI.SetActive(false);
        shopUI.SetActive(false);
        claimLandUI.SetActive(false);

        if (island.interfaceIndex == -1)
            return;

        switch (island.interfaceIndex)
        {
            case 0:
                harvestUI.SetActive(true);
                shopUI.SetActive(true);
                if (island.resourceType != -1)
                {
                    harvestImg.sprite = resourceSprites[island.resourceType];
                    resourceAmount.text = island.resourceAmount.ToString();
                }
                break;
            case 1:
                startWarUI.SetActive(true);
                warStrengthAmount.text = GameManager.Instance.botDefense[island.m_enemyLand].ToString();
                performUI.SetActive(true);
                break;
            case 2:
                claimLandUI.SetActive(true);
                break;
        }
    }

    public void LeftDpad(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (currentIsland == null)
                return;

            if (currentIsland.interfaceIndex == -1)
                return;

            switch (currentIsland.interfaceIndex)
            {
                case 0:
                    Shop();
                    break;
                case 1:
                    Invade();
                    break;
            }
        }
    }

    public void DownDpad(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (currentIsland == null)
                return;

            if (currentIsland.interfaceIndex == -1)
                return;

            switch (currentIsland.interfaceIndex)
            {
                case 0:
                    Harvest();
                    break;
                case 1:
                    Perform();
                    break;
                case 2:
                    ClaimLand();
                    break;
            }
        }
    }

    public void Perform()
    {

    }

    public void Shop()
    {
        if (currentIsland == null)
            return;

        ShopManager.Instance.OpenStorefront(currentIsland);
    }

    public void ClaimLand()
    {
        if (currentIsland == null || GameManager.Instance.royalDeedsAmount < 1)
            return;

        GameManager.Instance.AddResource(3, -1);

        currentIsland.m_isMyLand = true;
        currentIsland.interfaceIndex = 0;
        currentIsland.boat.SetActive(true);
        currentIsland.m_defense = 1;

        currentIsland.boat.GetComponent<SetTeamColor>().Change(team.materials[0]);

        currentIsland.Select();
        Show(currentIsland);
    }

    public void Invade()
    {
        if (currentIsland == null)
            return;

        if (GameManager.Instance.militaryStrength > GameManager.Instance.botDefense[currentIsland.m_enemyLand])
        {
            War war;

            if (currentIsland.TryGetComponent<War>(out war))
            {
                war.Defeated();

                currentIsland.Select();
                Show(currentIsland);
            }
        }
    }

    public void Harvest()
    {
        if (currentIsland == null || currentIsland.resourceAmount < 1)
            return;

        GameManager.Instance.AddResource(currentIsland.resourceType, currentIsland.resourceAmount);
        currentIsland.resourceAmount = 0;
        resourceAmount.text = currentIsland.resourceAmount.ToString();
    }
}
