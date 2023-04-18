using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] GameObject levelCompletedMenu;
    [SerializeField] GameObject levelLostMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject playingMenu;
    [SerializeField] GameObject HeadShotText;
    [SerializeField] GameObject BoomText;
    [SerializeField] TextMeshProUGUI noOfKillsText;

    Player player;
    public static bool GameIsPaused = false;
    [SerializeField] AudioManager audioManager;
    [SerializeField] TimeManager timeManager;
    Camera cam;
    [SerializeField] GameObject floatingCanvas;

    int noOfKills = 0;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        cam = Camera.main;

        if(audioManager == null)
            audioManager = AudioManager.instance;
        if (timeManager == null)
            timeManager = TimeManager.instance;
        player = Player.instance;
    }

    public void LevelCompleted()
    {
        levelCompletedMenu.SetActive(true);
        playingMenu.SetActive(false);
    }

    public void LevelLost()
    {
        levelLostMenu.SetActive(true);
        playingMenu.SetActive(false);
    }

    
    public void Resume()
    {
        playingMenu.SetActive(true);
        pauseMenu.SetActive(false);
        if (!timeManager.InSlowMotion)
        {
            Time.timeScale = 1f;
        }
        else
        {
            timeManager.StartSlowMotion();
        }

        GameIsPaused = false;
        if (player != null)
            player.gamePaused = false;
        audioManager.Play("Button");
    }

    public void Pause()
    {
        if (LadyController.ladyDead)
            return;
        playingMenu.SetActive(false);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        if (player != null)
            player.gamePaused = true;
        audioManager.Play("Button");
    }

    public void Restart()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        audioManager.Play("Button");
    }

    public void NextLevel()
    {
      
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        audioManager.Play("Button");
    }

    public void KillsText()
    {
        noOfKills++;
        noOfKillsText.text = noOfKills.ToString();
    }

    public void HeadShot(Vector3 _screenPos)
    {
        Vector3 offset = new Vector3(0f, 3f, 0f);
        Vector3 viewPortPos = cam.WorldToScreenPoint(_screenPos + offset);
        Instantiate(HeadShotText, viewPortPos, Quaternion.identity, floatingCanvas.transform);
    }
    public void Boom(Vector3 _screenPos)
    {
        Vector3 offset = new Vector3(0f, 5f, 0f);
        Vector3 viewPortPos = cam.WorldToScreenPoint(_screenPos + offset);
        Instantiate(BoomText, viewPortPos, Quaternion.identity, floatingCanvas.transform);
    }
}
