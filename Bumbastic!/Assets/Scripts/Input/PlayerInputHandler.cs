using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerMenu playerMenu;
    private Player player;

    public static event System.Action<byte> OnPlayerDeviceLost;
    public static event System.Action<byte> OnPlayerDeviceRegained;

    private void Awake()
    {
        playerMenu = GetComponent<PlayerMenu>();
        player = GetComponent<Player>();
    }

    public void OnDeviceLost()
    {
        Debug.Log("Device lost");
        if (playerMenu != null) OnPlayerDeviceLost?.Invoke(playerMenu.Id);
    }

    public void OnDeviceRegained()
    {
        Debug.Log("Device regained");
        if (playerMenu != null) OnPlayerDeviceRegained?.Invoke(playerMenu.Id);
    }
}
