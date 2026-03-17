using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Seagull.Interior_I1.SceneProps;

public class Light_Switch : MonoBehaviour, IInteractable
{
    public GameObject[] Light;
    public GameObject On;
    public GameObject Off;
    bool isLight = false;
    bool trigger  = false;

    // 當被互動時觸發
    public virtual void OnInteract()
    {
        trigger = true;
    }

    void Start()
    {
        foreach(var i in Light){
            i.GetComponent<LightSourceObject>().onTurnOff.Invoke();
        }
        On.SetActive(false);
        Off.SetActive(true);
    }
    void Update()
    {
        if(trigger && isLight)
        {
            foreach(var i in Light){
                i.GetComponent<LightSourceObject>().onTurnOff.Invoke();
            }   
            trigger = false;
            isLight = !isLight;
            On.SetActive(false);
            Off.SetActive(true);
        }
        else if(trigger && !isLight)
        {
            foreach(var i in Light){
                i.GetComponent<LightSourceObject>().onTurnOn.Invoke();
            }
            trigger = false;
            isLight = !isLight;
            Off.SetActive(false);
            On.SetActive(true);
        }
    }
}
