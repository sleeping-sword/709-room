using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using Seagull.Interior_I1.SceneProps;

public class Gramophone : MonoBehaviour, IInteractable
{
    public GameObject LockUI;
    public GameObject mouse;
    public GameObject config;
    public PlayerInput playermove;
    public PlayerInteraction playerInteraction;
    public bool unlock = false;
    public virtual void OnInteract()
    {
        if (!unlock)
        {
            config.GetComponent<publicConhig>().mouseflag++;
            LockUI.SetActive(true);
            playermove.enabled = false;
            playerInteraction.canInteract = false;
            mouse.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        // if(open)
        // {
        //     gameObject.GetComponent<ShiftableObject>().shift("0", 0f);
        //     open = !open;
        // }
        // else if(!open)
        // {
        //     gameObject.GetComponent<ShiftableObject>().shift("0", 1f);
        //     open = !open;
        // }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        LockUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void closeLock()
    {
        config.GetComponent<publicConhig>().mouseflag--;
        LockUI.SetActive(false);
        if(config.GetComponent<publicConhig>().mouseflag == 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            mouse.SetActive(true);
            playerInteraction.canInteract = true;
            playermove.enabled = true;
        }
    }

    public void open()
    {
        gameObject.GetComponent<ShiftableObject>().shift("0", 1f);
    }
}
