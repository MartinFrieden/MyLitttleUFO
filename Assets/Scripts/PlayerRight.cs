using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRight : MonoBehaviour
{
    Rigidbody2D player;
    Vector2 force = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        player = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player == null)
        {
            Destroy(this);
            return;
        }
        force.x = PlayerControl.instance.velocity.x;
        //player.velocity = PlayerControl.instance.velocity;
        player.AddForce(force);

    }
}
