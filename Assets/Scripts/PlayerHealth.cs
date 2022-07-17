using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
public class PlayerHealth : MonoBehaviour,IDamageable
{

    public int playerHP = 6;
    bool playerInvul = false;
    public Image healthDisplay;
    public Animator healthBarAnim;
    public GameObject GameOverPanel;
    public Animator faceAnim;
    public void OnTakeDamage(float damage)
    {
        faceAnim.SetTrigger("Hurt");
        if (playerHP <= 1) PlayerDeath();
        if(playerHP >=1 && !playerInvul)
        {
            AudioManager.instance.PlayCachedSound(AudioManager.instance.DiceRollSounds, transform.position,1f);
            AudioManager.instance.PlaySoundAtLocation(AudioManager.instance.MiscSounds[6], 1f, transform.position, true);
            playerHP--;
            UpdateHealthDisplay();
            StartCoroutine(Invulnerable());
        }
        if(playerHP == 1)
        {
            healthBarAnim.speed = 2.5f;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void UpdateHealthDisplay()
    {
        if(playerHP!=0) healthDisplay.sprite = UpgradeManager.instance.DiceNumbers[playerHP - 1];
        healthBarAnim.SetTrigger("Hurt");
    }
    void PlayerDeath()
    {
        PlayerMovement.instance.isDead = true;
        AudioManager.instance.PlaySoundAtLocation(AudioManager.instance.MiscSounds[7], 1f, transform.position);
        GameOverPanel.SetActive(true);
        Time.timeScale = 0;
        AudioManager.instance.isGameOver = true;
        GameManager.instance.isGameOver = true;
    }
    IEnumerator Invulnerable()
    {
        playerInvul = true;
        yield return new WaitForSeconds(0.3f);
        playerInvul = false;
    }
    [Button]
    public void Hurt()
    {
        
        OnTakeDamage(1);
    }
}
