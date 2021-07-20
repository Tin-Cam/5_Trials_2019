using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatMode : MonoBehaviour
{
    public bool isActivated;

    private RoomManager roomManager;
    private AudioManager audioManager;
    private string codeInput;

    // Start is called before the first frame update
    void Start()
    {
        roomManager = GetComponent<RoomManager>();
        audioManager = AudioManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isActivated){
            CheckCode();
            return;
        }
        //If shift is held
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){   
            InterludeSelect();
            PlayerAction();
            }
        else {
            RoomSelect();
            BossAction();
        }

    }

    //Checks if the secret code has been entered while holding shift
    private void CheckCode(){
        if(!(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))){
            codeInput = "";
            return;
        }
        //Code is 78387 (Written using shift symbols)
        string secretCode = "&#*#&";

        foreach (char c in Input.inputString)
        {
            codeInput += c;

            if(codeInput.Length > secretCode.Length)
                codeInput = codeInput.Substring(1);
        }
        if(codeInput == secretCode){
            isActivated = true;
            audioManager.Play("Pop_Up", 0.8f, 2f);
        }

        Debug.Log(codeInput);
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

    private void BossAction(){
        int command = 0;

        //Damage Boss
        if(Input.GetKeyDown(KeyCode.H))
            command = 1;
        //Kill Boss
        if(Input.GetKeyDown(KeyCode.K))
            command = 2;

        if(command != 0)
            GameManagerCommands(command);
    }

    private void PlayerAction(){
        int command = 0;

        //Damage Player
        if(Input.GetKeyDown(KeyCode.H))
            command = 11;
        //Kill Player
        if(Input.GetKeyDown(KeyCode.K))
            command = 12;
        //Toggle God mode
        if(Input.GetKeyDown(KeyCode.G))
            command = 13;

        if(command != 0)
            GameManagerCommands(command);
    }

    private void GameManagerCommands(int command){
        Debug.Log(command);
        GameManager gameManager = FindObjectOfType<GameManager>();
        if(gameManager == null)
            return;
        try{
            if(command == 1)
                gameManager.GetBoss().TakeDamage(1);
            if(command == 2)            
                gameManager.GetBoss().StartDeath();
            if(command == 11)
                gameManager.PlayerTakeDamage(1);
            if(command == 12)
                gameManager.PlayerTakeDamage(100);
            if(command == 13){
                gameManager.godMode = !gameManager.godMode;
                if(gameManager.godMode)
                    audioManager.Play("Door_Open", 0.80f, 2f);
                else
                    audioManager.Play("Discard", 0.80f, 0.5f);
            }
        }
        catch(System.NullReferenceException){
            Debug.LogError("Cheat Mode: Can't find stuff");
        }
    }
}
