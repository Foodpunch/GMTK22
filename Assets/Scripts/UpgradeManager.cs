using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;
    [SerializeField]
    GameObject UpgradePanel;
    public bool isActive;
    [SerializeField]
    ParticleSystem UpgradeParticles;

    public List<Upgrade> RecurringUpgrades;
    public List<Upgrade> UniqueUpgrades;
    List<Upgrade> SelectedUpgrades= new List<Upgrade>();
    // Start is called before the first frame update
    public Sprite[] DiceNumbers;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RollTutorial()
    {
        UpgradeParticles.gameObject.SetActive(true);
        isActive = true;
        UpgradePanel.SetActive(true);
        Time.timeScale = 0;
        PlayerUIManager.instance.ToggleText();
        SelectedUpgrades.Add(RecurringUpgrades[0]);
        for (int i = 0; i < SelectedUpgrades.Count; ++i)
        {
            SelectedUpgrades[i].gameObject.SetActive(true);
            if (!SelectedUpgrades[i].fixedAmount) SelectedUpgrades[i].RollStats();
        }
    }
    public void RollUpgrades()
    {
        UpgradeParticles.gameObject.SetActive(true);
        isActive = true;
        UpgradePanel.SetActive(true);
        Time.timeScale = 0;
        PlayerUIManager.instance.ToggleText();
        ChooseUpgrades();
    }
    public void UpgradeChosen()
    {
        isActive = false;
        UpgradePanel.SetActive(false);
        Time.timeScale = 1;
        PlayerUIManager.instance.ToggleText();
        for (int i = 0; i < SelectedUpgrades.Count; ++i)
        {
            SelectedUpgrades[i].gameObject.SetActive(false);
        }
        SelectedUpgrades.Clear();
    }
    void ChooseUpgrades()
    {
        if(GunManager.instance.TotalAmmo() <=20)//assuming maayabe this means 1 gun out of ammo?
        {
            SelectedUpgrades.Add(RecurringUpgrades[4]);//give max ammo option
        }
        else SelectedUpgrades.Add(RecurringUpgrades[Random.Range(0, RecurringUpgrades.Count)]);
        if (UniqueUpgrades.Count >0)
        {
              SelectedUpgrades.Add(UniqueUpgrades[Random.Range(0, UniqueUpgrades.Count)]);
        }
        else
        {
            SelectedUpgrades.Add(RecurringUpgrades[Random.Range(0, RecurringUpgrades.Count)]);
        }

        for (int i=0; i< SelectedUpgrades.Count;++i)
        {
            SelectedUpgrades[i].gameObject.SetActive(true);
            if(!SelectedUpgrades[i].fixedAmount) SelectedUpgrades[i].RollStats();
        }
    }
}
