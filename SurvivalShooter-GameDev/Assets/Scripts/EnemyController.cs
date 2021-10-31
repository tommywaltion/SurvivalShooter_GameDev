using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState {
    PATROl,
    CHASE,
    ATTACK
}

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent navAgent;

    private EnemyState enemy_state;

    public float walk_speed = 4f;
    public float run_speed = 10f;
    public float chase_distance = 25f;
    private float current_chase_distance;
    public float attack_distance = 2.5f;
    public float chase_after_attack_distance = 3f;

    public float patrol_radius_min = 20f, patrol_radius_max = 60f;
    public float partol_for_this_time = 15f;
    private float patrol_timer;

    public float wait_before_attack = 2f;
    private float attack_timer;

    private Transform target;

    void Awake() {
        navAgent = GetComponent<NavMeshAgent>();

        target = GameObject.FindWithTag("Player").transform;
    }

    void Start()
    {
        enemy_state = EnemyState.PATROl;
        patrol_timer = partol_for_this_time;
        attack_timer = wait_before_attack;
        current_chase_distance = chase_distance;
    }


    void Update()
    {
        if(enemy_state == EnemyState.PATROl) {
            Patrol();
        }
        if(enemy_state == EnemyState.CHASE) {
            Chase();
        }
        if(enemy_state == EnemyState.ATTACK) {
            Attack();
        }
    }

    void Patrol(){

        navAgent.isStopped = false;
        navAgent.speed = walk_speed;

        patrol_timer += Time.deltaTime;

        if(patrol_timer > partol_for_this_time){
            SetNewRandomDestination();
            patrol_timer = 0f;
        }

        if(Vector3.Distance(transform.position, target.position) <= chase_distance){
            enemy_state = EnemyState.CHASE;
        }
    }

    void SetNewRandomDestination() {
        float rand_radius = Random.Range(patrol_radius_min, patrol_radius_max);

        Vector3 randDir = Random.insideUnitSphere * rand_radius;
        randDir += transform.position;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDir, out navHit, rand_radius, -1);
        navAgent.SetDestination(navHit.position);
    }

    void Chase(){
        navAgent.isStopped = false;
        navAgent.speed = run_speed;

        navAgent.SetDestination(target.position);
        
        if(Vector3.Distance(transform.position, target.position) <= attack_distance){
            enemy_state = EnemyState.ATTACK;
            
            if(chase_distance != current_chase_distance){
                chase_distance = current_chase_distance;
            }
        }else if(Vector3.Distance(transform.position,target.position) > (chase_distance + 5f)){
            enemy_state = EnemyState.PATROl;
            patrol_timer = partol_for_this_time;
            if(chase_distance != current_chase_distance){
                chase_distance = current_chase_distance;
            }
        }
    }

    void Attack(){
        navAgent.velocity = Vector3.zero;
        navAgent.isStopped = true;

        attack_timer += Time.deltaTime;
        if(attack_timer > wait_before_attack){
            Debug.Log("Attacking"); 
            attack_timer = 0f;
        }

        if(Vector3.Distance(transform.position, target.position) > attack_distance + chase_after_attack_distance){
            enemy_state = EnemyState.CHASE;
        }
    }

}
