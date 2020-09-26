using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public AudioClip pickUpSound;
    Rigidbody2D rigid;
    Collider2D coll;

    private void Start()
    {
        rigid = this.GetComponent<Rigidbody2D>();
        coll = this.GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var audio = GetComponent<AudioSource>();
        if (collision.gameObject.tag == "Player")
        {
            if (audio)
            {
                audio.PlayOneShot(this.pickUpSound);
            }
            Destroy(coll);
            rigid.AddForce(Vector2.up*1000);
            Destroy(this.gameObject, 0.25f);
            UIController.instance.AddCoin();
            PlayerPrefs.Save();
        }
    }
}
