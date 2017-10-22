using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerTurn : MonoBehaviour {

	// Use this for initialization
    enum Status
    {
        NotYourTrun = 0,
        WaitingChooseSkill ,
        WaitingChooseTarget,
        RunningToEnemy,
        RunningBack,
        Attacking
    };
    enum ProcessType
    {
        NormalAttack = 0,
        Skill
    }
    loadplayers loader;
    private Status m_status;
    private ProcessType m_type;
    private GameObject[] m_list;
    private int TargetIndex;
    private Animator ani;
    float Speed = 10.0f;
    int stop_frame_number;
    int current_frame_number;
    public Vector3 init_pos;
    FocusController focus;
    CameraControl camera_control;
    Vector3 enmey_height;
	void Start () {
        loader = GameObject.Find("BattleLoader").GetComponent<loadplayers>();
        m_status = Status.NotYourTrun;
        m_type = ProcessType.NormalAttack;
        TargetIndex = 0;
       // init_pos = transform.position;
        ani = GetComponent<Animator>();
        focus = GameObject.Find("FocusObject").GetComponent<FocusController>();
        camera_control = GameObject.Find("CameraController").GetComponent<CameraControl>();
        enmey_height = new Vector3(0, 1.0f, 0);
    }

    // Update is called once per frame
    private void Update()
    {
        if(m_status == Status.RunningToEnemy)
        {
            if (ani.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                ;
            else {
                transform.LookAt(m_list[TargetIndex].transform);
                if(current_frame_number<stop_frame_number)
                {
                    float tt = 1.0f*current_frame_number / stop_frame_number;
                    transform.position = Vector3.Lerp(init_pos,m_list[TargetIndex].transform.position,tt);
                    current_frame_number++;
                }
                //float dis = Vector3.Distance(transform.position, m_list[TargetIndex].transform.position);
                //if (dis > 1)
                //{
                //    //transform.
                //    transform.Translate(Vector3.forward * Speed * Time.deltaTime, Space.Self);
                //}
                else
                {
                    //ani.SetBool("Moving", false);
                    do_attack();
                }
            }
        }
        if(m_status == Status.RunningBack)
        {

            if (!ani.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                transform.LookAt(init_pos);
                if (current_frame_number < stop_frame_number)
                {
                    float tt = 1.0f*current_frame_number / stop_frame_number;
                    transform.position = Vector3.Lerp(m_list[TargetIndex].transform.position,init_pos,tt);
                    current_frame_number++;
                }
                //float dis = Vector3.Distance(transform.position, init_pos);
                //Debug.Log("init_pos :" + init_pos + " dis : " + dis);
                //if (dis > 0.01)
                //{
                //    transform.Translate(Vector3.forward * Speed * Time.deltaTime, Space.Self);
                //}
                else
                {
                    ani.SetBool("Moving", false);
                    ani.CrossFade("Idle", 0.1f);
                    m_status = Status.NotYourTrun;
                    transform.LookAt(m_list[TargetIndex].transform);
                    camera_control.reset_position();
                    loader.Battle();

                }
            }
        }
        
    }
    void OnGUI () {
        if(m_status == Status.WaitingChooseSkill)
        {
            if(GUI.Button(new Rect(Screen.width - 100, Screen.height - 100, 60, 40), "普通攻击"))
            {
                m_status = Status.WaitingChooseTarget;
                m_type = ProcessType.NormalAttack;
                while (TargetIndex >= m_list.Length) TargetIndex--;
                focus.Foucus(m_list[TargetIndex]);
            }
        }
        if(m_status == Status.WaitingChooseTarget)
        {
            if(GUI.Button(new Rect(20,Screen.height-100,60,40),"left"))
            {
                TargetIndex--;
                if (TargetIndex < 0) TargetIndex += m_list.Length;
                Debug.Log("target index " + TargetIndex);
                focus.Foucus(m_list[TargetIndex]);
            }
            if (GUI.Button(new Rect(100, Screen.height - 100, 60, 40), "Right"))
            {
                TargetIndex++;
                if (TargetIndex >= m_list.Length) TargetIndex = 0;
                Debug.Log("target index " + TargetIndex);
                focus.Foucus(m_list[TargetIndex]);
            }
            if (GUI.Button(new Rect(Screen.width - 100, Screen.height - 100, 60, 40), "确定"))
            {

                ani.SetBool("Moving",true);
                ani.CrossFade("Walk", 0.1f);
                focus.ClearFoucs();
                camera_control.CloseToPoint(m_list[TargetIndex].transform.position+enmey_height,4.0f,40);
                current_frame_number = 0;
                stop_frame_number = 30;
                m_status = Status.RunningToEnemy;

              //  do_attack();
            }
        }
		
	}
    public void process_turn(GameObject[] enemy_list)
    {
        m_list = enemy_list;

        m_status = Status.WaitingChooseSkill;
        GameObject obj = transform.Find("BackPlayerCamera").gameObject;
        camera_control.changeCamera(obj);
       // while (m_status != Status.Attacking) ;
    }
    void do_attack()
    {
        ani.SetTrigger("Attack");
       // ani.CrossFade("Attack", 0.1f);
        m_status = Status.Attacking;
       
        float attack_time = ani.GetCurrentAnimatorStateInfo(0).length;
        StartCoroutine(WaitingForAttack(attack_time));


    }
    IEnumerator WaitingForAttack(float _time)
    {
        yield return new WaitForSeconds(_time);
        //ani.SetBool("Moving", true);
        BattleUnit t_battle = m_list[TargetIndex].GetComponent<BattleUnit>();
        int ap = GetComponent<BattleUnit>().Attack;
        t_battle.doDamage(ap);
        ani.SetTrigger("Attack");
        Debug.Log("player : " +name+"do "+ ap+" damage to enemy "+ m_list[TargetIndex].name);
        current_frame_number = 0;
        stop_frame_number = 60;
        m_status = Status.RunningBack;
        camera_control.changeCamera("PlayerCamera");


    }
}
