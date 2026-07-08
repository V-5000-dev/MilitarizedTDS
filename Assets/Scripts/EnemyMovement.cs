using System.Collections;
using UnityEditor.SpeedTree.Importer;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Transform targetWaypoint;
    private int indexWaypoint;
    public float speed;

    void Start()
    {
        targetWaypoint = WaypointsScript.waypoints[0];

        {

        }
    }
    void Update()
    {
        Vector3 direction = targetWaypoint.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
        transform.LookAt(targetWaypoint);
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
    public IEnumerator Stagger()
    {
        speed = speed / 2;
        yield return new WaitForSeconds(1f);
        speed = speed * 2;
    }
}
