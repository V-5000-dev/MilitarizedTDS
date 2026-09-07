using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Transform target;
    public GameObject impactEffect;
    public float speed = 0f;
    public float damage;
    public float critChance;
    public float critDamage;
    public int armorPen;
    public float splashRange;
    public float splashDamage;
    public float critSplashDamage;
    public float critSplashRange;
    public float dmgOverTime;
    public float dmgOverTimeDuration;
    public float critDmgOverTime;
    public float critDmgOverTimeDuration;
    public bool isCrit;

    void Update()
    {
        if (target == null)
        {
  //          Destroy(gameObject);
            return;
        }
        Vector3 dir = target.position - transform.position;
        float frameDistance = speed * Time.deltaTime;
        if (dir.magnitude <= frameDistance)
        {
            HitTarget();
            return;
        }
        transform.Translate(dir.normalized * frameDistance, Space.World);

    }
    public void SeekTarget(Transform _target)
    {
        target = _target;
    }
    public void HitTarget()
    {
        float finalDamage = damage;

        Destroy(gameObject);
        GameObject effect = Instantiate(impactEffect, transform.position, transform.rotation);
        float finalDmgOverTime = dmgOverTime;
        float finalDmgOverTimeDuration = dmgOverTimeDuration;
        isCrit = Random.Range(0f, 100f) < critChance;
        if (isCrit)
        {
            finalDamage = critDamage;
            splashDamage = critSplashDamage;
            finalDmgOverTime = critDmgOverTime;
            finalDmgOverTimeDuration = critDmgOverTimeDuration;
        }
        target.GetComponent<EnemyHealthBar>().TakeDamage(finalDamage, armorPen, splashDamage, splashRange, finalDmgOverTime, finalDmgOverTimeDuration, isCrit);
        Destroy(effect, 2f);
    }
}
