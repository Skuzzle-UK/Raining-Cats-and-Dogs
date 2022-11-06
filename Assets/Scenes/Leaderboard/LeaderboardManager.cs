using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderboardRow
{
    public string name;
    public int score;
    public DateTime date;
}

public static class LeaderboardManager
{
    private static readonly string secretKey = "ACoolSecretWord"; //must match secret key of php document
    private static readonly string addScoreURL = "http://rainingcatsanddogs.skuzzle.com/leaderboard/addScore.php?";
    private static readonly string displayTopURL = "http://rainingcatsanddogs.skuzzle.com/leaderboard/displayTop.php?";
    private static string _leaderboardJson;
    public static bool Downloading = false;

    public static void PostScore(string name, int score)
    {
        string hash = HashInput(name + score + secretKey);
        string postURL = $"{addScoreURL}name={name}&score={score}&hash={hash}";
        UnityWebRequest hs_post = UnityWebRequest.Post(postURL, hash);
        hs_post.SendWebRequest();
        if (hs_post.error != null)
        {
            Debug.Log($"There was an error posting the high score: {hs_post.error}");
        }
        _leaderboardJson = null;
    }

    public static IEnumerator DownloadTopScores(int limit)
    {
        Downloading = true;
        UnityWebRequest hs_get = UnityWebRequest.Get($"{displayTopURL}limit={limit}");
        yield return hs_get.SendWebRequest();
        if (hs_get.error != null)
        {
            Debug.Log($"There was an error getting the high score: {hs_get.error}");
        }
        else
        {
            _leaderboardJson = hs_get.downloadHandler.text;
        }
        Downloading = false;
    }

    private static string HashInput(string input)
    {
        SHA256Managed hm = new SHA256Managed();
        byte[] hashValue = hm.ComputeHash(System.Text.Encoding.ASCII.GetBytes(input));
        string hash_convert = BitConverter.ToString(hashValue).Replace("-", "").ToLower();
        return hash_convert;
    }

    public static string ScoresJson
    {
        get { return _leaderboardJson; }
    }
}
