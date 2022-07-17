using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;
    // Start is called before the first frame update

    [SerializeField]
    TextMeshProUGUI RollReadyText;
    [SerializeField]
    TextMeshProUGUI GunStatText;
    [SerializeField]
    Transform PlayerUIHolder;
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
        UpdateUIHolderPosition();
        UpdateGunStatText();
        UpdateRollReadyText();
    }
    void UpdateUIHolderPosition()
    {
        PlayerUIHolder.position = Camera.main.WorldToScreenPoint(PlayerMovement.instance.transform.position+transform.up);
    }
    public void UpdateGunStatText()
    {
        GunStatText.text =GunManager.instance.currGun.gunName+" : "+ GunManager.instance.currGun.currAmmo.ToString();
    }
    public void UpdateRollReadyText()
    {
        if(PlayerMovement.instance.rollReady)
        {
            RollReadyText.text = "READY";
        }
        else
        {
            RollReadyText.text = " ";
        }
    }
    public void ToggleText()
    {
        PlayerUIHolder.gameObject.SetActive(!PlayerUIHolder.gameObject.activeInHierarchy);
    }
}
