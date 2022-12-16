using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    //public static playerMovement instance;
    public GameObject gun;
    public Transform weaponSpawnPoint;

    Rigidbody2D rigidbody2d;
    public float speed = 3f;
    public Animator animator;
    float horizontal;
    float vertical;
    Vector2 movement;

    public static bool isUsingWeapon = false;
    private GameObject currentWeapon;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    /*private void Awake()
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
    public static bool isUsingGun()
    {
        return isUsingWeapon;
    }
    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);

        // Press G to take out or put back weapon
        if (!isUsingWeapon) {
          if(Input.GetKeyDown(KeyCode.G)){
              BlueAni.instance.cs = BlueCharacterStats.Gun;
              currentWeapon = Instantiate(gun, weaponSpawnPoint.position, Quaternion.identity);
              int id = Client.instance.myId;
              //GameManager.players[id].GetComponentInChildren<Weapon>().isMygun = true;
              currentWeapon.GetComponent<Weapon>().isMygun = true;
              currentWeapon.transform.parent = gameObject.transform;
              isUsingWeapon = true;
            }
        }
        else {
            if(Input.GetKeyDown(KeyCode.G)){
              BlueAni.instance.cs = BlueCharacterStats.no_gun;
              Destroy(currentWeapon);
              isUsingWeapon = false;
            }
        }
    }

    // void FixedUpdate()
    // {
    //     Vector3 position = rigidbody2d.position;
    //     position.x = position.x + 3.0f * horizontal * Time.deltaTime;
    //     position.y = position.y + 3.0f * vertical * Time.deltaTime;
    //     position.z = 0;
    //     position = new Vector3(
    //       Mathf.Clamp(position.x,-22f,21.5f),
    //       Mathf.Clamp(position.y,-16f,15.5f),
    //       0
    //     );
    //
    //     rigidbody2d.MovePosition(position);
    // }

    void FixedUpdate(){
      Vector2 position = rigidbody2d.position;
      position = rigidbody2d.position + movement * speed * Time.fixedDeltaTime;
      position = new Vector2(
      Mathf.Clamp(position.x,-22f,21.5f), Mathf.Clamp(position.y,-16f,15.5f));
      rigidbody2d.MovePosition(position);
    }
}
