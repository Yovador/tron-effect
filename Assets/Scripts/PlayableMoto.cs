using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PlayableMoto : MonoBehaviour
{
    [SerializeField]
    Color neon;
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float boostValue;

    bool isBoostOn = false;
    public bool isAlive { get; set; }  = true;
    List<Vector3> trailTurnPoint = new List<Vector3>() ;

    TrailBehaviour trail;

    private void Start()
    {
        trailTurnPoint.Add(transform.position);
        trail = GetComponentInChildren<TrailBehaviour>();
        trail.anchoredMoto = gameObject;
    }


    private void Update()
    {
        Move();
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
        Vector3 movementVector = transform.worldToLocalMatrix * transform.forward * distanceToTravel;
        transform.localPosition += movementVector;
        CreateTrail();
    }


    private void Die()
    {
        isAlive = false;
        Debug.Log(gameObject.name + " is dead. long live " + gameObject.name + ".");
    }

    private void CreateTrail()
    {
        trail.pointList = trailTurnPoint;
    }

    protected void Turn(float turnValue)
    {
        trailTurnPoint.Add(transform.position);
        Vector3 currentAngle = transform.localRotation.eulerAngles;

        Vector3 targetAngle = currentAngle;
        targetAngle.y += turnValue;

        transform.localRotation = Quaternion.Euler(targetAngle);

    }

    protected void ToggleBoost()
    {
        isBoostOn = !isBoostOn;
    }
}
