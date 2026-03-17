using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heart_drawer : MonoBehaviour, IInteractable
{
    public string requiredKeyName = "heart_key"; // 必須跟鑰匙 ItemName 一模一樣
    public Transform Drawer;
    [Header("對話設定")]
    public string lockedMessage = "主角：鎖住了... 感覺需要一個心型的鑰匙藏在某個地方。";
    public string unlockMessage = "主角：鑰匙剛好吻合！抽屜可以打開了...";
    Vector3 close_position;
    Vector3 open_position;
    bool isopen = false;
    bool isUnlocked = false;
    public virtual void OnInteract()
    {
        if (isUnlocked || GameFlowManager.Instance.HasItem(requiredKeyName))
        {
            // 有鑰匙 (或是之前已經開過了) -> 打開抽屜
            if (!isUnlocked)
            {
                // 第一次打開，播放對話
                if (UIManager.Instance != null) UIManager.Instance.ShowDialogue(unlockMessage);
                isUnlocked = true; // 標記為已解鎖，下次就不用再檢查鑰匙
                GetComponent<MeshCollider>().enabled = false;
            }

            if (isUnlocked && isopen)
            {
                Drawer.position = close_position;
                isopen = !isopen;
            }
            else if (isUnlocked && !isopen)
            {
                Drawer.position = open_position;
                isopen = !isopen;
            }
        }
        else
        {
            // 沒鑰匙 -> 顯示鎖住訊息
            if (UIManager.Instance != null) UIManager.Instance.ShowDialogue(lockedMessage);
        }
    }
        
    // Start is called before the first frame update
    void Start()
    {
        close_position = Drawer.position;
        open_position = new Vector3(close_position.x, close_position.y, close_position.z - 0.6f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
