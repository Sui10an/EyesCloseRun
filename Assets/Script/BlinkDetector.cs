using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using UnityEngine.EventSystems;
using TMPro;
using Mediapipe.Unity.Sample.FaceLandmarkDetection;

public class BlinkDetector : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI blinkText;
    public FaceLandmarkerRunner runner;

    // EAR風の閾値
    [SerializeField] private float eyeCloseThreshold = 0.03f;

    // 顔が小さすぎる場合は無視
    [SerializeField] private float minFaceWidth = 0.15f;

    // 数フレーム継続で確定
    [SerializeField] private int blinkFramesRequired = 3;

    private int blinkFrameCount = 0;
    private bool blinking = false;

    public static bool isclose = false;

    void Update()
    {
        if (runner == null)
            return;

        var result = runner.Result;

        if (result.faceLandmarks == null ||
            result.faceLandmarks.Count == 0)
        {
            ResetBlink();
            return;
        }

        var face = result.faceLandmarks[0];
        var landmarks = face.landmarks;

        // ===== 目 =====

        var leftTop = landmarks[159];
        var leftBottom = landmarks[145];

        var rightTop = landmarks[386];
        var rightBottom = landmarks[374];

        // ===== 顔幅 =====

        var leftCheek = landmarks[234];
        var rightCheek = landmarks[454];

        float faceWidth = Vector3.Distance(
            new Vector3(leftCheek.x, leftCheek.y, leftCheek.z),
            new Vector3(rightCheek.x, rightCheek.y, rightCheek.z)
        );

        // 顔が小さすぎる
        if (faceWidth < minFaceWidth)
        {
            ResetBlink();
            return;
        }

        // ===== 目の開き =====

        float leftEyeOpen = Vector3.Distance(
            new Vector3(leftTop.x, leftTop.y, leftTop.z),
            new Vector3(leftBottom.x, leftBottom.y, leftBottom.z)
        );

        float rightEyeOpen = Vector3.Distance(
            new Vector3(rightTop.x, rightTop.y, rightTop.z),
            new Vector3(rightBottom.x, rightBottom.y, rightBottom.z)
        );

        // 正規化(EAR風)
        float leftEAR = leftEyeOpen / faceWidth;
        float rightEAR = rightEyeOpen / faceWidth;

        bool isLeftClosed = leftEAR < eyeCloseThreshold;
        bool isRightClosed = rightEAR < eyeCloseThreshold;

        // ===== 正面判定 =====

        var nose = landmarks[1];

        float leftDist = Mathf.Abs(nose.x - leftCheek.x);
        float rightDist = Mathf.Abs(rightCheek.x - nose.x);

        float ratio = leftDist / rightDist;

        bool faceFront = ratio > 0.6f && ratio < 1.3f;

        if (!faceFront)
        {
            ResetBlink();
            return;
        }

        bool isBlink = isLeftClosed && isRightClosed;

        // ===== フレームフィルタ =====

        if (isBlink)
        {
            blinkFrameCount++;
        }
        else
        {
            blinkFrameCount = 0;
        }

        bool blinkConfirmed =
            blinkFrameCount >= blinkFramesRequired;

        if (blinkConfirmed)
        {
            if (!blinking)
            {
                blinking = true;
                Debug.Log("BLINK");
            }

            isclose = true;

            if (blinkText != null)
            {
                blinkText.text = "目を閉じてる！";
            }
        }
        else
        {
            if (blinking)
            {
                blinking = false;
            }

            isclose = false;

            if (blinkText != null)
            {
                blinkText.text = "目が開いている！";
            }
        }

        Debug.Log(
            $"FaceWidth={faceWidth:F3} " +
            $"LeftEAR={leftEAR:F3} " +
            $"RightEAR={rightEAR:F3}"
        );
    }

    private void ResetBlink()
    {
        blinking = false;
        isclose = false;
        blinkFrameCount = 0;

        if (blinkText != null)
        {
            blinkText.text = "顔未検出";
        }
    }
}