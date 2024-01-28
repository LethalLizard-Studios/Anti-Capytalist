using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RandomGeneration : MonoBehaviour
{
    [SerializeField] private List<Island> claimableIslands = new List<Island>();

    [SerializeField] private List<Vector3> claimablePos = new List<Vector3>();
    [SerializeField] private List<bool> hasBeenFilled = new List<bool>();

    private void Start()
    {
        SwapSpots();
    }

    private void SwapSpots()
    {
        foreach (Island island in claimableIslands)
        {
            claimablePos.Add(island.transform.position);
            hasBeenFilled.Add(false);
        }

        for (int i = 0; i < claimableIslands.Count; i++)
        {
            int rand = Random.Range(0, claimableIslands.Count);

            do
            {
                if (rand < claimableIslands.Count-1)
                    rand++;
                else
                    rand = 0;
            }
            while (hasBeenFilled[rand]);

            claimableIslands[i].transform.position = claimablePos[rand];
            hasBeenFilled[rand] = true;
        }
    }
}
