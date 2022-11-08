using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{

    public const string Collectible = "Collectible";
    public const string Finish = "Finish";
    public const string Friendly = "Friendly";

    // PARAMETERS
    [SerializeField] AudioClip collectSound;
    [SerializeField] AudioClip failSound;
    [SerializeField] AudioClip successSound;
    [SerializeField] ParticleSystem collectParticles;
    [SerializeField] ParticleSystem failParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] float sceneLoadDelay = 1.0f;

    // CACHE
    AudioSource audioSource;
    Movement movement;

    // STATE
    int score = 0;
    bool isTransitioning = false;
    bool collisionsDisabled = false;

    void Start()
    {
        movement = GetComponent<Movement>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessDebugKeys();
    }

    void ProcessDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            // toggle collision disabled
            collisionsDisabled = !collisionsDisabled;
            Debug.Log("Collisions disabled: " + collisionsDisabled);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (isTransitioning || collisionsDisabled)
        {
            return;
        }
        switch (other.gameObject.tag)
        {
            case Friendly:
                break;
            case Finish:
                StartTransitionSequence(nameof(LoadNextScene), successSound, successParticles);
                break;
            default:
                StartTransitionSequence(nameof(ReloadScene), failSound, failParticles);
                break;
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if (isTransitioning)
        {
            return;
        }
        switch (other.gameObject.tag)
        {
            case Collectible:
                ProcessCollectible(other.gameObject);
                break;
            default:
                break;
        }      
    }

    void StartTransitionSequence(string methodName, AudioClip sound, ParticleSystem particles)
    {
        // stop player from taking more actions
        isTransitioning = true;
        movement.enabled = false;

        // turn off movement particles
        foreach (ParticleSystem ps in movement.particleSystems)
        {
            ps.Stop();
        }

        // play appropriate SFX and VFX
        if (null != sound)
        {
            audioSource.PlayOneShot(sound);
        }
        if (null != particles)
        {
            particles.Play();
        }

        // TODO - replace with coroutine
        // invoke given method
        Invoke(methodName, sceneLoadDelay);
    }

    void ProcessCollectible(GameObject gameObject)
    {
        score++;
        audioSource.PlayOneShot(collectSound);
        Debug.Log("Score: " + score);
        gameObject.SetActive(false);
    }

    void LoadNextScene()
    {
        Debug.Log("Loading next scene.");
        LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void ReloadScene()
    {
        Debug.Log("Reloading scene.");
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void LoadScene(int sceneBuildIndex)
    {
        // load next scene if scene count is greater than next scene index
        if (SceneManager.sceneCountInBuildSettings > sceneBuildIndex)
        {
            SceneManager.LoadScene(sceneBuildIndex);
        }
        else
        {
            Debug.Log("Scene build index is greater than scene count. Loading the first scene.");
            SceneManager.LoadScene(0);
        }
    }

}
