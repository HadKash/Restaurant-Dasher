using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/*******************************************************
 * Script Name : [StackManager.cs]
 * Author      : Hadiyah Kashif
 * Created     : [07/25/2025]
 * Description : [Makes each consecutive ingredient in a stack grabbable if the one on
 *                top has been lifted.]
 *
 * Notes       : [If any of the toppings (tomatoes, lettuce, cheese, onion) starter stacks run out,
 *                a refill stack will come in. However if the refill stack runs out or the other 
 *                ingredients starter stacks run out, the a lid will appear. Takes note of 
 *                depleted ingredients]
 *******************************************************/

public class StackManager : MonoBehaviour
{
    public Transform restackPos;                 // Position to move the refill stack
    public GameObject starterStackParent;        // Parent object of the starter stack
    public GameObject refillStackParent;         // Parent object of the refill stack
    public GameObject containerCover;
    public float moveStackDelay = 1f;
    public GameObject stackCloudCluster;         // cluster parent for stack cloud puff
    public GameObject lidCloudCluster;           // cluster parent for lid cloud puff
    public Transform cloudStackPosition;         // stack location to where the puff will activate
    public Transform cloudLidPosition;           // lid location to where the puff will activate
    public OrderManager orderManager;            // used to add a depleted ingredient to a list

    CoverAppear cover;
    CloudPuffController stack;
    CloudPuffController lid;
    string ingredientType;

    private List<GameObject> starterStack;       // List of ingredients in starter stack
    private List<GameObject> refillStack;        // List of ingredients in refill stack
    private int currentIndex = 0;                // Tracks current top object in the stack
    private bool refillStackMoved = false;       // Flag to check if refill stack has been moved

    /**
     * Take each empty stack parent and add their children
     * to the starterStack list, and if available, to the 
     * refillStack list. Then call InitializeStack() to adjust 
     * the children's grabbability. Then call AttachGrabListener()
     * to attach a listener to the top child of each stack
     */
    void Start()
    {
        cover = containerCover.GetComponent<CoverAppear>(); 
        if (lidCloudCluster != null)
        {
            lid = lidCloudCluster.GetComponent<CloudPuffController>();
        }
        if (stackCloudCluster != null)
        {
            stack = stackCloudCluster.GetComponent<CloudPuffController>();
        }

        // Initialize stacks from children of the parents
        starterStack = new List<GameObject>();
        refillStack = new List<GameObject>();

        foreach (Transform child in starterStackParent.transform)
            starterStack.Add(child.gameObject);

        if (refillStackParent != null) // Null check for refill stack
        {
            foreach (Transform child in refillStackParent.transform)
                refillStack.Add(child.gameObject);
        }
        // obtain the object tag for depletion purposes
        ingredientType = starterStack[0].tag;

        InitializeStack();
        AttachGrabListener(starterStack[currentIndex]);
    }

    /**
     * Prepare the starter stack and refill stack for gameplay by adjusting
     * the stack's children's grabbability
     */
    void InitializeStack()
    {
        // Starter stack setup: disable all but the first object
        for (int i = 0; i < starterStack.Count; i++)
        {
            Rigidbody rb = starterStack[i].GetComponent<Rigidbody>();
            Collider col = starterStack[i].GetComponent<Collider>();
            XRGrabInteractable grabInteractable = starterStack[i].GetComponent<XRGrabInteractable>();

            rb.isKinematic = true;
            col.isTrigger = (i != currentIndex);            // Only the top object is solid
            grabInteractable.enabled = (i == currentIndex); // Only the top is grabbable
        }

        // Refill stack setup: disable all objects
        foreach (var item in refillStack)
        {
            Rigidbody rb = item.GetComponent<Rigidbody>();
            Collider col = item.GetComponent<Collider>();
            XRGrabInteractable grabInteractable = item.GetComponent<XRGrabInteractable>();

            rb.isKinematic = true;
            col.isTrigger = true;        // Keep objects inactive
            grabInteractable.enabled = false;
        }
    }

    /**
     * Attach grab listeners to the specified object to the top child
     * of each stack
     */
    void AttachGrabListener(GameObject obj)
    {
        XRGrabInteractable grabInteractable = obj.GetComponent<XRGrabInteractable>();
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        if (grabInteractable == null) return;

        // Clear existing listeners to avoid duplicates
        grabInteractable.selectEntered.RemoveAllListeners();
        grabInteractable.selectExited.RemoveAllListeners();

        // Add grab listener
        grabInteractable.selectEntered.AddListener((args) => OnGrabbed(obj));

        // Add release listener
        grabInteractable.selectExited.AddListener((args) =>
        {
            rb.isKinematic = false; // Allow physics interaction after release
        });
    }

    /**
     * Called when the current top object is grabbed to enable the
     * next child's grabbability
     */
    void OnGrabbed(GameObject grabbedObject)
    {
        if (starterStack.Contains(grabbedObject))
        {
            HandleStackGrabbed(starterStack);
        }
        else if (refillStack.Contains(grabbedObject))
        {
            HandleStackGrabbed(refillStack);
        }
    }

    /**
     * Handles logic for enabling the next object after a grab. 
     * If we've reached the last item in the refill stack (or stack 
     * if refill stack doesn't exist), then add a 1 to the number of depleted stacks.
     * Activate the lid when all the stacks in the container are depleted.
     * Call the puff animation right before the lid is activated
     * Add a depleted ingredient to a list
     */
    void HandleStackGrabbed(List<GameObject> currentStack)
    {
        //Transform lidLocation = cover.transform;
        currentIndex++;

        if (currentIndex < currentStack.Count) // Move to the next object
        {
            EnableNextItem(currentStack[currentIndex]);
        }
        // check if starterStack has run out (no refill stack exists)
        else if (currentIndex == currentStack.Count && refillStackParent == null)
        {
            cover.depletedStacks += 1;
            // set the container cover to active
            // add the ingredient to the depletedIngredients list
            if (cover.depletedStacks == cover.totalStacks)
            {
                orderManager.AddDepletedIngredient(ingredientType);
                StartCoroutine(LidDelay());
            }
        }
        // check if refillStack has run out
        else if (currentIndex == currentStack.Count && refillStackMoved)
        {
            cover.depletedStacks += 1;
            // set the container cover to active
            // add the ingredient the the depletedIngredients list
            if (cover.depletedStacks == cover.totalStacks)
            {
                orderManager.AddDepletedIngredient(ingredientType);
                StartCoroutine(LidDelay());
            }
        }
        else if (!refillStackMoved) // Starter stack is finished, move refill stack
        {
            StartCoroutine(MoveStackDelay(moveStackDelay));
        }
    }

    private IEnumerator LidDelay()
    {
        cover.ActivateCover();
        yield return new WaitForSeconds(1f);
        lid.PlayPuff(cloudLidPosition);
        
    }

    /**
     * Creates a delay in bringing up the refill stack to avoid physic problems
     * Plays the stack cloud puff animation
     */
    private IEnumerator MoveStackDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (stack == null) Debug.LogError("Stack Cloud Controller is NULL!");
        if (cloudStackPosition == null) Debug.LogError("Cloud Stack Position is NULL!");
        stack.PlayPuff(cloudStackPosition);
        MoveRefillStack();
    }

    /**
     * Enables the next object for interaction.
     */
    void EnableNextItem(GameObject nextObject)
    {
        Rigidbody rb = nextObject.GetComponent<Rigidbody>();
        Collider col = nextObject.GetComponent<Collider>();
        XRGrabInteractable grabInteractable = nextObject.GetComponent<XRGrabInteractable>();

        rb.isKinematic = true;  // Keep stationary until grabbed
        col.isTrigger = false;  // Enable physics interaction
        grabInteractable.enabled = true;

        AttachGrabListener(nextObject);

        DisableGrabHandModel disableHand = nextObject.GetComponent<DisableGrabHandModel>();
        if (disableHand != null)
        {
            grabInteractable.selectEntered.AddListener(disableHand.HideGrabbingHand);
            grabInteractable.selectExited.AddListener(disableHand.ShowGrabbingHand);
        }
    }

    /**
     * Moves the refill stack to the play area and enables the first tomato.
     */
    void MoveRefillStack()
    {
        if (refillStackParent == null || refillStack.Count == 0)
        {
            Debug.LogWarning("Refill stack is not set or is empty. No backup stack to move.");
            return;
        }
        

        refillStackParent.transform.position = restackPos.position; // Teleport refill stack
        refillStackMoved = true;
        currentIndex = 0; // Reset index for refill stack

        // Enable the first object in the refill stack
        EnableNextItem(refillStack[currentIndex]);
    }
}
