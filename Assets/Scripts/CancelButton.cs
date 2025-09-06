using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/*******************************************************
 * Script Name : [CancelButton.cs]
 * Author      : Hadiyah Kashif
 * Created     : [07/25/2025]
 * Description : [Once pressed, clears the ingredients from the plate.]
 *
 * Notes       : [NONE]
 *******************************************************/

public class CancelButton : MonoBehaviour
{

    public IngredientChecker clearedIngredients; // used to get the ingredient lists from IngredientChecker
    public AudioClip trashedOrder;

    public AudioSource buttonSource; // where the trashed sound will come from

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<XRSimpleInteractable>().selectEntered.AddListener(x => ClearIngredients());
    }

    /**
     * Clears the ingredients placed onto the plate and the list kept by IngredientChecker.
     * Also plays the sound of the order being thrown away
     */
    public void ClearIngredients()
    {
        foreach (GameObject ingredient in clearedIngredients.ingredientObjects)
        {
            Destroy(ingredient);
        }
        clearedIngredients.ingredientObjects.Clear();
        clearedIngredients.ingredientNames.Clear();
        PlayTrashSound();
    }

    /**
     * Plays the trashed sound
     */
    public void PlayTrashSound()
    {
        if (buttonSource != null)
        {
            buttonSource.clip = trashedOrder;
            buttonSource.Play();
        }
    }
}
