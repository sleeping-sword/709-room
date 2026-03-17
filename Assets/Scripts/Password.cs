using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Password : MonoBehaviour
{
    public TMP_InputField input;
    public GameObject passwordpage;
    string password;
    string correct_password = "Muffin";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void inputend()
    {
        password = input.text;
    }

    public void check()
    {
        if(password == correct_password)
        {
            Destroy(passwordpage);
        }
    }
}
