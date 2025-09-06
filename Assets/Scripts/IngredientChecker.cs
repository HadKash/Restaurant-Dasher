using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/*******************************************************
 * Script Name : [IngredientChecker.cs]
 * Author      : Hadiyah Kashif
 * Created     : [07/25/2025]
 * Description : [Collider at the end of the chute that takes note of the ingredient game
 *                object's tag and adds it to the ingredient list for order correctness]
 *
 * Notes       : [NONE]
 *******************************************************/

public class IngredientChecker : MonoBehaviour
{
    public List<GameObject> ingredientObjects;
    public List<string> ingredientNames;

    void Start()
    {
        ingredientObjects = new List<GameObject>();
        ingredientNames = new List<string>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered IngredientChecker's OnTriggerEnter() function");
        // get the added ingredient and its tag
        ingredientObjects.Add(other.gameObject);
        ingredientNames.Add(ingredientObjects[ingredientObjects.Count - 1].tag);
        Debug.Log("Added " + other.gameObject.tag + " to the plate");
    }
}
