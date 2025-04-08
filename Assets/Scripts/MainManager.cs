using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public TextMeshProUGUI BestScoreText;
    public TextMeshProUGUI ScoreText;
    public GameObject GameOverText;
    public GameObject PauseMenu;

    private bool m_GameOver = false;
    private bool m_Started = false;
    private bool isPaused = false;

    private int m_Points;
    private int bestScore = 0;
    private string playerName;
    private string bestPlayerName = "Player";
    
    // Start is called before the first frame update
    void Start()
    {
        playerName = PlayerPrefs.GetString("PlayerName", "Player");
        
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        bestPlayerName = PlayerPrefs.GetString("PlayerName", "Player");

        BestScoreText.text = $"Best score: {bestPlayerName}: {bestScore}";
        ScoreText.text = $"Ім'я: {playerName} | Рахунок: {m_Points}";

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        int[] pointCountArray = new [] {1,1,2,2,5,5};

        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }

        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Ім'я: {playerName} | Рахунок: {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        if (m_Points > bestScore)
        {
            bestScore = m_Points;
            bestPlayerName = playerName;

            PlayerPrefs.SetInt("BestScore", bestScore);
            PlayerPrefs.SetString("PlayerName", bestPlayerName);
            
            // BestScoreText.text = $"Best score: {bestPlayerName}: {bestScore}";
        }

        AddHighScore(playerName, m_Points);
    }


    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        PauseMenu.SetActive(true); 
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        PauseMenu.SetActive(false);
    }
    void AddHighScore(string name, int score)
    {
        List<HighScoreEntry> highScores = new List<HighScoreEntry>();

        int count = PlayerPrefs.GetInt("HighScoreCount", 0);
        for (int i = 0; i < count; i++)
        {
            string n = PlayerPrefs.GetString("HighScoreName" + i);
            int s = PlayerPrefs.GetInt("HighScoreScore" + i);
            highScores.Add(new HighScoreEntry { playerName = n, score = s });
        }

        highScores.Add(new HighScoreEntry { playerName = name, score = score });
        highScores.Sort((a, b) => b.score.CompareTo(a.score));
        if (highScores.Count > 10) highScores.RemoveAt(10);

        PlayerPrefs.SetInt("HighScoreCount", highScores.Count);
        for (int i = 0; i < highScores.Count; i++)
        {
            PlayerPrefs.SetString("HighScoreName" + i, highScores[i].playerName);
            PlayerPrefs.SetInt("HighScoreScore" + i, highScores[i].score);
        }

        PlayerPrefs.Save();
    }
    
    [System.Serializable]
    public class HighScoreEntry
    {
        public string playerName;
        public int score;
    }
}
