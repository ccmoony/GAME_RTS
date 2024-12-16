using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        // 加载SampleScene
        SceneManager.LoadScene("GameScene");
    }
    public void GameOver()
    {
        // 切换到Game Over场景
        SceneManager.LoadScene("gameover");
    }
}
