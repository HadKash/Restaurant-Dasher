using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/*******************************************************
 * Script Name : [SendButton.cs]
 * Author      : Hadiyah Kashif
 * Created     : [07/25/2025]
 * Description : [Once clicked, "sends" the order out to the customer.]
 *
 * Notes       : [If the order doesn't match, it lets you try again. Plays a sound
 *                for when the correct order is sent and when its incorrect.]
 *******************************************************/

public class SendButton : MonoBehaviour
{
    // These come from the objects with these scripts
    public IngredientChecker ingredientChecker;
    public OrderManager orderManager;
    public OrderManager.Order currentOrder;
    public CustomerSpawner customerSpawner;
    public UIScoreManager scoreManager;

    // Audio Clips
    public AudioClip dingSound;
    public AudioClip trashSound;
    public AudioClip successSound;
    
    // Audio Sources
    public AudioSource buttonSource; // for the button ding
    public AudioSource successSource; // temp, for testing successful orders
    public AudioSource thrownFoodSource; // for the thrown food, could be an audio source attached to the player??

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<XRSimpleInteractable>().selectEntered.AddListener(x => SendOrder());
    }

    /**
     * Checks if the ingredients on the plate matches the customers order.
     * Go to either FailedOrder() or SuccessfulOrder() to complete the respective actions
     */
    public void SendOrder()
    {
        // check if you're sending in an empty plate
        // or if the number of entered ingredients match the amount in the customer's order
        if (ingredientChecker.ingredientNames.Count == 0 ||
            ingredientChecker.ingredientNames.Count != orderManager.GetActiveOrder().GetIngredients().Count)
        {
            FailedOrder();
            return;
        }

        // check through the ingredients to see if it matches the customer's order
        currentOrder = orderManager.GetActiveOrder();
        List<string> orderIngredients = currentOrder.GetIngredients();
        for (int i = 0; i < ingredientChecker.ingredientNames.Count; i++)
        {
            if (!ingredientChecker.ingredientNames[i].Equals(orderIngredients[i]))
            {
                FailedOrder();
                return;
            }
        }

        // if you've made it this far, run the successful actions
        SuccessfulOrder();

    }

    /**
     * Plays an error/ trashed sound. Takes a life away from the user.
     * Clear the ingredient's object and name lists. Possibly have 
     * unique dialogue bashing the character for being dumb or something
     */
    public void FailedOrder()
    {
        PlayErrorSound();
        ClearIngredientLists();
        
    }

    /**
     * Plays a success sound. Add a point to the score. 
     * Clear the ingredient's object and name lists. Possibly
     * have unique dialogue commending the user, keep it snarky though.
     */
    public void SuccessfulOrder()
    {
        PlaySuccessSound();
        ClearIngredientLists();
        orderManager.ClearActiveOrder();
        // Add one to the score
        scoreManager.AddScore();
        // have the customer leave
        if (customerSpawner.lastSpawnedCustomer != null)
        {
            CustomerMovement customerMovement = customerSpawner.lastSpawnedCustomer.GetComponent<CustomerMovement>();
            if (customerMovement != null)
            {
                customerMovement.EarlyIdleToWalk();
            }
        }
    }

    /**
     * Plays when the order doesn't match the customer's
     */
    public void PlayErrorSound()
    {
        if (buttonSource != null)
        {
            buttonSource.clip = trashSound;
            buttonSource.Play();
        }
    }

    /**
     * Plays when the order matches the customer's
     */
    public void PlaySuccessSound()
    {
        if (buttonSource != null)
        {
            buttonSource.clip = successSound;
            buttonSource.Play();
        }
    }

    /**
     * Clears the ingredient lists for the next order
     */
    public void ClearIngredientLists()
    {
        // Destroy the placed game objects
        foreach (GameObject ingredient in ingredientChecker.ingredientObjects)
        {
            Destroy(ingredient);
        }
        // Clear the ingredients lists
        ingredientChecker.ingredientNames.Clear();
        ingredientChecker.ingredientObjects.Clear();
    }
}
