using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    TextMeshProUGUI m_name;
    Image[] m_Stars;

    public Image[] Stars { get => m_Stars; private set => m_Stars = value; }
    public TextMeshProUGUI Name { get => m_name; private set => m_name = value; }

    public void InitComponents()
    {
        Name = GetComponent<TextMeshProUGUI>();
        Stars = GetComponentsInChildren<Image>(true);
    }
}
