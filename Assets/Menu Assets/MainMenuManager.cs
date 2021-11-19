using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    
    public void QuitGame()
    {
        Debug.Log("QuitGame");
        Application.Quit();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
        GameManager.instance.GameStatus = GameManager.GameStatusEnum.MainGame;
    }

    public void CharacterSelect()
    {
        SceneManager.LoadScene("PlayerCharacter");
        GameManager.instance.GameStatus = GameManager.GameStatusEnum.CharSelect;
    }
}
