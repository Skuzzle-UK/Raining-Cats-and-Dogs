using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderBoardRow
{
    public string name;
    public int score;
    public DateTime date;
}

public class LeaderboardManager
{
    private static readonly string secretKey = ""; //must match secret key of php document
    private static readonly string addScoreURL = "http://rainingcatsanddogs.skuzzle.com/leaderboard/addScore.php?";
    private static readonly string displayTopURL = "http://rainingcatsanddogs.skuzzle.com/leaderboard/displayTop.php?";

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
    }

    public IEnumerator GetTopScores(int limit)
    {
        UnityWebRequest hs_get = UnityWebRequest.Get($"{displayTopURL}limit={limit}");
        yield return hs_get.SendWebRequest();
        if (hs_get.error != null)
        {
            Debug.Log($"There was an error getting the high score: {hs_get.error}");
        }
        else
        {
            string json = hs_get.downloadHandler.text;
            List<LeaderBoardRow> leaderboard = JsonConvert.DeserializeObject<List<LeaderBoardRow>>(json);
            foreach (LeaderBoardRow row in leaderboard)
            {
                Debug.Log($"{row.name} : {row.score} : {row.date}");
            }
        }
    }

    private static string HashInput(string input)
    {
        SHA256Managed hm = new SHA256Managed();
        byte[] hashValue = hm.ComputeHash(System.Text.Encoding.ASCII.GetBytes(input));
        string hash_convert = BitConverter.ToString(hashValue).Replace("-", "").ToLower();
        return hash_convert;
    }
}
