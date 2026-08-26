using System.Collections;
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

    [Header("カウントダウン設定")]
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private int countdownSeconds = 3;
    [SerializeField] private string startMessage = "START!";
    [SerializeField] private float startMessageDuration = 0.5f;

    [Header("クリア設定")]
    [SerializeField] private int timeBonusPerSecond = 100; // クリア時の残り時間ボーナス(1秒あたり)

    [Header("サウンド")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip clearSound;
    [SerializeField] private AudioClip gameOverSound;

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
        isGameActive = false; // カウントダウンが終わるまで動けない
        player = FindFirstObjectByType<PlayerController>().transform;
        start_z = player.position.z;
        UpdateLifeUI();

        StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        if (countdownText != null) countdownText.gameObject.SetActive(true);

        for (int i = countdownSeconds; i > 0; i--)
        {
            if (countdownText != null) countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        if (countdownText != null)
        {
            countdownText.text = startMessage;
            yield return new WaitForSeconds(startMessageDuration);
            countdownText.gameObject.SetActive(false);
        }

        isGameActive = true;
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
        timerText.color = (timeRemaining <= 10f) ? Color.red : Color.black;
    }

    public void TakeDamage()
    {
        if (!isGameActive || isGameOver) return;
        currentLife--;
        UpdateLifeUI();

        if (currentLife <= 0)
        {
            // ライフ0の時は被弾音を鳴らさない(EndGame内でゲームオーバー音が鳴る)
            currentLife = 0;
            EndGame();
            return;
        }

        // ★ライフが残っている時だけダメージSEを再生
        if (audioSource != null && damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
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

        // ★追加:クリアSEを再生
        if (audioSource != null && clearSound != null)
        {
            audioSource.PlayOneShot(clearSound);
        }

        Time.timeScale = 0f;   // プレイヤーやアニメも止める
        EndGame();
    }

    private void EndGame()
    {
        isGameOver = true;
        isGameActive = false;

        // ★追加:ゲームオーバーSE(クリア時はGameClear側でクリアSEが鳴るので鳴らさない)
        if (!isCleared && audioSource != null && gameOverSound != null)
        {
            audioSource.PlayOneShot(gameOverSound);
        }

        int score = CalculateScore();

        // ★ここから追加:ランキングに登録して順位を取得
        int rank = RankingManager.AddScore(score, isCleared);
        RankingDisplay.lastRank = rank;
        // ★ここまで追加

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (finalScoreText != null)
        {
            string title = isCleared ? "GAME CLEAR!" : "GAME OVER";

            // ★追加:ランクインしていれば順位の行を作る
            string rankLine = rank > 0 ? $"ランキング {rank}位!\n" : "";

            finalScoreText.text =
                $"{title}\n" +
                rankLine +                                                     // ★追加
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