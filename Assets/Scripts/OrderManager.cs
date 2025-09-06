using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*******************************************************
 * Script Name : [OrderManager.cs]
 * Author      : Hadiyah Kashif
 * Created     : [07/25/2025]
 * Description : [Creates orders with their list of ingredients (based off of tags) and associated game object order bubbles.
 *                Update the ingredient list when an ingredient becomes depleted, possibly inciting a game over.] 
 *
 * Notes       : [NONE]
 *******************************************************/

public class OrderManager : MonoBehaviour
{
    public class Order
    {
        private GameObject orderSpeechBubble;
        private List<string> ingredients;

        public Order(GameObject orderSpeechBubble, List<string> ingredients)
        {
            this.orderSpeechBubble = orderSpeechBubble;
            this.ingredients = ingredients;
        }

        public List<string> GetIngredients()
        {
            return ingredients;
        }

        public GameObject GetOrderBubble()
        {
            return orderSpeechBubble;
        }
    }

    public GameObject[] orderBubbles;
    public Transform orderPosition;
    public AudioClip popup;
    public AudioSource audioSource;
    public List<string> depletedIngredients;
    public UILivesManager livesManager;
    private List<Order> orders;
    private Order activeOrder;

    /**
     * Create the order lists, apply each order's respective game object order bubble,
     * and disable their appearance
     */
    void Start()
    {
        // Initialize the orders list and depletedIngredients list
        orders = new List<Order>();
        depletedIngredients = new List<string>();

        // ingredients listed from bottom to top
        // SINGLE TOPPING ORDERS
        orders.Add(new Order(orderBubbles[0], new List<string> { "BottomBun", "CookedChicken", "Lettuce", "TopBunSesame" }));
        orders.Add(new Order(orderBubbles[1], new List<string> { "BottomBun", "CookedFish", "Lettuce", "TopBunSesame" }));
        orders.Add(new Order(orderBubbles[2], new List<string> { "BottomBun", "CookedBeef", "Lettuce", "TopBunSesame" }));
        orders.Add(new Order(orderBubbles[3], new List<string> { "BottomBun", "CookedChicken", "Cheese", "TopBunSesame" }));
        orders.Add(new Order(orderBubbles[4], new List<string> { "BottomBun", "CookedFish", "Cheese", "TopBunSesame" }));
        orders.Add(new Order(orderBubbles[5], new List<string> { "BottomBun", "CookedBeef", "Cheese", "TopBunSesame" }));
        orders.Add(new Order(orderBubbles[6], new List<string> { "BottomBun", "CookedBeef", "Tomato", "TopBunRegular" }));
        orders.Add(new Order(orderBubbles[7], new List<string> { "BottomBun", "CookedFish", "Tomato", "TopBunRegular" }));
        orders.Add(new Order(orderBubbles[8], new List<string> { "BottomBun", "CookedChicken", "Tomato", "TopBunRegular" }));
        orders.Add(new Order(orderBubbles[9], new List<string> { "BottomBun", "CookedBeef", "Onion", "TopBunRegular" }));
        orders.Add(new Order(orderBubbles[10], new List<string> { "BottomBun", "CookedFish", "Onion", "TopBunRegular" }));
        orders.Add(new Order(orderBubbles[11], new List<string> { "BottomBun", "CookedChicken", "Onion", "TopBunRegular" }));
        // DOUBLE TOPPING ORDERS
        orders.Add(new Order(orderBubbles[12], new List<string> { "BottomBun", "CookedChicken", "Tomato", "Lettuce", "TopBunSesame" }));
        orders.Add(new Order(orderBubbles[13], new List<string> { "BottomBun", "CookedFish", "Tomato", "Lettuce", "TopBunSesame" }));
        orders.Add(new Order(orderBubbles[14], new List<string> { "BottomBun", "CookedBeef", "Tomato", "Lettuce", "TopBunSesame" }));
        orders.Add(new Order(orderBubbles[15], new List<string> { "BottomBun", "CookedChicken", "Cheese", "Lettuce", "TopBunSesame" }));
        orders.Add(new Order(orderBubbles[16], new List<string> { "BottomBun", "CookedFish", "Cheese", "Lettuce", "TopBunSesame" }));
        orders.Add(new Order(orderBubbles[17], new List<string> { "BottomBun", "CookedBeef", "Cheese", "Lettuce", "TopBunSesame" }));
        orders.Add(new Order(orderBubbles[18], new List<string> { "BottomBun", "CookedBeef", "Tomato", "Onion", "TopBunRegular" }));
        orders.Add(new Order(orderBubbles[19], new List<string> { "BottomBun", "CookedFish", "Tomato", "Onion", "TopBunRegular" }));
        orders.Add(new Order(orderBubbles[20], new List<string> { "BottomBun", "CookedChicken", "Tomato", "Onion", "TopBunRegular" }));
        orders.Add(new Order(orderBubbles[21], new List<string> { "BottomBun", "CookedBeef", "Cheese", "Onion", "TopBunSesame" }));
        orders.Add(new Order(orderBubbles[22], new List<string> { "BottomBun", "CookedFish", "Cheese", "Onion", "TopBunSesame" }));
        orders.Add(new Order(orderBubbles[23], new List<string> { "BottomBun", "CookedChicken", "Cheese", "Onion", "TopBunSesame" }));
        orders.Add(new Order(orderBubbles[24], new List<string> { "BottomBun", "CookedChicken", "Onion", "Lettuce", "TopBunRegular" }));
        orders.Add(new Order(orderBubbles[25], new List<string> { "BottomBun", "CookedFish", "Onion", "Lettuce", "TopBunRegular" }));
        orders.Add(new Order(orderBubbles[26], new List<string> { "BottomBun", "CookedBeef", "Onion", "Lettuce", "TopBunRegular" }));
        orders.Add(new Order(orderBubbles[27], new List<string> { "BottomBun", "CookedChicken", "Cheese", "Tomato", "TopBunRegular" }));
        orders.Add(new Order(orderBubbles[28], new List<string> { "BottomBun", "CookedFish", "Cheese", "Tomato", "TopBunRegular" }));
        orders.Add(new Order(orderBubbles[29], new List<string> { "BottomBun", "CookedBeef", "Cheese", "Tomato", "TopBunRegular" }));

        // Set all orders to inactive
        foreach (var order in orders)
        {
            GameObject orderBubble = order.GetOrderBubble();
            orderBubble.SetActive(false);
            //Debug.Log("order bubble hidden");
        }
    }

    public Order GetActiveOrder()
    {
        return activeOrder;
    }

    /**
     * Called when the customer enters idle animation and sets up an active
     * order with the assoc. game object order bubble
     */
    public void SetRandomOrder()
    {
        if (orders == null || orders.Count == 0)
        {
            Debug.LogError("Orders list is empty or null.");
            return;
        }

        Debug.Log("Have entered SetRandomOrder()");
        bool containsDepleted = true;
        int randomOrderNumber = -1;
        while (containsDepleted)
        {
            randomOrderNumber = Random.Range(0, orders.Count);
            List<string> orderIngredients = orders[randomOrderNumber].GetIngredients();
            for (int i = 0; i < orderIngredients.Count; i++)
            {
                if (depletedIngredients.Contains(orderIngredients[i]))
                {
                    break;
                }
                else if (!depletedIngredients.Contains(orderIngredients[i]) && i == orderIngredients.Count - 1)
                {
                    containsDepleted = false;
                }
            }
        }
        activeOrder = orders[randomOrderNumber];
        GameObject orderBubble = activeOrder.GetOrderBubble();
        orderBubble.SetActive(true);
        PlayPopupSound();
    }

    /**
     * Once a customer leaves the counter (either satisfied or disappointed),
     * remove the active order and active speech bubble
     */
    public void ClearActiveOrder()
    {
        if (activeOrder != null)
        {
            GameObject orderBubble = activeOrder.GetOrderBubble();
            if (orderBubble != null)
            {
                orderBubble.SetActive(false);
            }
            activeOrder = null;
        }
        Debug.Log("Active order cleared");
    }

    /**
     * Called when an order becomes available, an audio cue
     */
    private void PlayPopupSound()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.clip = popup;
            audioSource.Play();
        }
    }

    /**
     * Adds a depleted ingredient to the list
     */
    public void AddDepletedIngredient(string ingredientName)
    {
        depletedIngredients.Add(ingredientName);
        LackIngredientsEndGame();
        RemoveOrder(ingredientName);
    }

    /**
     * Remove orders containing depleted ingredients from the list
     */
    private void RemoveOrder(string depletedIngredient)
    {
        // go through every order in Orders and using their ingredient list,
        // remove orders that contain the depleted ingredient
        orders.RemoveAll(order => order.GetIngredients().Contains(depletedIngredient));
    }

    /**
     * If depletedIngredients contains certain ingredients in conjunction,
     * then it will incite a game over
     */
    private void LackIngredientsEndGame()
    {
        if (depletedIngredients.Contains("BottomBun"))
        {
            livesManager.DisplayLoseScreen();
        }
        else if (depletedIngredients.Contains("Tomato") && depletedIngredients.Contains("Lettuce") 
            && depletedIngredients.Contains("Onion") && depletedIngredients.Contains("Cheese"))
        {
            livesManager.DisplayLoseScreen();
        }
        else if (depletedIngredients.Contains("CookedChicken") && depletedIngredients.Contains("CookedBeef") 
            && depletedIngredients.Contains("CookedFish"))
        {
            livesManager.DisplayLoseScreen();
        }
        else if (depletedIngredients.Contains("TopBunSesame") && depletedIngredients.Contains("TopBunRegular"))
        {
            livesManager.DisplayLoseScreen();
        }
    }

}
