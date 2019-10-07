using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomExit : MonoBehaviour
{
    private GameManager gameManager;

    public int destination;

    private void ChangeRoom()
    {
        gameManager.LoadNewRoom(destination);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            ChangeRoom();
    }
    
    public void SetGameManager(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }
}
