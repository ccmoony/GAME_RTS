using System.Collections;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

public class RoundManager : MonoBehaviour
{
    public TextMeshProUGUI roundText; 

    public float displayDuration = 2f; 

    public float roundDuration = 30.0f;

    void Start()
    {
        StartCoroutine(ShowRoundText());
    }

    IEnumerator ShowRoundText()
    {
        roundText.text = "<color=#BF360C><b><size=30>Enemy arrive in 20s.</size></b></color>";
        yield return new WaitForSeconds(displayDuration);
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


        yield return new WaitForSeconds(roundDuration-displayDuration); 

        
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

        yield return new WaitForSeconds(roundDuration-displayDuration);

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
