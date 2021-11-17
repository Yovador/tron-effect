using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PlayableMoto
{
    public void OnTurnLeft()
    {
        Turn(-90);
    }
    public void OnTurnRight()
    {
        Turn(90);
    }

    public void OnBoost()
    {
        ToggleBoost();
    }
}
