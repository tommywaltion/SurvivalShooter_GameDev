using UnityEngine;

public class Targets : MonoBehaviour
{
    public float health = 50f;
    

    public void TakeDamage(float amount){
        health -= amount;
        if (health <= 0f)
        {
            Dead();
        }
    }

    void Dead(){
        Destroy(gameObject);
        EnemyManager.instance.EnemyDied(true);
    }
}
