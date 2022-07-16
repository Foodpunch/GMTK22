using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //Gun Blueprint or whatever hacking it in
    public float fireRate;
    public float spreadAngle;
    public int pelletCount;
    public Sprite gunSprite;
    public int currAmmo;
    public int maxAmmo;
    public bool InfAmmo;
    public Transform shootPoint;
    public GameObject bulletPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
