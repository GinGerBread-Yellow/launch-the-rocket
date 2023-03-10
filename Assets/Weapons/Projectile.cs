using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float speed;
    public float lifeTime;
    public float distance;
    //public int damage;
    public LayerMask whatIsSolid;

    public GameObject destroyEffect;

    private void Start()
    {
        Invoke("DestroyProjectile", lifeTime);
    }

    private void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, distance, whatIsSolid);
        if (hitInfo.collider != null) {
            DestroyProjectile();
        }
        // if (hitInfo.collider != null) {
        //     if (hitInfo.collider.CompareTag("Enemy")) {
        //         hitInfo.collider.GetComponent<Enemy>().TakeDamage(damage);
        //     }
        //     DestroyProjectile();
        // }


        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    public void DestroyProjectile() {
        GameObject effect = Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1.5f);
        Destroy(gameObject);
    }
}
