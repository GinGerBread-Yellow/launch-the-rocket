using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlueCharacterStats{
  Idle =0, Walk = 1, Gun = 7, no_gun = 8
}

public class BlueAni : MonoBehaviour
{
    public static BlueAni instance;
    public Animator anim;
    public BlueCharacterStats cs;
    public CPB coalSlider;
    public CPB waterSlider;
    public CPB metalSlider;
    

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

    // Start is called before the first frame update
    void Start()
    {
      anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
      if (Input.GetButtonDown("Horizontal")||Input.GetButtonDown("Vertical")){
        cs = BlueCharacterStats.Walk;
      }else if(Input.GetButtonUp("Horizontal")||Input.GetButtonUp("Vertical")){
        cs = BlueCharacterStats.Idle;
      }
      if(cs == BlueCharacterStats.Idle){
        anim.SetBool("walk",false);
      }
      if(cs == BlueCharacterStats.Walk){
        anim.SetBool("walk",true);
      }
    }
    public void updateResource(int _water, int _coal, int _metal)
    {
        waterSlider.UpdateAmount(_water);
        coalSlider.UpdateAmount(_coal);
        metalSlider.UpdateAmount(_metal);
    }
}
