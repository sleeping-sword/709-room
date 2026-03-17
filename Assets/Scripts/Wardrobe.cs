using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Seagull.Interior_I1.SceneProps;

public class Wardrobe : MonoBehaviour, IInteractable
{
    bool trigger = false;
    bool open = false;
    // ★★★ 新增：記錄玩家有沒有站進來 ★★★
    private bool isPlayerInside = false;
    public virtual void OnInteract()
    {
        trigger = true;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<RotatableObject>().rotate("0", 0f);
        gameObject.GetComponent<RotatableObject>().rotate("1", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if(trigger && open)
        {
            gameObject.GetComponent<RotatableObject>().rotate("0", 0f);
            gameObject.GetComponent<RotatableObject>().rotate("1", 1f);
            trigger = false;
            open = !open;
            // 關門後，立刻檢查是不是安全了
            CheckHidingStatus();
        }
        else if(trigger && !open)
        {
            gameObject.GetComponent<RotatableObject>().rotate("0", 0.6f);
            gameObject.GetComponent<RotatableObject>().rotate("1", 0.4f);
            trigger = false;
            open = !open;
            // ★★★ 開門時也要檢查 (變成危險狀態) ★★★
            CheckHidingStatus();
        }
    }

    // ---------------------------------------------------------
    // ★★★ 以下是新增的偵測功能 (Trigger) ★★★
    // ---------------------------------------------------------

    // 1. 玩家走進來
    private void OnTriggerEnter(Collider other)
    {
        // 記得把主角的 Tag 改成 "Player"
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            Debug.Log("玩家進入櫃子");
            CheckHidingStatus();
        }
    }

    // 2. 玩家走出去
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            Debug.Log("玩家離開櫃子");
            CheckHidingStatus();
        }
    }

    // 3. 檢查並回報給 GameFlowManager
    void CheckHidingStatus()
    {
        // 條件：門是關的 (!open) 且 玩家在裡面 (isPlayerInside)
        bool isSafe = !open && isPlayerInside;

        // 呼叫經理更新狀態
        if (GameFlowManager.Instance != null)
        {
            GameFlowManager.Instance.SetPlayerHiddenState(isSafe);
        }
    }

}

