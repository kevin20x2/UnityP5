using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BattleUnit : MonoBehaviour  {

    // Use this for initialization
    public int init_hp;
    public int HP;
    public int SP;
    public int Speed;
    public int Attack;
    public int Defence;
    private bool is_dead;
    Animator ani;
    DamageController dc;

    void Start() {
        HP = init_hp;
        is_dead = false;
        ani = GetComponent<Animator>();
        dc = GameObject.Find("DamageShower").GetComponent<DamageController>();
    }

    // Update is called once per frame
    void Update() {

    }
    public void doDamage(int dp)
    {
        HP -= dp;
        if (HP <= 0)
        {
            is_dead = true;
            Debug.Log(name + "is dead..");
            ani.SetBool("Dead", true);
            gameObject.tag = "Dead";
        }
        else
        {
            ani.SetTrigger("Damaged");
            float damage_time = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
            StartCoroutine(waiting_damaged(damage_time));
        }

        dc.showNumber(dp,transform.position);

        
    }
    IEnumerator waiting_damaged(float time)
    {
        yield return new WaitForSeconds(time);

        ani.SetTrigger("Damaged");

    }
    public bool get_status()
    {
        return is_dead;
    }
}
public class BattleUnitComparer : IComparer <GameObject>
{
    public int Compare(GameObject x,GameObject y)
    {
        if(x.GetComponent<BattleUnit>() &&
            y.GetComponent<BattleUnit>() )
        {
            return y.GetComponent<BattleUnit>().Speed.CompareTo(x.GetComponent<BattleUnit>().Speed);
        }
        return 0;
    }
}
