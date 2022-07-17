using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;
public class BaseEnemy : MonoBehaviour,IDamageable
{
    public float maxHP = 1f;
    public float currHP;
    
    public float moveSpeed = 4.5f;       //player speed is 8

    public Vector2 DirToPlayer;
    Transform playerTransform;
    bool isTrackingPlayer;
    Rigidbody2D _rb;

    public event Action deathEvent;
    public GameObject UpgradePickup;
    public bool willDropUpgrade;
    public Animator _anim;
    public int value;
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
        DirToPlayer = playerTransform.position - transform.position;
        DirToPlayer.Normalize();
    
    }
    private void FixedUpdate()
    {
        if(Vector2.Distance(playerTransform.position,transform.position)>1f)
        _rb.velocity = DirToPlayer * moveSpeed;
    }
    public void OnTakeDamage(float damage)
    {
        _rb.AddForce(-DirToPlayer * 5f);
        currHP -= damage;
        _anim.SetTrigger("Hurt");
        if(currHP<=0)
        {
            //int rand = UnityEngine.Random.Range(1, 101);
            //if (rand <= 20)
            //{
            //    Instantiate(UpgradePickup, transform.position, Quaternion.identity);
            //}
            Die();
        }
    }
    
    [Button]
    void Die()
    {
        if(deathEvent!=null)        //calling the death event function in gamemanager
        {
            deathEvent();
        }
        AudioManager.instance.PlaySoundAtLocation(AudioManager.instance.MiscSounds[3], 0.2f, transform.position, true);
        Vector2 dir = (transform.position - playerTransform.position).normalized;
        VFXManager.instance.BloodSplat(dir,transform.position);
        VFXManager.instance.BloodSpray(transform.position);
        if (willDropUpgrade) Instantiate(UpgradePickup, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        GameManager.instance.SpawnedEnemies.Remove(this);
        GameManager.instance.CashMoney += value;
        GameManager.instance.enemiesKilled++;
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<IDamageable>()?.OnTakeDamage(1);
            Die();
        }
    }
}
