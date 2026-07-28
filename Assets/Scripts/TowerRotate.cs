using System;
using UnityEngine;

public class TowerRotate : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private Transform turretBase;
    private float rangeMultiplier = 0.45f * 3;

    private Tower _tower;

    public Transform Target => target;

    private float EffectiveRange => _tower.Range * rangeMultiplier;

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
        return distance <= EffectiveRange;
    }

    private void Update()
    {
        if (target == null)
            return;

        Vector3 dir = target.position - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        turretBase.rotation = Quaternion.RotateTowards(
            turretBase.rotation, targetRot, Tower.rotationSpeed * Time.deltaTime * 60f);
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
        Gizmos.DrawWireCube(transform.position, new Vector3(EffectiveRange * 2, 0.1f, EffectiveRange * 2));
    }
}