//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class GameFlowManager : MonoBehaviour
//{
//    public static GameFlowManager Instance;

//    [Header("進度設定")]
//    public int currentCluesFound = 0;
//    public int cluesNeededForPhone = 6;

//    // ★★★ 新增：背包清單，用來存玩家拿過的東西名字 ★★★
//    public List<string> collectedItems = new List<string>();
//    [Header("提示 UI (選填)")]
//    public GameObject phoneHintUI;

//    [Header("恐怖事件素材")]
//    public AudioSource eventAudioSource; // 記得要拖入 GameManager 自己的 Audio Source
//    public AudioClip manYellClip;
//    public AudioClip doorBangClip;

//    void Awake()
//    {
//        Instance = this;
//    }

//    // --- 第一階段：蒐集線索 ---
//    //public void OnClueFound(string clueName)
//    //{
//    //    currentCluesFound++;
//    //    CheckProgress();
//    //}

//    public void OnClueFound(string clueName)
//    {
//        // 如果清單裡沒有這個東西，就加進去
//        if (!collectedItems.Contains(clueName))
//        {
//            collectedItems.Add(clueName);
//        }

//        currentCluesFound++;
//        Debug.Log($"進度更新：找到 {currentCluesFound} / {cluesNeededForPhone} 個線索 ({clueName})");

//        CheckProgress();
//    }

//    // ★★★ 新增：給別的腳本查有沒有某樣東西 ★★★
//    public bool HasItem(string itemName)
//    {
//        return collectedItems.Contains(itemName);
//    }

//    void CheckProgress()
//    {
//        if (currentCluesFound == cluesNeededForPhone)
//        {
//            TriggerPhoneHint();
//        }
//    }

//    void TriggerPhoneHint()
//    {
//        string message = "主角：這房間的氣氛很不對勁...也許該看看安琪的手機，在哪裡呢?......";
//        if (UIManager.Instance != null) UIManager.Instance.ShowDialogue(message);
//        if (phoneHintUI != null) phoneHintUI.SetActive(true);
//    }

//    // --- 第二階段：恐怖計時事件 ---
//    // 這個函式只要被呼叫一次，就會自己在背景倒數，不管玩家有沒有在看手機
//    public void StartPhoneUnlockEvent()
//    {
//        StartCoroutine(HorrorEventRoutine());
//    }

//    IEnumerator HorrorEventRoutine()
//    {
//        Debug.Log("手機解鎖... 恐怖計時開始！(玩家現在就算關掉手機介面，倒數也不會停)");

//        // ★★★ 測試時用 5秒，正式版改成 60秒 ★★★
//        //yield return new WaitForSeconds(60f);
//        yield return new WaitForSeconds(5f);

//        Debug.Log("恐怖事件觸發！");

//        // 1. 播放音效
//        if (eventAudioSource != null)
//        {
//            eventAudioSource.mute = false;
//            eventAudioSource.volume = 1.0f;

//            if (manYellClip) eventAudioSource.PlayOneShot(manYellClip);
//            if (doorBangClip) eventAudioSource.PlayOneShot(doorBangClip);
//        }

//        // 2. 顯示你指定的恐怖劇情 (已更新)
//        string horrorMessage = "主角：剛剛那個人好恐怖.....而且他剛剛似乎一直說什麼「還錢啦還錢啦」";

//        if (UIManager.Instance != null)
//        {
//            UIManager.Instance.ShowDialogue(horrorMessage);
//        }
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance;

    public GameObject PlayUI;
    public PlayerInput playermove;
    public PlayerInteraction playerInteraction;
    public bool isCatch = false;
    public GameObject GameOver;
    [Header("進度設定")]
    public int currentCluesFound = 0;
    public int cluesNeededForPhone = 6;

    // 背包清單，用來存玩家拿過的東西名字
    public List<string> collectedItems = new List<string>();

    [Header("提示 UI (選填)")]
    public GameObject phoneHintUI;

    [Header("恐怖事件素材 (舊有的)")]
    public AudioSource eventAudioSource; // 記得要拖入 GameManager 自己的 Audio Source
    public AudioClip manYellClip;
    public AudioClip doorBangClip;

    // ★★★ 新增：抽屜解鎖事件專用音效 ★★★
    [Header("抽屜解鎖事件")]
    public AudioClip drawerUnlockSound; // 請拖入「喀嚓」解鎖聲
    public AudioClip strangeVoiceSound; // 請拖入「請問裡面有人嗎」人聲

    [Header("入侵階段 (25秒後)")]
    public AudioClip roomDoorOpenSound;  // 房門被打開的聲音
    public AudioClip footstepsSound;     // 走路聲 (房務員走進來)

    // ★★★ 新增：結局相關音效 ★★★
    [Header("結局分支音效")]
    public AudioClip roomDoorCloseSound; // 關門聲 (存活時用)
    public AudioClip survivorVoice;      // "欸? 可能是我聽錯了吧"
    public AudioClip caughtVoice;        // "你是誰!!!..."
    public AudioClip policeSiren;        // 警車聲

    // ★★★ 新增：玩家是否處於安全躲藏狀態 ★★★
    public bool isPlayerHidden = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        openlight();
        isCatch = false;
        GameOver.SetActive(false);
    }

    public void openlight()
    {
        StartCoroutine(lightevent());
    }

    IEnumerator lightevent()
    {
        yield return new WaitForSeconds(5f);
        string horrorMessage = "玩家：房間好暗啊，先把電燈打開，應該在門口附近的牆壁上";
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowDialogue(horrorMessage);
        }
    }

    // --- 第一階段：蒐集線索 ---
    public void OnClueFound(string clueName)
    {
        // 如果清單裡沒有這個東西，就加進去
        if (!collectedItems.Contains(clueName))
        {
            collectedItems.Add(clueName);
        }

        currentCluesFound++;
        Debug.Log($"進度更新：找到 {currentCluesFound} / {cluesNeededForPhone} 個線索 ({clueName})");

        CheckProgress();
    }

    // ★★★ 新增：給櫃子腳本呼叫用，更新躲藏狀態 ★★★
    public void SetPlayerHiddenState(bool isHidden)
    {
        isPlayerHidden = isHidden;

        if (isPlayerHidden)
        {
            Debug.Log("【躲藏系統】玩家躲避成功！(安全)");
            // 在這裡可以加入：播放心跳聲、讓敵人找不到等等
        }
        else
        {
            Debug.Log("【躲藏系統】玩家離開躲藏/門被打開了 (危險)");
        }
    }

    // 給別的腳本查有沒有某樣東西
    public bool HasItem(string itemName)
    {
        return collectedItems.Contains(itemName);
    }

    void CheckProgress()
    {
        if (currentCluesFound == cluesNeededForPhone)
        {
            TriggerPhoneHint();
        }
    }

    void TriggerPhoneHint()
    {
        string message = "玩家：這房間的氣氛很不對勁...也許該看看安琪的手機，在哪裡呢?......";
        if (UIManager.Instance != null) UIManager.Instance.ShowDialogue(message);
        if (phoneHintUI != null) phoneHintUI.SetActive(true);
    }

    // --- 第二階段：手機恐怖計時事件 ---
    public void StartPhoneUnlockEvent()
    {
        StartCoroutine(HorrorEventRoutine());
    }

    IEnumerator HorrorEventRoutine()
    {
        Debug.Log("手機解鎖... 恐怖計時開始！");
        yield return new WaitForSeconds(5f); // 正式版記得改回 60f

        Debug.Log("恐怖事件觸發！");

        if (eventAudioSource != null)
        {
            eventAudioSource.mute = false;
            eventAudioSource.volume = 1.0f;
            if (manYellClip) eventAudioSource.PlayOneShot(manYellClip);
            if (doorBangClip) eventAudioSource.PlayOneShot(doorBangClip);
        }

        string horrorMessage = "剛剛那個人好恐怖.....而且他剛剛似乎一直說什麼「還錢啦還錢啦」";
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowDialogue(horrorMessage);
        }
    }

    // ★★★ 新增：抽屜解鎖事件邏輯 (給 SecretDrawer 呼叫) ★★★
    public void TriggerDrawerUnlock()
    {
        // 啟動協程，開始跑解鎖流程
        StartCoroutine(DrawerEventRoutine());
    }

    IEnumerator DrawerEventRoutine()
    {
        Debug.Log("【抽屜事件】開始：播放解鎖音效...");

        // 1. 播放解鎖音效 (只播這一次)
        if (eventAudioSource != null && drawerUnlockSound != null)
        {
            eventAudioSource.mute = false;
            eventAudioSource.PlayOneShot(drawerUnlockSound);
        }

        // 2. 等待 5 秒
        yield return new WaitForSeconds(5f);

        // 3. 播放人聲「請問裡面有人嗎？」
        Debug.Log("【抽屜事件】觸發：播放人聲");
        if (eventAudioSource != null && strangeVoiceSound != null)
        {
            eventAudioSource.PlayOneShot(strangeVoiceSound);

            // 如果你想要有人說話時也顯示字幕，可以在這裡加：
            if (UIManager.Instance != null)
                UIManager.Instance.ShowDialogue("門外：請問裡面有人嗎？...");
        }

        // 4. ★★★ 開始 25 秒倒數 ★★★
        Debug.Log("【入侵倒數】開始！玩家有 25 秒時間躲進衣櫃！");
        // 等待語音稍微講完 (約 3秒)
        yield return new WaitForSeconds(3f);

        // --- ★★★ 新增：主角緊張的內心獨白 ★★★ ---

        // 第一句
        if (UIManager.Instance != null)
            UIManager.Instance.ShowDialogue("怎麼辦!好像被房務人員聽到聲音了....");

        // 隔 2 秒 (留一點閱讀時間，但要緊湊)
        yield return new WaitForSeconds(2f);

        // 第二句
        if (UIManager.Instance != null)
            UIManager.Instance.ShowDialogue("安琪這個時間不在，所以這裡不該有人的!");

        // 隔 2 秒
        yield return new WaitForSeconds(2f);

        // 第三句 (提示躲藏)
        if (UIManager.Instance != null)
            UIManager.Instance.ShowDialogue("趕快趁這20秒的時間躲起來!不然任務不但失敗，你的徵信生涯也就再見了!快點!");

        // ---------------------------------------------

        // (選擇性) 如果你想讓玩家更有壓迫感，可以在這裡加一個倒數計時的心跳聲
        // yield return new WaitForSeconds(25f); 

        // 為了測試方便，我這邊先設 10 秒，正式版請改成 18f
        yield return new WaitForSeconds(22f);

        // 5. ★★★ 時間到，入侵判定！ ★★★
        Debug.Log("【入侵事件】時間到！房務員闖入！");

        if (eventAudioSource != null)
        {
            // 播放開門聲
            if (roomDoorOpenSound) eventAudioSource.PlayOneShot(roomDoorOpenSound);
            yield return new WaitForSeconds(0.5f);

            // 播放走進來的腳步聲
            if (footstepsSound) eventAudioSource.PlayOneShot(footstepsSound);
            yield return new WaitForSeconds(9f);
        }

        // 等待腳步聲稍微走進來一點 (約 2秒)
        yield return new WaitForSeconds(2f);

        // 6. 生死判定 (這裡改用呼叫協程)
        if (isPlayerHidden)
        {
            StartCoroutine(SurvivorSequence());
        }
        else
        {
            StartCoroutine(GameOverSequence());
        }

        //// 播放開門與腳步聲
        //if (eventAudioSource != null)
        //{
        //    if (roomDoorOpenSound) eventAudioSource.PlayOneShot(roomDoorOpenSound);

        //    // 稍微延遲 0.5 秒再播腳步聲，感覺比較像「先開門，再走進來」
        //    yield return new WaitForSeconds(0.5f);

        //    if (footstepsSound) eventAudioSource.PlayOneShot(footstepsSound);
        //}

        // 6. 生死判定
        // CheckSurvival();
    }

    
    public void ending()
    {
        StartCoroutine(EndingEvent());
    }
    IEnumerator EndingEvent()
    {
        Debug.Log("【結束事件】開始");
        if (UIManager.Instance != null)
            UIManager.Instance.ShowDialogue("玩家：謎團都解開了...");
        
        yield return new WaitForSeconds(5f);

        if (UIManager.Instance != null)
            UIManager.Instance.ShowDialogue("玩家：可以離開了...(前往門口)");
        // 2. 等待 5 秒
        
        }

    // void CheckSurvival()
    // {
    //     // 檢查剛剛櫃子傳來的狀態
    //     if (isPlayerHidden)
    //     {
    //         // --- 玩家存活 ---
    //         Debug.Log(">>> 判定結果：【存活】鬼沒發現玩家");

    //         if (UIManager.Instance != null)
    //             UIManager.Instance.ShowDialogue("欸?.....那可能是我聽錯了吧?");

    //         // 這裡之後可以接：鬼離開的音效，或是下一階段劇情
    //     }
    //     else
    //     {
    //         // --- 玩家死亡 ---
    //         Debug.Log(">>> 判定結果：【Game Over】你被抓到了！");

    //         if (UIManager.Instance != null)
    //             UIManager.Instance.ShowDialogue("房務員：你是誰！！！你怎麼會出現在這裡?我們709的VIP房客是女生! 房務員大喊:快點抓住他!");

    //         // ★★★ 呼叫組員的 Game Over 邏輯 ★★★
    //         // 例如：GameOverManager.Instance.ShowGameOverScreen();
    //     }
    // }

    // ★★★ 分支 A：存活劇情 ★★★
    IEnumerator SurvivorSequence()
    {
        Debug.Log(">>> 判定結果：【存活】鬼沒發現玩家");

        // 1. 播放 "欸? 可能是我聽錯了吧"
        if (eventAudioSource != null && survivorVoice != null)
        {
            eventAudioSource.PlayOneShot(survivorVoice);
        }

        if (UIManager.Instance != null)
            UIManager.Instance.ShowDialogue("房務員：欸?.....那可能是我聽錯了吧?");

        // 等待語音講完 (假設語音長度 3秒，請依照實際音檔調整)
        yield return new WaitForSeconds(3f);

        // 2. 播放腳步聲 (走向門口)
        if (eventAudioSource != null && footstepsSound != null)
        {
            eventAudioSource.PlayOneShot(footstepsSound);
        }

        // 等待腳步聲走遠 (約 2\9秒)
        yield return new WaitForSeconds(9f);

        // 3. 播放關門聲
        if (eventAudioSource != null && roomDoorCloseSound != null)
        {
            eventAudioSource.PlayOneShot(roomDoorCloseSound);
        }

        Debug.Log("【安全】房務員離開了");
        UIManager.Instance.ShowDialogue("呼...好險，差點就被發現了!");
        // 這邊可以接後續劇情...
    }

    // ★★★ 分支 B：被抓 (Game Over) 劇情 ★★★
    IEnumerator GameOverSequence()
    {
        Debug.Log(">>> 判定結果：【Game Over】你被抓到了！");

        isCatch = true;
        playermove.enabled = false;
        playerInteraction.canInteract = false;
        // 1. 播放被抓語音 "你是誰！！！"
        if (eventAudioSource != null && caughtVoice != null)
        {
            eventAudioSource.PlayOneShot(caughtVoice);
        }

        if (UIManager.Instance != null)
            UIManager.Instance.ShowDialogue("房務員：你是誰！！！你怎麼會出現在這裡?我們709的VIP房客是女生! 快點抓住他!");
            UIManager.Instance.ShowDialogue("房務員：你是誰！！！你怎麼會出現在這裡?我們709的VIP房客是女生! 快點抓住他!");

        // 等待語音講到重點 (假設 2秒後警車響起)
        yield return new WaitForSeconds(8f);

        // 2. 播放警車聲
        if (eventAudioSource != null && policeSiren != null)
        {
            eventAudioSource.PlayOneShot(policeSiren);
        }

        // 等待一下讓聲音跑完，再跳 Game Over 畫面
        yield return new WaitForSeconds(2f);

        // ★★★ 呼叫組員的 Game Over 畫面 ★★★
        Debug.Log("【系統】呼叫 Game Over 畫面...");
        PlayUI.SetActive(false);
        GameOver.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}