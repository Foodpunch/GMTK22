using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    float NextTimeToFire;

    [SerializeField]
    Transform shootPoint;

    public bool DInfinityUpgrade;

    public static GunManager instance;

    public List<Gun> Guns;
    List<Gun> AvailableGuns = new List<Gun>();
    public Gun currGun;

    [SerializeField]
    SpriteRenderer gunSprite;
    public bool pitySpawn = false;
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
        AvailableGuns.Clear();
        //Check Gun Ammo
        for (int i=0; i< Guns.Count; ++i)
        {
            if(Guns[i].currAmmo >0)
            {
                AvailableGuns.Add(Guns[i]);
            }
        }
        if (AvailableGuns.Count > 0)
        {
            currGun = AvailableGuns[UnityEngine.Random.Range(0, AvailableGuns.Count)];
        }
        else
        {
            currGun = Guns[UnityEngine.Random.Range(0, Guns.Count)]; 
        }
        if (TotalAmmo() <= 0 && !pitySpawn) StartCoroutine(PitySpawn());
        gunSprite.sprite = currGun.gunSprite;
        AudioManager.instance.PlaySoundAtLocation(AudioManager.instance.ReloadSounds[Guns.IndexOf(currGun)], transform.position);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !UpgradeManager.instance.isActive && currGun.currAmmo <= 0)
        {
            AudioManager.instance.PlaySoundAtLocation(AudioManager.instance.MiscSounds[2], transform.position);
        }
        if (Input.GetMouseButton(0) && !UpgradeManager.instance.isActive)
        {
            if (currGun.currAmmo <= 0) return;
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
            currGun.currAmmo--;
            GameManager.instance.shotsfired++;
        }
    }
    public void FireSniper()
    {
        RaycastHit2D[] raycasthit2D = Physics2D.RaycastAll(shootPoint.position, shootPoint.transform.right);
        for(int i = 0; i<raycasthit2D.Length; ++i)
        {
            raycasthit2D[i].collider.gameObject.GetComponent<IDamageable>()?.OnTakeDamage(currGun.damage);
            VFXManager.instance.Poof(raycasthit2D[i].collider.gameObject.transform.position);
            if(raycasthit2D[i].collider.gameObject.GetComponent<IDamageable>() == null)
            {
                AudioManager.instance.PlayCachedSound(AudioManager.instance.ImpactSounds, transform.position, 0.2f);
            }
        }
    }
    private void SpawnBullet()
    {
        //play sound and vfx here
        AudioManager.instance.PlaySoundAtLocation(AudioManager.instance.ShootSounds[Guns.IndexOf(currGun)], 0.35f,shootPoint.position);
        for (int i = 0; i < currGun.pelletCount; i++)
        {
            float spreadRange = UnityEngine.Random.Range(-(currGun.spreadAngle * currGun.pelletCount), currGun.spreadAngle * currGun.pelletCount);
            Quaternion randomArc = Quaternion.Euler(0, 0, spreadRange);
            GameObject bulletClone = Instantiate(currGun.bulletPrefab.gameObject, shootPoint.position, transform.rotation * randomArc);
            bulletClone.GetComponent<BulletScript>().bulletDamage = currGun.damage;
        }
    }
    public int TotalAmmo()
    {
        int temp = 0;
        for(int i=0; i<Guns.Count;i++)
        {
            temp += Guns[i].currAmmo;
        }
        return temp;
    }
    IEnumerator PitySpawn()
    {
        yield return new WaitForSeconds(3f);
        Vector2 pos;
        float RightX = Camera.main.ViewportToWorldPoint(Vector2.one).x - UnityEngine.Random.Range(0, 5);
        float LeftX = RightX * -1;
        Vector2 RightSpawnPos = new Vector2(RightX, UnityEngine.Random.Range(-3f, 3f));
        Vector2 LeftSpawnPos = new Vector2(LeftX, UnityEngine.Random.Range(-3f, 3f));
        int rand = UnityEngine.Random.Range(0, 2);
        if (rand % 2 == 0) pos = RightSpawnPos;
        else pos = LeftSpawnPos;
        Instantiate(UpgradeManager.instance.upgradeObj, GameManager.instance.Spawnpoints[UnityEngine.Random.Range(0,GameManager.instance.Spawnpoints.Length)].position, Quaternion.identity);
        pitySpawn = true;
    }
}
