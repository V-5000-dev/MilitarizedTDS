using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float defaultHealth;
    public float health;
    public int armorLevel;
    public float speed;
    public int spawnCost;
    public Material[] materials;
    public MeshRenderer enemyMesh;

    public void KillEnemy(bool damagePlayer)
    {
        if (damagePlayer)
            Debug.Log("Damage player");

        Destroy(this.gameObject);
    }
    public void Start()
    {
        enemyMesh.material = materials[Random.Range(0, materials.Length)];

    }
    

}
