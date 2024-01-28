using TMPro;
using UnityEngine;

public class TeamController : MonoBehaviour
{
    public Material[] materials;

    [SerializeField] private Color[] m_bots;
    [SerializeField] private Color m_player;

    [SerializeField] private TextMeshProUGUI m_territoryText;
    [SerializeField] private GameObject m_overlay;

    public Sprite[] kingCharacterSprites;

    public void UpdateUI(Island island)
    {
        if (island.m_isMyLand)
        {
            m_territoryText.text = "Your Land [" + island.m_defense + "]";
            m_territoryText.color = m_player;
            m_overlay.SetActive(true);
        }
        else if (island.m_enemyLand > -1)
        {
            m_territoryText.text = "Claimed Territory ["+island.m_defense+"]";
            m_territoryText.color = m_bots[island.m_enemyLand];
            m_overlay.SetActive(true);
        }
        else
        {
            m_overlay.SetActive(false);
        }
    }
}
