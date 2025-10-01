using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public GameObject m_pipe;
    public float m_firstPipeAt = 15.0f;
    public float m_horizSpacing = 6.0f;
    public float m_vertSpacing = 4.0f;
    public float m_maxY = 4.0f;
    public GameObject m_pauseMenu;

    Vector3 m_lastPos;
    Vector3 m_lastCamPos;
    PlayerBird m_player;
    bool m_isPaused = false;

    // score and time keeper

    public TextMeshProUGUI m_timerText;
    public TextMeshProUGUI m_scoreText;

    float m_time = 0f;
    int m_score = 0;
    bool m_timer = true;

    // Start is called before the first frame update
    void Start()
    {
        m_lastCamPos = Camera.main.transform.position;
        m_lastPos = m_lastCamPos;
        m_lastCamPos.x -= m_horizSpacing;   // spawn the first pipe now
        m_lastPos.x += m_firstPipeAt;       // the first pipe is this far forward from the starting position

        // grab the player for future reference
        m_player = FindFirstObjectByType<PlayerBird>();

        SetPause(false);
    }

    // Update is called once per frame
    void Update()
    {
        {   // TODO make pipes
            float world_pos = Camera.main.transform.position.x;

            while (world_pos - m_lastCamPos.x > m_horizSpacing) 
            {
                // determine vertical pos for pipe
                m_lastCamPos.x += m_horizSpacing;

                // spawn the pipe at x pos, random y 
                float yPos = Random.Range(-m_vertSpacing, m_maxY);
                Vector3 s_pos = new Vector3(m_lastPos.x, yPos, 0f);
                Instantiate(m_pipe, s_pos, Quaternion.identity);

                // advance last pipe x pos to nxt spwan
                m_lastPos.x += m_horizSpacing;
            }
        }

        if (null == m_player)
        {   // Player Died
            StartCoroutine(GameOver());
        }

        // keys
        if (Input.GetKeyDown(KeyCode.Escape))
        {   // this doubles as the option key in the android navigation bar
            SetPause(!m_isPaused);
        }

        // timer

        if (m_timer)
        {
            m_time += Time.deltaTime;

            int min = (int)(m_time / 60);
            int sec = (int)(m_time % 60);
            int tenths = (int)((m_time * 10) % 10);

            m_timerText.text = $"{min}:{sec:00}.{tenths}";
        }
    }

    IEnumerator GameOver()
    {
        // wait 3 seconds
        yield return new WaitForSecondsRealtime(3.0f);
        // and reload the scene
        m_timer = false;
        SceneManager.LoadScene(0);
    }

    public void SetPause(bool setPause)
    {
        if (setPause)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
        m_pauseMenu.SetActive(setPause);
        m_isPaused = setPause;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void AddScore(int pts)
    {
        m_score += pts;
        m_scoreText.text = "Score: " + m_score;
    }

    public void ResetUI()
    {
        m_time = 0f;
        m_score = 0;
        m_timerText.text = "0:00.0";
        m_scoreText.text = "Score: 0";
    }
}
