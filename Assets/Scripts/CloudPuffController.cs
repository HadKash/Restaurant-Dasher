using System.Collections;
using UnityEngine;

/*******************************************************
 * Script Name : [CloudPuffController.cs]
 * Author      : Hadiyah Kashif
 * Created     : [08/08/2025]
 * Description : [Plays the cloud cluster animation at the given location alongside a spawning audio]
 *
 * Notes       : [Activate multiple animators since in the cluster, each cloud has its own.]
 *******************************************************/

public class CloudPuffController : MonoBehaviour
{
    private Animator[] childAnimators;
    Vector3 hiddenPosition;         // holds the original position that's out of the player's view
    AudioSource cloudSparkle;

    void Awake()
    {
        childAnimators = GetComponentsInChildren<Animator>();
        hiddenPosition = transform.position;
        cloudSparkle = GetComponent<AudioSource>();
    }

    /**
     * set the puff animation to the right location and play all the cloud
     * childrens' animations
     */
    public void PlayPuff(Transform puffLocation)
    {
        Debug.Log($"PlayPuff at {puffLocation.position}");
        transform.position = puffLocation.position;
        PlaySparkleSound();
        foreach (Animator anim in childAnimators)
        {
            anim.Play("cloudAction", 0, 0f); // Restart clip
        }
        StartCoroutine(HideCluster());
        
    }

    /**
     * Called at the end of the animation to return the cluster
     * to the hidden position
     */
    IEnumerator HideCluster()
    {
        yield return new WaitForSeconds(2f);
        transform.position = hiddenPosition;
    }

    /**
     * Plays the sparkle sound to go along with the cloud animation
     */
    void PlaySparkleSound()
    {
        if (cloudSparkle != null)
        {
            cloudSparkle.Play();
        }
    }
}
