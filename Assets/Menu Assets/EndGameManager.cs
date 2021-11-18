using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour
{
    TMP_Text textComponent;

    private void Awake()
    {
        textComponent = GetComponentInChildren<TMP_Text>();
    }
    // Start is called before the first frame update
    void Start()
    {
        int playerscore = GameManager.instance.playerScore;
        int computerscore = GameManager.instance.computerScore;

        string textToDisplay = "";

        if (playerscore > computerscore)
        {
            textToDisplay = "Vous avez Gagné !";
        } else if (playerscore < computerscore)
        {
            textToDisplay = "Vous avez Perdu !";
        } else if (playerscore == computerscore)
        {
            textToDisplay = "Égalité !";
        }

        textToDisplay += "Votre Score est " + playerscore;
        Debug.Log(textToDisplay);

        textComponent.text = textToDisplay;
    }

    public void Return()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }
}
