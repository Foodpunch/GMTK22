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
        Destroy(gameObject);
    }
    void Explode()
    {
        VFXManager.instance.Boom(transform.position);
        AudioManager.instance.PlayCachedSound(AudioManager.instance.ExplosionSounds, transform.position, 0.2f);
        //insert cam shake -.2f here
        Collider2D[] explosionHits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D coll in explosionHits)
        {
            if (coll.GetComponent<Rigidbody2D>() != null)
            {
                Rigidbody2D objRB = coll.GetComponent<Rigidbody2D>();
                objRB.AddForce((objRB.transform.position - transform.position).normalized * 5f, ForceMode2D.Impulse);
                //SendDamage(coll);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision !=null)
        {
            //send damage here
            //if (collision.collider.GetComponent<IDamageable>() == null)
            //{
                
            //}
            AudioManager.instance.PlayCachedSound(AudioManager.instance.ImpactSounds, transform.position, 0.2f);
            VFXManager.instance.Spark(transform.position, collision.contacts[0].normal);
            Despawn();
        }
    }
}
