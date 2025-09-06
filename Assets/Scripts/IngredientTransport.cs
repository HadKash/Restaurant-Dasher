using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/*******************************************************
 * Script Name : [IngredientTransport.cs]
 * Author      : Hadiyah Kashif
 * Created     : [07/25/2025]
 * Description : [Transports ingredients from the chute to the pipe above the plate.]
 *
 * Notes       : [Onions and tomatoes have different transport positions because their
 *                origin is off center to the object. FreezeAfterDelay is to prevent 
 *                ingredients from sliding off the stack and preventing weird physics
 *                glitches by disabling their physics after a certain amount of time.]
 *******************************************************/

public class IngredientTransport : MonoBehaviour
{
    public Transform ingredientTransportLocation;
    public Transform onionTomatoTransportLocation;   //they're central position is off center compared to the other ingredients
    public float freezeDelay = 3.0f;

    private Quaternion tomatoRotation = Quaternion.Euler(-96.171f, 24.10001f, -113.468f);
    
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        XRGrabInteractable interactable = other.GetComponent<XRGrabInteractable>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        interactable.enabled = false;

        // transport onion to other position and all the others to the default position
        if (other.gameObject.CompareTag("Onion"))
        {
            other.gameObject.transform.position = onionTomatoTransportLocation.position;
        }
        else
        {
            other.gameObject.transform.position = ingredientTransportLocation.transform.position;
        }

        if (other.gameObject.CompareTag("Tomato"))
        {
            other.gameObject.transform.rotation = tomatoRotation;
        }
        else
        {
            other.gameObject.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);

        }

        // called to help the transported ingredient stop fidgeting
        StartCoroutine(FreezeAfterDelay(other.gameObject, freezeDelay));
    }

    /*
     * Helps the fallen ingredient to settle on the plate and not create
     * unnecessary physics problems
     */
    IEnumerator FreezeAfterDelay(GameObject gameobject, float delay)
    {
        Rigidbody rb = gameobject.GetComponent<Rigidbody>();
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
