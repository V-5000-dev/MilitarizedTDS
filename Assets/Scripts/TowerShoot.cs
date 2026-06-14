using UnityEngine;

public class TowerShoot : MonoBehaviour
{
    public TowerRotate towerRotate;
    public float fireRate = 1f;
    private float fireCountdown = 0f;
    public GameObject bulletPrefab;
    public Transform barrelPoint;

    void Start()
    {
        towerRotate = GetComponent<TowerRotate>();
    }

    // Update is called once per frame
    void Update()
    {
        if (towerRotate.target = null)
            return;
        
        if (fireCountdown <= 0)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;
    }
    void Shoot()
    {
        Debug.Log("Shoot");
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, barrelPoint.position, barrelPoint.rotation);
        BulletController bulletScript = bullet.GetComponent<BulletController>();
        if (bullet = null)
            return;
        bulletScript.SeekTarget(towerRotate.target);
        
    }

}
