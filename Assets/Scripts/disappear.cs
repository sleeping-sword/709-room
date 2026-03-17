using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disappear : MonoBehaviour, IInteractable
{
    public GameObject disappearThing;
    public GameObject keyUI;
    
    public virtual void OnInteract()
    {
        keyUI.SetActive(true);
        disappearThing.SetActive(false);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        keyUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
