// GENERATED AUTOMATICALLY FROM 'Assets/Resources/Input/InputManager.inputactions'

using System;
using UnityEngine;
using UnityEngine.Experimental.Input;


[Serializable]
public class InputManager : InputActionAssetReference
{
    public InputManager()
    {
    }
    public InputManager(InputActionAsset asset)
        : base(asset)
    {
    }
    private bool m_Initialized;
    private void Initialize()
    {
        // Player
        m_Player = asset.GetActionMap("Player");
        m_Player_Aiming = m_Player.GetAction("Aiming");
        m_Player_Throwing = m_Player.GetAction("Throwing");
        m_Player_Movement = m_Player.GetAction("Movement");
        // UI
        m_UI = asset.GetActionMap("UI");
        m_UI_Accept = m_UI.GetAction("Accept");
        m_UI_Back = m_UI.GetAction("Back");
        m_UI_Move = m_UI.GetAction("Move");
        m_UI_Start = m_UI.GetAction("Start");
        m_Initialized = true;
    }
    private void Uninitialize()
    {
        m_Player = null;
        m_Player_Aiming = null;
        m_Player_Throwing = null;
        m_Player_Movement = null;
        m_UI = null;
        m_UI_Accept = null;
        m_UI_Back = null;
        m_UI_Move = null;
        m_UI_Start = null;
        m_Initialized = false;
    }
    public void SetAsset(InputActionAsset newAsset)
    {
        if (newAsset == asset) return;
        if (m_Initialized) Uninitialize();
        asset = newAsset;
    }
    public override void MakePrivateCopyOfActions()
    {
        SetAsset(ScriptableObject.Instantiate(asset));
    }
    // Player
    private InputActionMap m_Player;
    private InputAction m_Player_Aiming;
    private InputAction m_Player_Throwing;
    private InputAction m_Player_Movement;
    public struct PlayerActions
    {
        private InputManager m_Wrapper;
        public PlayerActions(InputManager wrapper) { m_Wrapper = wrapper; }
        public InputAction @Aiming { get { return m_Wrapper.m_Player_Aiming; } }
        public InputAction @Throwing { get { return m_Wrapper.m_Player_Throwing; } }
        public InputAction @Movement { get { return m_Wrapper.m_Player_Movement; } }
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled { get { return Get().enabled; } }
        public InputActionMap Clone() { return Get().Clone(); }
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
    }
    public PlayerActions @Player
    {
        get
        {
            if (!m_Initialized) Initialize();
            return new PlayerActions(this);
        }
    }
    // UI
    private InputActionMap m_UI;
    private InputAction m_UI_Accept;
    private InputAction m_UI_Back;
    private InputAction m_UI_Move;
    private InputAction m_UI_Start;
    public struct UIActions
    {
        private InputManager m_Wrapper;
        public UIActions(InputManager wrapper) { m_Wrapper = wrapper; }
        public InputAction @Accept { get { return m_Wrapper.m_UI_Accept; } }
        public InputAction @Back { get { return m_Wrapper.m_UI_Back; } }
        public InputAction @Move { get { return m_Wrapper.m_UI_Move; } }
        public InputAction @Start { get { return m_Wrapper.m_UI_Start; } }
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled { get { return Get().enabled; } }
        public InputActionMap Clone() { return Get().Clone(); }
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
    }
    public UIActions @UI
    {
        get
        {
            if (!m_Initialized) Initialize();
            return new UIActions(this);
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get

        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.GetControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get

        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.GetControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_XInputControllerSchemeIndex = -1;
    public InputControlScheme XInputControllerScheme
    {
        get

        {
            if (m_XInputControllerSchemeIndex == -1) m_XInputControllerSchemeIndex = asset.GetControlSchemeIndex("XInputController");
            return asset.controlSchemes[m_XInputControllerSchemeIndex];
        }
    }
}
