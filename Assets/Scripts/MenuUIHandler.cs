using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuUIHandler : MonoBehaviour
{
    public TMP_InputField nameInputField;

    public void StartGame()
    {
        string playerName = nameInputField.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            PlayerPrefs.SetString("PlayerName", playerName);
        }

        SceneManager.LoadScene("main");
    }
    public void ShowHighScores()
    {
        SceneManager.LoadScene("HighScoreScene");
    }

}
