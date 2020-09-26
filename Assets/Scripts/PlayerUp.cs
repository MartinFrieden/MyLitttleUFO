using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUp : MonoBehaviour
{
    //переменная твердого тела игрока
    Rigidbody2D player;
    Vector2 force = Vector2.zero;

    void Start()
    {
        //
        player = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*if(player == null)
        {
            Destroy(this);
            return;
        }*/
        force.y = PlayerControl.instance.velocity.y;
        //player.velocity = PlayerControl.instance.velocity;
        player.AddForce(force);

    }
}
