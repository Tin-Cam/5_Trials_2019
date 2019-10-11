using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainDoor : MonoBehaviour
{
    protected Animator animator;
    protected Collider2D collision;

    public RoomExit exit;

    void Start()
    {
        animator = GetComponent<Animator>();
        collision = GetComponent<BoxCollider2D>();

        exit.gameObject.SetActive(false);
    }

    public void Open(bool isOpen)
    {
        OpenDoor(isOpen);
        SetDoorColision(isOpen);
        exit.gameObject.SetActive(isOpen);
    }

    private void OpenDoor(bool isOpen)
    {
        if (isOpen)
            animator.SetTrigger("Open");
        else
            animator.SetTrigger("Close");
    }

    private void SetDoorColision(bool isOpen)
    {
        collision.enabled = !isOpen;
    }
}
