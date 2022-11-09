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

        // create a list of all particle systems used by movement
        // so that other classes can stop all of them
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

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
            PlayParticlesAndSound(thrustParticles, thrustSound);
        }
        else
        {
            thrustParticles.Stop();
        }
    }


    void ProcessRotation()
    {
        // check for key presses
        bool leftPressed = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
        
        // reset angular velocity
        body.angularVelocity = Vector3.zero;

        // depending on key presses, apply different rotations and FX
        if (leftPressed && rightPressed)
        {
            StopRotationParticles();
        }
        else if (leftPressed)
        {
            ApplyRotation(Vector3.forward, leftRotateParticles, thrustSound);
        }
        else if (rightPressed)
        {
            ApplyRotation(Vector3.back, rightRotateParticles, thrustSound);
        }
        else
        {
            StopRotationParticles();
        }
    }

    private void ApplyThrust()
    {
        body.AddRelativeForce(Vector3.up * thrustForce * Time.fixedDeltaTime);
    }

    void ApplyRotation(Vector3 direction, ParticleSystem particles, AudioClip sound)
    {
        body.angularVelocity = direction.normalized * rotateSpeed * Time.fixedDeltaTime;
        PlayParticlesAndSound(particles, sound);
    }

    void StopRotationParticles()
    {
        leftRotateParticles.Stop();
        rightRotateParticles.Stop();
    }

    private void PlayParticlesAndSound(ParticleSystem particles, AudioClip sound)
    {
        if (null != sound && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(sound);
        }
        if (null != particles && !particles.isPlaying)
        {
            particles.Play();
        }
    }

}
