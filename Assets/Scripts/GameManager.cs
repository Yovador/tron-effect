using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    List<GameObject> motoStartPos = new List<GameObject>();
    public int playerScore { get; set; } = 0;
    public int computerScore { get; set; } = 0;
    public AudioSync audioSync { get; set; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Application.targetFrameRate = 60;
        audioSync = GetComponentInChildren<AudioSync>();
    }

    void Start()
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

    }

}
