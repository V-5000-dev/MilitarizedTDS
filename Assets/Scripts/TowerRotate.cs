using UnityEngine;

public class TowerRotate : MonoBehaviour
{
    public Transform target;
    public float range = 3;
    public string enemyTag = "Enemy";
    public Transform turretBase;
    public float rotationSpeed = 10f;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    bool targetInRange(GameObject enemy)
    {
        float distance = Vector3.Distance(enemy.transform.position, transform.position);
        return distance <= range;
    }

    void Update()
    {
        if (target == null)
            return;

        Vector3 dir = target.position - transform.position;
        Quaternion rot = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(turretBase.rotation, rot, Time.deltaTime * rotationSpeed).eulerAngles;
        turretBase.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (target != null && target.gameObject != null && targetInRange(target.gameObject))
        {
            if (nearestEnemy == null || nearestEnemy.transform == target)
                return;
        }

        if (nearestEnemy != null && targetInRange(nearestEnemy))
            target = nearestEnemy.transform;
        else
            target = null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(range * 2, 0.1f, range * 2));
    }
}