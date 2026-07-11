using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ローカルランキング管理(PlayerPrefsにJSON保存)。
/// MonoBehaviourではないので、どこからでも RankingManager.AddScore(score) で呼べる。
/// </summary>
public static class RankingManager
{
    private const string SaveKey = "ChikuwaRanking";
    private const int MaxEntries = 5; // トップ5まで保存

    [Serializable]
    public class RankingEntry
    {
        public int score;
        public string date;      // 記録日時
        public bool cleared;     // クリアした記録かどうか

        public RankingEntry(int score, bool cleared)
        {
            this.score = score;
            this.cleared = cleared;
            this.date = DateTime.Now.ToString("MM/dd HH:mm");
        }
    }

    // JsonUtilityはListを直接シリアライズできないのでラッパーを使う
    [Serializable]
    private class RankingData
    {
        public List<RankingEntry> entries = new List<RankingEntry>();
    }

    /// <summary>
    /// スコアを登録し、ランクイン順位(1始まり)を返す。圏外なら -1。
    /// </summary>
    public static int AddScore(int score, bool cleared)
    {
        RankingData data = Load();

        RankingEntry newEntry = new RankingEntry(score, cleared);
        data.entries.Add(newEntry);

        // スコア降順に並べ替え、上位のみ残す
        data.entries.Sort((a, b) => b.score.CompareTo(a.score));
        if (data.entries.Count > MaxEntries)
        {
            data.entries.RemoveRange(MaxEntries, data.entries.Count - MaxEntries);
        }

        Save(data);

        // 今回の記録が何位に入ったか調べる(同スコアの場合は上位扱い)
        int rank = data.entries.IndexOf(newEntry);
        return rank >= 0 ? rank + 1 : -1;
    }

    /// <summary>ランキング一覧を取得(スコア降順)。</summary>
    public static List<RankingEntry> GetRanking()
    {
        return Load().entries;
    }

    /// <summary>最高スコアを取得。記録がなければ0。</summary>
    public static int GetHighScore()
    {
        var entries = Load().entries;
        return entries.Count > 0 ? entries[0].score : 0;
    }

    /// <summary>ランキングを全消去(デバッグ用)。</summary>
    public static void ClearRanking()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        PlayerPrefs.Save();
    }

    private static RankingData Load()
    {
        if (!PlayerPrefs.HasKey(SaveKey)) return new RankingData();

        string json = PlayerPrefs.GetString(SaveKey);
        RankingData data = JsonUtility.FromJson<RankingData>(json);
        return data ?? new RankingData();
    }

    private static void Save(RankingData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }
}