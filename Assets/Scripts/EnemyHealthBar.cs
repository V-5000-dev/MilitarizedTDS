using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EnemyHealthBar : MonoBehaviour
{
    public EnemyManager enemyManager;
    public EnemyMovement enemyMovement;
    public int defaultHealth;

    public float health;
    private float lerpTimer;
    private float damage;
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
    public Camera cam;

    private float rangeMultiplier = 0.45f;

    public Color armorHealthBarColor;


    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
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
        damageText.transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
        cam.transform.rotation * Vector3.up);
    }



    // Update is called once per frame
    void Update()
    {

        health = Mathf.Clamp(health, 0, defaultHealth);

        if (health < 1)
        {
            enemyManager.KillEnemy(false);

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
    private float finalDamage;
    public void TakeDamage(float damage, int armorPen, float splashDamage, float splashRange, float dmgoverT, float dmgoverTDuration)
    {

        if (armorPen >= enemyManager.armorLevel)
        {
            finalDamage = damage;
            enemyMovement.StartCoroutine(enemyMovement.Stagger());
        }
        else
        {
            int armorPenned = Mathf.Max(0, enemyManager.armorLevel - armorPen);
            float dmgMulti = 1f - (0.25f * armorPenned);
            dmgMulti = Mathf.Clamp(dmgMulti, 0f, 1f);
            finalDamage = damage * dmgMulti;
        }
        splashRange *= rangeMultiplier;
        if (splashDamage > 0f && splashRange > 0f)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, splashRange);

            foreach (Collider hit in hits)
            {
                EnemyHealthBar enemy = hit.GetComponent<EnemyHealthBar>();
                if (enemy != null && enemy != this)
                {
                    enemy.health -= splashDamage;
                    enemy.damageText.text = "-" + splashDamage;
                    enemy.lerpTimer = 0f;
                    enemy.timeSinceDamage = 0f;
                    enemy.durationTimer = 0;
                }
            }
        }

        health -= finalDamage;
        damageText.text = "-" + finalDamage;
        lerpTimer = 0f;
        timeSinceDamage = 0f;
        durationTimer = 0;

        if (dmgoverT > 0f && dmgoverTDuration > 0f)
        {
            if (dotCoroutine != null)
                StopCoroutine(dotCoroutine);
            dotCoroutine = StartCoroutine(DamageOverTime(dmgoverT, dmgoverTDuration));
        }
    }

    private Coroutine dotCoroutine;

    private IEnumerator DamageOverTime(float dmgPerSecond, float dotDuration)
    {
        float elapsed = 0f;
        while (elapsed < dotDuration)
        {
            yield return new WaitForSeconds(1f);
            elapsed += 1f;
            health -= dmgPerSecond;
            damageText.text = "-" + dmgPerSecond;
            lerpTimer = 0f;
            timeSinceDamage = 0f;
            durationTimer = 0;
        }
        dotCoroutine = null;
    }
    public void Restorehealth(float healAmount)
    {
        health += healAmount;
        //health = Mathf.Round(health);
        lerpTimer = 0;
    }
}
