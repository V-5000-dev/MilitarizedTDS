using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Transform target;
    public GameObject impactEffect;
    public float speed = 70f;
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
        Destroy(effect, 2f);

    }
}
