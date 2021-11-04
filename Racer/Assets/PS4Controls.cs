// GENERATED AUTOMATICALLY FROM 'Assets/PS4Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PS4Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PS4Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PS4Controls"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""105a5c4f-92f4-4df7-9020-14875fa045a7"",
            ""actions"": [
                {
                    ""name"": ""L2"",
                    ""type"": ""Value"",
                    ""id"": ""f78e6031-89fa-434d-bc29-4ea3496722a2"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""R2"",
                    ""type"": ""Value"",
                    ""id"": ""ba24baf2-a827-4eff-915a-45d1e7727360"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Left-Right"",
                    ""type"": ""Value"",
                    ""id"": ""6f926af5-59f0-4682-bdf8-895d0bad52a9"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0a0fbd80-c521-41e4-a3d5-5bbfd318f9be"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""L2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""20381a74-b1fb-45b1-a82f-c3e3a1b52f8d"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""R2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f2557676-c084-45c7-aa31-7f476ef1ec59"",
                    ""path"": ""<Gamepad>/leftStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left-Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_L2 = m_Gameplay.FindAction("L2", throwIfNotFound: true);
        m_Gameplay_R2 = m_Gameplay.FindAction("R2", throwIfNotFound: true);
        m_Gameplay_LeftRight = m_Gameplay.FindAction("Left-Right", throwIfNotFound: true);
    }

    public void Dispose()
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

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

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

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_L2;
    private readonly InputAction m_Gameplay_R2;
    private readonly InputAction m_Gameplay_LeftRight;
    public struct GameplayActions
    {
        private @PS4Controls m_Wrapper;
        public GameplayActions(@PS4Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @L2 => m_Wrapper.m_Gameplay_L2;
        public InputAction @R2 => m_Wrapper.m_Gameplay_R2;
        public InputAction @LeftRight => m_Wrapper.m_Gameplay_LeftRight;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @L2.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnL2;
                @L2.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnL2;
                @L2.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnL2;
                @R2.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnR2;
                @R2.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnR2;
                @R2.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnR2;
                @LeftRight.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLeftRight;
                @LeftRight.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLeftRight;
                @LeftRight.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLeftRight;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @L2.started += instance.OnL2;
                @L2.performed += instance.OnL2;
                @L2.canceled += instance.OnL2;
                @R2.started += instance.OnR2;
                @R2.performed += instance.OnR2;
                @R2.canceled += instance.OnR2;
                @LeftRight.started += instance.OnLeftRight;
                @LeftRight.performed += instance.OnLeftRight;
                @LeftRight.canceled += instance.OnLeftRight;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);
    public interface IGameplayActions
    {
        void OnL2(InputAction.CallbackContext context);
        void OnR2(InputAction.CallbackContext context);
        void OnLeftRight(InputAction.CallbackContext context);
    }
}
