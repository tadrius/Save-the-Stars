using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    // PARAMETERS
    [SerializeField] private float thrustForce = 1000.0f;
    [SerializeField] private float rotateSpeed = 100.0f;
    [SerializeField] private AudioClip thrustSound;

    // CACHE
    private Rigidbody body;
    private AudioSource audioSource;

    // STATE

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
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
            if (!audioSource.isPlaying) {
                audioSource.PlayOneShot(thrustSound);
            };
        }
    }

    void ProcessRotation() {
        bool left = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
        body.angularVelocity = Vector3.zero;
        if (left && right) {
            return;
        } else if (left) {
            applyRotation(Vector3.forward);
        } else if (right) {
            applyRotation(Vector3.back);
        }; 
    }

    void applyRotation(Vector3 direction) {
        body.angularVelocity = direction.normalized * rotateSpeed * Time.fixedDeltaTime;
    }
}
