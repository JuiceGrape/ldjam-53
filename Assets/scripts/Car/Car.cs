using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private Wheel[] wheels;

    [SerializeField] private float baseAccelerationForce = 500f;
    [SerializeField] private float baseBrakingForce = 300f;
    [SerializeField] private float baseSteeringAngle = 25.0f;
    [SerializeField] private float baseMaxSpeed = 20.0f;
    [SerializeField] private bool accelerateIsBrake = true;

    public Vector3 CurrentSpeed { get; private set; }

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
        CurrentSpeed = transform.InverseTransformDirection(rigidbody.velocity);
        Accelerate(Input.GetAxisRaw("Drive"));
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
            if ((CurrentSpeed.z > 0.01 && value < 0) || 
                (CurrentSpeed.z < -0.01 && value > 0))
            {
                //Braking
                Brake(value);
                foreach (Wheel wheel in wheels)
                {
                    wheel.SetSpeed(0);
                }

            } 
            else
            {
                //Driving
                Brake(0);
                foreach (Wheel wheel in wheels)
                {
                    if (CurrentSpeed.magnitude >= baseMaxSpeed)
                    {
                        wheel.SetSpeed(0);
                    }
                    else
                    {
                        wheel.SetSpeed(baseAccelerationForce * value);
                    }
                    
                }
            }
        }
        else
        {
            foreach (Wheel wheel in wheels)
            {
                if (CurrentSpeed.magnitude >= baseMaxSpeed)
                {
                    wheel.SetSpeed(0);
                }
                else
                {
                    wheel.SetSpeed(baseAccelerationForce * value);
                }
            }
        }
    }

    public void Brake(float value)
    {
        foreach (Wheel wheel in wheels)
        {
            wheel.SetBrake(baseBrakingForce * Mathf.Abs(value));
        }
    }

}
