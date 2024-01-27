using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int lumberAmount = 0;
    public int stoneAmount = 0;
    public int foodAmount = 0;
    public int royalDeedsAmount = 0;

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
}
