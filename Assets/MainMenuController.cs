using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        
        SceneManager.LoadScene("bg story");
    }
    public void GameOver()
    {
        
        SceneManager.LoadScene("gameover");
    }
    public void Gamewin()
    {
        
        SceneManager.LoadScene("gamewin");
    }
    public void Replay()
    {
        SceneManager.LoadScene("Menu");
    }
}
