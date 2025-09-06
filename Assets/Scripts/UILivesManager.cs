using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*******************************************************
 * Script Name : [UILivesManager.cs]
 * Author      : Hadiyah Kashif
 * Created     : [08/24/2025]
 * Description : [Manages the lives of the player and updates the heart visuals with every loss.
 *                Plays the appropriate speaker audio when a life is lost and calls game over too.]
 *
 * Notes       : [NONE]
 *******************************************************/

public class UILivesManager : MonoBehaviour
{
    public GameObject[] hearts;
    public GameObject endGameCanvas;
    public GameObject endLidsParent;
    public TextMeshProUGUI endGameText;
    public BettyAudioLines speaker;
    public GameObject leftRayInteractor;
    public GameObject rightRayInteractor;
    public GameObject leftDirectInteractor;
    public GameObject rightDirectInteractor;

    private int lives = 3;
    private AllCoversAppear activateCovers;

    public int Lives => lives;

    private void Start()
    {
        activateCovers = endLidsParent.GetComponent<AllCoversAppear>();
    }
    /**
     * Called to decrement the number of lives
     */
    public void LoseLife()
    {
        if (lives > 0)
        {
            lives--;
            UpdateLivesUI();
            PlayBettyLines();
        }
    }

    /**
     * Called to update the visual UI assoc. with lives
     * If lives are depleted, call game over, deactivate
     * the direct interactors and activate the ray interactors
     */
    private void UpdateLivesUI()
    {
        hearts[lives].gameObject.SetActive(false);
        EraseSound();
        if (lives == 0)
        {
            activateCovers.EnableAllLids();
            leftRayInteractor.SetActive(true);
            rightRayInteractor.SetActive(true);
            leftDirectInteractor.SetActive(false);
            rightDirectInteractor.SetActive(false);
            DisplayLoseScreen();
        }

    }

    /**
     * Called to play an erasing sound when removing a life
     */
    private void EraseSound()
    {
        AudioSource eraseSound = GetComponent<AudioSource>();
        if (eraseSound != null && eraseSound.clip != null)
        {
            eraseSound.Play();
        }
    }

    /**
     * Play the appropriate the line associated with the number of lives
     */
    private void PlayBettyLines()
    {
        switch (lives)
        {
            case 2:
                speaker.PlayAudioClip(1);
                break;
            case 1:
                speaker.PlayAudioClip(2);
                break;
            case 0:
                speaker.PlayAudioClip(3);
                break;
        }
    }

    public void DisplayLoseScreen()
    {
        // specifically for a depletedIngredients activated gameover
        if (lives != 0)
        {
            lives = 0;  
            speaker.PlayAudioClip(9);   

        }
        endGameText.SetText("GAME OVER");
        endGameCanvas.SetActive(true);
    }
}
