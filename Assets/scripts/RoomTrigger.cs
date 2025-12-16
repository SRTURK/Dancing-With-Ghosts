using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    private Room room;

    void Awake()
    {
        room = GetComponentInParent<Room>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        RoomManager.Instance.EnterRoom(room);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        RoomManager.Instance.ExitRoom(room);
    }
}
