using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{

    public const string Collectible = "Collectible";
    public const string Finish = "Finish";
    public const string Friendly = "Friendly";

    private int score = 0;

    private void OnCollisionEnter(Collision other) {
        switch (other.gameObject.tag) {
            case Collectible:
                processCollectible(other.gameObject);
                break;
            case Friendly:
                Debug.Log("Collided with Friendly.");
                break;
            case Finish:
                LoadNextScene();
                break;
            default:
                ReloadScene();
                break;
        }
    }

    private void processCollectible(GameObject gameObject) {
        score++;
        Debug.Log("Score: " + score);
        gameObject.SetActive(false);
    }

    private void LoadNextScene() {
        int nextSceneBuildIndex = SceneManager.GetActiveScene().buildIndex + 1;
        Debug.Log("Next scene build index: " + nextSceneBuildIndex 
            + ", Scene count: " + SceneManager.sceneCountInBuildSettings);
        // load next scene if scene count is greater than next scene index
        if (SceneManager.sceneCountInBuildSettings > nextSceneBuildIndex) {
            Debug.Log("Loading next scene.");
            SceneManager.LoadScene(nextSceneBuildIndex);
        } else {
            Debug.Log("No more scenes. Restarting from first scene.");
            SceneManager.LoadScene(0);
        }
    }

    private void ReloadScene() {
        Debug.Log("Reloading scene.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
