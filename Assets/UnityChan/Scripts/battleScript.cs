using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Animator))]
//[RequireComponent(typ)]

public class battleScript : MonoBehaviour {

	// Use this for initialization
    private Animator Anim;

    static int idle = Animator.StringToHash("Base Layer.Idle");
    static int jump = Animator.StringToHash("Base Layer.Jump");

	void Start () {
        Anim = GetComponent<Animator>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnGUI()
    {
        GUI.Box(new Rect(Screen.width - 200, Screen.height - 100, 200, 100), "Test");
        if(GUI.Button(new Rect(Screen.width-150,Screen.height - 80,80,40),"jump"))
        {
           // Anim.SetBool("Jump",true);
            Anim.CrossFade("Base Layer.Damage",0);
            Debug.Log("in click");
        }
        
    }
}
