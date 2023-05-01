using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaunchArcRenderer : MonoBehaviour
{
    [SerializeField] private int sections = 100;
    [SerializeField] private float maxDistance = 20.0f;
    [SerializeField] private PizzaProjectile projectilePrefab;

    LineRenderer line;

    Vector3 startPoint;
    Vector3 endpos;
    Vector3 arcPoint;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = sections;
    }

    private void Update()
    {
        float hor = Input.GetAxis("HorizontalRight");
        float ver = Input.GetAxis("VerticalRight");
        if ((hor == 0.0f && ver == 0.0f) || !(PizzaController.instance.HasActiveRequest()))
        {
            line.enabled = false;
            return;
        }
        else
        {
            line.enabled = true;
        }
        Vector3 target = transform.position
            + transform.forward * ver * maxDistance
            + transform.right * hor * maxDistance;

        Plot(target, 7f);

        if (Input.GetButtonDown("Throw"))
        {
            PizzaProjectile projectile = Instantiate(projectilePrefab);
            projectile.Throw(startPoint, arcPoint, endpos);
        }
    }

    void Plot(Vector3 target, float height)
    {
        startPoint = transform.position;
        endpos = target + new Vector3(0, -5, 0);
        arcPoint = startPoint + (endpos - startPoint) / 2 + Vector3.up * height;

        for (int i = 0 ; i < sections; i++ ){
            float part = (float)i / ((float)sections - 1);
            line.SetPosition(i, GetPointAlongLine(transform.position, endpos, arcPoint, part));
        }
    }

    Vector3 GetPointAlongLine(Vector3 start, Vector3 end, Vector3 arc, float part)
    {
        Vector3 m1 = Vector3.Lerp(start, arc, part);
        Vector3 m2 = Vector3.Lerp(arc, end, part);

        return Vector3.Lerp(m1, m2, part);
    }

    //transform.position = bezier.GetQuadraticCoordinates(Mathf.Lerp(0.0,1.0,Time.time) , start.position ,  middle.position , end.position );
}