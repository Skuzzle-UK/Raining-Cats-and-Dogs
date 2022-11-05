using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float _timerStartVal = 5f;
    private float _timer;
    [SerializeField]
    private int _score;
    private bool playing = false;

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

        StartGame();
    }

    public void Update()
    {
        if (playing)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        playing = false;
        LeaderboardManager.PostScore("anon", 120);
        LoadingData.LoadScene("MainMenu");
    }

    public void StartGame()
    {
        _timer = (_timerStartVal * 60);
        playing = true;
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
}
