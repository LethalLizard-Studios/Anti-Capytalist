using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [SerializeField] private GameObject landUpgrade;
    [SerializeField] private GameObject boatUpgrade;

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

    public void OpenStorefront(Island island)
    {
        if (island == null)
            return;

        currentIsland = island;

        GameObject window = transform.GetChild(0).gameObject;

        window.SetActive(!window.activeSelf);

        if (!window.activeSelf)
            return;

        //Makes sure upgrade can't be bought twice
        landUpgrade.SetActive(!island.upgrade.activeSelf);
        boatUpgrade.SetActive(!island.bigBoat.activeSelf);
    }

    public void ButtonA(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            BuyLandUpgrade();
        }
    }

    public void ButtonB(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            BuyBoatUpgrade();
        }
    }

    public void BuyLandUpgrade()
    {
        if (currentIsland == null && !landUpgrade.activeSelf && currentIsland.upgrade == null)
            return;

        if (GameManager.Instance.lumberAmount >= 2 && GameManager.Instance.foodAmount >= 2)
        {
            landUpgrade.SetActive(false);

            GameManager.Instance.AddResource(0, -2);
            GameManager.Instance.AddResource(2, -3);

            currentIsland.resourceMultiplier *= 2;

            currentIsland.upgrade.SetActive(true);
            if (currentIsland.disableUpgrade != null)
                currentIsland.disableUpgrade.SetActive(false);
        }
    }

    public void BuyBoatUpgrade()
    {
        if (currentIsland == null && !boatUpgrade.activeSelf && currentIsland.bigBoat == null)
            return;

        if (GameManager.Instance.lumberAmount >= 3 && GameManager.Instance.stoneAmount >= 3)
        {
            boatUpgrade.SetActive(false);

            GameManager.Instance.AddResource(0, -3);
            GameManager.Instance.AddResource(1, -3);

            GameManager.Instance.AddResource(4, 2);

            currentIsland.bigBoat.SetActive(true);
            currentIsland.boat.SetActive(false);
        }
    }

    public void CloseStorefront()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
