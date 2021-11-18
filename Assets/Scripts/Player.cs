using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : PlayableMoto
{
    int boostButtonStatus = 0;
    public void OnTurnLeft()
    {
        Turn(-90);
    }
    public void OnTurnRight()
    {
        Turn(90);
    }

    //Unity new input system with openxr on Oculus is buggy, the interraction "Press & release" isn't working as expected
    //So here is a ugly workaround
    protected override void SetBoostInputWorkAround()
    {
        if(boostButtonStatus == 0)
        {
            isBoostOn = false;

        }
        else
        {
            isBoostOn = true;
        }
    }

    public void OnBoost(InputValue input)
    {

        boostButtonStatus = (int)input.Get<float>();
    }
}
