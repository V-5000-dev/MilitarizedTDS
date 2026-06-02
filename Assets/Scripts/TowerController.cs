using UnityEngine;

public class TowerController : MonoBehaviour
{
    public Transform target;
    public float range = 3;
    public string enemyTag = "Enemy";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }
    bool targetInRange()
    {
        float dx = Mathf.Abs(target.position.x - transform.position.x);
        float dy = Mathf.Abs(target.position.y - transform.position.y);
        float dz = Mathf.Abs(target.position.z - transform.position.z);

        return dx <= range && dy <= range && dz <= range;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
        }
    }
void OnDrawGizmosSelected()
{
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(transform.position, new Vector3(range * 2, range * 2, range * 2));
}
}
