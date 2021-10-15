using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PlayableMoto : MonoBehaviour
{
    [SerializeField]
    private Color NeonColor;
    public Color neonColor { 
        get { 
            return NeonColor; 
        }
        set
        {
            NeonColor = neonColor;
        }
    }

    [SerializeField, Range(0.1f, 100)]
    private float neonIntensity;

    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float boostValue;

    bool isBoostOn = false;
    public bool isAlive { get; set; }  = true;
    List<Vector3> trailTurnPoint = new List<Vector3>() ;
    TrailBehaviour trail;
    public bool hasStartedMoving { get; set; } = false;

    virtual protected void Start()
    {
        trail = GetComponentInChildren<TrailBehaviour>();
        trailTurnPoint.Add(trail.transform.position);
        trail.anchoredMoto = gameObject;
        trail.pointList = trailTurnPoint;
    }


    virtual protected void Update()
    {
        Move();
        UpdateColor();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collide");
        if (collision.collider.CompareTag("Wall"))
        {
            Die();
        }
    }


    private void Move()
    {
        float distanceToTravel = (moveSpeed + (boostValue * Convert.ToSingle(isBoostOn)) ) * Time.deltaTime;
        Vector3 movementVector = transform.forward * distanceToTravel;
        transform.position += movementVector;

        if (!hasStartedMoving)
        {
            hasStartedMoving = true;
        }
    }


    private void Die()
    {
        isAlive = false;
        Debug.Log(gameObject.name + " is dead. long live " + gameObject.name + ".");
        Destroy(gameObject);
    }


    virtual protected void Turn(float turnValue)
    {
        Vector3 currentAngle = transform.localRotation.eulerAngles;

        Vector3 targetAngle = currentAngle;
        targetAngle.y += turnValue;

        transform.localRotation = Quaternion.Euler(targetAngle);
        transform.position += transform.forward * Vector3.Distance(transform.position, trail.transform.position);
        trailTurnPoint.Add(trail.transform.position);
        trail.pointList = trailTurnPoint;
        trail.AddTrail((int)turnValue);

    }

    private void UpdateColor()
    {
        foreach (var obj in FindChildrenWithTag(gameObject, "hasNeon"))
        {

            obj.GetComponent<MeshRenderer>().materials[1].SetColor("_EmissionColor", neonColor * neonIntensity);
            
        }

    }

    protected void ToggleBoost()
    {
        isBoostOn = !isBoostOn;
    }

    public static List<GameObject> FindChildrenWithTag(GameObject obj, string tag)
    {
        List<GameObject> result = new List<GameObject>();

        foreach (Transform child in obj.transform)
        {
            GameObject childObj = child.gameObject;
            if (childObj.CompareTag(tag))
            {
                result.Add(childObj);
            }
            result.AddRange(FindChildrenWithTag(childObj, tag));

        }

        return result;
    }
}
