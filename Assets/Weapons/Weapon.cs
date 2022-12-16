using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public float offset;

    //public static Weapon instance;

    public GameObject projectile;
    public GameObject shotEffect;
    public Transform shotPoint;
    // public GameObject Player;
    // public Animator camAnim;

    public int _id;
    public Vector3 _difference;
    public float _rotZ;

    private float timeBtwShots;
    public float startTimeBtwShots;
    public bool isMygun;

    /*
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
    */

    private void Update()
    {
        if (isMygun)
        {
            // Handles the weapon rotation
            Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

            ClientSend.gunPos(difference, rotZ);

            if (timeBtwShots <= 0 && Client.instance.myId % 2 == 0)
            {
                if (Input.GetMouseButton(0))
                {
                    //ClientSend.shotGun(true);
                    GameObject e = Instantiate(shotEffect, shotPoint.position, Quaternion.identity);
                    Destroy(e, 1.5f);
                    Vector2 firePosition = new Vector2(shotPoint.position.x, shotPoint.position.y);
                    ClientSend.fireShot(firePosition, transform.rotation);
                    // camAnim.SetTrigger("shake");
                    Instantiate(projectile, shotPoint.position, transform.rotation);
                    timeBtwShots = startTimeBtwShots;
                }
            }
            else
            {
                timeBtwShots -= Time.deltaTime;
            }
        }

        else if(!isMygun)
        {
            Vector3 difference = _difference;
            float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
        }
    }
}