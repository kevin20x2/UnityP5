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
    EffectControl ec;

    void Start() {
        HP = init_hp;
        is_dead = false;
        ani = GetComponent<Animator>();
        dc = GameObject.Find("DamageShower").GetComponent<DamageController>();
        ec = GameObject.Find("DamageShower").GetComponent<EffectControl>();
    }

    // Update is called once per frame
    void Update() {

    }
    public void doDamage(int dp)
    {
        HP -= dp;
        Debug.Log(gameObject.tag + "get " + dp + "damage..");
        if (HP <= 0)
        {
            is_dead = true;
            Debug.Log(name + "is dead..");
            ani.SetBool("Dead", true);
            gameObject.tag = "Dead";
        }
        else
        {
            ani.SetBool("Damaged",true);
            float damage_time = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
            StartCoroutine(waiting_damaged(damage_time));
        }

        dc.showNumber(dp,transform.position);
        dc.show_damage(gameObject,init_hp,HP+dp,HP);
        ec.show_damage_effect(transform.position);

        
    }
    IEnumerator waiting_damaged(float time)
    {
        yield return new WaitForSeconds(time);
        ani.SetBool("Laying", true);

        ani.SetBool("Damaged",false);

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
