using UnityEngine;
using TMPro;
using System.Collections; // 1. 記得引入這個，才能用計時器

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI 元件")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;

    // 2. 用來記錄目前正在跑的計時器，這樣如果玩家點太快，我們可以重置它
    private Coroutine autoCloseCoroutine;

    void Awake()
    {
        Instance = this;
        CloseDialogue();
    }

    public void ShowDialogue(string content)
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = content;

        // 3. 如果上一個計時器還在跑（例如玩家 2 秒內連續點了兩樣東西），先把它停掉
        if (autoCloseCoroutine != null)
        {
            StopCoroutine(autoCloseCoroutine);
        }

        // 4. 開啟新的計時器：5 秒後自動關閉
        autoCloseCoroutine = StartCoroutine(AutoCloseRoutine());
    }

    public void CloseDialogue()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    // 5. 這就是計時器的邏輯
    IEnumerator AutoCloseRoutine()
    {
        // 等待 5 秒
        yield return new WaitForSeconds(5f);

        // 時間到，執行關閉
        CloseDialogue();
    }
}