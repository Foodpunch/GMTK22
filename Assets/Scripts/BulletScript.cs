using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    float bulletAirTime;
    public float bulletDamage;

    [SerializeField]
    bool isExplosive = false;
    float explosionRadius = 2f;


    Rigidbody2D _rb;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        bulletAirTime += Time.deltaTime;
        if(bulletAirTime >= 3f)
        {
            Despawn();
        }
    }

    private void Despawn()
    {
        if (isExplosive) Explode();
        _rb.Sleep();
        gameObject.SetActive(false);
        VFXManager.instance.Poof(transform.position);
        Destroy(gameObject,1f);
    }
    void Explode()
    {
        VFXManager.instance.Boom(transform.position);
        if(Vector2.Distance(PlayerMovement.instance.transform.position,transform.position) <=10f)
        {
            AudioManager.instance.PlaySoundAtLocation(AudioManager.instance.ExplosionSounds[0], transform.position);
        }
        else
        {
            AudioManager.instance.PlaySoundAtLocation(AudioManager.instance.ExplosionSounds[1], transform.position);
        }
      
        CamShaker.instance.Trauma += 0.2f;
        Collider2D[] explosionHits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D coll in explosionHits)
        {
            if (coll.GetComponent<Rigidbody2D>() != null)
            {
                Rigidbody2D objRB = coll.GetComponent<Rigidbody2D>();
                objRB.AddForce((objRB.transform.position - transform.position).normalized * 5f, ForceMode2D.Impulse);
                SendDamage(coll);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision !=null)
        {
            SendDamage(collision.collider);
            if (collision.collider.GetComponent<IDamageable>() == null)
            {
                if(!isExplosive) AudioManager.instance.PlayCachedSound(AudioManager.instance.ImpactSounds, transform.position, 0.2f);
                VFXManager.instance.Spark(transform.position, collision.contacts[0].normal);
            }
            Despawn();
        }
    }
    void SendDamage(Collider2D col)
    {
        //Debug.Log("trying to send damage to " + col.gameObject.name);
        if (col.GetComponent<IDamageable>() != null)
        {
            col.GetComponent<IDamageable>().OnTakeDamage(bulletDamage);
            AudioManager.instance.PlaySoundAtLocation(AudioManager.instance.MiscSounds[3], 0.2f, transform.position, true);
        }
    }
}
