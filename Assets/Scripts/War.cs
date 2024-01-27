using UnityEngine;

public class War : MonoBehaviour
{
    [SerializeField] private Island[] allOwnedLand;
    [SerializeField] private GameObject firePrefab;

    public void Defeated()
    {
        foreach (Island island in allOwnedLand)
        {
            island.m_enemyLand = -5;
            island.interfaceIndex = -1;

            Instantiate(firePrefab, island.transform.position, Quaternion.identity);

            if (island.boat != null && (island.boat.activeSelf || island.bigBoat.activeSelf))
                Instantiate(firePrefab, island.boat.transform.position, Quaternion.identity); 
        }
    }
}
