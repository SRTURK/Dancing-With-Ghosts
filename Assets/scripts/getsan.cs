using UnityEngine;
using TMPro;

public class SanTextDisplay : MonoBehaviour
{
    [Header("Reference")]
    public SanController sanController;

    private TMP_Text text;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (sanController == null || text == null)
            return;

        text.text = "SAN: " + Mathf.CeilToInt(sanController.currentSan);
    }
}
