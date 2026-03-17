using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class ESCManager : MonoBehaviour
{
    bool pause = false;
    public TextMeshProUGUI text;
    public GameObject mouse;
    public GameObject pauseUI;
    public GameObject config;
    public PlayerInput playermove;
    public PlayerInteraction playerInteraction;
    // Start is called before the first frame update
    void Start()
    {
        pauseUI.SetActive(false);
        text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (GameFlowManager.Instance.isCatch == false && Input.GetKeyDown(KeyCode.Escape) && !pause)
        {
            config.GetComponent<publicConhig>().mouseflag++;
            pause = !pause;
            playermove.enabled = false;
            playerInteraction.canInteract = false;
            mouse.SetActive(false);
            pauseUI.SetActive(true);
            // Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if(GameFlowManager.Instance.isCatch == false && Input.GetKeyDown(KeyCode.Escape) && pause)
        {
            config.GetComponent<publicConhig>().mouseflag--;
            pause = !pause;
            pauseUI.SetActive(false);
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

    public void restart()
    {
        text.text = "重新進入房間中，請稍後";
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
