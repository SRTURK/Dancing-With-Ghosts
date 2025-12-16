using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandGrabDebugTest : MonoBehaviour
{
    [Header("左手 Ray Interactor")]
    public XRBaseInteractor leftHandInteractor;

    [Header("右手 Ray Interactor")]
    public XRBaseInteractor rightHandInteractor;

    void OnEnable()
    {
        if (leftHandInteractor != null)
        {
            leftHandInteractor.selectEntered.AddListener(OnLeftGrab);
            leftHandInteractor.selectExited.AddListener(OnLeftRelease);
        }

        if (rightHandInteractor != null)
        {
            rightHandInteractor.selectEntered.AddListener(OnRightGrab);
            rightHandInteractor.selectExited.AddListener(OnRightRelease);
        }
    }

    void OnDisable()
    {
        if (leftHandInteractor != null)
        {
            leftHandInteractor.selectEntered.RemoveListener(OnLeftGrab);
            leftHandInteractor.selectExited.RemoveListener(OnLeftRelease);
        }

        if (rightHandInteractor != null)
        {
            rightHandInteractor.selectEntered.RemoveListener(OnRightGrab);
            rightHandInteractor.selectExited.RemoveListener(OnRightRelease);
        }
    }

    // ================= 左手 =================

    void OnLeftGrab(SelectEnterEventArgs args)
    {
        // 获取被抓取物体的名字
        string objName = args.interactableObject.transform.name;
        Debug.Log($"【左手 拿起】物体：{objName}");

        CheckTargetObject(objName, "左手");
    }

    void OnLeftRelease(SelectExitEventArgs args)
    {
        string objName = args.interactableObject.transform.name;
        Debug.Log($"【左手 放下】物体：{objName}");
    }

    // ================= 右手 =================

    void OnRightGrab(SelectEnterEventArgs args)
    {
        string objName = args.interactableObject.transform.name;
        Debug.Log($"【右手 拿起】物体：{objName}");

        CheckTargetObject(objName, "右手");
    }

    void OnRightRelease(SelectExitEventArgs args)
    {
        string objName = args.interactableObject.transform.name;
        Debug.Log($"【右手 放下】物体：{objName}");
    }

    // ================= 物体类型识别 =================

    void CheckTargetObject(string objName, string hand)
    {
        // 这里的逻辑已修改：
        // 1. 使用 == 替代了 Contains，实现精确识别。
        // 2. 增加了 Firstaid_2 和 TemperatureGun。

        if (objName == "Camera")
        {
            Debug.Log($">>> {hand} 拿到了【相机 Camera】");
        }
        else if (objName == "Flashlight")
        {
            Debug.Log($">>> {hand} 拿到了【手电 Flashlight】");
        }
        else if (objName == "Firstaid")
        {
            Debug.Log($">>> {hand} 拿到了【急救包 Firstaid】");
        }
        // --- 新增部分 ---
        else if (objName == "Firstaid_2")
        {
            Debug.Log($">>> {hand} 拿到了【急救包二号 Firstaid_2】");
        }
        else if (objName == "TemperatureGun")
        {
            Debug.Log($">>> {hand} 拿到了【额温枪 TemperatureGun】");
        }
        // ----------------
        else
        {
            // 如果名字哪怕错一个字符（比如多了空格），都会进入这里
            Debug.Log($">>> {hand} 拿到的是【未知物体】(名称: {objName})");
        }
    }
}