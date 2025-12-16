using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Collider))]
public class CustomDoorController : MonoBehaviour
{
    [Header("Door Rotation")]
    public float angleClosed = 0f;
    public float angleOpened = 90f;
    public float duration = 1.0f;

    [Header("Ray Interactors")]
    public XRRayInteractor leftRay;
    public XRRayInteractor rightRay;

    private bool isOpen = false;
    private bool isAnimating = false;
    private bool lastButtonState = false;

    void Update()
    {
        if (isAnimating)
            return;

        // 左手优先
        if (CheckRayAndInput(leftRay, XRNode.LeftHand))
            return;

        // 再检查右手
        CheckRayAndInput(rightRay, XRNode.RightHand);
    }

    bool CheckRayAndInput(XRRayInteractor ray, XRNode node)
    {
        if (ray == null)
            return false;

        if (!ray.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            return false;

        if (hit.collider.gameObject != gameObject)
            return false;

        InputDevice device = InputDevices.GetDeviceAtXRNode(node);
        if (!device.isValid)
            return false;

        bool pressed;
        if (device.TryGetFeatureValue(CommonUsages.secondaryButton, out pressed))
        {
            if (pressed && !lastButtonState)
            {
                ToggleDoor();
            }

            lastButtonState = pressed;
        }

        return true;
    }

    void ToggleDoor()
    {
        isOpen = !isOpen;
        float targetY = isOpen ? angleOpened : angleClosed;
        StartCoroutine(RotateDoorProcess(targetY));
    }

    IEnumerator RotateDoorProcess(float targetAngleY)
    {
        isAnimating = true;

        Quaternion startRotation = transform.localRotation;
        Quaternion endRotation = Quaternion.Euler(
            transform.localEulerAngles.x,
            targetAngleY,
            transform.localEulerAngles.z
        );

        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            transform.localRotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }

        transform.localRotation = endRotation;
        isAnimating = false;
    }
}
