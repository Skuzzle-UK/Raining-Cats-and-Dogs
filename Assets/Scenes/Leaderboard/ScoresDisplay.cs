using Newtonsoft.Json;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoresDisplay : MonoBehaviour
{
    TextMeshProUGUI tmp;
    string leaderboardJson;

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        tmp.text = "";

    }

    private void FixedUpdate()
    {
        if (leaderboardJson != LeaderboardManager.ScoresJson || LeaderboardManager.ScoresJson is null)
        {
            if (!LeaderboardManager.Downloading)
            {
                StartCoroutine(LeaderboardManager.DownloadTopScores(20));
            }
            leaderboardJson = LeaderboardManager.ScoresJson;
        }
        else if (leaderboardJson != "0")
        {
            tmp.text = "";
            int position = 0;
            List<LeaderboardRow> rows = JsonConvert.DeserializeObject<List<LeaderboardRow>>(leaderboardJson);
            foreach (LeaderboardRow row in rows)
            {
                position++;
                tmp.text += $"{position}. {row.name} : {row.score} : {row.date}\n";
            }
        }
        else
        {
            tmp.text = "No scores";
        }
    }
}
