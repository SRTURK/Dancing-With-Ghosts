using UnityEngine;
using TMPro;

public class TemperatureTextDisplay : MonoBehaviour
{
    public ThermometerGun thermometerGun;

    private TMP_Text text;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (thermometerGun == null || text == null)
            return;

        text.text = "Temperature: "
            + thermometerGun.CurrentTemperature.ToString("F1")
            + " °„C";
    }
}
