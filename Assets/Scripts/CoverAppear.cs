using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*******************************************************
 * Script Name : [CoverAppear.cs]
 * Author      : Hadiyah Kashif
 * Created     : [08/06/2025]
 * Description : [Sets active the lid for an ingredient that has run out.]
 *
 * Notes       : [Delays its appearance so that the grabbed ingredient doesn't get stuck underneath it]
 *******************************************************/

public class CoverAppear : MonoBehaviour
{
    // add the public object that will contain the cloud puff animation
    public int totalStacks;
    public int depletedStacks;

    MeshRenderer mesh;
    MeshCollider collider;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        collider = mesh.GetComponent<MeshCollider>();
        mesh.enabled = false;
        collider.enabled = false;
    }

    /** 
     * Once both stacks run out, set the container
     * cover to active, representing the ingredient as 
     * run out/ inaccessible
     */
    public void ActivateCover()
    {
        StartCoroutine(DelayActivateCover());
    }

    /**
     * Delays activating the cover so that the final grabbed game object
     * doesn't get locked under the container
     */
    IEnumerator DelayActivateCover()
    {
        yield return new WaitForSeconds(1f);
        mesh.enabled = true;
        collider.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
