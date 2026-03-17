using System.Collections;
using UnityEngine;

public class LockDrawer : MonoBehaviour, IInteractable
{
    public Transform Drawer;

    // 🔒 上鎖設定
    public bool isLocked = true;
    public string requiredKeyName = "鑰匙";

    // 抽屜狀態
    Vector3 close_position;
    Vector3 open_position;
    bool isOpen = false;

    // ★ 防止同一幀被呼叫多次
    bool isInteracting = false;

    void Awake()
    {
        // 保險起見，直接抓自己
        if (Drawer == null)
            Drawer = transform;

        close_position = Drawer.localPosition;
        open_position = close_position + new Vector3(0, 0, -0.6f);
    }

    public void OnInteract()
    {
        // 防重入
        if (isInteracting) return;
        isInteracting = true;

        // 上鎖檢查
        if (isLocked && !GameFlowManager.Instance.HasItem(requiredKeyName))
        {
            Debug.Log("抽屜上鎖，沒有鑰匙");
            isInteracting = false;
            return;
        }

        // 開關抽屜
        Drawer.localPosition = isOpen ? close_position : open_position;
        isOpen = !isOpen;

        // 下一幀解除鎖定
        StartCoroutine(ResetInteractFlag());
    }

    IEnumerator ResetInteractFlag()
    {
        yield return null;
        isInteracting = false;
    }
}
