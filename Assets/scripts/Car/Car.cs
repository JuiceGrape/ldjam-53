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

    public static bool Broken = false;

    public Vector3 CurrentSpeed { get; private set; }

    new Rigidbody rigidbody;

    Vector3 prevPos;
    // Start is called before the first frame update
    void Start()
    {
        wheels = GetComponentsInChildren<Wheel>();
        rigidbody = GetComponent<Rigidbody>();

        rigidbody.centerOfMass = new Vector3(0, -0.1f, 0);
        prevPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Broken)
        {
            Brake(1f);
            return;
        }

        CurrentSpeed = transform.InverseTransformDirection(rigidbody.velocity);
        Accelerate(Input.GetAxisRaw("Drive"));
        Steer(Input.GetAxisRaw("Horizontal"));

        if (Input.GetButton("Brake"))
        {
            Brake(1);
        }

        GameStats.RegisterDistance((transform.position - prevPos).magnitude / 1000.0f);
    }

    public void Steer(float value)
    {
        float steerAngle = Upgrades.instance.turning.CalculateValue(baseSteeringAngle);
        foreach (Wheel wheel in wheels)
        {
            wheel.SetSteering(steerAngle * value);
        }
    }

    public void Accelerate(float value)
    {
        float maxSpeed = Upgrades.instance.speed.CalculateValue(baseMaxSpeed);
        float accellerationForce = Upgrades.instance.accell.CalculateValue(baseAccelerationForce);
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
                    if (CurrentSpeed.magnitude >= maxSpeed)
                    {
                        wheel.SetSpeed(0);
                    }
                    else
                    {
                        wheel.SetSpeed(accellerationForce * value);
                    }
                    
                }
            }
        }
        else
        {
            foreach (Wheel wheel in wheels)
            {
                if (CurrentSpeed.magnitude >= maxSpeed)
                {
                    wheel.SetSpeed(0);
                }
                else
                {
                    wheel.SetSpeed(accellerationForce * value);
                }
            }
        }
    }

    public void Brake(float value)
    {
        float brakingForce = Upgrades.instance.brake.CalculateValue(baseBrakingForce);
        foreach (Wheel wheel in wheels)
        {
            wheel.SetBrake(brakingForce * Mathf.Abs(value));
        }
    }

}
