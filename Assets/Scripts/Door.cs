using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Door : MonoBehaviour, IInteractable
{
    // 當被互動時觸發
    public virtual void OnInteract()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void Start()
    {
        // gameObject.GetComponent<RotatableObject>().rotate(1f);
    }
    void Update()
    {
        
    }
}
