using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class phoneInterface : MonoBehaviour
{
    public GameObject Lock;
    public GameObject home;
    public GameObject chatapp;
    public GameObject ig;
    // Start is called before the first frame update
    void Start()
    {
        Lock.SetActive(true);
        home.SetActive(true);
        chatapp.SetActive(false);
        ig.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HomeToChat()
    {
        home.SetActive(false);
        chatapp.SetActive(true);
        ig.SetActive(false);
    }

    public void HomeToIg()
    {
        home.SetActive(false);
        chatapp.SetActive(false);
        ig.SetActive(true);
    }

    public void BackHome()
    {
        home.SetActive(true);
        chatapp.SetActive(false);
        ig.SetActive(false);
    }
}
