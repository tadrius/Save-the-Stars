using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    // PARAMETERS
    [SerializeField] private float thrustForce = 1000.0f;
    [SerializeField] private float rotateSpeed = 100.0f;
    [SerializeField] private AudioClip thrustSound;
    [SerializeField] private ParticleSystem thrustParticles;
    [SerializeField] private ParticleSystem leftRotateParticles;
    [SerializeField] private ParticleSystem rightRotateParticles;


    // CACHE
    private Rigidbody body;
    private AudioSource audioSource;
    public List<ParticleSystem> particleSystems;

    // STATE

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        particleSystems = new List<ParticleSystem>(3);
        particleSystems.Add(thrustParticles);
        particleSystems.Add(leftRotateParticles);
        particleSystems.Add(rightRotateParticles);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust() {
        if (Input.GetKey(KeyCode.Space)) {
            body.AddRelativeForce(Vector3.up * thrustForce * Time.fixedDeltaTime);
            PlayParticlesAndSound(thrustParticles, thrustSound);
        } else {
            thrustParticles.Stop();
        }
    }

    private void PlayParticlesAndSound(ParticleSystem particles, AudioClip sound) {
        if (!audioSource.isPlaying) {
            audioSource.PlayOneShot(thrustSound);
        }
        if (!particles.isPlaying) {
            particles.Play();
        }
    }

    void ProcessRotation() {
        bool leftPressed = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
        body.angularVelocity = Vector3.zero;
        if (leftPressed && rightPressed) {
            leftRotateParticles.Stop();
            rightRotateParticles.Stop();
        } else if (leftPressed) {
            ApplyRotation(Vector3.forward);
            PlayParticlesAndSound(leftRotateParticles, thrustSound);
        } else if (rightPressed) {
            ApplyRotation(Vector3.back);
            PlayParticlesAndSound(rightRotateParticles, thrustSound);
        } else {
            leftRotateParticles.Stop();
            rightRotateParticles.Stop();
        }
    }

    void ApplyRotation(Vector3 direction) {
        body.angularVelocity = direction.normalized * rotateSpeed * Time.fixedDeltaTime;
    }
}
