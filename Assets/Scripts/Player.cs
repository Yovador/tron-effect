using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : PlayableMoto
{
    [SerializeField, Range(0,1)]
    float moveSensibility = 0.2f; 

    public void OnMove(InputValue input)
    {
        Debug.Log("On Move");
        Vector2 inputVec = input.Get<Vector2>();

        if(inputVec.x <= -1 && inputVec.x > (-1 - (-moveSensibility) ) )
        {
            Debug.Log("Left");
        }
        
    }

}
