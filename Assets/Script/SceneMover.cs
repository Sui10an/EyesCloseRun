using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneMover : MonoBehaviour
{
    public void SceneMove()
    {
        SceneManager.LoadScene("Sample Scene");
    }
}
