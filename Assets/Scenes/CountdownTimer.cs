using System.Collections;
using UnityEngine;
using TMPro; 

public class CountdownTimer : MonoBehaviour
{
    public float countdownTime = 20f;  
    public float displayDuration = 2f; 
    public TextMeshProUGUI countdownText;   

    void Start()
    {
        
        countdownText.text = countdownTime.ToString("F0");
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        
        while (countdownTime > 0)
        {
            countdownTime -= Time.deltaTime;  
            countdownText.text = countdownTime.ToString("F0");  

            yield return null; 
        }

        countdownText.text="";

    
    }
}