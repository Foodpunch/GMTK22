using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    //Stats that the upgrade can modify
    //public int damage;
    //public float fireRate;
    //public float ammoCount;
    //public float currAmmo;
   
    public bool fixedAmount = false;
    public Gun GunToModify;
    int roll=0;
    public Image GunImage;
    public TextMeshProUGUI UpgradeDesc;
    public Image DiceImage;
    public bool maxAmmo;
    Button upgradeButton;
    // Start is called before the first frame update
    void Start()
    {
        GunImage.sprite = GunToModify.gunSprite;
        roll = DiceRoll();
        if (!fixedAmount)
        {
            UpgradeDesc.text = roll.ToString();
            DiceImage.sprite = UpgradeManager.instance.DiceNumbers[roll-1];
            //3 x 6 d6 
        }
        else
        {
            DiceImage.sprite = UpgradeManager.instance.DiceNumbers[6];
        }
        if(maxAmmo) GunImage.sprite = UpgradeManager.instance.DiceNumbers[7];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMaxAmmoFIXED(int maxAmmo)
    {
        GunToModify.maxAmmo += maxAmmo;
        GunToModify.currAmmo = GunToModify.maxAmmo;
    }
    //same as adding max ammo, also introduces bug when picking up max ammo upgrade
    //public void AddCurrAmmo18FIXED()
    //{
    //    GunToModify.currAmmo += 18;
    //}
    public void AddMaxAmmoDice()
    {
        SetDiceImage(roll);
        GunToModify.maxAmmo += roll;
    }
    public void AddDamageDice()
    {
        SetDiceImage(roll);
        GunToModify.damage += roll;
    }
    public void AddFireRateDice()
    {
        SetDiceImage(roll);
        GunToModify.fireRate += roll;
        RemoveFromList();
    }
    public void RollStats()
    {
        roll = DiceRoll();
        //SetDiceImage(roll);
    }
    public void MAXAMMO()
    {
        for(int i=0; i< GunManager.instance.Guns.Count;++i)
        {
            GunManager.instance.Guns[i].Reload(); 
        }
    }
    void SetDiceImage(int diceNum)
    {
        //temporary just to test
        UpgradeDesc.text = diceNum.ToString();
    }
    int DiceRoll()
    {
        return Random.Range(1,7);
    }
    public void PlaySound() //plays the appropriate gun reload sound
    {
        AudioManager.instance.PlaySoundAtLocation(AudioManager.instance.ReloadSounds[GunManager.instance.Guns.IndexOf(GunToModify)], transform.position);
    }
    void RemoveFromList() //super messy disgusting 
    {
        if(UpgradeManager.instance.UniqueUpgrades.Contains(this))
        {
            UpgradeManager.instance.UniqueUpgrades.Remove(this);
        }
    }
}
