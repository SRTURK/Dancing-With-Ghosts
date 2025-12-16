using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class FlashlightController : MonoBehaviour
{
    [Header("手电筒的灯")]
    public Light spotLight;   // 拖你的 Spot Light 进来

    [Header("左右手 Button1 输入")]
    public InputActionProperty leftButton1;
    public InputActionProperty rightButton1;

    private XRGrabInteractable grabInteractable;
    private bool isOn = false;

    // 当前是左手还是右手拿着
    private bool isHeldByLeft = false;
    private bool isHeldByRight = false;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        leftButton1.action.Enable();
        rightButton1.action.Enable();
    }

    void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);

        leftButton1.action.Disable();
        rightButton1.action.Disable();
    }

    void Update()
    {
        // 只有被拿着时才允许切换
        if (isHeldByLeft && leftButton1.action.WasPressedThisFrame())
        {
            ToggleLight("左手");
        }

        if (isHeldByRight && rightButton1.action.WasPressedThisFrame())
        {
            ToggleLight("右手");
        }
    }

    // ================== 抓取事件 ==================

    void OnGrab(SelectEnterEventArgs args)
    {
        string interactorName = args.interactorObject.transform.name;

        if (interactorName.Contains("Left"))
        {
            isHeldByLeft = true;
            isHeldByRight = false;
            Debug.Log("Flashlight 被【左手】拿起");
        }
        else if (interactorName.Contains("Right"))
        {
            isHeldByRight = true;
            isHeldByLeft = false;
            Debug.Log("Flashlight 被【右手】拿起");
        }
    }

    void OnRelease(SelectExitEventArgs args)
    {
        isHeldByLeft = false;
        isHeldByRight = false;
        Debug.Log("Flashlight 被放下");
    }

    // ================== 手电筒开关 ==================

    void ToggleLight(string hand)
    {
        isOn = !isOn;
        spotLight.enabled = isOn;

        Debug.Log($"【{hand}】切换手电筒状态：{(isOn ? "打开" : "关闭")}");
    }
}

