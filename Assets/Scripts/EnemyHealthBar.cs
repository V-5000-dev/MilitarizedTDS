using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HealthBarManager : EnemyManager
{
 
    private float lerpTimer;
    [Header("Health Bar")]
    public float chipSpeed = 1.5f;
    public Image frontHPBar;
    public Image backHPBar;
    public Image outlineHPBar;
    [Header("Damage Overlay")]
    public float duration = 2f;
    public float fadeSpeed = 1.5f;
    private float durationTimer;
    private float timeSinceDamage = 0f;

    public Color[] armorHealthBarColor;


    // Start is called before the first frame update
    void Start()
    {
        health = defaultHealth;

        Vector3 scale = backHPBar.transform.localScale;
        scale.x = 0.0025f * defaultHealth;
        backHPBar.transform.localScale = scale;
        frontHPBar.transform.localScale = scale;
        scale.x = scale.x + 0.01f;
        scale.y = 0.06f;
        scale.z = 0.06f;
        outlineHPBar.transform.localScale = scale;
        switch (armorLevel)
        {
            case 0:
                frontHPBar.color = armorHealthBarColor[0];
                break;
            case 1:
                frontHPBar.color = armorHealthBarColor[1];
                break;
            case 2:
                frontHPBar.color = armorHealthBarColor[2];
                break;
            case 3:
                frontHPBar.color = armorHealthBarColor[3];
                break;


        }

        TakeDamage(50);

    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, defaultHealth);
        UpddateHealthUI();

        if (health < 1)
        {
            KillEnemy(false);

        }      
        
    }
   
    public void UpddateHealthUI()
    {
      
        float FillF = frontHPBar.fillAmount;
        float FillB = backHPBar.fillAmount;
        float HPFraction = health / defaultHealth;
        if(FillB > HPFraction)
        {

            frontHPBar.fillAmount = HPFraction;
            backHPBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHPBar.fillAmount = Mathf.Lerp(FillB, HPFraction, percentComplete);
        }
        if(FillF < HPFraction)
        {
            backHPBar.color = Color.green;
            backHPBar.fillAmount = HPFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHPBar.fillAmount = Mathf.Lerp(FillF, backHPBar.fillAmount, percentComplete);
            
        }
        
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;
        timeSinceDamage = 0f;
        durationTimer = 0;
    }
    public void Restorehealth(float healAmount)
    {
        health += healAmount;
        //health = Mathf.Round(health);
        lerpTimer = 0;
    }
}
