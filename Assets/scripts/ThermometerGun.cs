using UnityEngine;
using UnityEngine.XR;

public class ThermometerGun : MonoBehaviour
{
    [Header("Input")]
    public XRNode handNode = XRNode.RightHand;

    [Header("Debug")]
    public bool logTemperature = true;

    public float CurrentTemperature { get; private set; } = 25f;

    private bool lastButtonState = false;

    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(handNode);
        if (!device.isValid)
            return;

        bool buttonPressed;
        if (device.TryGetFeatureValue(CommonUsages.primaryButton, out buttonPressed))
        {
            if (buttonPressed && !lastButtonState)
            {
                MeasureTemperature();
            }

            lastButtonState = buttonPressed;
        }
    }

    void MeasureTemperature()
    {
        if (RoomManager.Instance == null)
        {
            CurrentTemperature = 25f;
            return;
        }

        CurrentTemperature = RoomManager.Instance.GetCurrentTemperature();

        if (logTemperature)
        {
            Debug.Log("Measured Temperature: " + CurrentTemperature + "°„C");
        }
    }
}
