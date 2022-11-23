using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoresDisplay : MonoBehaviour
{
    private TextMeshProUGUI _position;
    private TextMeshProUGUI _name;
    private TextMeshProUGUI _date;
    private TextMeshProUGUI _score;

    string leaderboardJson;

    private void Awake()
    {
        _position = transform.Find("Position").GetComponent<TextMeshProUGUI>();
        _position.text = "";
        _name = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        _name.text = "";
        _date = transform.Find("Date").GetComponent<TextMeshProUGUI>();
        _date.text = "";
        _score = transform.Find("Score").GetComponent<TextMeshProUGUI>();
        _score.text = "";
    }

    private void FixedUpdate()
    {
        if (leaderboardJson != LeaderboardManager.ScoresJson || LeaderboardManager.ScoresJson is null)
        {
            if (!LeaderboardManager.Downloading)
            {
                StartCoroutine(LeaderboardManager.DownloadTopScores(25));
            }
            leaderboardJson = LeaderboardManager.ScoresJson;
        }
        else if (leaderboardJson != "0")
        {
            _position.text = "";
            _name.text = "";
            _date.text = "";
            _score.text = "";
            int position = 0;
            List<LeaderboardRow> rows = JsonConvert.DeserializeObject<List<LeaderboardRow>>(leaderboardJson);
            foreach (LeaderboardRow row in rows)
            {
                int days = (int)(DateTime.Now - row.date).TotalDays;
                int minutes = 0;
                if (days == 0)
                {
                    minutes = (int)(DateTime.Now - row.date).TotalMinutes;
                }
                position++;
                _position.text += $"{position}\n";
                _name.text += $"{row.name}\n";
                if (days == 1) _date.text += $"{days} day\n";
                if (days >= 2) _date.text += $"{days} days\n";
                if (days == 0 && minutes == 1) _date.text += $"{minutes} min\n";
                if (days == 0 && minutes >= 2) _date.text += $"{minutes} mins\n";
                if (days == 0 && minutes == 0) _date.text += $"< 1 min\n";
                _score.text += $"{row.score}\n";
            }
        }
        else
        {
            _name.text = "No scores submitted yet.";
        }
    }
}
