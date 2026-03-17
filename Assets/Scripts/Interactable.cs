using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 建立互動介面
public interface IInteractable
{
    void OnInteract();
}

public class Interactable : MonoBehaviour
{
    public float interactDistance = 3f;  // 最大互動距離
    // public IInteractable[] interactableScript;  // 改用介面
    
    public GameObject[] parent;

    //void Awake()
    //{
    //    // 自動取得同物件上實作 IInteractable 的腳本
    //    if(parent == null){
    //        interactableScript = GetComponent<IInteractable>();
    //    }
    //    else
    //    {
    //        interactableScript = parent[0].GetComponent<IInteractable>();
    //    }
    //}
    // void Awake()
    // {
    //     // 修改處：增加判斷 "|| parent.Length == 0"
    //     // 如果 parent 陣列是 null 或者 是空的(長度0)，就找自己身上的腳本
    //     if (parent == null || parent.Length == 0)
    //     {
    //         interactableScript[0] = GetComponent<IInteractable>();
    //     }
    //     else
    //     {
    //         // 確定裡面有東西，才去抓第 0 個
    //         for(int i = 0; i < parent.Length; i++)
    //         {
    //             Debug.Log(i);
    //             interactableScript[i] = parent[i].GetComponent<IInteractable>();
    //         }
            
    //     }
    // }

    // // 當被互動時觸發
    // public virtual void Interact()
    // {
    //     Debug.Log($"{name} 被互動了！");
    //     if(interactableScript != null)
    //     {
    //         // 呼叫任何實現 IInteractable 的腳本
    //         foreach(var i in interactableScript)
    //         {
    //             i.OnInteract();
    //         }
            
    //     }
    //     else
    //     {
    //         Debug.Log($"{name} 沒有互動腳本");
    //     }
    // }

    private IInteractable[] interactableScripts;

    void Awake()
    {
        if (parent == null || parent.Length == 0)
        {
            // 直接抓「自己身上」所有 IInteractable
            interactableScripts = GetComponents<IInteractable>();
        }
        else
        {
            // 從 parent 物件們抓 IInteractable
            List<IInteractable> list = new List<IInteractable>();

            foreach (var p in parent)
            {
                if (p == null) continue;

                var comps = p.GetComponents<IInteractable>();
                list.AddRange(comps);
            }

            interactableScripts = list.ToArray();
        }
    }

    public virtual void Interact()
    {
        if (interactableScripts == null || interactableScripts.Length == 0)
        {
            Debug.Log($"{name} 沒有任何 IInteractable");
            return;
        }

        Debug.Log($"{name} 被互動了！");

        foreach (var i in interactableScripts)
        {
            i.OnInteract();
        }
    }
}
