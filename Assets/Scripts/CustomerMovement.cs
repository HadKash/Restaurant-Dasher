using System.Collections;
using UnityEngine;

/*******************************************************
 * Script Name : [YourScriptName.cs]
 * Author      : Hadiyah Kashif
 * Created     : [08/04/2025]
 * Description : [Moves the customer towards the counter and has them leave as well]
 *
 * Notes       : [Also updates the orderDuration timers per 3 correct orders]
 *******************************************************/

public class CustomerMovement : MonoBehaviour
{
    public float moveDuration = 2f;
    public float moveSpeed = 1f;
    public float orderDuration;
    public Vector3 moveDirection = Vector3.forward;

    // object references from script components
    public OrderManager orderManager;
    public CustomerSpawner spawner;
    public SendButton button;
    public CustomerAppearance customerAppearance;
    public UILivesManager livesManager;
    public UIScoreManager scoreManager;

    private Animator animator;
    private Coroutine orderTimerCoroutine;

    /**
     * When the game starts, have the customer start moving to the counter
     */
    void Start()
    {
        animator = GetComponent<Animator>();
        button = FindAnyObjectByType<SendButton>();
        customerAppearance = GetComponent<CustomerAppearance>();
        livesManager = FindAnyObjectByType<UILivesManager>();
        scoreManager = FindAnyObjectByType<UIScoreManager>();

        // have the first customer wait till the intro audio ends
        if (livesManager.Lives == 3 && scoreManager.Score == 0)
        {
            StartCoroutine(CustomerStartDelay());
        }
        else 
        {
            UpdateOrderDuration();
            animator.SetBool("isWalking", true);
            StartCoroutine(WalkToIdle());
        }

    }

    /**
     * Delay the first customer to come out by the end of the intro audio clip
     */
    IEnumerator CustomerStartDelay()
    {
        yield return new WaitForSeconds(24f);   // ~24 seconds is the length of the intro clip
        animator.SetBool("isWalking", true);
        StartCoroutine(WalkToIdle());
    }

    /**
     * For the customer game object to walk from spawn
     * to the counter and transition to the idle animation
     */
    IEnumerator WalkToIdle()
    {
        animator.SetBool("isWalking", true);

        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        animator.SetBool("isWalking", false);
        // set up an order from the order generator
        if (orderManager != null)
        {
            orderManager.SetRandomOrder(); 
        }
        orderTimerCoroutine = StartCoroutine(IdleToWalk());
        // change customer's appearance
        customerAppearance.Change();
    }

    /**
     * The duration that the customer will remain in their idle animation
     * before transitioning to walking (leaving), via an order timer
     * Clear out the order and adjust the lives as well
     */
    IEnumerator IdleToWalk()
    {
        yield return new WaitForSeconds(orderDuration);
        animator.SetBool("isWalking", true);
        // lose a life
        livesManager.LoseLife();
        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
        // call next customer if the player has lives left
        if (spawner != null && livesManager.Lives != 0)
        {
            spawner.SpawnCustomer();
        }
        // clear out the active order plus the ingredient lists
        if (orderManager != null && button != null)
        {
            orderManager.ClearActiveOrder();
            button.ClearIngredientLists();
        }
        // destroy current customer
        Destroy(gameObject);
    }

    /**
     * Called when the order is completed prior to the 
     * timer running out
     */
    public void EarlyIdleToWalk()
    {
        if (orderTimerCoroutine != null) 
        { 
            StopCoroutine(orderTimerCoroutine);
        }
        StartCoroutine(LeaveEarly());
        // customer's changing appearance
        customerAppearance.CancelAppearanceCoroutines();
    }

    /**
     * Customer leaves early due to a correct order
     */
    private IEnumerator LeaveEarly()
    {
        animator.SetBool("isWalking", true);
        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
        if (spawner != null)
        {
            spawner.SpawnCustomer(); // call next customer
        }
        
        Destroy(gameObject); // destroy customer after walking away
    }

    /**
     *  for every 3 successful orders, reduce the orderDuration
     *  time by 6 seconds, adjusting the patienceTimer as well
     *  timer: 30 > 24 > 18 > 12
     *  order: 0  > 3  < 6  > 9
     *  Game Over once player hits a score of 12 (max)
     */
    private void UpdateOrderDuration()
    {
        
        if (scoreManager.Score >= 3 && scoreManager.Score < 6)
        {
            orderDuration = 24f;
            customerAppearance.patienceTimer = orderDuration / 3;
        }
        else if (scoreManager.Score >= 6 && scoreManager.Score < 9)
        {
            orderDuration = 18f;
            customerAppearance.patienceTimer = orderDuration / 3;
        }
        else if (scoreManager.Score >= 9 && scoreManager.Score < 12)
        {
            orderDuration = 12f;
            customerAppearance.patienceTimer = orderDuration / 3;
        }
        else if (scoreManager.Score == 12)
        {
            // enable win screen
        }
    }
}
