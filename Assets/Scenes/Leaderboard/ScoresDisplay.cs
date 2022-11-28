using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;
using UnityEngine.UIElements;
using System.Collections;

public class ScoresDisplay : MonoBehaviour
{
    private TextMeshProUGUI _position;
    private TextMeshProUGUI _name;
    private TextMeshProUGUI _date;
    private TextMeshProUGUI _score;

    string highScoresJson;
    string playerScoresJson;

    private ToggleSlider _playerPositionToggleSlider;
    private bool _toggleOn;

    [SerializeField]
    private GameObject _playerHighlighter;
    private float _playerHighlighterStartYPosition;

    private void Awake()
    {
        _playerHighlighterStartYPosition = _playerHighlighter.transform.localPosition.y;

        _playerPositionToggleSlider = GetComponentInChildren<ToggleSlider>();

        if (!PlayerPrefs.HasKey("bestscore") && !PlayerPrefs.HasKey("name"))
        {
            _playerPositionToggleSlider.gameObject.SetActive(false);
        }

        _position = transform.Find("Position").GetComponent<TextMeshProUGUI>();
        _position.text = "";
        _name = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        _name.text = "";
        _date = transform.Find("Date").GetComponent<TextMeshProUGUI>();
        _date.text = "";
        _score = transform.Find("Score").GetComponent<TextMeshProUGUI>();
        _score.text = "";
    }


    private void Start()
    {
        try
        {
            if (PlayerPrefs.GetInt("playerPositionViewChecked") > 0)
            {
                _playerPositionToggleSlider.TurnOn();
                _toggleOn = _playerPositionToggleSlider.On;
            }
        }
        catch { }
    }

    private void FixedUpdate()
    {
        if (!_playerPositionToggleSlider.On)
        {
            DisplayHighScores();
        }
        else
        {
            DisplayPlayerScores();
        }
        SavePrefs();
    }

    private void DisplayHighScores()
    {
        if (highScoresJson != LeaderboardManager.HighScoresJson || LeaderboardManager.HighScoresJson is null)
        {
            if (!LeaderboardManager.DownloadingHigh)
            {
                StartCoroutine(LeaderboardManager.DownloadTopScores(25));
            }
            highScoresJson = LeaderboardManager.HighScoresJson;
        }
        else if (highScoresJson != "0")
        {
            _position.text = "";
            _name.text = "";
            _date.text = "";
            _score.text = "";
            List<LeaderboardRow> rows = JsonConvert.DeserializeObject<List<LeaderboardRow>>(highScoresJson);
            foreach (LeaderboardRow row in rows)
            {
                int days = (int)(DateTime.Now - row.date).TotalDays;
                int minutes = 0;
                if (days == 0)
                {
                    minutes = (int)(DateTime.Now - row.date).TotalMinutes;
                }
                _position.text += $"{row.position}\n";
                _name.text += $"{row.name}\n";
                if (days == 1) _date.text += $"{days} day\n";
                if (days >= 2) _date.text += $"{days} days\n";
                if (days == 0 && minutes == 1) _date.text += $"{minutes} min\n";
                if (days == 0 && minutes >= 2) _date.text += $"{minutes} mins\n";
                if (days == 0 && minutes == 0) _date.text += $"< 1 min\n";
                _score.text += $"{row.score}\n";
            }
            _playerHighlighter.SetActive(false);
        }
        else
        {
            _name.text = "No scores submitted yet.";
        }
    }

    private void DisplayPlayerScores()
    {
        _playerHighlighter.SetActive(true);
        if (playerScoresJson != LeaderboardManager.PlayerScoresJson || LeaderboardManager.PlayerScoresJson is null)
        {
            if (!LeaderboardManager.DownloadingPlayer)
            {
                StartCoroutine(LeaderboardManager.DownloadPlayerScores(25));
            }
            playerScoresJson = LeaderboardManager.PlayerScoresJson;
        }
        else if (playerScoresJson != "0")
        {
            _position.text = "";
            _name.text = "";
            _date.text = "";
            _score.text = "";
            List<LeaderboardRow> rows = JsonConvert.DeserializeObject<List<LeaderboardRow>>(playerScoresJson);
            
            foreach (LeaderboardRow row in rows)
            {
                int days = (int)(DateTime.Now - row.date).TotalDays;
                int minutes = 0;
                if (days == 0)
                {
                    minutes = (int)(DateTime.Now - row.date).TotalMinutes;
                }
                _position.text += $"{row.position}\n";
                _name.text += $"{row.name}\n";
                if (days == 1) _date.text += $"{days} day\n";
                if (days >= 2) _date.text += $"{days} days\n";
                if (days == 0 && minutes == 1) _date.text += $"{minutes} min\n";
                if (days == 0 && minutes >= 2) _date.text += $"{minutes} mins\n";
                if (days == 0 && minutes == 0) _date.text += $"< 1 min\n";
                _score.text += $"{row.score}\n";
                if (row.name == PlayerPrefs.GetString("name") && row.score == PlayerPrefs.GetInt("bestscore"))
                {
                    _playerHighlighter.transform.localPosition = new Vector3(_playerHighlighter.transform.localPosition.x, _playerHighlighterStartYPosition - (27.5f * (row.position - rows[0].position)), 0);
                }
            }
        }
        else
        {
            _name.text = "No scores submitted yet.";
        }
    }

    private void SavePrefs()
    {
        if (_toggleOn != _playerPositionToggleSlider.On)
        {
            _toggleOn = _playerPositionToggleSlider.On;
            if (_playerPositionToggleSlider.On)
            {
                PlayerPrefs.SetInt("playerPositionViewChecked", 1);
                PlayerPrefs.Save();
                return;
            }
            PlayerPrefs.SetInt("playerPositionViewChecked", 0);
            PlayerPrefs.Save();
        }
    }
}
