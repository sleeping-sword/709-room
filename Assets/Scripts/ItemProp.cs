using UnityEngine;

// 1. 記得要有 ", IInteractable" 來對接同學的腳本
public class ItemProp : MonoBehaviour, IInteractable
{
    [Header("物品設定")]
    public string itemName; // 物品名稱 (給 Hover 提示和字幕標題用)

    [TextArea(3, 10)]
    public string storyDescription; // 點擊後顯示的劇情內容

    [Header("狀態")]
    public bool isCollected = false; // 記錄是否已經拿過這個線索

    // 2. 這是介面規定一定要有的函式 (被點擊時觸發)
    public void OnInteract()
    {
        ShowInfo();
    }

    // 實際執行的邏輯
    public void ShowInfo()
    {
        // --- 功能 A：呼叫 UI 顯示字幕 ---
        if (UIManager.Instance != null)
        {
            // 格式： [物品名稱] (換行) [描述]
            UIManager.Instance.ShowDialogue($"{itemName}：\n{storyDescription}");
        }
        else
        {
            // 如果沒抓到 UI，至少在 Console 印出來方便除錯
            Debug.Log($"【調查發現】{itemName}: {storyDescription}");
        }

        // --- 功能 B：通知劇情總管 (如果是第一次發現) ---
        if (!isCollected)
        {
            isCollected = true; // 標記為已蒐集

            // 通知 GameFlowManager 增加進度
            if (GameFlowManager.Instance != null)
            {
                GameFlowManager.Instance.OnClueFound(itemName);
            }
        }
    }
}