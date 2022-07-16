using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    [SerializeField]
    float fireRate;     ///firerate, more number more better
    float spreadAngle = 8f;
    [SerializeField]
    int pelletCount = 5;

    float NextTimeToFire;

    [SerializeField]
    Transform shootPoint;

    public bool DInfinityUpgrade;

    public static GunManager instance;

    public List<Gun> Guns;
    public Gun currGun;

    [SerializeField]
    SpriteRenderer gunSprite;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //currGun = Guns[0];      //pistol
    }
    public void RollGun()
    {
        currGun = Guns[UnityEngine.Random.Range(0, Guns.Count)];
        gunSprite.sprite = currGun.gunSprite;
        AudioManager.instance.PlaySoundAtLocation(AudioManager.instance.ReloadSounds[Guns.IndexOf(currGun)], transform.position);
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
           
            FireGun();
        }
    }

    private void FireGun()
    {
        if (Time.time >= NextTimeToFire)
        {
            if (Guns.IndexOf(currGun) == 3)
            {
                FireSniper();
            }
            SpawnBullet();
            NextTimeToFire = Time.time + (1f / currGun.fireRate);
        }
    }
    public void FireSniper()
    {
        RaycastHit2D[] raycasthit2D = Physics2D.RaycastAll(shootPoint.position, shootPoint.transform.right);
        for(int i = 0; i<raycasthit2D.Length; ++i)
        {
            raycasthit2D[i].collider.gameObject.GetComponent<IDamageable>()?.OnTakeDamage(currGun.damage);
            if(raycasthit2D[i].collider.gameObject.GetComponent<IDamageable>() == null)
            {
                AudioManager.instance.PlayCachedSound(AudioManager.instance.ImpactSounds, transform.position, 0.2f);
            }
        }
    }
    private void SpawnBullet()
    {
        //play sound and vfx here
        AudioManager.instance.PlaySoundAtLocation(AudioManager.instance.ShootSounds[Guns.IndexOf(currGun)], 0.2f,shootPoint.position);
        for (int i = 0; i < currGun.pelletCount; i++)
        {
            float spreadRange = UnityEngine.Random.Range(-(currGun.spreadAngle * currGun.pelletCount), currGun.spreadAngle * currGun.pelletCount);
            Quaternion randomArc = Quaternion.Euler(0, 0, spreadRange);
            GameObject bulletClone = Instantiate(currGun.bulletPrefab.gameObject, shootPoint.position, transform.rotation * randomArc);
        }
    }
}
