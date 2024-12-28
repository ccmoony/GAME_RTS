using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        // 加载SampleScene
        SceneManager.LoadScene("bg story");
    }
    public void GameOver()
    {
        // 切换到Game Over场景
        SceneManager.LoadScene("gameover");
    }
    public void Gamewin()
    {
        // 切换到Game Win场景
        SceneManager.LoadScene("gamewin");
    }
}
