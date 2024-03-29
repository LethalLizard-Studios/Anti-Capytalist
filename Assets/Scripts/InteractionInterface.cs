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
    [SerializeField] private GameObject performanceScene;
    [SerializeField] private Camera openWorldCam;
    [SerializeField] private GameObject openWorldUI;
    [SerializeField] private Image kingCharacterImg;
    [SerializeField] private MeshRenderer kingsKeepColor;

    [SerializeField] private GameObject harvestUI;
    [SerializeField] private Image harvestImg;
    [SerializeField] private Sprite[] resourceSprites;
    [SerializeField] private TextMeshProUGUI resourceAmount;

    [SerializeField] private GameObject startWarUI;
    [SerializeField] private TextMeshProUGUI warStrengthAmount;

    [SerializeField] private GameObject shopUI;

    [SerializeField] private GameObject claimLandUI;
    [SerializeField] private TextMeshProUGUI claimPriceAmount;

    [SerializeField] private GameObject tradeUI;

    private int claimPrice = 1;

    public void Start()
    {
        GameManager.Instance.isInGame = true;
    }

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
        tradeUI.SetActive(false);

        if (island.interfaceIndex == -1)
            return;

        switch (island.interfaceIndex)
        {
            case 0:
                harvestUI.SetActive(true);
                if (!currentIsland.bigBoat.activeSelf || !currentIsland.upgrade.activeSelf)
                {
                    shopUI.SetActive(true);
                }

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
            case 3:
                tradeUI.SetActive(true);
                break;
        }
    }

    public void UpdateHarvestAmount()
    {
        if (currentIsland != null)
            resourceAmount.text = currentIsland.resourceAmount.ToString();
    }

    public void LeftDpad(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (currentIsland == null || performanceScene.activeSelf)
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

    public void AButton(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (currentIsland == null || performanceScene.activeSelf)
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
                case 3:
                    Trade();
                    break;
            }
        }
    }

    public void Perform()
    {
        if (currentIsland == null)
            return;

        GameManager.Instance.PlayClickSFX();

        kingCharacterImg.sprite = team.kingCharacterSprites[currentIsland.m_enemyLand];
        kingsKeepColor.material = team.materials[currentIsland.m_enemyLand + 1];

        performanceScene.SetActive(true);
        openWorldCam.enabled = false;
        openWorldUI.SetActive(false);
    }

    public void FinishPerformance()
    {
        performanceScene.SetActive(false);
        openWorldCam.enabled = true;
        openWorldUI.SetActive(true);
    }

    public void Shop()
    {
        if (currentIsland == null || !shopUI.activeSelf || performanceScene.activeSelf)
            return;

        GameManager.Instance.PlayClickSFX();

        ShopManager.Instance.OpenStorefront(currentIsland);
    }

    public void Trade()
    {
        if (currentIsland == null || !tradeUI.activeSelf || performanceScene.activeSelf)
            return;

        GameManager.Instance.PlayClickSFX();

        ShopManager.Instance.OpenTrader(currentIsland);
    }

    public void ClaimLand()
    {
        if (currentIsland == null || performanceScene.activeSelf)
            return;

        if (GameManager.Instance.royalDeedsAmount < claimPrice)
        {
            GameManager.Instance.DenyClickSFX();
            return;
        }

        GameManager.Instance.PlayClickSFX();

        GameManager.Instance.AddResource(3, -claimPrice);

        claimPrice++;
        claimPriceAmount.text = claimPrice.ToString();

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
        if (currentIsland == null || performanceScene.activeSelf)
            return;

        GameManager.Instance.PlayClickSFX();

        if (GameManager.Instance.militaryStrength > GameManager.Instance.botDefense[currentIsland.m_enemyLand])
        {
            War war;

            if (currentIsland.TryGetComponent<War>(out war))
            {
                war.Defeated();

                currentIsland.Select();
                Show(currentIsland);

                VictoryConditions.Instance.DefeatedKingdom();
            }
        }
        else
        {
            GameManager.Instance.DenyClickSFX();
        }
    }

    public void Harvest()
    {
        if (currentIsland == null || currentIsland.resourceAmount < 1 || performanceScene.activeSelf)
            return;

        GameManager.Instance.PlayClickSFX();

        GameManager.Instance.AddResource(currentIsland.resourceType, currentIsland.resourceAmount);
        currentIsland.resourceAmount = 0;
        resourceAmount.text = currentIsland.resourceAmount.ToString();
    }
}
