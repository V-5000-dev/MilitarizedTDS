using UnityEngine;

public class TowerRotate : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private Transform turretBase;

    private Tower _tower;

    public Transform Target => target;

    private void Awake()
    {
        _tower = GetComponent<Tower>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);
    }

    private bool TargetInRange(GameObject enemy)
    {
        float distance = Vector3.Distance(enemy.transform.position, transform.position);
        return distance <= _tower.Range;
    }

    private void Update()
    {
        if (target == null)
            return;

        Vector3 dir = target.position - transform.position;
        Quaternion rot = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(turretBase.rotation, rot, Time.deltaTime * Tower.rotationSpeed).eulerAngles;
        turretBase.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    private void UpdateTarget()
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

        if (target != null && target.gameObject != null && TargetInRange(target.gameObject))
        {
            if (nearestEnemy == null || nearestEnemy.transform == target)
                return;
        }

        if (nearestEnemy != null && TargetInRange(nearestEnemy))
            target = nearestEnemy.transform;
        else
            target = null;
    }

    private void OnDrawGizmosSelected()
    {
        if (_tower == null) _tower = GetComponent<Tower>();
        if (_tower == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(_tower.Range * 2, 0.1f, _tower.Range * 2));
    }
}