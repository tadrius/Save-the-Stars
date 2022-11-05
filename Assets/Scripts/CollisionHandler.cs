using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{

    public const string Collectible = "Collectible";
    public const string Finish = "Finish";
    public const string Friendly = "Friendly";

    [SerializeField] private AudioClip collectSound;
    [SerializeField] private AudioClip failSound;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private float sceneLoadDelay = 1.0f;
    AudioSource audioSource;

    private Movement movement;
    // TODO - have score carry over between scenes
    private int score = 0;

    private void Start() {
        movement = GetComponent<Movement>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other) {
        switch (other.gameObject.tag) {
            case Collectible:
                ProcessCollectible(other.gameObject);
                break;
            case Friendly:
                break;
            case Finish:
                StartSuccessSequence();
                break;
            default:
                StartFailSequence();
                break;
        }
    }

    private void StartSuccessSequence() {
        // TODO - add razzle-dazzle
        movement.enabled = false;
        audioSource.PlayOneShot(successSound);
        Invoke(nameof(LoadNextScene), sceneLoadDelay);
    }

    private void StartFailSequence() {
        // TODO - add razzle-dazzle
        movement.enabled = false;
        audioSource.PlayOneShot(failSound);
        Invoke(nameof(ReloadScene), sceneLoadDelay);
    }

    private void ProcessCollectible(GameObject gameObject) {
        score++;
        audioSource.PlayOneShot(collectSound);
        Debug.Log("Score: " + score);
        gameObject.SetActive(false);
    }

    private void LoadNextScene() {
        int nextSceneBuildIndex = SceneManager.GetActiveScene().buildIndex + 1;
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
