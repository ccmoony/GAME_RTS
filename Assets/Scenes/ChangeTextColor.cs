using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ChangeTMPTextAppearance : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI buttonText; 
    public Color normalColor = Color.white; 
    public Color highlightColor = Color.green; 
    public float normalFontSize = 14f; 
    public float enlargedFontSize = 20f; 
    public float transitionSpeed = 5f; 

    private bool isHovered = false;

    void Start()
    {
        if (buttonText == null)
        {
            buttonText = GetComponentInChildren<TextMeshProUGUI>(); 
        }
        buttonText.color = normalColor; 
        buttonText.fontSize = normalFontSize; 
    }

    void Update()
    {
        if (buttonText != null)
        {

            buttonText.color = Color.Lerp(buttonText.color, isHovered ? highlightColor : normalColor, Time.deltaTime * transitionSpeed);
   
            buttonText.fontSize = Mathf.Lerp(buttonText.fontSize, isHovered ? enlargedFontSize : normalFontSize, Time.deltaTime * transitionSpeed);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true; 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false; 
    }
}
