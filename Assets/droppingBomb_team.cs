using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class droppingBomb_team : MonoBehaviour
{
    //public static droppingBomb_team instance;
    public Vector2 bombPosition;
    [SerializeField] private GameObject bombPrefab;
    public Animator player;
    public Rigidbody2D rb;

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
    // Update is called once per frame
    void Update()
    {

    }

    /*
    void DropBomb()
    {
        Instantiate(bombPrefab, this.gameObject.transform.position, Quaternion.identity);
        Vector2 bombPosition = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        Debug.Log(bombPosition);
        Debug.Log(Quaternion.identity);
        ClientSend.sendBomb(bombPosition);
    }
    */

    public void RenderOtherBomb(Vector2 bombPosition)
    {
        Instantiate(bombPrefab, bombPosition, Quaternion.identity);
    }

    IEnumerator actionTime()
    {
        yield return new WaitForSeconds(2);
        player.SetBool("isBombed", false);
        rb.constraints &= ~RigidbodyConstraints2D.FreezePosition;
    }

    public void getBombedRed()
    {
        player.SetBool("increase_metal", false);
        player.SetBool("increase_water", false);
        player.SetBool("increase_coal", false);
        player.SetBool("isBombed", true);
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        StartCoroutine(actionTime());
    }

    public void getBombedBlue()
    {
        Debug.Log("blue get bombed");
        player.SetBool("isBombed", true);
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        StartCoroutine(actionTime());
    }
}
