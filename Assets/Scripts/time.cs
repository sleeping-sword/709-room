using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class time : MonoBehaviour
{
    public TextMeshProUGUI Date;
    public TextMeshProUGUI Time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Date.text = DateTime.Now.Year.ToString() + "/" +  DateTime.Now.Month.ToString() + "/" +  DateTime.Now.Day.ToString();
        Time.text = DateTime.Now.Hour.ToString() + ":" +  DateTime.Now.Minute.ToString();
    }
}
