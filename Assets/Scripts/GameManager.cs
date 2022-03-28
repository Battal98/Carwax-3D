using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    #region Bools
    public static bool isGameStarted = false;
    public static bool isGameEnded = false;
    public static bool isGameRestarted = false;
    public bool isClickedWashButton;
    public bool isClickedAirButton;
    public bool isClickedBubbleButton;
    public bool isClickedButton;
    #endregion

    #region GameObject, TextMeshPro and Lists
    public GameObject MainMenu;
    public GameObject WinMenu;
    public GameObject LoseMenu;
    public GameObject GameMenu;
    public GameObject WateryPanel;
    public GameObject DirtyPanel;
    public GameObject FoamPanel;
    public GameObject HumanPanel;
    public TextMeshProUGUI LevelsText;
    public GameObject aiBot;
    public List<GameObject> Levels = new List<GameObject>();
    #endregion
    #region ParticleSystem
    public List<ParticleSystem> WaterParticles;
    public ParticleSystem BubbleParticle;
    public ParticleSystem BubbleParticle2;
    public ParticleSystem AirParticle;
    public ParticleSystem AirParticle2;
    #endregion

    #region Varriants
    public int levelCount = 0;
    public int nextLevel = 0;
    public float timer;
    #endregion
    private void Awake()
    {
        if (instance == null)
            instance = this;

    }
    private IEnumerator Start()
    {
        StartGame();
        if (levelCount !=0)
        {
            LevelsText.text = "Level " + (nextLevel);
        }

        else
        {
            LevelsText.text = "Tutorial";
            aiBot.SetActive(false);
        }

        yield return new WaitForSeconds(1);
    }

    void Update()
    {
        if (isClickedWashButton)
        {
            WaterParticles[0].Play();
            WaterParticles[1].Play();
            WaterParticles[2].Play();
            WaterParticles[3].Play();
            WaterParticles[4].Play();
            WaterParticles[5].Play();
        }

        if (isClickedBubbleButton)
        {
            BubbleParticle.Play();
            BubbleParticle2.Play();
        }

        if (isClickedAirButton)
        {
            AirParticle.Play();
            AirParticle2.Play();
        }

    }
    public void StartGame()
    {

        if (isGameRestarted)
        {
            MainMenu.SetActive(false);
            GameMenu.SetActive(true);
            PlayerController.instance.isMoving = true;
            AiController.instance.isMoving = true;
        }

        levelCount = PlayerPrefs.GetInt("levelCount", levelCount);
        nextLevel = PlayerPrefs.GetInt("nextLevel", nextLevel);

        Debug.Log("created level");
        if (levelCount < 0 || levelCount >= Levels.Count)
        {
            levelCount = 1;
            Debug.Log(levelCount);
            //PlayerPrefs.SetInt("levelCount", levelCount);
        }

        CreateLevel(levelCount);
        
    }

    // Create Level.
    public void CreateLevel(int Levelindex)
    {
        Instantiate(Levels[Levelindex]);
        
    }

    public void StartTheGame()
    {
        MainMenu.SetActive(false);
        GameMenu.SetActive(true);
        isGameStarted = true;
        PlayerController.instance.isMoving = true;
        AiController.instance.isMoving = true;

    }
    public void NextLevelButton()
    {
        nextLevel++;
        levelCount++;
        PlayerPrefs.SetInt("levelCount", levelCount);
        PlayerPrefs.SetInt("nextLevel", nextLevel);
        isGameEnded = false;
        isGameRestarted = true;
        isGameStarted = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartButton()
    {
        isGameEnded = false;
        isGameRestarted = true;
        isGameStarted = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnLevelEnded()
    {
        isGameEnded = true;
        Debug.Log("Level Bitti.");
        WinMenu.SetActive(true);
        GameMenu.SetActive(false);
    }

    public void OnLevelFailed()
    {
        Debug.Log("fail");
        LoseMenu.SetActive(true);
        GameMenu.SetActive(false);
        isGameEnded = true;
        PlayerController.instance.isMoving = false;
        AiController.instance.isMoving = false;
    }

    public void ChangeWashBool (bool _isClicked)
    {
        isClickedWashButton = _isClicked;
        isClickedButton = _isClicked;
    }

    public void ChangeAirBool(bool _isClicked)
    {
        isClickedAirButton = _isClicked;
        isClickedButton = _isClicked;
    }

    public void ChangeBubbleBool(bool _isClicked)
    {
        isClickedBubbleButton = _isClicked;
        isClickedButton = _isClicked;
    }
}
