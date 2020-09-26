using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class SpaceRocks : MonoBehaviour
{
    [Header("Set in Inspector: Rock")]
    public float speed = 10f;

    private BoundsCheck bndCheck;

    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
    }
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


    // Update is called once per frame
    void Update()
    {
        Move();
        if (bndCheck != null && bndCheck.offLeft)
        {
            if (pos.x < bndCheck.camWidth - bndCheck.radius)
            {
                Destroy(gameObject);
            }
        }
    }

    public virtual void Move()
    {
        Vector2 tempPos = pos;
        tempPos.x -= speed * Time.deltaTime;
        pos = tempPos;
        this.transform.Rotate(Vector3.back, 3f);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.KillPlayer(Player.DamageType.Slicing);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
