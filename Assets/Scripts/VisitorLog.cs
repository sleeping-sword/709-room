using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

// 這裡一定要有 IInteractable，確保跟你的 Interactable.cs 對接
public class VisitorLog : MonoBehaviour, IInteractable
{
    [Header("介面設定")]
    public GameObject logUI;
    public GameObject mouse;
    public GameObject config;
    public PlayerInput playermove;
    public PlayerInteraction playerInteraction;

    [Header("即時狀態 (請在遊戲中觀察這兩行)")]
    public bool isSignalReceived = false; // 有沒有收到訊號？
    public bool isOpen = false;           // 現在是開還是關？

    // private bool canClose = false;

    void Start()
    {
        // 遊戲一開始先隱藏
        if (logUI != null) logUI.SetActive(false);
        else Debug.LogError($"【嚴重錯誤】{name} 的 Log UI 欄位是空的！請拖入 UI 物件！");
    }

    // 當 Interactable 呼叫這個函式時...
    public void OnInteract()
    {
        isSignalReceived = true; // 亮燈！代表訊號通了
        Debug.Log($"【成功】VisitorLog 收到來自 Interactable 的呼叫了！");

        if (!isOpen)
        {
            OpenLog();
        }
    }

    void OpenLog()
    {
        isOpen = true;
        // canClose = false;

        if (logUI != null)
        {
            config.GetComponent<publicConhig>().mouseflag++;
            playermove.enabled = false;
            playerInteraction.canInteract = false;
            mouse.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            logUI.SetActive(true);
            Debug.Log($"📄【執行】已執行 SetActive(true)，UI 應該要出現了。UI名稱: {logUI.name}");
        }

        StartCoroutine(EnableCloseDelay());
    }

    IEnumerator EnableCloseDelay()
    {
        yield return new WaitForSeconds(0.5f);
        // canClose = true;
    }

    public void CloseLog()
    {
        isOpen = false;
        // canClose = false;
        isSignalReceived = false; // 關閉時把燈熄滅
        config.GetComponent<publicConhig>().mouseflag--;
        if (logUI != null) logUI.SetActive(false);
        if(config.GetComponent<publicConhig>().mouseflag == 0)
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
    //     if (isOpen && canClose && Input.GetKeyDown(KeyCode.E))
    //     {
    //         CloseLog();
    //     }
    }
}