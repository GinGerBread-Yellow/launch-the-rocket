using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_team : MonoBehaviour
{
    public static Weapon_team instance;
    public float offset;

    //public int _id;
    //public Vector3 _difference;
    //public float _rotZ;

    public GameObject projectile;
    public GameObject shotEffect;
    // public GameObject Player;
    // public Animator camAnim;

    private float timeBtwShots;
    public float startTimeBtwShots;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }
    public void RenderShot(Vector3 pos, Quaternion temp_quat)
    {
        GameObject e = Instantiate(shotEffect, pos, Quaternion.identity);
        Destroy(e, 1.5f);
        Instantiate(projectile, pos, temp_quat);
    }

    private void Update()
    {
        // Handles the weapon rotation
        
        //Vector3 difference = _difference;
        //float rotZ = _rotZ;
        //transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        /*
        if (timeBtwShots <= 0)
        {
            if (shot)
            {
                GameObject e = Instantiate(shotEffect, shotPoint.position, Quaternion.identity);
                Destroy(e, 1.5f);
                // camAnim.SetTrigger("shake");
                Instantiate(projectile, shotPoint.position, transform.rotation);
                timeBtwShots = startTimeBtwShots;
            }
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
        */
    }
}