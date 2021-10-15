using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerControlledMoto : PlayableMoto
{

    [SerializeField, Range(1, 100)]
    int turnLuck;
    [SerializeField, Range(0.1f, 5f)]
    float detectionRange = 1f;
    List<GameObject> raycastSources;
    private enum Side { Forward, Left, Right }
    protected override void Start()
    {
        base.Start();
        raycastSources = FindChildrenWithTag(gameObject, "RaycastSource");
        raycastSources[(int)Side.Forward].transform.localRotation = Quaternion.Euler(Vector3.zero);
        raycastSources[(int)Side.Left].transform.localRotation = Quaternion.Euler(0, -90, 0);
        raycastSources[(int)Side.Right].transform.localRotation = Quaternion.Euler(0, 90, 0);

        foreach (var rayOriginObj in raycastSources)
        {
            Vector3 direction = rayOriginObj.transform.forward * detectionRange;
            BoxCollider collider = GetComponent<BoxCollider>();
            Debug.DrawRay(GetOriginOfRay(rayOriginObj), direction, Color.gray, 10f);
            Debug.DrawRay(GetOriginOfRay(rayOriginObj, collider.size.z/2), direction, Color.blue, 10f);
            Debug.DrawRay(GetOriginOfRay(rayOriginObj, -collider.size.z/2), direction, Color.magenta, 10f);
        }


    }

    protected override void Update()
    {
        //Debug//
        #region Debug
        //Debug.Log(" forward " + transform.parent.InverseTransformDirection(transform.forward));

        Vector3 direction = Vector3.Cross(transform.forward, transform.up).normalized;
            
        /*Debug.DrawRay(raycastSource.transform.position, direction * detectionRange, Color.red);
        Debug.DrawRay(raycastSource.transform.position,- direction * detectionRange, Color.blue);
        Debug.DrawRay(raycastSource.transform.position, transform.forward * detectionRange, Color.yellow);*/


        /*if (isVertical)
        {
        
            raycastSource.transform.localRotation = Quaternion.Euler(0, 90, 0);
            Vector3 secondRayOrigin = GetOriginOfRay(raycastSource, collider.size.z / 2);
            Debug.DrawRay(secondRayOrigin, raycastSource.transform.forward * detectionRange, Color.magenta);
            Vector3 thirdRayOrigin = GetOriginOfRay(raycastSource, -collider.size.z / 2);
        Debug.DrawRay(thirdRayOrigin, raycastSource.transform.forward * detectionRange, Color.grey);
        /*}
        else
        {
            Debug.Log("Is Horizontal");
            Vector3 secondRayOrigin = transform.parent.InverseTransformDirection(raycastSource.transform.position + (new Vector3(collider.size.x / 2, 0, 0)));
            Debug.DrawRay(secondRayOrigin, raycastSource.transform.forward * detectionRange, Color.magenta);
            Vector3 thirdRayOrigin = transform.parent.InverseTransformDirection( raycastSource.transform.position + (new Vector3(-collider.size.x / 2, 0, 0)));
            Debug.DrawRay(thirdRayOrigin, raycastSource.transform.forward * detectionRange, Color.grey);
        }*/

        /////////////////////
        #endregion

        base.Update();
        ComputerAlgo();

    }


    private void ComputerAlgo()
    {
        if (CheckFoWall(Side.Forward))
        {
            Debug.Log("Turning because of wall");
            TurnSequence();
        }
        else if(Random.Range(1, 100) <= turnLuck)
        {
            Debug.Log("Turning because of random");
            TurnSequence();
        }
    
    }

    private void TurnSequence()
    {

        //Choose Direction
        Debug.Log("Starting turn sequence");

        Side side = Side.Right;
        Side otherSide = Side.Left;
        int sideValue = 1;

        if (Random.Range(1, 100) <= 50) //Turn Right
        {
            side = Side.Right;
            otherSide = Side.Left;
            sideValue = 1;
        }
        else //Turn Left
        {
            side = Side.Left;
            otherSide = Side.Right;
            sideValue = -1;
        }

        if (!CheckFoWall(side))
        {
            Debug.Log("No wall on " + side);
            Turn(sideValue * 90);
        }
        else if (!CheckFoWall(otherSide))
        {
            Debug.Log("No wall on other side : " + side);

            Turn(sideValue * -90);
        }
        else if (!CheckFoWall(Side.Forward))
        {
            Debug.Log("No wall forward : " + side);

            return;
        }
        else
        {
            Debug.Log("Wall Everywhere PANIK");
            bool trashBool; // I didn't succeed to make the out parameter optional, so I have to assign with *something*
            float sideDistance = GetDistanceFromWall(side, out trashBool);
            float otherSideDistance = GetDistanceFromWall(otherSide, out trashBool);
            float forwardDistance = GetDistanceFromWall(Side.Forward, out trashBool);
            Debug.Log("Forward Distance : " + forwardDistance + " Side Distance : " + sideDistance + " other Side Distance : " + otherSideDistance);



            if(forwardDistance > sideDistance && forwardDistance > otherSideDistance)
            {
                return;
            }
            if (sideDistance > otherSideDistance && sideDistance > forwardDistance)
            {
                Turn(sideValue * 90);
            }
            else
            {
                Turn(sideValue * -90);
            }
        }
    }

    private bool CheckFoWall(Side side)
    {
        bool result;
        BoxCollider collider = GetComponent<BoxCollider>();
        float offset = (collider.size.z / 2) + (collider.size.z / 5);
        bool firstRayResult;
        GetDistanceFromWall(side, out firstRayResult, offset);
        bool secondRayResult;
        GetDistanceFromWall(side, out secondRayResult, -offset);
        result = firstRayResult || secondRayResult;
        return result;
    }

    private float GetDistanceFromWall(Side side,  out bool hasTouched, float offset = 0f)
    {
        int layerMask = ~LayerMask.GetMask("Moto");
        RaycastHit hit;
        Color color = Color.white ;
        GameObject rayOriginObj = raycastSources[(int)side];

        switch (side)
        {
            case Side.Forward:
                color = Color.yellow;
                break;
            case Side.Left:
                color = Color.magenta;
                break;
            case Side.Right:
                color = Color.green;
                break;
        }
        Vector3 direction = rayOriginObj.transform.forward * detectionRange;

        hasTouched = Physics.Raycast(GetOriginOfRay(rayOriginObj, offset), direction, out hit, detectionRange, layerMask);
        Debug.DrawRay(GetOriginOfRay(rayOriginObj, offset), direction, color, Time.deltaTime);
        return hit.distance;
    }

    private Vector3 GetOriginOfRay(GameObject gameObject, float offset = 0f)
    {
        return gameObject.transform.position + Vector3.Cross(gameObject.transform.forward, gameObject.transform.up) * offset;
    }
}
