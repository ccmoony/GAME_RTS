using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip musicA; 
    public AudioClip musicB; 
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        
        audioSource.clip = musicA;
        audioSource.Play();

        
        StartCoroutine(PlayMusicB());
    }

    
    private IEnumerator PlayMusicB()
    {
        
        yield return new WaitForSeconds(musicA.length);

        
        audioSource.clip = musicB;
        audioSource.loop = true;  
        audioSource.Play();
    }
}

