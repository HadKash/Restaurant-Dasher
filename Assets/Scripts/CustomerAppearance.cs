using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*******************************************************
 * Script Name : [CustomerAppearance.cs]
 * Author      : Hadiyah Kashif
 * Created     : [08/06/2025]
 * Description : [Changes the appearance of the customer the longer it takes the player to 
 *                 create the order, includes changing material color and eyebrow rotation
 *                 to mimic the proper emotion (happy (neutral), slightly upset, angry)]
 *
 * Notes       : [patienceTimer is the orderDuration / 3, and this value decreases every 3 successful orders]
 *******************************************************/

public class CustomerAppearance : MonoBehaviour
{
    // patience timer is the total order time divided by 3
    public float patienceTimer;
    //public GameObject timer;
    public AudioClip[] timerClips;

    Renderer renderer;
    int eyebrowRotation = -20;
    Transform leftBrow;
    Transform rightBrow;
    GameObject timer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        leftBrow = transform.Find("LeftEyebrow");
        rightBrow = transform.Find("RightEyebrow");
        timer = GameObject.Find("timer sound");
    }

    /**
     * Starts the ChangeAppearance() coroutine with an adjusted PatienceTimer
     */
    public void Change()
    {
        Debug.Log("Called ChangeAppearance");
        StartCoroutine(ChangeAppearance());
    }

    /**
     * Changes the customers appearance the longer the player takes to make their order
     */
    IEnumerator ChangeAppearance()
    {
        // start the slow timer while on green appearance
        PlayTimerSound(0);
        // change to yellow appearance and apply eyebrow rotation (tad peeved) and play the medium speed timer audio
        yield return new WaitForSeconds(patienceTimer);
        PlayTimerSound(1);
        renderer.material.SetColor("_BaseColor", Color.yellow);
        leftBrow.localRotation = Quaternion.Euler(Mathf.Abs(eyebrowRotation), 0, 0); // 20
        rightBrow.localRotation = Quaternion.Euler(eyebrowRotation, 0, 0); // -20

        // change to red appearance and apply eyebrow rotation (really peeved) and play the fast speed timer audio
        yield return new WaitForSeconds(patienceTimer);
        PlayTimerSound(2);
        renderer.material.SetColor("_BaseColor", Color.red);
        leftBrow.localRotation = Quaternion.Euler(eyebrowRotation, 0, 0); // -20
        rightBrow.localRotation = Quaternion.Euler(Mathf.Abs(eyebrowRotation), 0, 0); // 20

        // adding this so that the times up audio can play
        yield return new WaitForSeconds(patienceTimer);
        PlayTimerSound(3);

    }

    /**
     * Play the appropriate timer audio (differing by timer speed)
     */
    private void PlayTimerSound(int timerIndex)
    {
        if (timer != null) 
        {            
            AudioSource audioSource = timer.GetComponent<AudioSource>();
            if (timerIndex == 3)
            {
                audioSource.loop = false;
            }
            else
            {
                audioSource.loop = true;
            }
            audioSource.clip = timerClips[timerIndex];
            audioSource.Play();
        }

    }

    /**
     * Stop the timer audio from playing, specifically stopping the speed timers
     */
    private void StopTimerSound()
    {
        if (timer != null)
        {
            AudioSource audioSource = timer.GetComponent<AudioSource>();
            audioSource.Stop();
        }
    }

    /**
     * Cancel the ChangeAppearance() coroutine once the order is successful
     */
    public void CancelAppearanceCoroutines()
    {
        StopAllCoroutines();
        StopTimerSound();
    }
}
