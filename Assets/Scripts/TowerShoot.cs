using UnityEngine;

public class TowerShoot : MonoBehaviour
{
    public TowerRotate towerRotate;
    public float damage ;
    public float fireRate = 1f;
    public int magSize;
    public float reloadSpeed;
    public int armorPen = 0;
    public bool hiddenDetect = false;
    
    private float fireCountdown = 0f;
    public GameObject bulletPrefab;
    public Transform barrelPoint;

    void Start()
    {
        towerRotate = GetComponent<TowerRotate>();
    }

    void Update()
    {
        if (towerRotate.target == null)
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
        if (bullet == null)
            return;

        BulletController bulletScript = bullet.GetComponent<BulletController>();
        bulletScript.damage = damage;
        bulletScript.SeekTarget(towerRotate.target);
    }
}