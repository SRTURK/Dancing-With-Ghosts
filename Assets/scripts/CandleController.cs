using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using System;

public class CandleController : MonoBehaviour
{
    public GameObject flameObject;

    public bool IsLit { get; private set; } = false;

    public static event Action<CandleController, bool> OnCandleStateChanged;

    private XRBaseInteractable interactable;
    private bool lastButtonState = false;
    private SpriteRenderer flameRenderer;

    void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();
        flameRenderer = flameObject.GetComponent<SpriteRenderer>();

        SetFlame(false);
    }

    void Start()
    {
        if (CandleManager.Instance != null)
        {
            CandleManager.Instance.RegisterCandle(this);
        }
    }

    void Update()
    {
        if (!interactable.isHovered)
        {
            lastButtonState = false;
            return;
        }

        if (interactable.interactorsHovering.Count == 0)
            return;

        // ★ 关键修正点在这里
        IXRHoverInteractor hoverInteractor = interactable.interactorsHovering[0];
        XRBaseInteractor xrInteractor = hoverInteractor as XRBaseInteractor;

        if (xrInteractor == null)
            return;

        InputDevice device = GetDeviceFromInteractor(xrInteractor);
        if (!device.isValid)
            return;

        if (device.TryGetFeatureValue(CommonUsages.secondaryButton, out bool pressed))
        {
            if (pressed && !lastButtonState)
            {
                ToggleFlame();
            }
            lastButtonState = pressed;
        }
    }

    void ToggleFlame()
    {
        SetFlame(!IsLit);
    }

    void SetFlame(bool lit)
    {
        IsLit = lit;

        if (flameRenderer != null)
            flameRenderer.enabled = lit;

        OnCandleStateChanged?.Invoke(this, IsLit);
    }

    InputDevice GetDeviceFromInteractor(XRBaseInteractor interactor)
    {
        bool isLeft = interactor.name.ToLower().Contains("left");
        bool isRight = interactor.name.ToLower().Contains("right");

        InputDeviceCharacteristics c = InputDeviceCharacteristics.Controller;
        if (isLeft) c |= InputDeviceCharacteristics.Left;
        if (isRight) c |= InputDeviceCharacteristics.Right;

        var devices = new System.Collections.Generic.List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(c, devices);

        return devices.Count > 0 ? devices[0] : default;
    }
}
