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

    private void Start()
    {
        trail = GetComponentInChildren<TrailBehaviour>();
        trailTurnPoint.Add(trail.transform.position);
        trail.anchoredMoto = gameObject;
        trail.pointList = trailTurnPoint;
    }


    private void Update()
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
        Debug.DrawRay(transform.position, transform.forward * 20, Color.yellow);

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


    protected void Turn(float turnValue)
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
        List<GameObject> neonObjs = new List<GameObject>(GameObject.FindGameObjectsWithTag("hasNeon"));

        foreach (var obj in neonObjs)
        {
            obj.GetComponent<MeshRenderer>().materials[1].SetColor("_EmissionColor", neonColor * neonIntensity);
        }
    }

    protected void ToggleBoost()
    {
        isBoostOn = !isBoostOn;
    }
}
