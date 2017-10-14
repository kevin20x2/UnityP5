using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodControl : MonoBehaviour {

    // Use this for initialization
    public int maxi_blood = 100;
    int target_blood;
    int current_blood;
    Material mat;
	void Start () {
        mat = GetComponent<MeshRenderer>().material;
        current_blood = maxi_blood;
     //   set_blood(50);
        
	}
    public void set_blood(int hp)
    {
        target_blood = hp;

    }
    public void change_object(GameObject enermy)
    {
        BattleUnit bu = enermy.GetComponent<BattleUnit>();
        maxi_blood = bu.init_hp;
        current_blood = bu.HP;
        target_blood = bu.HP;
        float percent = 1.0f * current_blood / maxi_blood;
        mat.SetFloat("_Percent", percent);

    }

    // Update is called once per frame
    void Update () {
        if(current_blood  != target_blood)
        {
            float percent = 1.0f * current_blood /maxi_blood;
            mat.SetFloat("_Percent",percent);
            if (current_blood < target_blood) current_blood += 1;
            else current_blood -= 1;
        }
		
	}
}
