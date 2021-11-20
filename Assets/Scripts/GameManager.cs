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
    public CharacterSelect characterSelect { get; set; }
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
        characterSelect = GetComponentInChildren<CharacterSelect>();
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
                playerScore = 0;
                computerScore = 0;
                break;
            case GameStatusEnum.CharSelect:
                characterSelect.DisplaySelectableCharacter();
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

        SwitchAudio();
        SwitchCharacter();
    }

    public void SwitchCharacter()
    {
        Debug.Log("SwitchCharacter " + characterSelect.SelectedCharacter.name);
        foreach (var obj in motoStartPos)
        {
            if (obj.GetComponent<Player>() != null)
            {
                obj.GetComponent<Player>().SwitchModel();
            }
        }
        GameObject model = GameObject.FindGameObjectWithTag("BasePlayer").gameObject;

        Destroy(model.transform.GetChild(0).gameObject);
        GameObject newModel = Instantiate(characterSelect.SelectedCharacter, model.transform);
        newModel.transform.localScale *= 10;

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

    void SwitchAudio()
    {
        foreach (var audioSource in GetComponentsInChildren<AudioSource>())
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            else
            {
                audioSource.Play();
            }
        }
    }

    void Endgame()
    {
        if(playerScore >= winCondition || computerScore >= winCondition)
        {
            SwitchAudio();
            gameStatus = GameStatusEnum.EndGame;
            motoStartPos = new List<GameObject>();
            SceneManager.LoadScene("EndGame");
        }
    }

}
