using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    float bulletAirTime;
    public float bulletDamage;

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
        _rb.Sleep();
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision !=null)
        {
            //send damage here
            Despawn();
        }
    }
}
