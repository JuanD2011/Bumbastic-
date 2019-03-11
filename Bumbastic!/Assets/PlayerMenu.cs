using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.Plugins.PlayerInput;

public class PlayerMenu : MonoBehaviour
{
    private byte id;
    private bool ready = false;

    public bool Ready { get => ready; }
    public byte Id { get => id; }

    public delegate void ReadyDelegate(byte id);
    public ReadyDelegate OnReady;
    public ReadyDelegate OnNotReady;

    private void Start()
    {
        id = (byte)gameObject.GetComponent<PlayerInput>().playerIndex;
    }

    public void OnStart(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!ready)
            {
                ready = true;
                MenuManager.menu.PlayersReady(id);
            }
        }
    }

    public void OnBack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (ready)
            {
                ready = false;
                MenuManager.menu.PlayerNotReady(id);
            }
        }
    }
}
