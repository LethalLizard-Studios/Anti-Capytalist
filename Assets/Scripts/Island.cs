using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Island : MonoBehaviour
{
    public Transform m_playerMovePt;
  
    public bool m_isSelected = false;

    public int m_defense = 0;

    public int interfaceIndex = -1;

    public bool m_isMyLand = false;
    public int m_enemyLand = -1;

    public int resourceType = -1;
    public int resourceAmount = 0;
    public int resourceMultiplier = 1;
    [Space(8)]
    public GameObject boat;
    public GameObject bigBoat;
    public GameObject upgrade;
    public GameObject disableUpgrade;

    private bool isHighlighted = false;

    private void Start()
    {
        AddToListCollect();
    }

    public void Select()
    {
        AddToListCollect();
        InteractionInterface.Instance.Show(this);
        m_isSelected = true;
    }

    public void Deselect()
    {
        m_isSelected = false;
    }

    public void AddToListCollect()
    {
        if (m_isMyLand && !GameManager.Instance.collectIslands.Contains(this))
            GameManager.Instance.collectIslands.Add(this);
    }
}
