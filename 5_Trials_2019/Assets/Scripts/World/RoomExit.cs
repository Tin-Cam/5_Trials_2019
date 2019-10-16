using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomExit : MonoBehaviour
{
    public Room room;

    public int destination;


    private void ChangeRoom()
    {
        room.ChangeRoom(destination);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            ChangeRoom();
    }
}