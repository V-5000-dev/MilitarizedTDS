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
        if (roundsFired >= MagSize && !isReloading)
        {
            isReloading = true;
            StartCoroutine(Reload());
        }

        if (towerRotate.Target == null)
            return;

        if (fireCountdown <= 0 && roundsFired < MagSize && !isReloading)
        {
            Shoot();
            roundsFired++;
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
        bulletScript.armorPen = _tower.ArmorPen;
        bulletScript.critChance = _tower.CritChance;
        bulletScript.critDamage = _tower.CritDamage;
        bulletScript.splashRange = _tower.SplashRange;
        bulletScript.splashDamage = _tower.SplashDamage;
        bulletScript.critSplashDamage = _tower.CritSplashDamage;
        bulletScript.critSplashRange = _tower.CritSplashRange;
        bulletScript.dmgOverTime = _tower.OverTimeDmg;
        bulletScript.dmgOverTimeDuration = _tower.OverTimeDuration;
        bulletScript.critDmgOverTime = _tower.CritOverTimeDmg;
        bulletScript.critDmgOverTimeDuration = _tower.CritOverTimeDuration;

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
        if (RoundsReload)
        {
            roundsFired -= 1;
            if (roundsFired > 0 && towerRotate.Target == null)
            {
                StartCoroutine(Reload());
                yield break;
            }
        }
        else
        {
            roundsFired = 0;
        }

        isReloading = false;
    }
}