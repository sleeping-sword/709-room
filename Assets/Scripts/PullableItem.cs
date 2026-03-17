using UnityEngine;
using System.Collections;

// 1. 繼承 ItemProp (讓它擁有名字、描述功能)
// 2. 重新實作 IInteractable (讓我們可以自定義互動)
public class PullableItem : ItemProp, IInteractable
{
    [Header("--- 第二階段：拉出來後的內容 ---")]
    public string revealedName; // 拉出來後的新名字
    [TextArea(3, 10)]
    public string revealedDesc; // 拉出來後的新劇情

    [Header("--- 拉動設定 ---")]
    public Vector3 moveDirection = new Vector3(0, 0, -1); // 往哪個方向拉 (X,Y,Z)
    public float pullDistance = 0.8f; // 拉多遠
    public float moveSpeed = 2.0f;    // 拉多快

    private bool isPulledOut = false; // 是否已經拉出來了
    private int clickCount = 0;       // 點擊次數

    // 覆寫互動邏輯 (使用 new 關鍵字來取代父類別的定義)
    public new void OnInteract()
    {
        // 情況 A: 已經拉出來了 -> 變成普通道具，顯示新資訊
        if (isPulledOut)
        {
            ShowInfo(); // 呼叫 ItemProp原本的顯示功能
            return;
        }

        // 情況 B: 還沒拉出來
        clickCount++;

        if (clickCount == 1)
        {
            // 第 1 次點擊：顯示「未知物品」的提示文字
            // (這時候顯示的是你在 Inspector 上面原本填的 Item Name 和 Description)
            ShowInfo();
        }
        else if (clickCount >= 2)
        {
            // 第 2 次點擊：執行拉出動作
            StartCoroutine(PullOutRoutine());
        }
    }

    IEnumerator PullOutRoutine()
    {
        // 1. 播放拉出來的動畫 (簡單位移)
        Vector3 startPos = transform.position;
        // 計算目標位置：目前位置 + (方向 * 距離)
        Vector3 endPos = transform.position + (moveDirection.normalized * pullDistance);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        // 2. 標記為已拉出
        isPulledOut = true;

        // 3. 【關鍵】偷天換日：把名字和描述換成第二階段的內容
        itemName = revealedName;
        storyDescription = revealedDesc;

        // 4. 顯示提示
        if (UIManager.Instance)
            UIManager.Instance.ShowDialogue("你把它拉出來了！原來是...");

        // 5. 自動再顯示一次新資訊 (選用)
        // ShowInfo(); 
    }
}