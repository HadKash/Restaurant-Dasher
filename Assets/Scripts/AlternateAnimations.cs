using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternateAnimations : MonoBehaviour
{
    public Animator purpleAnimator;
    public Animator pinkAnimator;

    private void Start()
    {
        StartCoroutine(LoopAnimations());
    }

    /**
     * Switches between the animations of the purple and pink characters
     */
    private IEnumerator LoopAnimations()
    {
        while (true)
        {
            // Play A’s animation
            purpleAnimator.Play("purple eyebrows", 0, 0f);
            yield return new WaitForSeconds(1f);

            // Play B’s animation
            pinkAnimator.Play("pink eyebrows", 0, 0f);
            yield return new WaitForSeconds(1f);
        }
    }

}

