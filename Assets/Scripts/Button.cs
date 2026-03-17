using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    public GameObject lockmanager;
    Color presscolor = new Color(234f/255f, 211f/255f, 123f/255f);
    // Start is called before the first frame update
    void Start()
    {
        lockmanager = GameObject.Find("lock");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButton()
    {
        this.GetComponent<Image>().color = presscolor;
        lockmanager.GetComponent<Lock>().buttonnumber = int.Parse(this.tag);
        lockmanager.GetComponent<Lock>().pressbutton = true;
    }
}
