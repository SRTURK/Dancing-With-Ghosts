using UnityEngine;
using UnityEditor;

public class RoomTriggerGenerator : EditorWindow
{
    GameObject asylumRoot;

    // 需要排除的房间名字
    string[] excludeNames = new string[]
    {
        "Pillar",
        "HallDownstair",
        "WoodDownstair",
        "Basement_Stairs",
        "HallUpstair",
        "WoodUpstair",
        "Dome",
        "HallUnderground"
    };

    [MenuItem("Tools/Generate Room Triggers (Exact)")]
    public static void ShowWindow()
    {
        GetWindow<RoomTriggerGenerator>("Room Trigger Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("生成房间触发器（自动命名 + 自动绑定脚本）", EditorStyles.boldLabel);

        asylumRoot = EditorGUILayout.ObjectField("Asylum 根节点", asylumRoot, typeof(GameObject), true) as GameObject;

        if (GUILayout.Button("生成"))
        {
            if (asylumRoot == null)
            {
                Debug.LogError("请指定 Asylum 根节点。");
                return;
            }

            GenerateTriggers();
        }
    }

    void GenerateTriggers()
    {
        int count = 0;

        foreach (Transform floor in asylumRoot.transform)
        {
            foreach (Transform room in floor)
            {
                if (ShouldExclude(room.name))
                    continue;

                string triggerName = room.name + "Trigger";

                // 已存在 Trigger 则跳过
                Transform existing = room.Find(triggerName);
                if (existing != null)
                {
                    Debug.Log("跳过（已存在 Trigger）：" + triggerName);
                    continue;
                }

                // 获取房间所有 Mesh Renderer
                Renderer[] renderers = room.GetComponentsInChildren<Renderer>();
                if (renderers.Length == 0)
                {
                    Debug.LogWarning("房间缺少 Renderer：" + room.name);
                    continue;
                }

                // 计算 Bounds
                Bounds bounds = new Bounds(renderers[0].bounds.center, Vector3.zero);
                foreach (Renderer r in renderers)
                    bounds.Encapsulate(r.bounds);

                // 创建 Trigger 空物体
                GameObject trigger = new GameObject(triggerName);
                Undo.RegisterCreatedObjectUndo(trigger, "Create Room Trigger");
                trigger.transform.SetParent(room);
                trigger.transform.position = bounds.center;
                trigger.transform.rotation = Quaternion.identity;
                trigger.transform.localScale = Vector3.one;

                // 添加 BoxCollider
                BoxCollider bc = trigger.AddComponent<BoxCollider>();
                bc.isTrigger = true;
                bc.size = bounds.size;
                bc.center = Vector3.zero;

                // 自动挂载 RoomTrigger 脚本
                MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>("Assets/Scripts/RoomTrigger.cs");
                if (script != null)
                {
                    trigger.AddComponent(script.GetClass());
                }
                else
                {
                    Debug.LogError("无法找到 RoomTrigger.cs，请确认路径：Assets/Scripts/RoomTrigger.cs");
                }

                count++;
                Debug.Log("生成触发器：" + triggerName);
            }
        }

        Debug.Log("全部房间触发器生成完成，共：" + count);
    }

    bool ShouldExclude(string name)
    {
        foreach (string ex in excludeNames)
        {
            if (name.Equals(ex, System.StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }
}
