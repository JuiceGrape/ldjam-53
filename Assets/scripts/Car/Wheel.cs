using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WheelCollider))]
public class Wheel : MonoBehaviour
{
    [SerializeField] private Transform wheelMesh;
    [SerializeField] private bool canSteer = false;
    [SerializeField] private bool hasPower = true;
    private WheelCollider wheelCollider;
    void Start()
    {
        wheelCollider = GetComponent<WheelCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
        wheelMesh.rotation = rot;
    }

    public void SetSpeed(float speed)
    {
        if (hasPower)
            wheelCollider.motorTorque = speed;
    }

    public void SetBrake(float force)
    {
        wheelCollider.brakeTorque = force;
    }

    public void SetSteering(float angle)
    {
        if (canSteer)
            wheelCollider.steerAngle = angle;
    }
}
