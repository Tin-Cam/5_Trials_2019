using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatMode : MonoBehaviour
{
    public bool isActivated;

    private RoomManager roomManager;

    // Start is called before the first frame update
    void Start()
    {
        roomManager = GetComponent<RoomManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isActivated)
            return;

        //If shift is held
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))    
            InterludeSelect();
        else
            RoomSelect();
    }

    private void RoomSelect(){
        if (Input.GetKeyDown("1"))
        {
            roomManager.LoadRoom("Boss1");
        }
        if (Input.GetKeyDown("2"))
        {
            roomManager.LoadRoom("Boss2");
        }
        if (Input.GetKeyDown("3"))
        {
            roomManager.LoadRoom("Boss3");
        }
        if (Input.GetKeyDown("4"))
        {
            roomManager.LoadRoom("Boss4");
        }
        if (Input.GetKeyDown("5"))
        {
            roomManager.LoadRoom("Boss5");
        }
        if (Input.GetKeyDown("6"))
        {
            roomManager.LoadRoom("Boss6");
        }
        if (Input.GetKeyDown("0"))
        {
            roomManager.LoadRoom("Starting_Room");
        }
    }

    private void InterludeSelect(){
        if (Input.GetKeyDown("1"))
        {
            roomManager.LoadInterludeCutscene(1);
        }
        if (Input.GetKeyDown("2"))
        {
            roomManager.LoadInterludeCutscene(2);
        }
        if (Input.GetKeyDown("3"))
        {
            roomManager.LoadInterludeCutscene(3);
        }
        if (Input.GetKeyDown("4"))
        {
            roomManager.LoadInterludeCutscene(4);
        }
        if (Input.GetKeyDown("5"))
        {
            roomManager.LoadInterludeCutscene(5);
        }
        if (Input.GetKeyDown("6"))
        {
            roomManager.LoadInterludeCutscene(6);
        }
        if (Input.GetKeyDown("0"))
        {
            roomManager.LoadInterludeCutscene(0);
        }
    }
}
