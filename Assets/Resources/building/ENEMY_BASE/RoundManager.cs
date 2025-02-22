using System.Collections;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

public class RoundManager : MonoBehaviour
{
    public TextMeshProUGUI roundText; 

    public float displayDuration = 2f; 

    public float roundDuration = 30.0f;
    public float duration = 1.0f;
    public float startSize = 70f;
    public float endSize = 30f;

    void Start()
    {
        StartCoroutine(ShowRoundText());
    }

    IEnumerator ShowRoundText()
    {
        roundText.text = $"<color=#BF360C><b><size={(int)startSize}>Enemy arrive in 20s.</size></b></color>";
        float elapsedTime = 0f; 


        while (elapsedTime < duration)
        {

            float newSize = Mathf.Lerp(startSize, endSize, elapsedTime / duration);
            roundText.text = $"<color=#BF360C><b><size={(int)newSize}>Enemy arrive in 20s.</size></b></color>";

            elapsedTime += Time.deltaTime; 
            yield return null; 
        }

        roundText.text = $"<color=#BF360C><b><size={(int)endSize}>Enemy arrive in 20s.</size></b></color>";

        yield return new WaitForSeconds(displayDuration - duration);

        if (roundText != null)
        {
            roundText.gameObject.SetActive(false); 
        }


        yield return new WaitForSeconds(roundDuration-displayDuration); 

        if (roundText != null)
        {
            roundText.text = "Round 1"; 
            roundText.gameObject.SetActive(true); 
        }

        yield return new WaitForSeconds(displayDuration); 

        if (roundText != null)
        {
            roundText.gameObject.SetActive(false); 
        }


        yield return new WaitForSeconds(1.5f*roundDuration-displayDuration); 

        
        if (roundText != null)
        {
            roundText.text = "Round 2";
            roundText.gameObject.SetActive(true); 
        }

        yield return new WaitForSeconds(displayDuration); 

        if (roundText != null)
        {
            roundText.gameObject.SetActive(false); 
        }

        yield return new WaitForSeconds(1.5f*roundDuration-displayDuration);

        if (roundText != null)
        {
            roundText.text = "Round 3";
            roundText.gameObject.SetActive(true); 
        }

        yield return new WaitForSeconds(displayDuration);
        
        if (roundText != null)
        {
            roundText.gameObject.SetActive(false); 
        }

    }
}
