using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public float damage = 10f;
    public float fireRate = 15f;
    
    public Camera Camera;
    public ParticleSystem flash;
    public GameObject impactEffect;

    private float nextTimeToFire = 0f;

    void Update() {
        if(Input.GetButton("Fire1") && Time.time >= nextTimeToFire){
            nextTimeToFire = Time.time + 1f/fireRate;
            Shoot();
        }
    }

    void Shoot(){
        flash.Play();

        RaycastHit hit;
        if(Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hit)){
            Targets target = hit.transform.GetComponent<Targets>();
            if(target != null){
                target.TakeDamage(damage);
            }

            GameObject impactGO = Instantiate(impactEffect,hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 0.75f);
        }
    }
}
