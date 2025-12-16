using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

public class SaltJar : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject saltPilePrefab;
    public float spawnHeight = 0.01f;

    [Header("Spawn Limit")]
    public int maxSpawnTimes = 3;

    private int spawnedCount = 0;

    private XRGrabInteractable grabInteractable;
    private XRBaseInteractor currentInteractor;

    private InputDevice leftHand;
    private InputDevice rightHand;

    private bool lastPrimaryState = false;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void Start()
    {
        InitDevices();

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void InitDevices()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, devices);
        if (devices.Count > 0)
            leftHand = devices[0];

        devices.Clear();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);
        if (devices.Count > 0)
            rightHand = devices[0];
    }

    void Update()
    {
        if (currentInteractor == null)
            return;

        if (!leftHand.isValid || !rightHand.isValid)
        {
            InitDevices();
            return;
        }

        bool currentPrimaryState = false;

        if (IsLeftHandInteractor(currentInteractor))
        {
            leftHand.TryGetFeatureValue(CommonUsages.primaryButton, out currentPrimaryState);
        }
        else if (IsRightHandInteractor(currentInteractor))
        {
            rightHand.TryGetFeatureValue(CommonUsages.primaryButton, out currentPrimaryState);
        }

        if (!lastPrimaryState && currentPrimaryState)
        {
            TrySpawnSalt();
        }

        lastPrimaryState = currentPrimaryState;
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        currentInteractor = args.interactorObject as XRBaseInteractor;
        lastPrimaryState = false;
        Debug.Log("Salt jar grabbed by: " + currentInteractor.name);
    }

    void OnRelease(SelectExitEventArgs args)
    {
        currentInteractor = null;
        lastPrimaryState = false;
        Debug.Log("Salt jar released");
    }

    bool IsLeftHandInteractor(XRBaseInteractor interactor)
    {
        return interactor.name.ToLower().Contains("left");
    }

    bool IsRightHandInteractor(XRBaseInteractor interactor)
    {
        return interactor.name.ToLower().Contains("right");
    }

    void TrySpawnSalt()
    {
        if (spawnedCount >= maxSpawnTimes)
        {
            Debug.Log("Salt empty");
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10f))
        {
            Vector3 spawnPos = hit.point + Vector3.up * spawnHeight;
            Instantiate(saltPilePrefab, spawnPos, Quaternion.identity);

            spawnedCount++;
            Debug.Log("Spawn count " + spawnedCount + " / " + maxSpawnTimes);
        }
        else
        {
            Debug.LogWarning("Raycast did not hit ground");
        }
    }
}
