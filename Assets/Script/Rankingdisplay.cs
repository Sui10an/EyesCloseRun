using System.Text;
using TMPro;
using UnityEngine;

/// <summary>
/// ランキングをTextMeshProに一覧表示する。
/// ランキングパネルの中のTextにアタッチするか、パネルにアタッチしてTextを参照させる。
/// パネルがSetActive(true)されるたびに自動で最新表示に更新される。
/// </summary>
public class RankingDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rankingText;

    [Tooltip("今回の記録の順位をハイライトする場合に設定(-1なら無し)")]
    public static int lastRank = -1;

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (rankingText == null) return;

        var entries = RankingManager.GetRanking();

        if (entries.Count == 0)
        {
            rankingText.text = "No records yet";
            return;
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("RANKING");

        for (int i = 0; i < entries.Count; i++)
        {
            var e = entries[i];
            string line = $"{i + 1,2}. {e.score,6:N0}";

            // 今回の記録なら黄色でハイライト(文字は増やさず色だけで示す)
            if (i + 1 == lastRank)
            {
                line = $"<color=yellow>{line}</color>";
            }

            sb.AppendLine(line);
        }

        rankingText.text = sb.ToString();
    }
}