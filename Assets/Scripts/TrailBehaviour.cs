using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TrailBehaviour : MonoBehaviour
{

    [SerializeField, Range(0.1f, 100)]
    private float neonTrailIntensity = 1;
    [SerializeField]
    private GameObject trailPrefab;
    private List<GameObject> trailList = new List<GameObject>();

    public GameObject anchoredMoto { get; set; }
    public List<Vector3> pointList { get; set; } = new List<Vector3>();
    private int currentAngle = 0;


    [Obsolete("This variable has nos more use in the current state of the game. (yes I just wanted to try a deprecated method)")]
    private int evenlyPointNumber = 2;


    private void Update()
    {
        
        if (anchoredMoto.GetComponent<PlayableMoto>().hasStartedMoving)
        {

            UpdateCube();
            foreach (var obj in trailList)
            {
                UpdateNeon(obj);
            }
        }

        
    }

    private void OnDrawGizmos()
    {
        if (pointList.Count > 0)
        {
            List<Vector3> usablePointList = new List<Vector3>(pointList);
            usablePointList.Add(transform.position);
            foreach (var circlePos in usablePointList)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(circlePos, 0.1f * anchoredMoto.GetComponent<PlayableMoto>().parentScale);
            }
        }
            /*foreach (var circlePos in GetEvenlySpacedPoint(usablePointList))
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(circlePos, 0.05f);
            }

        }*/
    }

    [Obsolete("This method has nos more use in the current state of the game. (yes I just wanted to try a deprecated method)")]
    private List<Vector3> GetEvenlySpacedPoint(List<Vector3> pointList )
    {
        List<Vector3> evenlyPoint = new List<Vector3>();

        for (int i = 0; i < pointList.Count; i++)
        {
            Vector3 currentPoint = pointList[i];
            if(i > 0)
            {
                Vector3 directionVector = currentPoint - pointList[i - 1];
                float spaceBetweenPoint = directionVector.magnitude / evenlyPointNumber;
                for (int j = 0; j < evenlyPointNumber; j++)
                {
                    Vector3 newPoint = pointList[i - 1] + (directionVector.normalized * spaceBetweenPoint*j);
                    evenlyPoint.Add(newPoint);

                }
            }
            if(i == pointList.Count - 1)
            {
                evenlyPoint.Add(currentPoint);
            }

        }

        return evenlyPoint;
    }

    private void UpdateCube()
    {
        List<Vector3> points = new List<Vector3>(pointList);
        points.Add(transform.position);

        if(trailList.Count == 0)
        {
            AddTrail();
        }

        ResizeTrail(trailList[trailList.Count - 1], points[trailList.Count-1], points[trailList.Count]);

    }

    public void AddTrail(int newAngle = 0)
    {
        List<Vector3> points = new List<Vector3>(pointList);
        points.Add(transform.position);

        if (trailList.Count != 0)
        {
            for (int i = 0; i < trailList.Count; i++)
            {
                ResizeTrail(trailList[i], points[i], points[i+1], false);
            }
        }
        trailList.Add(Instantiate(trailPrefab, transform.parent.parent));
        currentAngle += newAngle;
    }

    private void UpdateNeon(GameObject gameObject)
    {
        gameObject.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", anchoredMoto.GetComponent<PlayableMoto>().NeonColor * neonTrailIntensity);
    }

    private void ResizeTrail(GameObject obj, Vector3 firstPoint, Vector3 secondPoint, bool rotate = true)
    {

        Vector3 forward = secondPoint - firstPoint;
        float magnitude = Mathf.Abs(forward.magnitude / anchoredMoto.GetComponent<PlayableMoto>().parentScale);
        obj.transform.position = (firstPoint + secondPoint) / 2;
        obj.transform.localScale = new Vector3(obj.transform.localScale.x, obj.transform.localScale.y, magnitude + obj.transform.localScale.x);
        if (rotate)
        {
            obj.transform.localRotation = Quaternion.Euler(0, currentAngle, 0);
        }

        
    }

    public void ResetTrail()
    {
        currentAngle = 0;
        foreach ( var obj in trailList)
        {
            Destroy(obj);
        }
        trailList = new List<GameObject>();
    }
}
