using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lock : MonoBehaviour
{
    public GameObject lockUI;
    public bool pressbutton = false;
    public int buttonnumber;
    public GameObject[] buttons;
    int count = 0;
    bool correct = false;
    int[] correct_num = {6, 3, 2, 1, 5, 9};
    int[] input_num = {0, 0, 0, 0, 0, 0};
    Color origin = new Color(177f/255f, 191f/255f, 195f/255f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(count == 6)
        {
            count = 0;
            correct = true;
            for(int i = 0; i < 6; i++)
            {
                if(correct_num[i] != input_num[i])
                {
                    correct = false;
                    break;
                }
            }
            if (correct)
            {
                Destroy(lockUI);
            }
            else
            {
                foreach(var b in buttons)
                {
                    b.GetComponent<Image>().color = origin;
                }
            }
        }
        else if(pressbutton)
        {
            input_num[count] = buttonnumber;
            count++;
            pressbutton = false;
        }
    }
}
