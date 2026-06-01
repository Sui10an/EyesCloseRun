using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class CameraCapture : MonoBehaviour
{
    private WebCamTexture webcam;
    private VisualElement cameraView;

    IEnumerator Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        cameraView = root.Q<VisualElement>("cameraView");

        if (WebCamTexture.devices.Length == 0)
        {
            Debug.LogError("カメラなし");
            yield break;
        }

        webcam = new WebCamTexture(WebCamTexture.devices[0].name, 1280, 720, 30);
        webcam.Play();

        // 初期化待ち（重要）
        yield return new WaitUntil(() => webcam.width > 16);

        // 表示
        Texture2D tex = new Texture2D(webcam.width, webcam.height);
        tex.SetPixels(webcam.GetPixels());
        tex.Apply();

        cameraView.style.backgroundImage = new StyleBackground(tex);
        Debug.Log("表示開始");
    }
    void Update()
    {
        if (webcam != null && webcam.isPlaying)
        {
            // 表示
            Texture2D tex = new Texture2D(webcam.width, webcam.height);
            tex.SetPixels(webcam.GetPixels());
            tex.Apply();

            cameraView.style.backgroundImage = new StyleBackground(tex);
        }
    }
}