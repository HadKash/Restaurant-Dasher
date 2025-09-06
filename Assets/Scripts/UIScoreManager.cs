using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*******************************************************
 * Script Name : [UIScoreManager.cs]
 * Author      : Hadiyah Kashif
 * Created     : [08/24/2025]
 * Description : [Keeps track of the player's score by adding tally marks to a board. 
 *                Displays the win screen at 12 successful orders.]
 *
 * Notes       : [Plays the appropriate audio clip when a certain amount of 
 *                successful orders are complete.]
 *******************************************************/

public class UIScoreManager : MonoBehaviour
{
    public GameObject[] tallyMarks;
    public GameObject endGameCanvas;
    public TextMeshProUGUI endGameText;
    public BettyAudioLines speaker;

    private int score = 0;

    public int Score => score;   // read only property (used to adjust orderDuration)

    /**
     * Increase the score and update the UI
     */
    public void AddScore()
    {
        score++;
        UpdateScoreUI();
        PlayBettyLines();
        if (score == 12)
        {
            DisplayWinScreen();
        }
    }

    /**
     * Update the UI bu adding a tally mark and play the chalk sound
     * as if you're drawing it down yourself
     */
    private void UpdateScoreUI()
    {
        tallyMarks[score - 1].gameObject.SetActive(true);
        ChalkSound();
    }

    /**
     * Call to play the appropriate Betty line according to the score
     */
    private void PlayBettyLines()
    {
        switch (score)
        {
            case 1:
                speaker.PlayAudioClip(4);
                break;
            case 3:
                speaker.PlayAudioClip(5);
                break;
            case 6:
                speaker.PlayAudioClip(6);
                break;
            case 9:
                speaker.PlayAudioClip(7);
                break;
            case 12:
                speaker.PlayAudioClip(8);
                break;
        }
    }

    /**
     * Play the chalk sound
     */
    private void ChalkSound()
    {
        AudioSource chalkSound = GetComponent<AudioSource>();
        if (chalkSound != null && chalkSound.clip != null)
        {
            chalkSound.Play();
        }
    }

    private void DisplayWinScreen()
    {
        endGameText.SetText("YOU WIN!!!");
        endGameCanvas.SetActive(true);
    }
}
