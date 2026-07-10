using UnityEngine;
using TMPro;  // TextMeshPro用
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("ライフ設定")]
    [SerializeField] private int maxLife = 5;
    private int currentLife;

    [Header("タイマー設定")]
    private float timeLimit = 90f;
    private float timeRemaining;
    public static bool isGameActive = false;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI lifeText;       // ← TextMeshProUGUIに変更
    [SerializeField] private TextMeshProUGUI timerText;      // ← 同上
    [SerializeField] private TextMeshProUGUI finalScoreText; // ← 同上
    [SerializeField] private GameObject gameOverPanel;

    [Header("クリア設定")]
    [SerializeField] private int timeBonusPerSecond = 100; // クリア時の残り時間ボーナス(1秒あたり)

    private Transform player;
    private bool isGameOver = false;
    private bool isCleared = false;

    float start_z;

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
        start_z = player.position.z;
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
        float distanceZ = player != null ? player.position.z - start_z : 0f;
        int distanceScore = Mathf.FloorToInt(distanceZ) * 10;
        int lifeBonus = currentLife * 500;
        int score = distanceScore + lifeBonus;

        if (isCleared)
            score += Mathf.FloorToInt(timeRemaining) * timeBonusPerSecond;

        return score;
    }

    public void GameClear()
    {
        if (!isGameActive || isGameOver) return;
        isCleared = true;
        Time.timeScale = 0f;   // プレイヤーやアニメも止める
        EndGame();
    }
    private void EndGame()
    {
        isGameOver = true;
        isGameActive = false;

        int score = CalculateScore();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (finalScoreText != null)
        {
            string title = isCleared ? "GAME CLEAR!" : "GAME OVER";
            finalScoreText.text =
                $"{title}\n" +
                $"スコア：{score}\n" +
                $"進んだ距離：{Mathf.FloorToInt(player.position.z - start_z)}m\n" +
                $"残りHP：{currentLife}/{maxLife}";
        }
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name);
    }
    public void RemoveTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(
            "TitleScene");
    }

    public bool IsGameOver => isGameOver;
}