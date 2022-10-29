using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField] float thrustForce = 1000.0f;
    [SerializeField] float rotateSpeed = 100.0f;

    Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        this.body = this.GetComponent<Rigidbody>();
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
        };  
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
