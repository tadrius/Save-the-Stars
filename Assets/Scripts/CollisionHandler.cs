using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{

    // TODO - where do constants go?
    public const string Collectible = "Collectible";
    public const string Finish = "Finish";
    public const string Friendly = "Friendly";

    // PARAMETERS
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private AudioClip failSound;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private float sceneLoadDelay = 1.0f;

    // CACHE
    private AudioSource audioSource;
    private Movement movement;

    // STATE
    private int score = 0;
    private bool isTransitioning = false;

    private void Start() {
        movement = GetComponent<Movement>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other) {
        if (isTransitioning) {
            return;
        }
        switch (other.gameObject.tag) {
            case Collectible:
                ProcessCollectible(other.gameObject);
                break;
            case Friendly:
                break;
            case Finish:
                StartSequence(nameof(LoadNextScene), successSound);
                break;
            default:
                StartSequence(nameof(ReloadScene), failSound);
                break;
        }
    }

    private void StartSequence(string methodName, AudioClip sound) {
        // TODO - add additional flair

        // stop player from taking more actions
        isTransitioning = true;
        movement.enabled = false;

        // play appropriate sound
        if (null != sound) {
            audioSource.PlayOneShot(sound);
        }

        // TODO - replace with coroutine
        // invoke given method
        Invoke(methodName, sceneLoadDelay);
    }

    private void ProcessCollectible(GameObject gameObject) {
        score++;
        audioSource.PlayOneShot(collectSound);
        Debug.Log("Score: " + score);
        gameObject.SetActive(false);
    }

    private void LoadScene(int sceneBuildIndex) {
        // load next scene if scene count is greater than next scene index
        if (SceneManager.sceneCountInBuildSettings > sceneBuildIndex) {
            SceneManager.LoadScene(sceneBuildIndex);
        } else {
            Debug.Log("Scene build index is greater than scene count. Loading the first scene.");
            SceneManager.LoadScene(0);
        }
    }

    private void LoadNextScene() {
        Debug.Log("Loading next scene.");
        LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void ReloadScene() {
        Debug.Log("Reloading scene.");
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
