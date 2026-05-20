using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float defaultHealth;
    public float health;
    public int armorLevel;
    public float speed;
    public void KillEnemy(bool damagePlayer)
    {
        if (damagePlayer)
            Debug.Log("Damage player");

        Destroy(this.gameObject);
    } 

}
