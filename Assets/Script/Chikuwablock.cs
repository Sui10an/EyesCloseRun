using System.Collections;
using UnityEngine;

/// <summary>
/// ちくわブロック(演出強化版):
/// プレイヤーが乗ると震え+赤点滅で「消えるぞ!」をアピールし、
/// 一定時間後にパーティクルと音を出して消える。オプションで復活。
/// </summary>
public class ChikuwaBlock : MonoBehaviour
{
    [Header("タイミング設定")]
    [Tooltip("乗ってから消えるまでの秒数")]
    [SerializeField] private float fallDelay = 1.5f;

    [Tooltip("消えてから復活するまでの秒数(0以下なら復活しない)")]
    [SerializeField] private float respawnDelay = 3f;

    [Header("震え設定")]
    [SerializeField] private float shakeAmount = 0.05f;
    [SerializeField] private float shakeSpeed = 40f;
    [Tooltip("消える直前ほど震えを強くする")]
    [SerializeField] private bool intensifyShake = true;

    [Header("点滅アピール設定")]
    [Tooltip("警告時に混ぜる色(赤推奨)")]
    [SerializeField] private Color warningColor = new Color(1f, 0.3f, 0.3f);

    [Tooltip("点滅の速さ(乗り始め)")]
    [SerializeField] private float blinkSpeedStart = 4f;

    [Tooltip("点滅の速さ(消える直前)。だんだん速くなって焦らせる")]
    [SerializeField] private float blinkSpeedEnd = 20f;

    [Header("サウンド(任意)")]
    [Tooltip("乗っている間ループ再生する警告音(ガタガタ音など)")]
    [SerializeField] private AudioClip rumbleSound;

    [Tooltip("消える瞬間の音(ポンッなど)")]
    [SerializeField] private AudioClip popSound;

    [Header("パーティクル(任意)")]
    [Tooltip("消える瞬間に出すエフェクトのPrefab")]
    [SerializeField] private ParticleSystem popEffectPrefab;

    private Vector3 originalPosition;
    private float standTimer;
    private bool playerOnBlock;
    private bool isGone;

    private Renderer blockRenderer;
    private Collider blockCollider;
    private AudioSource audioSource;
    private Color originalColor;
    private static readonly int ColorProp = Shader.PropertyToID("_Color");
    private MaterialPropertyBlock propBlock;

    private void Awake()
    {
        originalPosition = transform.position;
        blockRenderer = GetComponent<Renderer>();
        blockCollider = GetComponent<Collider>();
        propBlock = new MaterialPropertyBlock();
        originalColor = blockRenderer.sharedMaterial.HasProperty(ColorProp)
            ? blockRenderer.sharedMaterial.GetColor(ColorProp)
            : Color.white;

        // AudioSource がなければ自動追加
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && (rumbleSound != null || popSound != null))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1f; // 3Dサウンド
        }
    }

    private void Update()
    {
        if (isGone) return;

        if (playerOnBlock)
        {
            standTimer += Time.deltaTime;
            float progress = Mathf.Clamp01(standTimer / fallDelay); // 0→1

            UpdateShake(progress);
            UpdateBlink(progress);
            UpdateRumbleSound();

            if (standTimer >= fallDelay)
            {
                StartCoroutine(Disappear());
            }
        }
        else
        {
            // 降りたら全部リセット
            standTimer = 0f;
            transform.position = originalPosition;
            SetColor(originalColor);
            StopRumbleSound();
        }
    }

    // ── 震え ──────────────────────────────
    private void UpdateShake(float progress)
    {
        float intensity = shakeAmount;
        if (intensifyShake)
        {
            intensity *= Mathf.Lerp(1f, 2.5f, progress);
        }

        Vector3 offset = new Vector3(
            Mathf.Sin(Time.time * shakeSpeed) * intensity,
            0f,
            Mathf.Cos(Time.time * shakeSpeed * 0.9f) * intensity * 0.5f
        );
        transform.position = originalPosition + offset;
    }

    // ── 点滅:だんだん速く赤くチカチカ ──────
    private void UpdateBlink(float progress)
    {
        float blinkSpeed = Mathf.Lerp(blinkSpeedStart, blinkSpeedEnd, progress);
        // 0〜1を往復する値。progressが上がるほど警告色が濃く出る
        float t = (Mathf.Sin(Time.time * blinkSpeed * Mathf.PI) + 1f) * 0.5f;
        t *= Mathf.Lerp(0.5f, 1f, progress);
        SetColor(Color.Lerp(originalColor, warningColor, t));
    }

    private void SetColor(Color color)
    {
        blockRenderer.GetPropertyBlock(propBlock);
        propBlock.SetColor(ColorProp, color);
        blockRenderer.SetPropertyBlock(propBlock);
    }

    // ── サウンド ──────────────────────────
    private void UpdateRumbleSound()
    {
        if (audioSource == null || rumbleSound == null) return;
        if (!audioSource.isPlaying)
        {
            audioSource.clip = rumbleSound;
            audioSource.loop = true;
            audioSource.Play();
        }
        // 消える直前ほどピッチを上げて緊張感アップ
        audioSource.pitch = Mathf.Lerp(1f, 1.5f, standTimer / fallDelay);
    }

    private void StopRumbleSound()
    {
        if (audioSource != null && audioSource.isPlaying && audioSource.loop)
        {
            audioSource.Stop();
        }
    }

    // ── 消える処理 ────────────────────────
    private IEnumerator Disappear()
    {
        isGone = true;
        playerOnBlock = false;
        transform.position = originalPosition;
        StopRumbleSound();

        // 消える瞬間の演出
        if (popEffectPrefab != null)
        {
            ParticleSystem fx = Instantiate(popEffectPrefab, transform.position, Quaternion.identity);
            Destroy(fx.gameObject, fx.main.duration + fx.main.startLifetime.constantMax);
        }
        if (audioSource != null && popSound != null)
        {
            audioSource.pitch = 1f;
            audioSource.PlayOneShot(popSound);
        }

        blockRenderer.enabled = false;
        blockCollider.enabled = false;
        SetColor(originalColor);

        if (respawnDelay > 0f)
        {
            yield return new WaitForSeconds(respawnDelay);
            blockRenderer.enabled = true;
            blockCollider.enabled = true;
            standTimer = 0f;
            isGone = false;
        }
    }

    // ── 接触判定 ──────────────────────────
    private void OnCollisionEnter(Collision collision)
    {
        if (IsPlayerOnTop(collision)) playerOnBlock = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (IsPlayerOnTop(collision)) playerOnBlock = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) playerOnBlock = false;
    }

    private bool IsPlayerOnTop(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return false;

        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y < -0.5f) return true;
        }
        return false;
    }
}