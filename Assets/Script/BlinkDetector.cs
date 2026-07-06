using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Unity.Sample.FaceLandmarkDetection;
using System.Reflection;
using UnityEngine.EventSystems;

public class BlinkDetector : MonoBehaviour
{
    public FaceLandmarkerRunner runner;
    //public Button targetButton;

    float threshold = 0.012f;
    private bool blinking = false;
    public static bool isclose = false;

    void Update()
    {
        //Debug.Log(runner);
        if (runner == null)
            return;

        var result = runner.Result;


        if (result.faceLandmarks == null ||
            result.faceLandmarks.Count == 0)
        {

            // ★ 顔が消えたときの処理を追加
            if (blinking)
            {
                blinking = false;
                isclose = false;

                Debug.Log("顔消えた → 停止");

                // ExecuteEvents.Execute(
                //     targetButton.gameObject,
                //     new PointerEventData(EventSystem.current),
                //     ExecuteEvents.pointerUpHandler
                // );
            }

            return;
        }

        // 顔ランドマーク取得
        var face = result.faceLandmarks[0];
        // foreach (var member in face.GetType().GetMembers())
        // {
        //     Debug.Log(member.Name);
        // }

        // ★ここが正解
        var landmarks = face.landmarks;

        // 左目
        var leftTop = landmarks[159];
        var leftBottom = landmarks[145];

        // 右目
        var rightTop = landmarks[386];
        var rightBottom = landmarks[374];

        float leftEyeOpen = Vector3.Distance(
            new Vector3(leftTop.x, leftTop.y, leftTop.z),
            new Vector3(leftBottom.x, leftBottom.y, leftBottom.z)
        );

        float rightEyeOpen = Vector3.Distance(
            new Vector3(rightTop.x, rightTop.y, rightTop.z),
            new Vector3(rightBottom.x, rightBottom.y, rightBottom.z)
        );


        bool isLeftClosed = leftEyeOpen < threshold;
        bool isRightClosed = rightEyeOpen < threshold;

        var nose = landmarks[1];
        var leftCheek = landmarks[234];
        var rightCheek = landmarks[454];

        float leftDist = Mathf.Abs(nose.x - leftCheek.x);
        float rightDist = Mathf.Abs(rightCheek.x - nose.x);

        // 左右の距離比
        float ratio = leftDist / rightDist;

        bool faceFront = ratio > 0.6f && ratio < 1.3f;

        Debug.Log(faceFront);

        // 両目閉じ
        bool isBlink = isLeftClosed && isRightClosed;

        if (faceFront == false)
        {
            // 横を向いているので解除
            if (blinking)
            {
                blinking = false;
                isclose = false;

                // ExecuteEvents.Execute(
                //     targetButton.gameObject,
                //     new PointerEventData(EventSystem.current),
                //     ExecuteEvents.pointerUpHandler
                // );
            }

            return;
        }

        if (isBlink)
        {
            if (!blinking)
            {
                blinking = true;
                Debug.Log("BLINK");

                // ExecuteEvents.Execute(
                //     targetButton.gameObject,
                //     new PointerEventData(EventSystem.current),
                //     ExecuteEvents.pointerDownHandler
                // );
            }
            isclose = true;
            Debug.Log("目を閉じてる！");
        }
        else
        {
            if (blinking)
            {
                blinking = false;
                // ExecuteEvents.Execute(
                //     targetButton.gameObject,
                //     new PointerEventData(EventSystem.current),
                //     ExecuteEvents.pointerUpHandler
                // );
            }
            isclose = false;
        }
    }
}