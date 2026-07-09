using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Unity.Sample.PoseLandmarkDetection;
using System.Reflection;
using UnityEngine.EventSystems;

public class ArmUpDetector : MonoBehaviour
{
    public PoseLandmarkerRunner runner;

    public static bool wasJump = false;


    void Update()
    {
        Debug.Log(runner.name);

        if (runner == null)
            return;

        var result = runner.Result;

        if (result.poseLandmarks == null ||
            result.poseLandmarks.Count == 0)
        {
            if (result.poseLandmarks == null)
            {
                Debug.Log("PoseLandmarks is null");
                return;
            }
            if (result.poseLandmarks.Count == 0)
            {
                Debug.Log("PoseLandmarks count is 0");
                return;
            }
            return;
        }

        Debug.Log("Pose Detected");

        // ランドマーク取得
        var pose = result.poseLandmarks[0];
        // foreach (var member in pose.GetType().GetMembers())
        // {
        //     Debug.Log(member.Name);
        // }

        // ★ここが正解
        var landmarks = pose.landmarks;
        // Debug.Log("Pose Detected");

        var leftShoulder = landmarks[11];
        var leftWrist = landmarks[15];

        var rightShoulder = landmarks[12];
        var rightWrist = landmarks[16];

        bool leftHandUp = leftWrist.y < leftShoulder.y;
        bool rightHandUp = rightWrist.y < rightShoulder.y;

        // 
        bool jump = leftHandUp || rightHandUp;

        // Debug.Log($"Left: {leftHandUp}, Right: {rightHandUp}");


        if (jump && !wasJump)
        {
            wasJump = true;
            //rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }

        if (!jump)
        {
            wasJump = false;
        }
    }
}