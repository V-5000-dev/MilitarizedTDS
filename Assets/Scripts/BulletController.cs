using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Transform target;
    public float speed = 70f;
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
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
        Debug.Log("HIT");

    }
}
