using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class RoomExit : MonoBehaviour
{
    public string destination;

    private void ChangeRoom()
    {
        AudioManager.instance.Play("Door_Enter");
        RoomManager.instance.LoadRoom(destination);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            ChangeRoom();
    }
}