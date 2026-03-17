using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class gramophoneLock : MonoBehaviour
{
    public TMP_InputField input;
    public GameObject gramophone;
    string password;
    string correct_password = "29393091";

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
        Debug.Log(password);
        Debug.Log(correct_password);
        if(password == correct_password)
        {
            Debug.Log("!!");
            gramophone.GetComponent<Gramophone>().unlock = true;
            gramophone.GetComponent<Gramophone>().open();
            gramophone.GetComponent<Gramophone>().closeLock();
            gramophone.GetComponent<Gramophone>().enabled = false;
        }
    }
}
