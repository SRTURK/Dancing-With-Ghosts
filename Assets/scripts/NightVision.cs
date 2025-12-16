using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.XR.Interaction.Toolkit;

public class NightVisionController : MonoBehaviour
{
    [Header("Visual Effects")]
    [Tooltip("拖拽场景里的 Post-process Volume 物体")]
    public PostProcessVolume nightVisionVolume;

    [Header("左右手 Primary Button 输入")]
    public InputActionProperty leftPrimaryInput;
    public InputActionProperty rightPrimaryInput;

    private XRGrabInteractable grabInteractable;
    private bool isNightVisionOn = false;

    // 关键：记录当前是被哪只手拿着
    private bool isHeldByLeft = false;
    private bool isHeldByRight = false;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        // 绑定抓取和松开事件
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        // 启用输入动作
        leftPrimaryInput.action.Enable();
        rightPrimaryInput.action.Enable();
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);

        leftPrimaryInput.action.Disable();
        rightPrimaryInput.action.Disable();
    }

    private void Update()
    {
        // === 严格的隔离逻辑 ===

        // 情况1：如果被左手拿着，只监听左手按键
        if (isHeldByLeft && leftPrimaryInput.action.WasPressedThisFrame())
        {
            Debug.Log("左手触发夜视仪");
            ToggleNightVision();
        }

        // 情况2：如果被右手拿着，只监听右手按键
        else if (isHeldByRight && rightPrimaryInput.action.WasPressedThisFrame())
        {
            Debug.Log("右手触发夜视仪");
            ToggleNightVision();
        }
    }

    // ================== 抓取检测 (和你的手电筒逻辑保持一致) ==================

    private void OnGrab(SelectEnterEventArgs args)
    {
        // 获取抓取者的名字，判断是左手还是右手
        // 这里的判断依据你的 XR Origin 里的名字，通常包含 "Left" 或 "Right"
        string interactorName = args.interactorObject.transform.name;

        if (interactorName.Contains("Left"))
        {
            isHeldByLeft = true;
            isHeldByRight = false;
        }
        else if (interactorName.Contains("Right"))
        {
            isHeldByRight = true;
            isHeldByLeft = false;
        }
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        // 松手时，重置状态并强制关闭夜视仪
        isHeldByLeft = false;
        isHeldByRight = false;

        if (isNightVisionOn)
        {
            isNightVisionOn = false;
            UpdateVisuals();
        }
    }

    // ================== 功能逻辑 ==================

    public void ToggleNightVision()
    {
        isNightVisionOn = !isNightVisionOn;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (nightVisionVolume != null)
        {
            nightVisionVolume.weight = isNightVisionOn ? 1f : 0f;
        }
    }
}