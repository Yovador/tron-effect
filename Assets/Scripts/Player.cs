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
        Vector2 inputVec = input.Get<Vector2>();
        Debug.Log("On Move : " + inputVec );

        if(inputVec.x >= -1 && inputVec.x < 0)
        {
            Turn(-90);
        }
        if (inputVec.x <= 1 && inputVec.x > 0)
        {
            Turn(90);
        }
    }

    public void OnBoost()
    {
        ToggleBoost();
    }
}
