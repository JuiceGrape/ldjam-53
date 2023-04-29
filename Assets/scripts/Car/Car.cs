using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private Wheel[] wheels;

    [SerializeField] private float baseAccelerationForce = 500f;
    [SerializeField] private float baseBrakingForce = 300f;
    [SerializeField] private float baseSteeringAngle = 25.0f;
    [SerializeField] private bool accelerateIsBrake = true;

    private bool isDriving = false;
    private bool isBraking = false;

    new Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        wheels = GetComponentsInChildren<Wheel>();
        rigidbody = GetComponent<Rigidbody>();

        rigidbody.centerOfMass = new Vector3(0, -0.1f, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Accelerate(Input.GetAxisRaw("Vertical"));
        Steer(Input.GetAxisRaw("Horizontal"));

        if (Input.GetKey(KeyCode.Space))
        {
            Brake(1);
        }
    }

    public void Steer(float value)
    {
        foreach (Wheel wheel in wheels)
        {
            wheel.SetSteering(baseSteeringAngle * value);
        }
    }

    public void Accelerate(float value)
    {
        
        if (accelerateIsBrake)
        {
            var locVel = transform.InverseTransformDirection(rigidbody.velocity);
            Debug.Log(locVel.z);
            if ((locVel.z > 0.01 && value < 0) || 
                (locVel.z < -0.01 && value > 0))
            {
                Debug.Log("Braking");
                Brake(1);
                foreach (Wheel wheel in wheels)
                {
                    wheel.SetSpeed(0);
                }

            } 
            else
            {
                Debug.Log("Driving");
                Brake(0);
                foreach (Wheel wheel in wheels)
                {
                    wheel.SetSpeed(baseAccelerationForce * value);
                }
            }
            Debug.Log("------------------");
        }
        else
        {
            foreach (Wheel wheel in wheels)
            {
                wheel.SetSpeed(baseAccelerationForce * value);
            }
        }
    }

    public void Brake(float value)
    {
        foreach (Wheel wheel in wheels)
        {
            wheel.SetBrake(baseBrakingForce * value);
        }
    }

}
