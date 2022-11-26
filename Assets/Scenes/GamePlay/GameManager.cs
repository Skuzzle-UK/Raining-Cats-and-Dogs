using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField]
    private InputActionAsset _actions;
    private bool _firstTargetEjected = false;
    public bool FirstTargetEjected { get { return _firstTargetEjected; } }

    [SerializeField]
    private GameObject _getReadyUI;

    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        try
        {
            var rebinds = PlayerPrefs.GetString("rebinds");
            if (!string.IsNullOrEmpty(rebinds))
                _actions.LoadBindingOverridesFromJson(rebinds);
        }
        catch { }

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
            if (FirstTargetEjected)
            {
                Instance._timer -= Time.deltaTime;
            }
            if (Instance._timer <= 0)
            {
                GameOver();
            }
        }
        else
        {
            Time.timeScale = 0;
        };
    }

    public void TargetEjected()
    {
        _firstTargetEjected = true;
        _getReadyUI.active = false;
    }

    public void StartGame()
    {
        Instance._UI.SetActive(true);
        Instance._gameOverUI.SetActive(false);
        Instance._timer = (_timerStartVal * 60);
        Instance.playing = true;
        try
        {
            GameAudio.Instance.UnMuteSFX();
        }
        catch (Exception e) { Debug.Log(e); }
    }

    private void GameOver()
    {
        Instance.playing = false;
        try
        {
            GameAudio.Instance.MuteSFX();
        }
        catch (Exception e) { Debug.Log(e); }
        Instance._UI.SetActive(false);
        Instance._gameOverUI.SetActive(true);
        Instance._nameInput.Select();
        Instance._nameInput.ActivateInputField();
    }

    public int TimeRemaining
    {
        get
        {
            int val = (int)Math.Round(Instance._timer);
            return val;
        }
    }

    public void ReduceTime(float seconds)
    {
        Instance._timer -= seconds;
    }

    public void AddTime(float time)
    {
        Instance._timer += time;
    }

    public int Score
    {
        get
        { return Instance._score; }
    }

    public void AddScore(int value)
    {
        Instance._score += value;
    }

    public void SubmitScore()
    {
        LeaderboardManager.PostScore(Instance._nameInput.text, Instance.Score);
        QuitGame();
    }

    public void QuitGame()
    {
        Instance.playing = true;
        Time.timeScale = 1;
        LoadingData.LoadScene("MainMenu");
    }
}
