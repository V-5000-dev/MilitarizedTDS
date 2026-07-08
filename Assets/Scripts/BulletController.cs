using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Transform target;
    public GameObject impactEffect;
    public float speed = 70f;
    public float damage;
    public float critChance;
    public float critDamage;
    public float armorPen;

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
        Destroy(gameObject);
        GameObject effect = Instantiate(impactEffect, transform.position, transform.rotation);
        float finalDamage = Random.Range(0f, 100f) < critChance ? critDamage : damage;
        target.GetComponent<EnemyHealthBar>().TakeDamage(finalDamage, armorPen);
        Destroy(effect, 2f);
    }
}
