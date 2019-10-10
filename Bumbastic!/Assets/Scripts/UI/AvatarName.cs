using UnityEngine;
using TMPro;

public class AvatarName : MonoBehaviour
{
    TextMeshProUGUI avatarName = null;
    Player player = null;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        avatarName = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        avatarName.text = player.PrefabName;
    }
}
