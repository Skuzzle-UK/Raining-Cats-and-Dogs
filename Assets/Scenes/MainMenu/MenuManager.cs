using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private LeaderboardManager _leaderboard = new LeaderboardManager();
    public static MenuManager Instance { get; private set; }
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
    }

    public void StartGame()
    {
        LoadingData.LoadScene("GamePlay");
    }

    public void ShowLeaderboard()
    {
        StartCoroutine(_leaderboard.GetTopScores(2));
    }
}
