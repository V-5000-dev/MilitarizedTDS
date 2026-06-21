using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed;
    private Transform targetWaypoint;
    private int indexWaypoint;

    void Start()
    {
        targetWaypoint = WaypointsScript.waypoints[0];
    }
    void Update()
    {
        Vector3 direction = targetWaypoint.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
        if (Vector3.Distance(transform.position, targetWaypoint.position) <= 0.01f)
        {
            GetNextWaypoint();
        }
    }
    void GetNextWaypoint()
    {
        if (indexWaypoint >= WaypointsScript.waypoints.Length - 1)
        {
            EndWaypoint();
            return;
        }
           
        indexWaypoint++;
        targetWaypoint = WaypointsScript.waypoints[indexWaypoint];
    }
    void EndWaypoint()
    {
        Destroy(gameObject);
    }
}
