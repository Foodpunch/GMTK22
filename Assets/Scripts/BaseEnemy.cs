using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

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
            Die();
        }
    }
    
    [Button]
    void Die()
    {
        AudioManager.instance.PlaySoundAtLocation(AudioManager.instance.MiscSounds[3], 0.2f, transform.position, true);
        Vector2 dir = (transform.position - playerTransform.position).normalized;
        VFXManager.instance.BloodSplat(dir,transform.position);
        VFXManager.instance.BloodSpray(transform.position);
        gameObject.SetActive(false);
    }
}
