using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaProjectile : MonoBehaviour
{
    new Collider collider;
    void Start()
    {
        collider = GetComponent<Collider>();
    }

    Vector3 startPos;
    Vector3 arcPos;
    Vector3 endPos;

    bool thrown = false;

    public void Throw(Vector3 start, Vector3 arc, Vector3 end)
    {
        transform.position = start;
        startPos = start;
        arcPos = arc;
        endPos = end;
        thrown = true;
        Debug.Log("going");
    }

    float positionAlongLine = 0.0f;
    float speed = 1.0f;

    void Update()
    {
        if (!thrown)
            return;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 720f * Time.deltaTime, 0));
        
        Vector3 nextPos = GetPointAlongLine(startPos, endPos, arcPos, positionAlongLine);


        transform.position = nextPos;

        positionAlongLine += speed * Time.deltaTime;

        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale * 2, Quaternion.identity, -1);
        int i = 0;
        //Check when there is a new collider coming into contact with the box
        foreach(Collider hit in hitColliders)
        {
            DropoffPoint point = hit.gameObject.GetComponent<DropoffPoint>();
            if (point != null)
            {
                if (point == PizzaController.instance.GetActiveRequest().cachedTarget)
                {
                    point.Score(transform.position);
                    Destroy(gameObject);
                }
            }
        }

        if (positionAlongLine > 1.0f)
        {
            PizzaController.instance.FailRequest();
            Destroy(gameObject);
        }
        
    }

    Vector3 GetPointAlongLine(Vector3 start, Vector3 end, Vector3 arc, float part)
    {
        Vector3 m1 = Vector3.Lerp(start, arc, part);
        Vector3 m2 = Vector3.Lerp(arc, end, part);

        return Vector3.Lerp(m1, m2, part);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (thrown)
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(transform.position, transform.localScale * 4);
    }
}
