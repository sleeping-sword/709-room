//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.InputSystem;

//public class PlayerInteraction : MonoBehaviour
//{
//    public Camera playerCamera;
//    public float rayDistance = 3f; // 最大互動距離

//    void Update()
//    {
//        // 用中央準心射出 Ray
//        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

//        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
//        {
//            // 嘗試取得 Interactable Script
//            Interactable interactable = hit.collider.GetComponent<Interactable>();

//            if (interactable != null)
//            {
//                // 判斷左鍵點擊
//                if (Input.GetMouseButtonDown(0)) // 0 = 左鍵
//                {
//                    // 再次確認距離安全
//                    if (hit.distance <= interactable.interactDistance)
//                    {
//                        interactable.Interact();
//                    }
//                }
//            }
//        }
//    }
//}

using UnityEngine;
using TMPro; // 記得引用 TMP

public class PlayerInteraction : MonoBehaviour
{
    [Header("設定")]
    public Camera playerCamera;
    public float rayDistance = 3f;
    public LayerMask interactLayer;
    public bool canInteract = true;

    [Header("UI 連結")]
    public GameObject hoverLabelObject; // 拖入那個 HoverLabel 物件
    public TMP_Text hoverLabelText;     // 拖入同一個物件 (它會自動抓 TMP 組件)

    private Interactable currentInteractable;

    void Update()
    {
        if(!canInteract) return;

        CheckHover();

        // 點擊互動
        if (Input.GetMouseButtonDown(0))
        {
            if (currentInteractable != null)
            {
                currentInteractable.Interact();
            }
        }
    }

    void CheckHover()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, interactLayer))
        {
            Interactable hitInteractable = hit.collider.GetComponent<Interactable>();

            if (hitInteractable != null)
            {
                if (hitInteractable != currentInteractable)
                {
                    currentInteractable = hitInteractable;
                    ShowLabel(currentInteractable);
                }
            }
            else
            {
                ClearHover();
            }
        }
        else
        {
            ClearHover();
        }
    }

    void ShowLabel(Interactable target)
    {
        string nameToShow = "";

        // 1. 先找自己身上有沒有 ItemProp
        ItemProp prop = target.GetComponent<ItemProp>();

        if (prop != null)
        {
            nameToShow = prop.itemName;
        }
        // 2. 如果自己沒有，檢查有沒有爸爸 (針對藥丸這種代理人結構)
        else if (target.parent != null && target.parent.Length > 0 && target.parent[0] != null)
        {
            ItemProp parentProp = target.parent[0].GetComponent<ItemProp>();
            if (parentProp != null)
            {
                nameToShow = parentProp.itemName;
            }
        }

        // 3. 決定顯示什麼
        if (!string.IsNullOrEmpty(nameToShow))
        {
            hoverLabelText.text = $" {nameToShow} "; // 顯示名字
            hoverLabelObject.SetActive(true);
        }
        else
        {
            hoverLabelText.text = " 調查 "; // 沒名字就顯示通用字
            hoverLabelObject.SetActive(true);
        }
    }

    void ClearHover()
    {
        if (currentInteractable != null)
        {
            currentInteractable = null;
            if (hoverLabelObject) hoverLabelObject.SetActive(false);
        }
    }
}