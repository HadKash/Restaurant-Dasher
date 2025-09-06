using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*******************************************************
 * Script Name : [GrillPatties.cs]
 * Author      : Hadiyah Kashif
 * Created     : [07/25/2025]
 * Description : ["Grills" the patties on the grill via a collider by changing the patties material.
 *                 Plays a smoke effect when it burns. Plays audio for when it's grilling, when it's
 *                 complete, and when it burns.]
 *
 * Notes       : [NONE]
 *******************************************************/

public class GrillPatties : MonoBehaviour
{
    public Material cookedChicken;
    public Material cookedBeef;
    public Material cookedFish;
    public Material burntChicken;
    public Material burntBeef;
    public Material burntFish;

    public AudioClip sizzleSound;      // Sizzle sound for the patty
    public AudioClip dingSound;        // Ding sound when patty is cooked
    public AudioClip burntBeepSound;   // Beeping sound when patty is burnt
    public float sizzleVolume = 1.5f;
    public float dingVolume = 0.5f;
    public float beepVolume = 1.0f;

    public ParticleSystem smokeEffect;
    public GameObject pattySizzleSource;  

    private Dictionary<string, Material> cookedMaterials;
    private Dictionary<string, Material> burntMaterials;

    private Coroutine cookingCoroutine; // Tracks the cooking coroutine
    private AudioSource grillAudioSource; // For playing sounds from the grill


    void Start()
    {
        // Initialize dictionaries for cooked and burnt materials
        cookedMaterials = new Dictionary<string, Material>
        {
            { "RawChicken", cookedChicken },
            { "RawBeef", cookedBeef },
            { "RawFish", cookedFish }
        };

        burntMaterials = new Dictionary<string, Material>
        {
            { "CookedChicken", burntChicken },
            { "CookedBeef", burntBeef },
            { "CookedFish", burntFish }
        };

        // Add an AudioSource to the grill if it doesn't already have one
        grillAudioSource = gameObject.GetComponent<AudioSource>();
        if (grillAudioSource == null)
        {
            grillAudioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    /**
     * Start cooking the patty
     */
    private void OnCollisionEnter(Collision collision)
    {
        if (cookedMaterials.ContainsKey(collision.gameObject.tag))
        {
            GameObject patty = collision.gameObject;

            // Start cooking coroutine
            if (cookingCoroutine == null)
            {
                cookingCoroutine = StartCoroutine(CookPatty(patty, patty.tag));
            }

            // Play sizzle sound from patty
            PlaySizzleSound(patty);
        }
    }

    /**
     * Stop cooking the patty, whether it's cooked or raw
     */
    private void OnCollisionExit(Collision collision)
    {
        if (burntMaterials.ContainsKey(collision.gameObject.tag) || cookedMaterials.ContainsKey(collision.gameObject.tag))
        {
            if (cookingCoroutine != null)
            {
                StopCoroutine(cookingCoroutine);
                cookingCoroutine = null;
            }

            // Stop sizzle sound
            StopSizzleSound();
            StopSmokeEffect();
        }
    }

    /**
     * Cook the patty by changing it's material to cooked or burnt accordingly
     */
    private IEnumerator CookPatty(GameObject patty, string pattyType)
    {
        Renderer pattyRenderer = patty.GetComponent<Renderer>();

        if (pattyRenderer == null)
        {
            Debug.LogWarning("No Renderer found on the patty.");
            yield break;
        }

        Debug.Log($"{pattyType} is on the grill. Cooking starts now!");

        // Wait for 5 seconds: Cooked
        yield return new WaitForSeconds(5f);
        if (pattyRenderer != null)
        {
            pattyRenderer.material = cookedMaterials[pattyType];
            ChangeTag(patty);
            pattyType = patty.tag;
            Debug.Log($"{pattyType} is now cooked!");
            PlayDingSound(); // Play the ding sound to signal the patty is cooked
        }

        // Wait for another 3 seconds: Burnt
        yield return new WaitForSeconds(3f);
        if (pattyRenderer != null)
        {
            pattyRenderer.material = burntMaterials[pattyType];
            patty.tag = "BurntPatty";
            Debug.Log($"{pattyType} is now burnt!");
            PlayBeepSound();

            StartSmokeEffect(patty.transform);
            StopSizzleSound();
        }

        cookingCoroutine = null;
    }

    /**
     * Change the tag to reflect its cooked condition
     */
    private void ChangeTag(GameObject tag)
    {
        if (tag.tag == "RawChicken")
        {
            tag.tag = "CookedChicken";
        }
        else if (tag.tag == "RawBeef")
        {
            tag.tag = "CookedBeef";
        }
        else if (tag.tag == "RawFish")
        {
            tag.tag = "CookedFish";
        }
    }

    /**
     * Plays the smoke effect at the patty's location when the patty becomes burnt
     */
    private void StartSmokeEffect(Transform pattyTransform)
    {
        if (smokeEffect != null)
        {
            smokeEffect.transform.position = pattyTransform.position; // Move to patty
            if (!smokeEffect.isPlaying)
            {
                smokeEffect.Play();
            }
        }
    }

    /**
     * End the smoke effect
     */
    private void StopSmokeEffect()
    {
        if (smokeEffect != null && smokeEffect.isPlaying)
        {
            smokeEffect.Stop();
        }
    }

    /**
     * Play the sizzle sound from the patty's location
     */
    private void PlaySizzleSound(GameObject patty)
    {
        pattySizzleSource.transform.position = patty.transform.position;
        AudioSource pattySizzleAudio = pattySizzleSource.GetComponent<AudioSource>();
        if (pattySizzleAudio != null && !pattySizzleAudio.isPlaying)
        {
            pattySizzleAudio.clip = sizzleSound;
            pattySizzleAudio.loop = true;
            pattySizzleAudio.volume = sizzleVolume;
            pattySizzleAudio.Play();
        }
    }

    /**
     * Stop the sizzle sound coming from the patty, particularly when the patty gets removed from the grill
     * or when it's burnt
     */
    private void StopSizzleSound()
    {
        AudioSource pattySizzleAudio = pattySizzleSource.GetComponent<AudioSource>();
        if (pattySizzleAudio != null && pattySizzleAudio.isPlaying)
        {
            pattySizzleAudio.Stop();
        }
    }

    /**
     * Play this once the patty is done cooking
     */
    private void PlayDingSound()
    {
        if (grillAudioSource != null && dingSound != null)
        {
            grillAudioSource.PlayOneShot(dingSound, dingVolume);
        }
    }

    /**
     * Play the beeping sound when the patty has burnt
     */
    private void PlayBeepSound()
    {
        if (grillAudioSource != null && burntBeepSound != null)
        {
            grillAudioSource.PlayOneShot(burntBeepSound, beepVolume);
        }
    }
}
