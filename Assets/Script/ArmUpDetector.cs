using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Unity.Sample.PoseLandmarkDetection;
using System.Reflection;
using UnityEngine.EventSystems;

public class ArmUpDetector : MonoBehaviour
{
    public PoseLandmarkerRunner runner;
    //public Button targetButton;

    //float threshold = 0.012f;
    public static bool wasJump = false;


    void Update()
    {
        //Debug.Log(runner);
        if (runner == null)
            return;

        var result = runner.Result;


        if (result.poseLandmarks == null ||
            result.poseLandmarks.Count == 0)
        {

            return;
        }

        // 顔ランドマーク取得
        var pose = result.poseLandmarks[0];
        // foreach (var member in pose.GetType().GetMembers())
        // {
        //     Debug.Log(member.Name);
        // }

        // ★ここが正解
        var landmarks = pose.landmarks;

        var leftShoulder = landmarks[11];
        var leftWrist = landmarks[15];

        var rightShoulder = landmarks[12];
        var rightWrist = landmarks[16];

        bool leftHandUp = leftWrist.y < leftShoulder.y;
        bool rightHandUp = rightWrist.y < rightShoulder.y;

        // 
        bool jump = leftHandUp && rightHandUp;


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