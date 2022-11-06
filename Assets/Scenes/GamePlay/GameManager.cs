using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float _timerStartVal = 5f;
    private float _timer;
    [SerializeField]
    private int _score;
    public bool playing = false;
    private GameObject _gameOverUI;
    private GameObject _UI;
    private TMP_InputField _nameInput;

    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        _UI = GameObject.Find("UI");
        _gameOverUI = GameObject.Find("GameOverUI");
        _nameInput = _gameOverUI.GetComponentInChildren<TMP_InputField>();

        StartGame();
    }

    public void Update()
    {
        if (playing)
        {
            Time.timeScale = 1;
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                GameOver();
            }
        }
        else
        {
            Time.timeScale = 0;
        };
    }

    public void StartGame()
    {
        _UI.SetActive(true);
        _gameOverUI.SetActive(false);
        _timer = (_timerStartVal * 60);
        playing = true;
    }

    private void GameOver()
    {
        playing = false;
        _UI.SetActive(false);
        _gameOverUI.SetActive(true);
    }

    public int TimeRemaining
    {
        get
        {
            int val = (int)Math.Round(_timer);
            return val;
        }
    }

    public void AddTime(float time)
    {
        _timer += time;
    }

    public int Score
    {
        get
        { return _score; }
    }

    public void AddScore(int value)
    {
        _score += value;
    }

    public void SubmitScore()
    {
        LeaderboardManager.PostScore(_nameInput.text, Score);
        LoadingData.LoadScene("MainMenu");
    }
}
