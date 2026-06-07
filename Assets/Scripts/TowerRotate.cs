using UnityEngine;

public class TowerRotate : MonoBehaviour
{
    public Transform target;
    public float range = 3;
    public string enemyTag = "Enemy";
    public Transform turretBase;
    public float rotationSpeed = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }
    bool targetInRange(GameObject target)
    {
        float dx = Mathf.Abs(target.transform.position.x - transform.position.x);
        float dy = Mathf.Abs(target.transform.position.y - transform.position.y);
        float dz = Mathf.Abs(target.transform.position.z - transform.position.z);

        return dx <= range && dy <= range && dz <= range;
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
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
            if(distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        if(nearestEnemy != null && targetInRange(nearestEnemy))
        {
            target = nearestEnemy.transform;
        }
    }
void OnDrawGizmosSelected()
{
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(transform.position, new Vector3(range * 2, range * 2, range * 2));
}
}
