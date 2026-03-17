using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endManager : MonoBehaviour
{
    public GameObject Result;
    public GameObject Choose;
    public GameObject Show;
    public GameObject Noshow;
    int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        Result.SetActive(false);
        Choose.SetActive(false);
        Show.SetActive(false);
        Noshow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (index == 0 && Input.GetMouseButtonDown(0))
        {
            Result.SetActive(true);
            index++;
        }
        else if(index == 1 && Input.GetMouseButtonDown(0))
        {
            Result.SetActive(false);
            Choose.SetActive(true);
            index++;
        }
    }

    public void chooseshow()
    {
        Show.SetActive(true);
    }

    public void choosenoshow()
    {
        Noshow.SetActive(true);
    }

    public void restart()
    {
        SceneManager.LoadScene(0);
    }
}
