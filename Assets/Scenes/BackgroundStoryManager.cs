using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class BackgroundStoryManager : MonoBehaviour
{
    public Text storyText;  // 用于显示背景故事的Text组件

    private string[] storyLines = {
        "On the serene island of Elaria, life was peaceful.",
        "Villagers thrived among lush fields and ancient forests, untouched by war for generations.",
        "<color=#530B0F>But at dawn, dark sails appeared on the horizon, bringing destruction and chaos.</color>",
        "<color=#530B0F>As flames consumed the land and hope waned</color>",
        "<color=#530B0F>You, a young guardian, must rise.</color>",
        "<color=#530B0F>Rally the people   Defend your home</color>",
        "<color=#530B0F><b><size=100>The battle begins now!</size></b></color>"


    };
    private int currentLine = 0;

    void Start()
    {
        // 初始化显示第一行背景故事
        storyText.text = "";
    }

    void Update()
    {
        // 监听鼠标点击事件
        if (Input.GetMouseButtonDown(0)) // 0代表鼠标左键
        {
            if (currentLine < storyLines.Length)
            {
                // 在已有文本后附加当前行的背景故事
                storyText.text += storyLines[currentLine] + "\n"; // \n 表示换行
                currentLine++;
            }
            else
            {
                // 背景故事结束后加载游戏场景
                SceneManager.LoadScene("GameScene");
            }
        }
    }
}
