using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EnemyHealthBar : EnemyManager
{
 
    private float lerpTimer;
    [Header("Health Bar")]
    public float chipSpeed = 3f;
    public Image frontHPBar;
    public Image backHPBar;
    public Image outlineHPBar;
    public TextMeshProUGUI damageText;
    [Header("Damage Overlay")]
    public float duration = 2f;
    public float fadeSpeed = 1.5f;
    private float durationTimer;
    private float timeSinceDamage = 0f;

    public Color armorHealthBarColor;


    // Start is called before the first frame update
    void Start()
    {
        Vector3 scale = backHPBar.transform.localScale;
        scale.x = 0.001f * defaultHealth;
        backHPBar.transform.localScale = scale;
        frontHPBar.transform.localScale = scale;
        scale.x = scale.x + 0.01f;
        scale.y = 0.05f;
        scale.z = 0.05f;
        outlineHPBar.transform.localScale = scale;

        frontHPBar.color = armorHealthBarColor;
        health = defaultHealth;

        frontHPBar.fillAmount = 1f;
        backHPBar.fillAmount = 1f;

        damageText.gameObject.SetActive(false);
    }



    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, defaultHealth);

        if (health < 1)
        {
            KillEnemy(false);

        }
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {

        float FillF = frontHPBar.fillAmount;
        float FillB = backHPBar.fillAmount;
        float HPFraction = health / defaultHealth;
        if (FillB > HPFraction)
        {
            damageText.gameObject.SetActive(true);
            frontHPBar.fillAmount = HPFraction;
            backHPBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHPBar.fillAmount = Mathf.Lerp(FillB, HPFraction, percentComplete);
            if (backHPBar.fillAmount == frontHPBar.fillAmount)
            {
                backHPBar.color = armorHealthBarColor;
                damageText.gameObject.SetActive(false);
            }
                

        }
        else
        {
            lerpTimer = 0f;
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
