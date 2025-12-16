using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    [Header("All Rooms")]
    public List<Room> allRooms = new List<Room>();

    [Header("Runtime State")]
    private List<Room> activeRooms = new List<Room>();

    public Room CurrentRoom
    {
        get
        {
            if (activeRooms.Count == 0)
                return null;

            return activeRooms[activeRooms.Count - 1];
        }
    }

    void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        CollectRooms();
    }

    void Start()
    {
        
        InitializeRooms();
    }

    void CollectRooms()
    {
        allRooms.Clear();
        allRooms.AddRange(FindObjectsOfType<Room>());
        Debug.Log("RoomManager 收集房间数量: " + allRooms.Count);
    }

    void InitializeRooms()
    {
        if (allRooms.Count == 0)
            return;

        int ghostIndex = Random.Range(0, allRooms.Count);
        Room ghostRoom = allRooms[ghostIndex];

        foreach (Room room in allRooms)
        {
            if (room == ghostRoom)
            {
                room.isGhostRoom = true;
                room.temperature = 5f;
            }
            else
            {
                room.isGhostRoom = false;
                room.temperature = Random.Range(20f, 28f);
            }
        }

        Debug.Log($"Ghost Room is: {ghostRoom.name}");
    }

    public void EnterRoom(Room room)
    {
        if (!activeRooms.Contains(room))
        {
            activeRooms.Add(room);
            Debug.Log($"Enter Room: {room.name}, Temp: {room.temperature}");
        }
    }

    public void ExitRoom(Room room)
    {
        if (activeRooms.Contains(room))
        {
            activeRooms.Remove(room);
            Debug.Log($"Exit Room: {room.name}");
        }
    }

    public float GetCurrentTemperature()
    {
        if (CurrentRoom == null)
            return 25f;

        return CurrentRoom.temperature;
    }
}
