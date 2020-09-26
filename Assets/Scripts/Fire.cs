using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public float speedX;
    public Vector2 pos
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }
    public Vector2 force;

    Rigidbody2D rigid;

    BoundsCheck bndCheck;
    
  
    void Start()
    {
        rigid = this.GetComponent<Rigidbody2D>();
        bndCheck = this.GetComponent<BoundsCheck>();
        force = new Vector2(0, Random.Range(500, 800));
        rigid.AddForce(force);
    }

    void Update()
    {
        Move();
        if (bndCheck != null && bndCheck.offDown)
        {
            Destroy(gameObject);
        }
    }

    public void Move()
    {
        Vector2 tempPos = pos;
        tempPos.x -= speedX * Time.deltaTime;
        pos = tempPos;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.KillPlayer(Player.DamageType.Burning);
        }
    }
}
