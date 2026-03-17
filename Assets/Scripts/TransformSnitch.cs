using UnityEngine;
using System.Diagnostics;

public class TransformSnitch : MonoBehaviour
{
    Vector3 lastLocalPos;

    void Start()
    {
        lastLocalPos = transform.localPosition;
    }

    void LateUpdate()
    {
        if (transform.localPosition != lastLocalPos)
        {
            UnityEngine.Debug.LogError(
                "Transform 被改了！\n" +
                "物件：" + name + "\n" +
                "舊：" + lastLocalPos + "\n" +
                "新：" + transform.localPosition + "\n" +
                new StackTrace(true)
            );

            lastLocalPos = transform.localPosition;
        }
    }
}
