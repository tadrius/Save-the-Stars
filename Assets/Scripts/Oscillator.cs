using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{

    [SerializeField] Vector3 translateVector;
    [SerializeField] [Range(0, 1)] float translatePeriodOffset = 0.0f;
    [SerializeField] Vector3 rotateVector;
    [SerializeField] [Range(0, 1)] float rotatePeriodOffset = 0.0f;
    [SerializeField] float frequency = 1.0f;

    Rigidbody body;

    Vector3 startPosition;
    Vector3 startAngle;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        startPosition = transform.position;
        startAngle = transform.localRotation.eulerAngles;
    }

    // Update is called once per frame
    private void FixedUpdate() 
    {
        translate(getMovementFactor(Time.fixedTime, translatePeriodOffset));
        rotate(getMovementFactor(Time.fixedTime, rotatePeriodOffset));
    }

    private float getMovementFactor(float time, float periodOffset) {
        // if dividing by period rather than multiplying by frequency, protect against division by 0
        // if (period <= Mathf.Epsilon) { return; }

        float cycles = time * frequency; // frequency as inverse of period
        const float tau = Mathf.PI * 2.0f ; // number of radians in a circle
        float sinWave = Mathf.Sin((cycles + periodOffset) * tau); // value from -1 : 1
        return (sinWave + 1.0f) / 2.0f; // convert value to 0 : 1
    }

    private void translate(float movementFactor) {
        Vector3 translateOffset = translateVector * movementFactor;
        body.position = startPosition + translateOffset;        
    }

    private void rotate(float movementFactor) {
        Vector3 rotateOffset = rotateVector * movementFactor;
        Vector3 rotate = startAngle + rotateOffset;
        body.rotation = Quaternion.Euler(rotate.x, rotate.y, rotate.z);  
    }
}
