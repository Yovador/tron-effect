using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailBehaviour : MonoBehaviour
{
    [SerializeField, Range(2, 100)]
    private int pointNumber;
    public GameObject anchoredMoto { get; set; }
    public List<Vector3> pointList { get; set; } = new List<Vector3>();


    private void OnDrawGizmos()
    {
        if (pointList.Count > 0)
        {
            List<Vector3> usablePointList = new List<Vector3>();
            foreach (var point in pointList)
            {
                usablePointList.Add(point);
            }
            usablePointList.Add(anchoredMoto.transform.position);
            foreach (var circlePos in usablePointList)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(circlePos, 0.1f);
            }
            foreach (var circlePos in GetEvenlySpacedPoint(usablePointList))
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
