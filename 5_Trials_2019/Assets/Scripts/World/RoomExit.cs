using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class RoomExit : MonoBehaviour
{
    public int destination;

    private void ChangeRoom()
    {
        AudioManager.instance.Play("Door_Enter");
        RoomLoader.instance.LoadRoom(destination);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            ChangeRoom();
    }
}