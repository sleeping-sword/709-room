using System.Collections;
using UnityEngine;

public class SmartDrawer : MonoBehaviour, IInteractable
{
    [Header("Drawer Mesh (ONLY this moves)")]
    public Transform drawerMesh;   // 真的會動的那個

    [Header("Lock")]
    public bool isLocked = false;
    public string requiredKeyName = "鑰匙";

    [Header("Move")]
    public float openDistance = 0.6f;

    [Header("Lock Hint")]
    [TextArea]
    public string lockedHint =
        "抽屜被鎖住了……\n也許鑰匙藏在房間的某個地方。";

    Vector3 closedLocalPos;
    bool isOpen = false;
    bool guard = false;
    Vector3 targetLocalPos;   // 我們真正想要的位置
    bool forceLock = false;   // 是否啟用回鎖


    void Awake()
    {
        if (drawerMesh == null)
        {
            Debug.LogError("SmartDrawer：drawerMesh 沒指定");
            enabled = false;
            return;
        }

        closedLocalPos = drawerMesh.localPosition;
        // ★ 新增這一行
        targetLocalPos = closedLocalPos;
    }

    //public void OnInteract()
    //{
    //    if (guard) return;
    //    guard = true;

    //    // 🔒 上鎖但沒鑰匙
    //    if (isLocked && !GameFlowManager.Instance.HasItem(requiredKeyName))
    //    {
    //        ShowLockedHint();
    //        guard = false;
    //        return;
    //    }

    //    // 開 / 關抽屜（只動 DrawerMesh）
    //    if (!isOpen)
    //    {
    //        drawerMesh.localPosition =
    //            closedLocalPos + Vector3.back * openDistance;
    //    }
    //    else
    //    {
    //        drawerMesh.localPosition = closedLocalPos;
    //    }

    //    isOpen = !isOpen;
    //    StartCoroutine(ReleaseGuard());
    //}
    public void OnInteract()
    {
        if (guard) return;
        guard = true;

        // 上鎖檢查
        if (isLocked && !GameFlowManager.Instance.HasItem(requiredKeyName))
        {
            ShowLockedHint();
            guard = false;
            return;
        }

        // ★★ 關鍵改這裡 ★★
        if (!isOpen)
        {
            targetLocalPos = closedLocalPos + Vector3.back * openDistance;
        }
        else
        {
            targetLocalPos = closedLocalPos;
        }

        isOpen = !isOpen;
        forceLock = true;

        StartCoroutine(ReleaseGuard());
    }


    void ShowLockedHint()
    {
        // 有 UIManager 就用 UI
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowDialogue(lockedHint);
        }
        else
        {
            // 沒 UIManager 至少印在 Console
            Debug.Log(lockedHint);
        }
    }

    IEnumerator ReleaseGuard()
    {
        yield return null;
        guard = false;
    }

    void LateUpdate()
    {
        if (forceLock)
        {
            drawerMesh.localPosition = targetLocalPos;
        }
    }

}
