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
    public int position;
}

public static class LeaderboardManager
{
    private static readonly string secretKey = "ACoolSecretWord"; //must match secret key of php document
    private static readonly string addScoreURL = "https://rainingcatsanddogs.skuzzle.com/leaderboard/addScore.php?";
    private static readonly string displayTopURL = "https://rainingcatsanddogs.skuzzle.com/leaderboard/displayTop.php?";
    private static readonly string displayPlayerURL = "https://rainingcatsanddogs.skuzzle.com/leaderboard/displayPlayer.php?";
    private static string _leaderboardHighScoresJson;
    private static string _leaderboardPlayerScoresJson;
    public static bool DownloadingHigh = false;
    public static bool DownloadingPlayer = false;

    public static void PostScore(string name, int score)
    {
        if (score > PlayerPrefs.GetInt("bestscore"))
        {
            PlayerPrefs.SetInt("bestscore", score);
            PlayerPrefs.SetString("name", name);
            PlayerPrefs.Save();
        }
        string hash = HashInput(name + score + secretKey);
        string postURL = $"{addScoreURL}name={name}&score={score}&hash={hash}";
        UnityWebRequest hs_post = UnityWebRequest.Post(postURL, hash);
        hs_post.SendWebRequest();
        if (hs_post.error != null)
        {
            Debug.Log($"There was an error posting the high score: {hs_post.error}");
        }
        _leaderboardHighScoresJson = null;
        _leaderboardPlayerScoresJson = null;
    }

    public static IEnumerator DownloadTopScores(int limit)
    {
        DownloadingHigh = true;
        UnityWebRequest hs_get = UnityWebRequest.Get($"{displayTopURL}limit={limit}");
        yield return hs_get.SendWebRequest();
        if (hs_get.error != null)
        {
            Debug.Log($"There was an error getting the high score: {hs_get.error}");
        }
        else
        {
            _leaderboardHighScoresJson = hs_get.downloadHandler.text;
        }
        DownloadingHigh = false;
    }

    public static IEnumerator DownloadPlayerScores(int limit)
    {

        if (PlayerPrefs.HasKey("bestscore") && PlayerPrefs.HasKey("name"))
        {
            DownloadingPlayer = true;
            UnityWebRequest hs_get = UnityWebRequest.Get($"{displayPlayerURL}bestscore={PlayerPrefs.GetInt("bestscore")}&name={PlayerPrefs.GetString("name")}&limit={limit}");
            Debug.Log($"{displayPlayerURL}bestscore={PlayerPrefs.GetInt("bestscore")}&name={PlayerPrefs.GetString("name")}&limit={limit}");
            yield return hs_get.SendWebRequest();
            if (hs_get.error != null)
            {
                Debug.Log($"There was an error getting the player score: {hs_get.error}");
            }
            else
            {
                _leaderboardPlayerScoresJson = hs_get.downloadHandler.text;
            }
            DownloadingPlayer = false;
        }
    }

    private static string HashInput(string input)
    {
        SHA256Managed hm = new SHA256Managed();
        byte[] hashValue = hm.ComputeHash(System.Text.Encoding.ASCII.GetBytes(input));
        string hash_convert = BitConverter.ToString(hashValue).Replace("-", "").ToLower();
        return hash_convert;
    }

    public static string HighScoresJson
    {
        get { return _leaderboardHighScoresJson; }
    }

    public static string PlayerScoresJson
    {
        get { return _leaderboardPlayerScoresJson; }
    }
}
