using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : Singleton<PlayerControl>
{
    public bool IsUp { get; set; }
    public bool IsDown { get; set; }
    public bool IsLeft { get; set; }
    public bool IsRight { get; set; }
    public Vector2 velocity = Vector2.zero;
    public float speedY = 10f;
    public float speedX = 10f;
    

    private void Start()
    {
        
    }
    private void FixedUpdate()
    {

        if (IsUp)
        {
            velocity.y = speedY;
        }
        else //if (IsDown)
        {
            velocity.y = -speedY;
        }
        /*else
        {
            velocity.y = 0;
        }*/

        /*if (IsRight)
        {
            velocity.x = speedX;
        }
        else if (IsLeft)
        {
            velocity.x = -speedX;
        }
        else
        {
            velocity.x = 0;
        }*/
    }
}
