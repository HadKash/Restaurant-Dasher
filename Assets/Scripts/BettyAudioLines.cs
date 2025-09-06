using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*******************************************************
 * Script Name : [BettyAudioLines.cs]
 * Author      : Hadiyah Kashif
 * Created     : [08/30/2025]
 * Description : [Plays the appropriate audio line and plays the speaker animation for the duration
 *                of the audio line.]
 *
 * Notes       : [NONE]
 *******************************************************/

public class BettyAudioLines : MonoBehaviour
{

    public AudioClip[] lines;

    AudioSource audioSource;
    Animator speakerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        speakerAnimator = GetComponent<Animator>();
        // Play the intro audio clip
        PlayAudioClip(0);
    }

    /**
     * Play the audio clip and the speaker animation
     */
    public void PlayAudioClip(int audioClipIndex)
    {
        if (audioSource != null) 
        {
            audioSource.clip = lines[audioClipIndex];
            audioSource.Play();
            speakerAnimator.SetBool("isSpeaking", true);
            StartCoroutine(StopAnimation(lines[audioClipIndex].length));
        }   
    }

    /**
     * Stop the speaker from animating by the end of the audio clip
     */
    private IEnumerator StopAnimation(float audioClipDuration)
    {
        yield return new WaitForSeconds(audioClipDuration);
        // add the animation stopping here
        speakerAnimator.SetBool("isSpeaking", false);
    }

}
