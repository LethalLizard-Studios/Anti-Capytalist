using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [SerializeField] private GameObject landUpgrade;
    [SerializeField] private GameObject boatUpgrade;
    [SerializeField] private Image resourceImg;
    [SerializeField] private Sprite[] resources;

    private AudioSource coinSource;

    private Island currentIsland;

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

    private void Start()
    {
        coinSource = GetComponent<AudioSource>();
    }

    public void OpenTrader(Island island)
    {
        if (island == null)
            return;

        currentIsland = island;

        GameObject window = transform.GetChild(1).gameObject;

        window.SetActive(!window.activeSelf);

        if (!window.activeSelf)
            return;
    }

    public void OpenStorefront(Island island)
    {
        if (island == null)
            return;

        currentIsland = island;

        GameObject window = transform.GetChild(0).gameObject;

        window.SetActive(!window.activeSelf);

        if (!window.activeSelf)
            return;

        resourceImg.sprite = resources[island.resourceType];

        //Makes sure upgrade can't be bought twice
        landUpgrade.SetActive(!island.upgrade.activeSelf);
        boatUpgrade.SetActive(!island.bigBoat.activeSelf);
    }

    public void ButtonX(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (IsActive())
            {
                BuyLandUpgrade();
            }
            else if (IsTradeActive())
            {
                TradeStone();
            }
        }
    }

    public void ButtonB(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (IsActive())
            {
                BuyBoatUpgrade();
            }
            else if (IsTradeActive())
            {
                TradeFood();
            }
        }
    }

    public void TradeStone()
    {
        if (currentIsland == null || !IsTradeActive())
            return;

        if (GameManager.Instance.stoneAmount >= 3)
        {
            coinSource.pitch = Random.Range(0.95f, 1.05f);
            coinSource.Play();

            GameManager.Instance.AddResource(1, -3);
            GameManager.Instance.AddResource(0, 1);
        }
        else
        {
            GameManager.Instance.DenyClickSFX();
        }
    }

    public void TradeFood()
    {
        if (currentIsland == null || !IsTradeActive())
            return;

        if (GameManager.Instance.foodAmount >= 3)
        {
            coinSource.pitch = Random.Range(0.95f, 1.05f);
            coinSource.Play();

            GameManager.Instance.AddResource(2, -3);
            GameManager.Instance.AddResource(0, 1);
        }
        else
        {
            GameManager.Instance.DenyClickSFX();
        }
    }

    public void BuyLandUpgrade()
    {
        if (currentIsland == null || !landUpgrade.activeSelf || currentIsland.upgrade == null || !IsActive())
            return;

        if (GameManager.Instance.lumberAmount >= 2 && GameManager.Instance.foodAmount >= 2)
        {
            coinSource.pitch = Random.Range(0.95f, 1.05f);
            coinSource.Play();

            landUpgrade.SetActive(false);

            GameManager.Instance.AddResource(0, -2);
            GameManager.Instance.AddResource(2, -3);

            currentIsland.resourceMultiplier *= 2;

            currentIsland.upgrade.SetActive(true);
            if (currentIsland.disableUpgrade != null)
                currentIsland.disableUpgrade.SetActive(false);
        }
        else
        {
            GameManager.Instance.DenyClickSFX();
        }
    }

    public void BuyBoatUpgrade()
    {
        if (currentIsland == null || !boatUpgrade.activeSelf || currentIsland.bigBoat == null || !IsActive())
            return;

        if (GameManager.Instance.lumberAmount >= 3 && GameManager.Instance.stoneAmount >= 3)
        {
            coinSource.pitch = Random.Range(0.95f, 1.05f);
            coinSource.Play();

            boatUpgrade.SetActive(false);

            GameManager.Instance.AddResource(0, -3);
            GameManager.Instance.AddResource(1, -3);

            GameManager.Instance.AddResource(4, 2);

            currentIsland.m_defense += 2;
            currentIsland.bigBoat.SetActive(true);
            currentIsland.boat.SetActive(false);
        }
        else
        {
            GameManager.Instance.DenyClickSFX();
        }
    }

    public void CloseStorefront()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
    }

    private bool IsActive()
    {
        return transform.GetChild(0).gameObject.activeSelf;
    }

    private bool IsTradeActive()
    {
        return transform.GetChild(1).gameObject.activeSelf;
    }
}
