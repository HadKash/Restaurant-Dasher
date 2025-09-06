using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*******************************************************
 * Script Name : [AllCoversAppear.cs]
 * Author      : Hadiyah Kashif
 * Created     : [09/02/2025]
 * Description : [Called at game over to activate all the lids.]
 *
 * Notes       : [The length of the cloud animation isn't exact (0.5), cuts a little short but its fine.]
 *******************************************************/

public class AllCoversAppear : MonoBehaviour
{
    public GameObject[] lids;                   // from left to right (start sesame and end at fish)
    public Transform[] lidLocations;            // index matches the index of the lids
    public CloudPuffController[] cloudController;   // controls when the clouds play their animations (small, medium, large)

    private MeshRenderer[] mesh = new MeshRenderer[10];
    private MeshCollider[] col = new MeshCollider[10];

    void Start()
    {
        for (int i = 0; i < mesh.Length; i++)
        {
            mesh[i] = lids[i].GetComponent<MeshRenderer>();
            col[i] = lids[i].GetComponent<MeshCollider>();
        }
    }

    /**
     * Call the coroutine to activate the lids
     */
    public void EnableAllLids()
    {
        Debug.Log("Calling ActivateLids()");
        StartCoroutine(ActivateLids());
    }

    /**
     * Set the lids to active one by one, and have the cloud animations play as well
     * Check if the lid isn't already active before setting it to active
     */
    private IEnumerator ActivateLids()
    {
        // Sesame Top Bun
        if (!mesh[0].enabled && !col[0].enabled)
        {
            Debug.Log("Activate sesame lid");
            mesh[0].enabled = true;
            col[0].enabled = true;
            cloudController[1].PlayPuff(lidLocations[0]);
            lids[0].SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
        // Regular Top Bun
        if (!mesh[1].enabled && !col[1].enabled)
        {
            Debug.Log("Activate regular lid");
            mesh[1].enabled = true;
            col[1].enabled = true;
            cloudController[1].PlayPuff(lidLocations[1]);
            lids[1].SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
        // Bottom Bun
        if (!mesh[2].enabled && !col[2].enabled)
        {
            Debug.Log("Activate bottom lid");
            mesh[2].enabled = true;
            col[2].enabled = true;
            cloudController[2].PlayPuff(lidLocations[2]);
            lids[2].SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
        // Lettuce
        if (!mesh[3].enabled && !col[3].enabled)
        {
            Debug.Log("Activate lettuce lid");
            mesh[3].enabled = true;
            col[3].enabled = true;
            cloudController[0].PlayPuff(lidLocations[3]);
            lids[3].SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
        // Tomato
        if (!mesh[4].enabled && !col[4].enabled)
        {
            Debug.Log("Activate tomato lid");
            mesh[4].enabled = true;
            col[4].enabled = true;
            cloudController[0].PlayPuff(lidLocations[4]);
            lids[4].SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
        // Cheese
        if (!mesh[5].enabled && !col[5].enabled)
        {
            Debug.Log("Activate cheese lid");
            mesh[5].enabled = true;
            col[5].enabled = true;
            cloudController[0].PlayPuff(lidLocations[5]);
            lids[5].SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
        // Onion
        if (!mesh[6].enabled && !col[6].enabled)
        {
            Debug.Log("Activate onion lid");
            mesh[6].enabled = true;
            col[6].enabled = true;
            cloudController[0].PlayPuff(lidLocations[6]);
            lids[6].SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
        // Chicken Patty
        if (!mesh[7].enabled && !col[7].enabled)
        {
            Debug.Log("Activate chicken lid");
            mesh[7].enabled = true;
            col[7].enabled = true;
            cloudController[0].PlayPuff(lidLocations[7]);
            lids[7].SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
        // Beef Patty
        if (!mesh[8].enabled && !col[8].enabled)
        {
            Debug.Log("Activate beef lid");
            mesh[8].enabled = true;
            col[8].enabled = true;
            cloudController[0].PlayPuff(lidLocations[8]);
            lids[8].SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
        // Fish Patty
        if (!mesh[9].enabled && !col[9].enabled)
        {
            Debug.Log("Activate fish lid");
            mesh[9].enabled = true;
            col[9].enabled = true;
            cloudController[0].PlayPuff(lidLocations[9]);
            lids[9].SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
