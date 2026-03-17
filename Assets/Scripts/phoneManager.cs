using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class phoneManager : MonoBehaviour
{
    bool usephone = false; //閒置
    public GameObject mouse;
    public GameObject phoneinterface;
    public GameObject config;
    public PlayerInput playermove;
    public PlayerInteraction playerInteraction;
    // Start is called before the first frame update
    void Start()
    {
        phoneinterface.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameFlowManager.Instance.isCatch == false && Input.GetKeyDown(KeyCode.E) && !usephone)
        {
            config.GetComponent<publicConhig>().mouseflag++;
            usephone = !usephone;
            phoneinterface.SetActive(true);
            playermove.enabled = false;
            playerInteraction.canInteract = false;
            mouse.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if(GameFlowManager.Instance.isCatch == false && Input.GetKeyDown(KeyCode.E) && usephone)
        {
            config.GetComponent<publicConhig>().mouseflag--;
            usephone = !usephone;
            phoneinterface.SetActive(false);
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
}
