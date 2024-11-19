using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ChangeTMPTextAppearance : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI buttonText; // 按钮上的 TMP 组件
    public Color normalColor = Color.white; // 默认字体颜色
    public Color highlightColor = Color.green; // 鼠标悬停时字体颜色
    public float normalFontSize = 14f; // 默认字体大小
    public float enlargedFontSize = 20f; // 鼠标悬停时字体大小
    public float transitionSpeed = 5f; // 平滑过渡速度

    private bool isHovered = false;

    void Start()
    {
        if (buttonText == null)
        {
            buttonText = GetComponentInChildren<TextMeshProUGUI>(); // 自动获取 TMP 组件
        }
        buttonText.color = normalColor; // 初始化字体颜色
        buttonText.fontSize = normalFontSize; // 初始化字体大小
    }

    void Update()
    {
        if (buttonText != null)
        {
            // 平滑过渡颜色
            buttonText.color = Color.Lerp(buttonText.color, isHovered ? highlightColor : normalColor, Time.deltaTime * transitionSpeed);
            // 平滑过渡字体大小
            buttonText.fontSize = Mathf.Lerp(buttonText.fontSize, isHovered ? enlargedFontSize : normalFontSize, Time.deltaTime * transitionSpeed);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true; // 鼠标悬停时触发
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false; // 鼠标移出时触发
    }
}
