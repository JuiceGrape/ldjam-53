using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private Wheel[] wheels;

    [SerializeField] private float baseAccelerationForce = 500f;
    [SerializeField] private float baseBrakingForce = 300f;
    [SerializeField] private float baseSteeringAngle = 25.0f;

    private bool isDriving = false;
    private bool isBraking = false;
    // Start is called before the first frame update
    void Start()
    {
        wheels = GetComponentsInChildren<Wheel>();
        GetComponent<Rigidbody>().centerOfMass = new Vector3(0, -0.1f, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Accelerate(Input.GetAxis("Vertical"));
        Steer(Input.GetAxis("Horizontal"));

        if (Input.GetKey(KeyCode.S))
        {
            Brake(1);
        }
        else
        {
            Brake(0);
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
        foreach (Wheel wheel in wheels)
        {
            wheel.SetSpeed(baseAccelerationForce * value);
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
