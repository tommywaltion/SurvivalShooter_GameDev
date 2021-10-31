using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    [SerializeField]
    private GameObject Enemy_Prefab;

    public Transform[] SpawnPoints;

    [SerializeField]
    private int enemy_count;

    private int intial_count;

    public float wait_before_spawning_time = 10f;

    // Start is called before the first frame update
    void Awake(){
        MakeInstance();
    }

    void Start(){
        intial_count = enemy_count;

        spawnEnemy();

        StartCoroutine("CheckToSpawnEnemies");
    }

    void MakeInstance(){
        if(instance == null){
            instance = this;
        }
    }

    void spawnEnemy(){
        int index = 0;
        for(int i = 0; i < enemy_count; i++){
            if(index >= SpawnPoints.Length){
                index = 0;
            }
            Instantiate(Enemy_Prefab,SpawnPoints[index].position, Quaternion.identity);
            index++;
        }

        enemy_count = 0;
    }

    IEnumerator CheckToSpawnEnemies(){
        yield return new WaitForSeconds(wait_before_spawning_time);

        spawnEnemy();

        StartCoroutine("CheckToSpawnEnemies");
    }

    public void EnemyDied(bool enemy){
        if (enemy){
            enemy_count++;
            if(enemy_count > intial_count){
                enemy_count = intial_count;
            }
        }
    }

    public void StopSpawning(){
        StopCoroutine("CheckToSpawnEnemies");
    }
}
