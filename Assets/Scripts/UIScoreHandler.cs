using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScoreHandler : MonoBehaviour
{
    TMP_Text textComponent;
    private enum ScoreOwner { Player, Computer}
    [SerializeField]
    private ScoreOwner scoreOwner = ScoreOwner.Player; 

    private void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        string textToDisplay = "";

        if(scoreOwner == ScoreOwner.Player)
        {
            textToDisplay = GameManager.instance.playerScore.ToString();
        }
        else
        {
            textToDisplay = GameManager.instance.computerScore.ToString();
        }

        if (textComponent.text != textToDisplay)
        {
            textComponent.text = textToDisplay;
        }
    }
}
