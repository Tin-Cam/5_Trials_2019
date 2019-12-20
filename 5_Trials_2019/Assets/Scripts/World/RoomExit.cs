using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class RoomExit : MonoBehaviour
{
    public Room room;

    public int destination;

    private void ChangeRoom()
    {
        AudioManager.instance.Play("Door_Enter");
        room.ChangeRoom(destination);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            ChangeRoom();
    }


}