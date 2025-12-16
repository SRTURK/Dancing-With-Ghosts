using UnityEngine;
using System.Collections.Generic;

public class CandleManager : MonoBehaviour
{
    public static CandleManager Instance;

    private List<CandleController> candles = new List<CandleController>();
    private bool allLitLogged = false;

    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        CandleController.OnCandleStateChanged += OnCandleStateChanged;
    }

    void OnDisable()
    {
        CandleController.OnCandleStateChanged -= OnCandleStateChanged;
    }

    public void RegisterCandle(CandleController candle)
    {
        if (!candles.Contains(candle))
        {
            candles.Add(candle);
        }
    }

    void Start()
    {
        UpdateState();
    }

    void OnCandleStateChanged(CandleController candle, bool lit)
    {
        UpdateState();
    }

    void UpdateState()
    {
        int litCount = 0;

        foreach (var candle in candles)
        {
            if (candle != null && candle.IsLit)
                litCount++;
        }

        int total = candles.Count;

        Debug.Log($"已点燃蜡烛数量：{litCount}/{total}");

        // 通知 SAN 系统
        if (SanController.Instance != null)
        {
            SanController.Instance.UpdateCandleState(litCount, total);
        }

        if (total > 0 && litCount == total)
        {
            if (!allLitLogged)
            {
                Debug.Log("已点燃全部蜡烛");
                allLitLogged = true;
            }
        }
        else
        {
            allLitLogged = false;
        }
    }
}
