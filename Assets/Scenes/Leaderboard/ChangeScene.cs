using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        LoadingData.LoadScene("MainMenu");
    }
}
