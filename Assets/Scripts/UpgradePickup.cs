using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePickup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            UpgradeManager.instance.RollUpgrades();
            AudioManager.instance.PlaySoundAtLocation(AudioManager.instance.MiscSounds[4], transform.position);
            AudioManager.instance.PlaySoundAtLocation(AudioManager.instance.MiscSounds[5], transform.position);
            gameObject.SetActive(false);
        }
    }
}
