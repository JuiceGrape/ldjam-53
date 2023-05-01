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
    [SerializeField] private float maxcrashDamage = 40;
    [SerializeField] private float minDamagingImpulse = 20000f;
    [SerializeField] private float maxDamagingImpulse = 70000f;

    [SerializeField] private AudioSource audioIdle;
    [SerializeField] private AudioSource audioDriving;
    [SerializeField] private AudioSource audioBraking;

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
            audioIdle.enabled = false;
            audioDriving.enabled = false;
            return;
        }

        CurrentSpeed = transform.InverseTransformDirection(rigidbody.velocity);
        Accelerate(Input.GetAxisRaw("Drive"));
        Steer(Input.GetAxisRaw("Horizontal"));

        if (Input.GetButton("Brake"))
        {
            Brake(1);
            accelerateIsBrake = false;
        }
        else
        {
            accelerateIsBrake = true;
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
        if (value == 0.0f && CurrentSpeed.magnitude <= 3f)
        {
            audioIdle.enabled = true;
            audioDriving.enabled = false;
        }
        else
        {
            audioIdle.enabled = false;
            audioDriving.enabled = true;
        }

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
        float correctedValue = Mathf.Abs(value);

        if (!Broken && correctedValue > 0.1f && CurrentSpeed.magnitude >= 1.0f)
        {
            audioBraking.enabled = true;
        }
        else
        {
            audioBraking.enabled = false;
        }

        float brakingForce = Upgrades.instance.brake.CalculateValue(baseBrakingForce);
        foreach (Wheel wheel in wheels)
        {
            wheel.SetBrake(brakingForce * correctedValue);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<Zombie>() != null
            || collision.collider.tag == "InvisibleWall")
        {
            return;
        }

        if (collision.impulse.magnitude > minDamagingImpulse)
        {
            float damage = UnitIntervalRange(minDamagingImpulse, maxDamagingImpulse, 0, maxcrashDamage, collision.impulse.magnitude);
            PlayerController.instance.TakeDamage(damage, false);
        }
    }

    float UnitIntervalRange(float stageStartRange, float stageFinishRange, float newStartRange, float newFinishRange, float floatingValue)
    {
        float outRange = Mathf.Abs(newFinishRange - newStartRange);
        float inRange = Mathf.Abs(stageFinishRange - stageStartRange);
        float range = (outRange / inRange);
        return (newStartRange + (range * (floatingValue - stageStartRange)));
    }

}
