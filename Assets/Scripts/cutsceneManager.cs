using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class CutsceneFrame
{
    public Sprite background;

    [TextArea(2, 4)]
    public string[] texts;   // ← 重點：多段文字
}

public class cutsceneManager : MonoBehaviour
{
    [Header("UI")]
    public Image backgroundImage;
    public Image fadeImage;
    public TextMeshProUGUI storyText;

    [Header("Frames")]
    public CutsceneFrame[] frames;

    [Header("Settings")]
    public float fadeSpeed = 1.5f;
    public float typingSpeed = 0.04f;
    public TextMeshProUGUI text;
    public GameObject Rule;

    int frameIndex = 0;
    int textIndex = 0;

    bool isTyping = false;
    Coroutine typingCoroutine;

    void Start()
    {
        storyText.text = "";
        text.text = "";
        fadeImage.color = Color.black;
        Rule.SetActive(false);
        StartCoroutine(FadeInFrame());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopTypingAndShowAll();
            }
            else
            {
                NextTextOrFrame();
            }
        }
    }

    void StopTypingAndShowAll()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        storyText.text = frames[frameIndex].texts[textIndex];
        isTyping = false;
    }

    void NextTextOrFrame()
    {
        textIndex++;

        // 還有下一段文字
        if (textIndex < frames[frameIndex].texts.Length)
        {
            typingCoroutine = StartCoroutine(
                TypeText(frames[frameIndex].texts[textIndex])
            );
        }
        else
        {
            // 這個畫面文字跑完 → 換畫面
            frameIndex++;
            textIndex = 0;

            if (frameIndex < frames.Length)
            {
                StartCoroutine(FadeOutIn());
            }
            else
            {
                StartCoroutine(FadeOutAndEnd());
            }
        }
    }

    IEnumerator FadeInFrame()
    {
        SetFrame(frameIndex);

        float alpha = 1f;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        typingCoroutine = StartCoroutine(
            TypeText(frames[frameIndex].texts[textIndex])
        );
    }

    IEnumerator FadeOutIn()
    {
        float alpha = 0f;
        while (alpha < 1)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        SetFrame(frameIndex);

        while (alpha > 0)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        typingCoroutine = StartCoroutine(
            TypeText(frames[frameIndex].texts[textIndex])
        );
    }

    IEnumerator FadeOutAndEnd()
    {
        Rule.SetActive(true);
        yield return null;
        // float alpha = 0f;
        // while (alpha < 1)
        // {
        //     alpha += Time.deltaTime * fadeSpeed;
        //     fadeImage.color = new Color(0, 0, 0, alpha);
        //     yield return null;
        // }

        Debug.Log("前導劇情結束");
    }

    void SetFrame(int i)
    {
        backgroundImage.sprite = frames[i].background;
        storyText.text = "";
    }

    IEnumerator TypeText(string line)
    {
        isTyping = true;
        storyText.text = "";

        foreach (char c in line)
        {
            storyText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    public void EnterRoom()
    {
        text.text = "進入中，請稍後";
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}