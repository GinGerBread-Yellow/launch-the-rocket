using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum blue_status_team
{
    Idle = 0, Walk = 1, Gun = 7, no_gun = 8
}

public class BlueAni_team : MonoBehaviour
{
    public Transform transform;
    //public static BlueAni_team instance;
    public GameObject gun_variant;
    public Transform weaponSpawnPoint;
    public Animator anim;
    //public Animator animator;
    public blue_status_team cs_team;

    Rigidbody2D rigidbody2d;
    public PlayerManager self;

    private bool isUsingWeapon = false;
    private GameObject currentWeapon;

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

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position * 0.7f + self.pos * 0.3f;

        cs_team = (blue_status_team)self.status;

        if (cs_team == blue_status_team.Idle)
        {
            anim.SetBool("walk", false);
        }
        if (cs_team == blue_status_team.Walk)
        {
            anim.SetBool("walk", true);
        }

        if (!isUsingWeapon)
        {
            if (cs_team == blue_status_team.Gun)
            {
                currentWeapon = Instantiate(gun_variant, weaponSpawnPoint.position, Quaternion.identity);
                currentWeapon.GetComponent<Weapon>().isMygun = false;
                currentWeapon.transform.parent = gameObject.transform;
                isUsingWeapon = true;
            }
        }
        else
        {
            if (cs_team == blue_status_team.no_gun)
            {
                Destroy(currentWeapon);
                isUsingWeapon = false;
            }
        }
    }
}
