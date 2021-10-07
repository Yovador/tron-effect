using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailBehaviour : MonoBehaviour
{
    [SerializeField, Range(2, 100)]
    private int pointNumber;
    public GameObject anchoredMoto { get; set; }
    private List<Vector3> testPointList = new List<Vector3>();

    private void Start()
    {
        testPointList.Add(new Vector3(1, 1, 0));
        testPointList.Add(new Vector3(3, 2, 0));
        testPointList.Add(new Vector3(-2, 3, 0));
        testPointList.Add(new Vector3(0, 4, 0));


    }
    private void OnDrawGizmos()
    {
        if (testPointList.Count > 0)
        {
            foreach (var circlePos in testPointList)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(circlePos, 0.1f);
            }
            foreach (var circlePos in GetEvenlySpacedPoint(testPointList))
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(circlePos, 0.05f);
            }
        }
    }

    private List<Vector3> GetEvenlySpacedPoint(List<Vector3> pointList )
    {
        List<Vector3> evenlyPoint = new List<Vector3>();

        for (int i = 0; i < pointList.Count; i++)
        {
            Vector3 currentPoint = pointList[i];
            if(i > 0)
            {
                Vector3 directionVector = currentPoint - pointList[i - 1];
                float spaceBetweenPoint = directionVector.magnitude / pointNumber;
                for (int j = 0; j < pointNumber; j++)
                {
                    Debug.Log(i);
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

}
