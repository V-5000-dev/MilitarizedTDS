using System.Collections;
using UnityEngine;

public class TowerShoot : TowerData
{
    [SerializeField] private TowerRotate towerRotate;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform barrelPoint;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private float maxSoundDistance = 30f;

    private Tower _tower;
    private AudioSource _audioSource;
    private Camera _mainCamera;
    private float fireCountdown = 0f;
    public int roundsFired = 0;
    public bool isReloading = false;

    new private void Awake()
    {
        base.Awake();
        _tower = GetComponent<Tower>();
        if (towerRotate == null)
            towerRotate = GetComponent<TowerRotate>();
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.spatialBlend = 0f;
        _mainCamera = Camera.main;
    }
    private void OnEnable()
    {
    isReloading = false;
    roundsFired = 0;
    fireCountdown = 0f;
    }   
    

    private float GetVolumeByDistance()
    {
        if (_mainCamera == null) return 1f;
        float dist = Vector3.Distance(_mainCamera.transform.position, transform.position);
        return Mathf.Clamp01(1f - dist / maxSoundDistance);
    }

    private void Update()
    {
        if (towerRotate.Target == null)
            return;

        if (fireCountdown <= 0 && roundsFired < MagSize)
        {
            Shoot();
            roundsFired++;
            fireCountdown = 1f / _tower.FireRate;

        }
        if (roundsFired >= MagSize && !isReloading)
        {
            isReloading = true;
            StartCoroutine(Reload());
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
        bulletScript.armorPen = _tower.ArmorPen;
        bulletScript.critChance = _tower.CritChance;
        bulletScript.critDamage = _tower.CritDamage;
        bulletScript.SeekTarget(towerRotate.Target);

        if (shootSound != null)
            _audioSource.PlayOneShot(shootSound, GetVolumeByDistance());
    }
    IEnumerator Reload()
    {
        isReloading = true;
        if (reloadSound != null)
            _audioSource.PlayOneShot(reloadSound, GetVolumeByDistance());
        yield return new WaitForSeconds(ReloadSpeed);
        roundsFired = 0;
        isReloading = false;
    }
}