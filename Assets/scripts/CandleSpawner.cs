using UnityEngine;
using System.Collections.Generic;

public class CandleSpawner : MonoBehaviour
{
    [Header("Candle Settings")]
    [Tooltip("蜡烛预制体")]
    public GameObject candlePrefab;

    [Tooltip("要生成的蜡烛数量")]
    public int candleCount = 3;

    void Start()
    {
        Debug.Log("CandleSpawner Start 被调用");
        SpawnCandles();
    }

    void SpawnCandles()
    {
        Debug.Log("房间数量: " + RoomManager.Instance.allRooms.Count);
        if (candlePrefab == null)
        {
            Debug.LogError("CandleSpawner: candlePrefab is null");
            return;
        }

        if (RoomManager.Instance == null)
        {
            Debug.LogError("CandleSpawner: RoomManager not found");
            return;
        }

        List<Room> availableRooms = new List<Room>(RoomManager.Instance.allRooms);

        if (availableRooms.Count < candleCount)
        {
            Debug.LogWarning("房间数量不足，自动减少蜡烛数量");
            candleCount = availableRooms.Count;
        }

        for (int i = 0; i < candleCount; i++)
        {
            int index = Random.Range(0, availableRooms.Count);
            Room room = availableRooms[index];
            availableRooms.RemoveAt(index);

            if (room == null)
                continue;

            if (room.candleSpawnPoint == null)
            {
                Debug.LogWarning("房间缺少 CandleSpawnPoint: " + room.name);
                continue;
            }

            Instantiate(
                candlePrefab,
                room.candleSpawnPoint.position,
                room.candleSpawnPoint.rotation
            );
        }

        Debug.Log("CandleSpawner: 蜡烛生成完成");
    }
}
