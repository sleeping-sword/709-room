using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Seagull.Interior_I1.SceneProps;

public class Cabinet : MonoBehaviour, IInteractable
{
    bool trigger = false;
    bool open = false;
    public virtual void OnInteract()
    {
        trigger = true;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<RotatableObject>().rotate("0", 1f);
        gameObject.GetComponent<RotatableObject>().rotate("1", 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(trigger && open)
        {
            gameObject.GetComponent<RotatableObject>().rotate("0", 1f);
            gameObject.GetComponent<RotatableObject>().rotate("1", 0f);
            trigger = false;
            open = !open;
        }
        else if(trigger && !open)
        {
            gameObject.GetComponent<RotatableObject>().rotate("0", 0.3f);
            gameObject.GetComponent<RotatableObject>().rotate("1", 0.7f);
            trigger = false;
            open = !open;
        }
    }
}
