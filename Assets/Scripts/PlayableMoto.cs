using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PlayableMoto : MonoBehaviour
{
    [SerializeField]
    private Color NeonColor;
    public Color neonColor
    {
        get
        {
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
    protected bool IsBoostOn { get { return isBoostOn; } }
    public bool isAlive { get; set; } = true;
    List<Vector3> trailTurnPoint = new List<Vector3>();
    TrailBehaviour trail;
    public bool hasStartedMoving { get; set; } = false;
    public float parentScale { get; set; }
    private Vector3 startingPos;
    private Quaternion startingRotation;

    private void Awake()
    {
        parentScale = transform.parent.localScale.x;
        trail = GetComponentInChildren<TrailBehaviour>();
        trail.anchoredMoto = gameObject;
        UpdateTrailPath();
        startingPos = transform.localPosition;
        startingRotation = transform.localRotation;
    }

    protected virtual void Start()
    {
    }

    virtual protected void Update()
    {
        Move();
        UpdateColor();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collide with " + collision.collider.name);
        if (collision.collider.CompareTag("Wall"))
        {
            Die();
        }
    }


    private void Move()
    {
        float distanceToTravel = (parentScale * (moveSpeed + (boostValue * Convert.ToSingle(isBoostOn)))) * Time.deltaTime;
        Vector3 movementVector = transform.forward * distanceToTravel;
        transform.position += movementVector;

        if (!hasStartedMoving)
        {
            hasStartedMoving = true;
        }
    }


    private void Die()
    {
        Debug.Log(gameObject.name + " is dead. long live " + gameObject.name + ".");
        StopAllCoroutines();
        isBoostOn = false;
        isAlive = false;
        GameManager.instance.EndRound();

    }


    virtual protected void Turn(float turnValue)
    {
        Vector3 currentAngle = transform.localRotation.eulerAngles;

        Vector3 targetAngle = currentAngle;
        targetAngle.y += turnValue;

        transform.localRotation = Quaternion.Euler(targetAngle);
        transform.position += transform.forward * Vector3.Distance(transform.position, trail.transform.position);
        UpdateTrailPath();
        trail.AddTrail((int)turnValue);

    }

    private void UpdateColor()
    {
        foreach (var obj in YovaUtilities.FindChildrenWithTag(gameObject, "hasNeon"))
        {
            obj.GetComponent<MeshRenderer>().materials[1].SetColor("_EmissionColor", neonColor * neonIntensity);
        }

    }

    public void ResetMoto()
    {
        trail.ResetTrail();
        transform.localPosition = startingPos;
        transform.localRotation = startingRotation;
        trailTurnPoint = new List<Vector3>();
        UpdateTrailPath();
        isAlive = true;

    }

    private void UpdateTrailPath()
    {
        trailTurnPoint.Add(trail.transform.position);
        trail.pointList = trailTurnPoint;
    }

    protected void ToggleBoost()
    {
        isBoostOn = !isBoostOn;
    }

   
}
