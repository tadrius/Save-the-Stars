using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField] float thrustForce = 1000.0f;
    [SerializeField] float rotateSpeed = 100.0f;

    Rigidbody body;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        this.body = this.GetComponent<Rigidbody>();
        this.audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust() {
        if (Input.GetKey(KeyCode.Space)) {
            this.body.AddRelativeForce(Vector3.up * thrustForce * Time.fixedDeltaTime);
            // turn on looping and only play if not playing so sound plays out fully then repeats
            this.audioSource.loop = true;
            if (!this.audioSource.isPlaying) {
                this.audioSource.Play();
            };
        } else {
            // turn off looping so sound doesn't stop abruptly
            this.audioSource.loop = false;
        }
    }

    void ProcessRotation() {
        bool left = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
        this.body.angularVelocity = Vector3.zero;
        if (left && right) {
            return;
        } else if (left) {
            applyRotation(Vector3.forward);
        } else if (right) {
            applyRotation(Vector3.back);
        }; 
    }

    void applyRotation(Vector3 direction) {
        this.body.angularVelocity = direction.normalized * rotateSpeed * Time.fixedDeltaTime;
    }
}
