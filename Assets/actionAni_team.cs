using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public enum CharacterStats_team
{
    Idle = 0, Walk = 1, Coal = 2, Metal = 3, Water = 4, Bomb = 5, lab = 6
}

public class actionAni_team : MonoBehaviour
{
    public Transform transform;
    //public static actionAni_team instance;
    public Animator anim;
    public Animation animation;
    public CharacterStats_team cs_team;
    public Rigidbody2D rb;
    //public int increment;
    public PlayerManager self;


    // Start is called before the first frame update

    private void Awake()
    {
        /*if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            //Destroy(this);
        }*/
    }
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        animation = gameObject.GetComponent<Animation>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        transform.position = transform.position * 0.7f + self.pos * 0.3f;
        Debug.Log(String.Format("id = {0} pos = {1}",self.id,self.pos));
 
        cs_team = (CharacterStats_team)self.status;

        if (cs_team == CharacterStats_team.Idle)
        {
            anim.SetBool("walk", false);
        }
        if (cs_team == CharacterStats_team.Walk)
        {
            anim.SetBool("walk", true);
        }
    }

    IEnumerator actionTime()
    {
        yield return new WaitForSeconds(2);
        anim.SetBool("coal", false);
        anim.SetBool("metal", false);
        anim.SetBool("water", false);
        anim.SetBool("develop", true);
        yield return new WaitForSeconds(1);
        rb.constraints &= ~RigidbodyConstraints2D.FreezePosition;
    }

    // Start the animation while colliding and pressing Space
    void OnCollisionStay2D(Collision2D aaa) //aaa為自定義碰撞事件
    {
        if (cs_team == CharacterStats_team.Coal && aaa.gameObject.tag == "Coal")//aa.gameObject.tag == "Coal" && Input.GetKey(KeyCode.Space))
        {
            anim.SetBool("coal", true);
            rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            anim.SetBool("increase_coal", true);
            StartCoroutine(actionTime());
        }
        if (cs_team == CharacterStats_team.Metal && aaa.gameObject.tag == "Metal")//(aaa.gameObject.tag == "Metal" && Input.GetKey(KeyCode.Space))
        {
            anim.SetBool("metal", true);
            rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            anim.SetBool("increase_metal", true);
            StartCoroutine(actionTime());
        }
        if (cs_team == CharacterStats_team.lab && aaa.gameObject.tag == "lab")//(aaa.gameObject.tag == "lab" && Input.GetKey(KeyCode.Space))
        {
            anim.SetBool("develop", false);
            if (anim.GetBool("increase_coal"))
            {
                //Vector3 _localCollection = new Vector3(increment,0,0);
                anim.SetBool("increase_coal", false);
            }
            if (anim.GetBool("increase_metal"))
            {
                //Vector3 _localCollection = new Vector3(0,increment,0);
                anim.SetBool("increase_metal", false);
            }
            if (anim.GetBool("increase_water"))
            {
                //Vector3 _localCollection = new Vector3(0, 0, increment);
                anim.SetBool("increase_water", false);
            }
        }

    }
    void OnTriggerStay2D(Collider2D aaa)
    {
        if (cs_team == CharacterStats_team.Water && aaa.gameObject.tag == "Water")//(aaa.gameObject.tag == "Water" && Input.GetKey(KeyCode.Space))
        {
            anim.SetBool("water", true);
            rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            anim.SetBool("increase_water", true);
            StartCoroutine(actionTime());
        }
    }
}