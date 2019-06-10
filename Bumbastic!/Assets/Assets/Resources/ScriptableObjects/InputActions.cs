// GENERATED AUTOMATICALLY FROM 'Assets/Resources/ScriptableObjects/InputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.Utilities;

public class InputActions : IInputActionCollection
{
    private InputActionAsset asset;
    public InputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""Game"",
            ""id"": ""46390bbf-98bf-4af4-9fef-9baa1c9a1f2a"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""id"": ""43e17e44-23bb-40e2-9fcf-b3816130cbf8"",
                    ""expectedControlLayout"": """",
                    ""continuous"": true,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                },
                {
                    ""name"": ""Aim"",
                    ""id"": ""d1e57093-b1a8-41e3-9ba0-79fe01f268c7"",
                    ""expectedControlLayout"": """",
                    ""continuous"": true,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                },
                {
                    ""name"": ""Throw"",
                    ""id"": ""a9978a69-e5a6-4b48-8041-1dad9251ed91"",
                    ""expectedControlLayout"": """",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                },
                {
                    ""name"": ""Start"",
                    ""id"": ""f04e5d61-224e-4d88-95ad-f43e125573fb"",
                    ""expectedControlLayout"": """",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""cd25f9dd-1943-4e8d-8e08-bd805d6960e0"",
                    ""path"": ""<XInputController>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""b5af3361-3804-4fd7-b379-69005d08993d"",
                    ""path"": ""<DualShockGamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""349a4aea-5b18-495d-b54c-18b4c2711773"",
                    ""path"": ""<HID::810-1 Joystick>/Stick"",
                    ""interactions"": """",
                    ""processors"": ""InvertVector2(invertX=false)"",
                    ""groups"": ""New control scheme;Generic1"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""014f7596-10bb-48ed-8d7e-d750036dfcea"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""up"",
                    ""id"": ""44b17965-07da-4e78-a46f-abeb201affac"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MK"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""down"",
                    ""id"": ""f02391d8-cdf8-4a1c-bf04-a5dc2341543a"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MK"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""left"",
                    ""id"": ""81c8b961-dd0a-4efb-81a4-7980fcc30db7"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MK"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2bbcf6ef-2094-4801-9a7b-3b9c22f7a996"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MK"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""2b6a7f9e-2968-48d1-aaa0-792a17191254"",
                    ""path"": ""<XInputController>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""1917a7de-8b8a-45b2-b63c-1b0fde219ed2"",
                    ""path"": ""<DualShockGamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""0ff2310f-543b-42a4-adb1-d4489aa1f37e"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""Up"",
                    ""id"": ""31cbeb32-ef76-495f-9225-66b3a0fd7adf"",
                    ""path"": ""<HID::810-1 Joystick>/rz"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Generic1"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""Down"",
                    ""id"": ""58dec988-7824-48da-827d-ffc48f087228"",
                    ""path"": ""<HID::810-1 Joystick>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Generic1"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""Left"",
                    ""id"": ""402f614a-ad3a-4525-90a4-7a6ceff43c67"",
                    ""path"": ""<HID::810-1 Joystick>/rz"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Generic1"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""Right"",
                    ""id"": ""43909a3f-d773-4ad8-a517-e4271d744d12"",
                    ""path"": ""<HID::810-1 Joystick>/rz"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Generic1"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""97c5ac02-b2d1-40f3-a5ea-2f69c3e02684"",
                    ""path"": ""<XInputController>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""Throw"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""e5cc5ba6-61fb-4189-898c-e2579eb80c7c"",
                    ""path"": ""<DualShockGamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""Throw"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""51761b2c-8ef5-422d-9836-d5df27784c11"",
                    ""path"": ""<HID::810-1 Joystick>/button8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""New control scheme;Generic1"",
                    ""action"": ""Throw"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""89510731-ac74-42e1-8bfe-03c0abce1ccd"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MK"",
                    ""action"": ""Throw"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""15b86e6e-f872-4b96-afc8-6022275b307f"",
                    ""path"": ""<XInputController>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""Start"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""b22678d7-0ea7-4f65-a503-2c8c30b3d629"",
                    ""path"": ""<DualShockGamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""Start"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""9891af09-bf13-45df-a22e-98c59845b070"",
                    ""path"": ""<HID::810-1 Joystick>/button10"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Generic1"",
                    ""action"": ""Start"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""36279981-1618-42ca-83ba-add9efda64ef"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MK"",
                    ""action"": ""Start"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""d5922bc1-9074-4f93-9df6-95e4a5365acb"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MK"",
                    ""action"": ""Start"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                }
            ]
        },
        {
            ""name"": ""Menu"",
            ""id"": ""1734b7aa-e1e9-4522-be4e-83d6df04603b"",
            ""actions"": [
                {
                    ""name"": ""LeftSkin"",
                    ""id"": ""0c96a2f9-68b2-47cd-aaa3-6ae8d89fd6f8"",
                    ""expectedControlLayout"": """",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                },
                {
                    ""name"": ""RightSkin"",
                    ""id"": ""a7b2bfce-b224-443a-a229-6e44477a2b07"",
                    ""expectedControlLayout"": """",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                },
                {
                    ""name"": ""Submit"",
                    ""id"": ""bd8af8d1-ae3a-43e0-a62d-b223577930f0"",
                    ""expectedControlLayout"": """",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                },
                {
                    ""name"": ""Back"",
                    ""id"": ""d03f3e76-c25b-42bb-9ee7-b5a212ab6d38"",
                    ""expectedControlLayout"": """",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                },
                {
                    ""name"": ""Start"",
                    ""id"": ""fd9efb46-8286-47c3-a7ae-ece3b6f326b9"",
                    ""expectedControlLayout"": """",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                },
                {
                    ""name"": ""Scrolling"",
                    ""id"": ""bbcc7bc3-788a-4bec-923d-c3dbbf5466df"",
                    ""expectedControlLayout"": """",
                    ""continuous"": true,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""aa7a8165-3372-48ca-bfde-0fb98a0cf4f3"",
                    ""path"": ""<DualShockGamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""LeftSkin"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""4275110c-4b62-46ac-9a5d-f316c3cc4e29"",
                    ""path"": ""<XInputController>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""LeftSkin"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""a152467e-18ed-4af0-a5ff-b9c9beca6a06"",
                    ""path"": ""<HID::810-1 Joystick>/button5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""New control scheme;Generic1"",
                    ""action"": ""LeftSkin"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""1a66844a-d628-4adf-81a7-bd9bc3948d7f"",
                    ""path"": ""<DualShockGamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""LeftSkin"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""762f218c-27bd-4ed6-845e-21b826828373"",
                    ""path"": ""<XInputController>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""LeftSkin"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""2a7a6f61-e57c-4163-89fc-87b9292c4b49"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MK"",
                    ""action"": ""LeftSkin"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""1e53b4da-6b26-4a4d-8f9c-1b996f5ac5dc"",
                    ""path"": ""<DualShockGamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""RightSkin"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""1a2b6978-88e8-4263-b754-a5e3f21cb75e"",
                    ""path"": ""<XInputController>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""RightSkin"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""244f8606-351c-43ae-baca-54d619ac78de"",
                    ""path"": ""<HID::810-1 Joystick>/button6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""New control scheme;Generic1"",
                    ""action"": ""RightSkin"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""2318179c-dcdd-4500-8d31-f5197f32bd1c"",
                    ""path"": ""<DualShockGamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""RightSkin"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""cca8d0c4-844b-4cc0-8ce8-a7ee5db636b0"",
                    ""path"": ""<XInputController>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""RightSkin"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""606100ff-1949-4db5-8b24-aa8cdc0bc6dd"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MK"",
                    ""action"": ""RightSkin"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""9e23b6df-7ca3-429b-bc42-93e09f609adc"",
                    ""path"": ""<DualShockGamepad>/buttonSouth"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""515d509a-4755-4b3a-8c92-9a38566351de"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""773ceba7-b104-41c4-8c92-e137829e7b1c"",
                    ""path"": ""<HID::810-1 Joystick>/button3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""New control scheme;Generic1"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""7a4adc0a-c88c-4187-8251-1e1843f75170"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MK"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""25829fbb-73d6-4953-95e2-c14aa40f1898"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MK"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""2c8fbc8b-1046-4bf6-8f04-51b77862ef67"",
                    ""path"": ""<DualShockGamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""63ffea5b-346e-46e1-bd92-9659be89e425"",
                    ""path"": ""<XInputController>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""011e7d03-66b0-4ccb-b09e-ff5ce448ffeb"",
                    ""path"": ""<HID::810-1 Joystick>/button2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""New control scheme;Generic1"",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""9d45eb88-af4b-4b0d-9451-820184bd9ebb"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MK"",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""df440f94-a254-4ca3-8844-c82ab6dd4332"",
                    ""path"": ""<DualShockGamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";PS4"",
                    ""action"": ""Start"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""830b597f-d5db-4eb5-bced-bca5f12a93e0"",
                    ""path"": ""<XInputController>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""Start"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""8853e82e-61ef-403e-9765-6598666de77c"",
                    ""path"": ""<HID::810-1 Joystick>/button10"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""New control scheme;Generic1"",
                    ""action"": ""Start"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""317fc811-cc5a-444c-bb03-985e3b8b6e93"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MK"",
                    ""action"": ""Start"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""b9e00623-2741-4e9a-b577-5a1e88b0061e"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MK"",
                    ""action"": ""Start"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""1cbad20e-77f6-4423-b7ea-61990bc57ab1"",
                    ""path"": ""<DualShockGamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4"",
                    ""action"": ""Scrolling"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""88d051da-dafc-4d33-b462-d30d0e6d6830"",
                    ""path"": ""<XInputController>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""Scrolling"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""cd0a20db-7783-4092-bde8-560a42c3499c"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MK"",
                    ""action"": ""Scrolling"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Xbox"",
            ""basedOn"": """",
            ""bindingGroup"": ""Xbox"",
            ""devices"": [
                {
                    ""devicePath"": ""<XInputController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""PS4"",
            ""basedOn"": """",
            ""bindingGroup"": ""PS4"",
            ""devices"": [
                {
                    ""devicePath"": ""<DualShockGamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Generic1"",
            ""basedOn"": """",
            ""bindingGroup"": ""Generic1"",
            ""devices"": [
                {
                    ""devicePath"": ""<HID::810-1 Joystick>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""MK"",
            ""basedOn"": """",
            ""bindingGroup"": ""MK"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Game
        m_Game = asset.GetActionMap("Game");
        m_Game_Move = m_Game.GetAction("Move");
        m_Game_Aim = m_Game.GetAction("Aim");
        m_Game_Throw = m_Game.GetAction("Throw");
        m_Game_Start = m_Game.GetAction("Start");
        // Menu
        m_Menu = asset.GetActionMap("Menu");
        m_Menu_LeftSkin = m_Menu.GetAction("LeftSkin");
        m_Menu_RightSkin = m_Menu.GetAction("RightSkin");
        m_Menu_Submit = m_Menu.GetAction("Submit");
        m_Menu_Back = m_Menu.GetAction("Back");
        m_Menu_Start = m_Menu.GetAction("Start");
        m_Menu_Scrolling = m_Menu.GetAction("Scrolling");
    }
    ~InputActions()
    {
        UnityEngine.Object.Destroy(asset);
    }
    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }
    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }
    public ReadOnlyArray<InputControlScheme> controlSchemes
    {
        get => asset.controlSchemes;
    }
    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }
    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    public void Enable()
    {
        asset.Enable();
    }
    public void Disable()
    {
        asset.Disable();
    }
    // Game
    private InputActionMap m_Game;
    private IGameActions m_GameActionsCallbackInterface;
    private InputAction m_Game_Move;
    private InputAction m_Game_Aim;
    private InputAction m_Game_Throw;
    private InputAction m_Game_Start;
    public struct GameActions
    {
        private InputActions m_Wrapper;
        public GameActions(InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move { get { return m_Wrapper.m_Game_Move; } }
        public InputAction @Aim { get { return m_Wrapper.m_Game_Aim; } }
        public InputAction @Throw { get { return m_Wrapper.m_Game_Throw; } }
        public InputAction @Start { get { return m_Wrapper.m_Game_Start; } }
        public InputActionMap Get() { return m_Wrapper.m_Game; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled { get { return Get().enabled; } }
        public InputActionMap Clone() { return Get().Clone(); }
        public static implicit operator InputActionMap(GameActions set) { return set.Get(); }
        public void SetCallbacks(IGameActions instance)
        {
            if (m_Wrapper.m_GameActionsCallbackInterface != null)
            {
                Move.started -= m_Wrapper.m_GameActionsCallbackInterface.OnMove;
                Move.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnMove;
                Move.cancelled -= m_Wrapper.m_GameActionsCallbackInterface.OnMove;
                Aim.started -= m_Wrapper.m_GameActionsCallbackInterface.OnAim;
                Aim.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnAim;
                Aim.cancelled -= m_Wrapper.m_GameActionsCallbackInterface.OnAim;
                Throw.started -= m_Wrapper.m_GameActionsCallbackInterface.OnThrow;
                Throw.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnThrow;
                Throw.cancelled -= m_Wrapper.m_GameActionsCallbackInterface.OnThrow;
                Start.started -= m_Wrapper.m_GameActionsCallbackInterface.OnStart;
                Start.performed -= m_Wrapper.m_GameActionsCallbackInterface.OnStart;
                Start.cancelled -= m_Wrapper.m_GameActionsCallbackInterface.OnStart;
            }
            m_Wrapper.m_GameActionsCallbackInterface = instance;
            if (instance != null)
            {
                Move.started += instance.OnMove;
                Move.performed += instance.OnMove;
                Move.cancelled += instance.OnMove;
                Aim.started += instance.OnAim;
                Aim.performed += instance.OnAim;
                Aim.cancelled += instance.OnAim;
                Throw.started += instance.OnThrow;
                Throw.performed += instance.OnThrow;
                Throw.cancelled += instance.OnThrow;
                Start.started += instance.OnStart;
                Start.performed += instance.OnStart;
                Start.cancelled += instance.OnStart;
            }
        }
    }
    public GameActions @Game
    {
        get
        {
            return new GameActions(this);
        }
    }
    // Menu
    private InputActionMap m_Menu;
    private IMenuActions m_MenuActionsCallbackInterface;
    private InputAction m_Menu_LeftSkin;
    private InputAction m_Menu_RightSkin;
    private InputAction m_Menu_Submit;
    private InputAction m_Menu_Back;
    private InputAction m_Menu_Start;
    private InputAction m_Menu_Scrolling;
    public struct MenuActions
    {
        private InputActions m_Wrapper;
        public MenuActions(InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @LeftSkin { get { return m_Wrapper.m_Menu_LeftSkin; } }
        public InputAction @RightSkin { get { return m_Wrapper.m_Menu_RightSkin; } }
        public InputAction @Submit { get { return m_Wrapper.m_Menu_Submit; } }
        public InputAction @Back { get { return m_Wrapper.m_Menu_Back; } }
        public InputAction @Start { get { return m_Wrapper.m_Menu_Start; } }
        public InputAction @Scrolling { get { return m_Wrapper.m_Menu_Scrolling; } }
        public InputActionMap Get() { return m_Wrapper.m_Menu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled { get { return Get().enabled; } }
        public InputActionMap Clone() { return Get().Clone(); }
        public static implicit operator InputActionMap(MenuActions set) { return set.Get(); }
        public void SetCallbacks(IMenuActions instance)
        {
            if (m_Wrapper.m_MenuActionsCallbackInterface != null)
            {
                LeftSkin.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnLeftSkin;
                LeftSkin.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnLeftSkin;
                LeftSkin.cancelled -= m_Wrapper.m_MenuActionsCallbackInterface.OnLeftSkin;
                RightSkin.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnRightSkin;
                RightSkin.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnRightSkin;
                RightSkin.cancelled -= m_Wrapper.m_MenuActionsCallbackInterface.OnRightSkin;
                Submit.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnSubmit;
                Submit.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnSubmit;
                Submit.cancelled -= m_Wrapper.m_MenuActionsCallbackInterface.OnSubmit;
                Back.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnBack;
                Back.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnBack;
                Back.cancelled -= m_Wrapper.m_MenuActionsCallbackInterface.OnBack;
                Start.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnStart;
                Start.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnStart;
                Start.cancelled -= m_Wrapper.m_MenuActionsCallbackInterface.OnStart;
                Scrolling.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnScrolling;
                Scrolling.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnScrolling;
                Scrolling.cancelled -= m_Wrapper.m_MenuActionsCallbackInterface.OnScrolling;
            }
            m_Wrapper.m_MenuActionsCallbackInterface = instance;
            if (instance != null)
            {
                LeftSkin.started += instance.OnLeftSkin;
                LeftSkin.performed += instance.OnLeftSkin;
                LeftSkin.cancelled += instance.OnLeftSkin;
                RightSkin.started += instance.OnRightSkin;
                RightSkin.performed += instance.OnRightSkin;
                RightSkin.cancelled += instance.OnRightSkin;
                Submit.started += instance.OnSubmit;
                Submit.performed += instance.OnSubmit;
                Submit.cancelled += instance.OnSubmit;
                Back.started += instance.OnBack;
                Back.performed += instance.OnBack;
                Back.cancelled += instance.OnBack;
                Start.started += instance.OnStart;
                Start.performed += instance.OnStart;
                Start.cancelled += instance.OnStart;
                Scrolling.started += instance.OnScrolling;
                Scrolling.performed += instance.OnScrolling;
                Scrolling.cancelled += instance.OnScrolling;
            }
        }
    }
    public MenuActions @Menu
    {
        get
        {
            return new MenuActions(this);
        }
    }
    private int m_XboxSchemeIndex = -1;
    public InputControlScheme XboxScheme
    {
        get
        {
            if (m_XboxSchemeIndex == -1) m_XboxSchemeIndex = asset.GetControlSchemeIndex("Xbox");
            return asset.controlSchemes[m_XboxSchemeIndex];
        }
    }
    private int m_PS4SchemeIndex = -1;
    public InputControlScheme PS4Scheme
    {
        get
        {
            if (m_PS4SchemeIndex == -1) m_PS4SchemeIndex = asset.GetControlSchemeIndex("PS4");
            return asset.controlSchemes[m_PS4SchemeIndex];
        }
    }
    private int m_Generic1SchemeIndex = -1;
    public InputControlScheme Generic1Scheme
    {
        get
        {
            if (m_Generic1SchemeIndex == -1) m_Generic1SchemeIndex = asset.GetControlSchemeIndex("Generic1");
            return asset.controlSchemes[m_Generic1SchemeIndex];
        }
    }
    private int m_MKSchemeIndex = -1;
    public InputControlScheme MKScheme
    {
        get
        {
            if (m_MKSchemeIndex == -1) m_MKSchemeIndex = asset.GetControlSchemeIndex("MK");
            return asset.controlSchemes[m_MKSchemeIndex];
        }
    }
    public interface IGameActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnThrow(InputAction.CallbackContext context);
        void OnStart(InputAction.CallbackContext context);
    }
    public interface IMenuActions
    {
        void OnLeftSkin(InputAction.CallbackContext context);
        void OnRightSkin(InputAction.CallbackContext context);
        void OnSubmit(InputAction.CallbackContext context);
        void OnBack(InputAction.CallbackContext context);
        void OnStart(InputAction.CallbackContext context);
        void OnScrolling(InputAction.CallbackContext context);
    }
}
