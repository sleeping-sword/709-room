//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Phone : MonoBehaviour, IInteractable
//{
//    public GameObject phone;
//    public GameObject phonemanager;
//    public GameObject phoneUI;
//    public virtual void OnInteract()
//    {
//        phoneUI.SetActive(true);
//        phonemanager.SetActive(true);
//        phone.SetActive(false);
//    }

//    // Start is called before the first frame update
//    void Start()
//    {
//        phonemanager.SetActive(false);
//        phoneUI.SetActive(false);
//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : MonoBehaviour, IInteractable
{
    [Header("物件連結 (維持原本設定)")]
    public GameObject phone;        // 桌上的 3D 手機物件
    public GameObject phonemanager; // 手機管理器
    public GameObject phoneUI;      // 手機畫面 UI

    [Header("未解鎖時的提示")]
    [TextArea]
    public string lockedMessage = "應該先確認一下房間有沒有其他線索";

    // Start is called before the first frame update
    void Start()
    {
        // 確保遊戲開始時，手機介面是關閉的
        if (phonemanager != null) phonemanager.SetActive(false);
        if (phoneUI != null) phoneUI.SetActive(false);
    }

    // 當玩家點擊手機時觸發
    public virtual void OnInteract()
    {
        // 呼叫檢查函式
        CheckConditionAndOpen();
    }

    void CheckConditionAndOpen()
    {
        // 1. 安全檢查：確認 GameFlowManager 存在
        if (GameFlowManager.Instance == null)
        {
            Debug.LogError("找不到 GameFlowManager！請確認場景中有 GameManager 物件。");
            return;
        }

        // 2. 取得目前進度
        int current = GameFlowManager.Instance.currentCluesFound;
        int needed = GameFlowManager.Instance.cluesNeededForPhone; // 會讀取你在 GameManager 設定的 6

        // 3. 判斷邏輯
        if (current >= needed)
        {
            // --- 條件達成 ---
            Debug.Log("條件達成，開啟手機！");

            // 【新增】1. 告訴總管開始倒數計時 (這行是新加的)
            if (GameFlowManager.Instance != null)
            {
                GameFlowManager.Instance.StartPhoneUnlockEvent();
            }

            // 【重要】2. 執行原本的邏輯 (這三行絕對要保留！)
            // 如果刪掉這些，手機畫面就不會出來了
            if (phoneUI != null) phoneUI.SetActive(true);
            if (phonemanager != null) phonemanager.SetActive(true);
            if (phone != null) phone.SetActive(false);

            // 【選用】3. 顯示提示字幕
            if (UIManager.Instance)
                UIManager.Instance.ShowDialogue("安琪的手機... 密碼應該就在這些線索裡。");
        }
        else
        {

            // 組合提示文字，例如："這時候看手機...(2/6)"
            string statusMsg = $"{lockedMessage}\n(目前收集{current} / {needed})";

            // 呼叫 UI 顯示字幕
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowDialogue(statusMsg);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}