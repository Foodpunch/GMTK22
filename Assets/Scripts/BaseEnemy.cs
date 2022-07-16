using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour,IDamageable
{
    public float maxHP = 1f;
    public float currHP;
    public int moveSpeed;


    Transform playerTransform;
    bool isTrackingPlayer;
    Rigidbody2D _rb;

    

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        currHP = maxHP;
        playerTransform = PlayerMovement.instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTakeDamage(float damage)
    {
        currHP -= damage;
        if(currHP<=0)
        {

        }
    }
    void Die()
    {
        AudioManager.instance.PlaySoundAtLocation(AudioManager.instance.MiscSounds[3], 0.2f, transform.position, true);
        gameObject.SetActive(false);
    }
}
