//using UnityEngine;
//using System.Collections;
//using UnityEngine.InputSystem;

//public class SecretDrawer : MonoBehaviour, IInteractable
//{
//    [Header("設定")]
//    public string requiredKeyName = "Key_WashingMachine"; // 必須跟鑰匙 ItemName 一模一樣
//    public GameObject docsUI; // 剛剛做的文件 UI Panel
//    public GameObject mouse;
//    public GameObject config;
//    public PlayerInput playermove;
//    public PlayerInteraction playerInteraction;

//    [Header("對話設定")]
//    public string lockedMessage = "鎖住了... 應該有個銀色鑰匙藏在某個地方。";
//    public string unlockMessage = "鑰匙剛好吻合！裡面好像有東西...";

//    private bool isUnlocked = false; // 記錄是否已經解鎖過
//    private bool isUiOpen = false;   // UI 開關狀態
//    // private bool canClose = false;   // 防手滑冷卻

//    void Start()
//    {
//        if (docsUI != null) docsUI.SetActive(false);
//    }

//    public void OnInteract()
//    {
//        // 如果 UI 已經打開，不要重複觸發
//        if (isUiOpen) return;

//        // 檢查背包有沒有鑰匙
//        if (GameFlowManager.Instance.HasItem(requiredKeyName) || isUnlocked)
//        {
//            // 有鑰匙 (或是之前已經開過了) -> 打開抽屜
//            if (!isUnlocked)
//            {
//                // 第一次打開，播放對話
//                if (UIManager.Instance != null) UIManager.Instance.ShowDialogue(unlockMessage);
//                isUnlocked = true; // 標記為已解鎖，下次就不用再檢查鑰匙
//                return;
//            }

//            config.GetComponent<publicConhig>().mouseflag++;
//            playermove.enabled = false;
//            playerInteraction.canInteract = false;
//            mouse.SetActive(false);
//            Cursor.lockState = CursorLockMode.None;
//            Cursor.visible = true;
//            OpenDocs();
//        }
//        else
//        {
//            // 沒鑰匙 -> 顯示鎖住訊息
//            if (UIManager.Instance != null) UIManager.Instance.ShowDialogue(lockedMessage);
//        }
//    }

//    void OpenDocs()
//    {
//        isUiOpen = true;
//        // canClose = false;

//        if (docsUI != null) docsUI.SetActive(true);

//        // 0.5秒冷卻，避免按E開鎖時瞬間關掉
//        StartCoroutine(EnableCloseDelay());
//    }

//    IEnumerator EnableCloseDelay()
//    {
//        yield return new WaitForSeconds(0.5f);
//        // canClose = true;
//    }

//    public void CloseDocs()
//    {
//        isUiOpen = false;
//        // canClose = false;
//        config.GetComponent<publicConhig>().mouseflag--;
//        if (docsUI != null) docsUI.SetActive(false);
//        if(config.GetComponent<publicConhig>().mouseflag == 0)
//        {
//            Cursor.lockState = CursorLockMode.Locked;
//            Cursor.visible = false;
//            mouse.SetActive(true);
//            playerInteraction.canInteract = true;
//            playermove.enabled = true;
//        }
//    }

//    void Update()
//    {
//        // 按 E 關閉 UI
//        //if (isUiOpen && canClose && Input.GetKeyDown(KeyCode.E))
//        if (isUiOpen && Input.GetKeyDown(KeyCode.E))
//        {
//            CloseDocs();
//        }
//    }
//}

using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class SecretDrawer : MonoBehaviour, IInteractable
{
    [Header("設定")]
    public string requiredKeyName = "Key_WashingMachine";
    public GameObject docsUI;
    public GameObject mouse;
    public GameObject config;
    public PlayerInput playermove;
    public PlayerInteraction playerInteraction;

    [Header("對話設定")]
    public string lockedMessage = "鎖住了... 應該有個銀色鑰匙藏在某個地方。";
    public string unlockMessage = "鑰匙剛好吻合！裡面好像有東西...";

    private bool isUnlocked = false;
    private bool isUiOpen = false;

    // ★★★ 1. 把這個加回來，不然按E會瞬間開關很不穩 ★★★
    private bool canClose = false;

    void Start()
    {
        if (docsUI != null) docsUI.SetActive(false);
    }

    public void OnInteract()
    {
        if (isUiOpen) return;

        // 檢查背包有沒有鑰匙
        if (GameFlowManager.Instance.HasItem(requiredKeyName) || isUnlocked)
        {
            // 有鑰匙 (或是之前已經開過了) -> 打開抽屜
            if (!isUnlocked)
            {
                // 第一次打開，播放對話
                if (UIManager.Instance != null) UIManager.Instance.ShowDialogue(unlockMessage);

                // ★★★ 2. 關鍵修正：這裡必須呼叫經理，聲音才會出來！ ★★★
                GameFlowManager.Instance.TriggerDrawerUnlock();

                isUnlocked = true; // 標記為已解鎖
                return;
            }

            // 第二次以後點擊，進入閱讀模式
            config.GetComponent<publicConhig>().mouseflag++;
            playermove.enabled = false;
            playerInteraction.canInteract = false;
            mouse.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            OpenDocs();
        }
        else
        {
            // 沒鑰匙
            if (UIManager.Instance != null) UIManager.Instance.ShowDialogue(lockedMessage);
        }
    }

    void OpenDocs()
    {
        isUiOpen = true;

        // ★★★ 3. 重置冷卻時間 ★★★
        canClose = false;

        if (docsUI != null) docsUI.SetActive(true);

        StartCoroutine(EnableCloseDelay());
    }

    IEnumerator EnableCloseDelay()
    {
        yield return new WaitForSeconds(0.5f);
        // ★★★ 4. 時間到才允許關閉 ★★★
        canClose = true;
    }

    public void CloseDocs()
    {
        isUiOpen = false;
        canClose = false;

        config.GetComponent<publicConhig>().mouseflag--;
        if (docsUI != null) docsUI.SetActive(false);
        if (config.GetComponent<publicConhig>().mouseflag == 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            mouse.SetActive(true);
            playerInteraction.canInteract = true;
            playermove.enabled = true;
        }
    }

    void Update()
    {
        // ★★★ 5. 這裡加上 canClose 檢查，手感才會好 ★★★
        // if (isUiOpen && canClose && Input.GetKeyDown(KeyCode.E))
        // {
        //     CloseDocs();
        // }
    }
}