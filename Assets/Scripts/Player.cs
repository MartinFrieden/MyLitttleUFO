using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Sprite detachedSprite;
    public Sprite burnedSprite;

    public Transform cameraFollowTarget;

    public GameObject deathPrefab;
    public GameObject flameDeathPrefab;
    public GameObject bloodFountainPrefab;

    public float delayBeforeRemoving = 1f;
    public float delayBeforeReleasingGhost = 0.25f;
    
    bool dead = false;

    ParticleSystem particles;
    public AudioClip bangSound;

    
    public enum DamageType
    {
        Slicing,
        Burning
    }

    private void Awake()
    {
        particles = this.GetComponent<ParticleSystem>();
        
    }

    public void ShowDamageEffect(DamageType type)
    {
        switch (type)
        {
            case DamageType.Burning:
                if (flameDeathPrefab != null)
                {
                    Instantiate(
                    flameDeathPrefab, cameraFollowTarget.position,
                    cameraFollowTarget.rotation
                    );
                }
                break;
            case DamageType.Slicing:
                if (deathPrefab != null)
                {
                    Instantiate(
                    deathPrefab,
                    cameraFollowTarget.position,
                    cameraFollowTarget.rotation
                    );
                }
                break;
        }
    }

    public void ApplyDamageSprite(Player.DamageType damageType)
    {
        Sprite spriteToUse = null;
        var audio = GetComponent<AudioSource>();
        switch (damageType)
        {

            case DamageType.Slicing:
                spriteToUse = detachedSprite;
                
                spriteToUse = burnedSprite;
                if (audio)
                {
                    audio.PlayOneShot(this.bangSound);
                }
                particles.Play();
                break;
            case DamageType.Burning:
                spriteToUse = burnedSprite;
                if (audio)
                {
                    audio.PlayOneShot(this.bangSound);
                }
                particles.Play();
                break;
            default:
                break;
        }
        if (spriteToUse != null)
        {
            GetComponent<SpriteRenderer>().sprite = spriteToUse;
        }
    }

    public void DestroyPlayer(DamageType type)
    {
        dead = true;
        switch (type)
        {
            case DamageType.Slicing:
                this.ApplyDamageSprite(type);
                break;
            case DamageType.Burning:
                    this.ApplyDamageSprite(type);
                break;
            default:
                break;
        }
        var remove = gameObject.AddComponent<RemoveAfterDelay>();
    }

    //эффекты подбора монет
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "")
        {
            
        }
    }
}
