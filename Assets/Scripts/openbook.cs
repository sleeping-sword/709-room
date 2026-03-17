using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class openbook : MonoBehaviour, IInteractable
{
    public GameObject bookUI;
    public GameObject mouse;
    public GameObject config;
    public PlayerInput playermove;
    public PlayerInteraction playerInteraction;
    
    public virtual void OnInteract()
    {
        config.GetComponent<publicConhig>().mouseflag++;
        bookUI.SetActive(true);
        playermove.enabled = false;
        playerInteraction.canInteract = false;
        mouse.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        bookUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void closebook()
    {
        config.GetComponent<publicConhig>().mouseflag--;
        bookUI.SetActive(false);
        if(config.GetComponent<publicConhig>().mouseflag == 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            mouse.SetActive(true);
            playerInteraction.canInteract = true;
            playermove.enabled = true;
        }
    }
}
