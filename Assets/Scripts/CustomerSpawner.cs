using UnityEngine;

/*******************************************************
 * Script Name : [CustomerSpawner.cs]
 * Author      : Hadiyah Kashif
 * Created     : [08/04/2025]
 * Description : [Spawns a customer]
 *
 * Notes       : [NONE]
 *******************************************************/

public class CustomerSpawner : MonoBehaviour
{
    // NOTE: prefab different from gameobject
    // anything done to the prefab effects all customers, not just the active one
    public GameObject customerPrefab;
    public GameObject lastSpawnedCustomer;


    void Start()
    {
        SpawnCustomer();
    }

    public void SpawnCustomer()
    {
        GameObject customer = Instantiate(customerPrefab, transform.position, Quaternion.identity);
        customer.GetComponent<CustomerMovement>().spawner = this;
        lastSpawnedCustomer = customer;

        // gets the order manager reference for the OrderManager object in the CustomerMovement script
        CustomerMovement movement = customer.GetComponent<CustomerMovement>();
        OrderManager orderManager = FindAnyObjectByType<OrderManager>();
        movement.orderManager = orderManager;

    }
}
