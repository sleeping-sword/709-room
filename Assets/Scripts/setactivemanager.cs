using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class setactivemanager : MonoBehaviour
{
    public GameObject playingUI;
    public GameObject gameUI;
    public GameObject phonemanager;
    // public GameObject esc;
    public GameObject phoneinterface;
    public GameObject bookinterface;
    public PlayerInput playermove;
    public PlayerInteraction playerInteraction;
    // Start is called before the first frame update
    void Start()
    {
        playingUI.SetActive(true);
        gameUI.SetActive(true);
        bookinterface.SetActive(true);
        phonemanager.SetActive(false);
        // esc.SetActive(true);
        phoneinterface.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerInteraction.canInteract = true;
        playermove.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
