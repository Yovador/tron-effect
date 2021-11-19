using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    List<GameObject> motoStartPos = new List<GameObject>();
    public int playerScore { get; set; } = 0;
    public int computerScore { get; set; } = 0;
    public AudioSync audioSync { get; set; }

    public enum GameStatusEnum { Menu, CharSelect, MainGame, EndGame }
    private GameStatusEnum gameStatus = GameStatusEnum.Menu;
    public GameStatusEnum GameStatus { get { return gameStatus; } set { gameStatus = value;} }

    [SerializeField] private int winCondition = 30;
    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Application.targetFrameRate = 60;
        audioSync = GetComponentInChildren<AudioSync>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        Debug.Log("GameManager : " + gameStatus);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (gameStatus)
        {
            case GameStatusEnum.Menu:
                break;
            case GameStatusEnum.CharSelect:
                break;
            case GameStatusEnum.MainGame:
                StartGame();
                break;
            case GameStatusEnum.EndGame:
                break;
            default:
                break;
        }
    }

    public void StartGame()
    {
        foreach (var obj in GameObject.FindGameObjectsWithTag("Moto"))
        {
            motoStartPos.Add(obj);
        }
    }


    public void EndRound()
    {

        bool isPlayerAlive = false;
        bool areAllBotDead = true;

        foreach (var obj in motoStartPos)
        {
            if(obj.GetComponent<PlayableMoto>().isAlive)
            {
               if(obj.GetComponent<Player>() != null)
               {
                    isPlayerAlive = true;
               }
               else
               {
                    areAllBotDead = false;
               }
            }
            obj.GetComponent<PlayableMoto>().ResetMoto();
        }

        if(isPlayerAlive && areAllBotDead)
        {
            playerScore++;
        }
        else if(!isPlayerAlive && !areAllBotDead)
        {
            computerScore++;
        }

        Debug.Log("End round " + playerScore);
        Endgame();

    }

    void Endgame()
    {
        if(playerScore >= winCondition || computerScore >= winCondition)
        {
            SceneManager.LoadScene("EndGame");
            gameStatus = GameStatusEnum.EndGame;
        }
    }

}
