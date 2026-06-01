using UnityEngine;
using TMPro;  // TextMeshPro用

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("ライフ設定")]
    [SerializeField] private int maxLife = 5;
    private int currentLife;

    [Header("タイマー設定")]
    private float timeLimit = 90f;
    private float timeRemaining;
    private bool isGameActive = false;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI lifeText;       // ← TextMeshProUGUIに変更
    [SerializeField] private TextMeshProUGUI timerText;      // ← 同上
    [SerializeField] private TextMeshProUGUI finalScoreText; // ← 同上
    [SerializeField] private GameObject gameOverPanel;

    private Transform player;
    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        currentLife = maxLife;
        timeRemaining = timeLimit;
        isGameActive = true;
        player = FindFirstObjectByType<PlayerController>().transform;
        UpdateLifeUI();
    }

    private void Update()
    {
        if (!isGameActive || isGameOver) return;

        timeRemaining -= Time.deltaTime;
        UpdateTimerUI();

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            EndGame();
        }
    }

    private void UpdateLifeUI()
    {
        if (lifeText == null) return;
        string hearts = "";
        for (int i = 0; i < maxLife; i++)
            hearts += (i < currentLife) ? "♥" : "♡";
        lifeText.text = hearts;
    }

    private void UpdateTimerUI()
    {
        if (timerText == null) return;
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        timerText.color = (timeRemaining <= 10f) ? Color.red : Color.white;
    }

    public void TakeDamage()
    {
        if (!isGameActive || isGameOver) return;
        currentLife--;
        UpdateLifeUI();
        if (currentLife <= 0)
        {
            currentLife = 0;
            EndGame();
        }
    }

    private int CalculateScore()
    {
        float distanceZ = player != null ? player.position.z : 0f;
        int distanceScore = Mathf.FloorToInt(distanceZ) * 10;
        int lifeBonus = currentLife * 500;
        return distanceScore + lifeBonus;
    }

    private void EndGame()
    {
        isGameOver = true;
        isGameActive = false;

        int score = CalculateScore();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (finalScoreText != null)
            finalScoreText.text =
                $"スコア：{score}\n" +
                $"進んだ距離：{Mathf.FloorToInt(player.position.z)}m\n" +
                $"残りHP：{currentLife}/{maxLife}";
    }

    public void Retry()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public bool IsGameOver => isGameOver;
}