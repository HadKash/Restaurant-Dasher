using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*******************************************************
 * Script Name : [GameOverUIManager.cs]
 * Author      : Hadiyah Kashif
 * Created     : [08/26/2025]
 * Description : [Used in the end game menu buttons to refresh the scene and exit to scene 0 (Start Game)]
 *
 * Notes       : [NONE]
 *******************************************************/

public class GameOverUIManager : MonoBehaviour
{
    // Start is called before the first frame update
     
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(0);
    }
}
