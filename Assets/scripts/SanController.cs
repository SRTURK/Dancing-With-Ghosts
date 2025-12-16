using UnityEngine;

public class SanController : MonoBehaviour
{
    public static SanController Instance;

    [Header("San Settings")]
    public float maxSan = 100f;
    public float currentSan = 100f;

    [Header("Drain Speed")]
    [Tooltip("0 支蜡烛时每秒下降")]
    public float drainSpeedWhenZero = 1f;   // x

    [Tooltip("全部蜡烛点燃时每秒下降")]
    public float drainSpeedWhenAll = 0.2f;  // y

    private float currentDrainSpeed;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentSan = maxSan;
    }

    void Update()
    {
        currentSan -= currentDrainSpeed * Time.deltaTime;
        currentSan = Mathf.Clamp(currentSan, 0f, maxSan);
    }

    // 由 CandleManager 调用
    public void UpdateCandleState(int litCount, int totalCount)
    {
        if (totalCount <= 0)
        {
            currentDrainSpeed = drainSpeedWhenZero;
            return;
        }

        float t = (float)litCount / totalCount;
        currentDrainSpeed = Mathf.Lerp(drainSpeedWhenZero, drainSpeedWhenAll, t);

        Debug.Log(
            $"San: {currentSan:F1}/{maxSan}, " +
            $"Candles: {litCount}/{totalCount}, " +
            $"Drain: {currentDrainSpeed:F2}/s"
        );
    }
}
