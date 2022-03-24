using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    public GameObject bestScore;
    public GameObject congratulations;
    public GameObject inputField;

    private bool m_Started = false; 
    public static int m_Points;
    
    private bool m_GameOver = false;

    //score manager stuff
    private ScoreManager scoreManager;
    private bool m_HighScore = false;

    // Start is called before the first frame update
    void Start()
    {
        scoreManager = new ScoreManager();
        scoreManager.ReadHighScore();
        updateBestScore(scoreManager.GetPrevName(), scoreManager.GetPrevScore());

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
        }else if (m_HighScore)
        {
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)){
                congratulations.SetActive(false);
                inputField.SetActive(false);
                updateBestScore(scoreManager.GetPrevName(), m_Points);
                m_HighScore = false;
                m_GameOver = true;
                m_Points = 0;
                GameOverText.SetActive(true);
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
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        if (m_Points > scoreManager.GetPrevScore())
        {
            congratulations.SetActive(true);
            inputField.SetActive(true);
            m_HighScore = true;
        }
        else
        {
            m_GameOver = true;
            GameOverText.SetActive(true);
        }
    }

    private void updateBestScore(string name = "nobody", int score = 0)
    {
        GameObject bestScoreObject = GameObject.Find("Best Score");
        bestScoreObject.GetComponent<Text>().text = "Best Score : " + name + " " + score;
    }
}
