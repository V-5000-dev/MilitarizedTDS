using UnityEngine;

public class TowerShoot : MonoBehaviour
{
    [SerializeField] private TowerRotate towerRotate;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform barrelPoint;

    private Tower _tower;
    private float fireCountdown = 0f;

    private void Awake()
    {
        _tower = GetComponent<Tower>();
        if (towerRotate == null)
            towerRotate = GetComponent<TowerRotate>();
    }

    private void Update()
    {
        if (towerRotate.Target == null)
            return;

        if (fireCountdown <= 0)
        {
            Shoot();
            fireCountdown = 1f / _tower.FireRate;
        }
        fireCountdown -= Time.deltaTime;
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, barrelPoint.position, barrelPoint.rotation);
        if (bullet == null)
            return;

        BulletController bulletScript = bullet.GetComponent<BulletController>();
        bulletScript.damage = _tower.Damage;
        bulletScript.SeekTarget(towerRotate.Target);
    }
}