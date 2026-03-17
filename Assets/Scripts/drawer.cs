using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawer : MonoBehaviour, IInteractable
{
    public Transform Drawer;
    Vector3 close_position;
    Vector3 open_position;
    bool isopen = false;
    public virtual void OnInteract()
    {
        if (isopen)
        {
            Drawer.position = close_position;
            isopen = !isopen;
        }
        else if (!isopen)
        {
            Drawer.position = open_position;
            isopen = !isopen;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        close_position = Drawer.position;
        open_position = new Vector3(close_position.x, close_position.y, close_position.z - 0.6f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
