using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    // Use this for initialization
    enum Status
    {
        Waiting = 0,
        RunningToplayer ,
        Attacking,
        RunningBack
    };
    loadplayers Loader;
    GameObject TargetPlayer;
    CameraControl camera_control;
    Status m_status;
    public Vector3 init_pos;
    float Speed = 10.0f;
    private void Awake()
    {
        m_status = Status.Waiting;
        init_pos = transform.position;
        Loader = GameObject.Find("BattleLoader").GetComponent<loadplayers>();
        Debug.Log("awake " + name);
        camera_control = GameObject.Find("CameraController").GetComponent<CameraControl>();
        
    }
    void Start () {
        
        Debug.Log("start " + name);
		
	}
    public int find_attack_player(GameObject[] player_list)
    {
        int  targetIndex = Random.Range(0, player_list.Length);
        return targetIndex;
    }
    public void process_turn(GameObject[] player_list)
    {
        Debug.Log("in process turn" + player_list.Length);
        int index = find_attack_player(player_list);
        TargetPlayer = player_list[index];
        Debug.Log("in process turn" + TargetPlayer.tag);
        m_status = Status.RunningToplayer;
       // Update();
        

        
    }
	
    void LaunchAttack()
    {
        
        camera_control.changeCamera(TargetPlayer.transform.Find("FrontPlayerCamera").gameObject);
    //    GetComponent<Animation>().CrossFade("Attack",0.1f);
        GetComponent<Animator>().SetBool("Attack",true);
        GetComponent<Animator>().SetTrigger("Attacking");
      
        float animation_time = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;

        StartCoroutine(WaitForRunBack(animation_time));

    }
    IEnumerator WaitForRunBack(float time)
    {
        yield return new WaitForSeconds(time);
        GetComponent<Animator>().SetBool("Attack",false);
        GetComponent<Animator>().SetTrigger("Attacking");
        BattleUnit t_battle = TargetPlayer.GetComponent<BattleUnit>();
        t_battle.doDamage(t_battle.Attack + Random.Range(-5,5));
        //Debug.Log(tag +"do 1 Damage to "+TargetPlayer.tag);


        float animation_time = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        StartCoroutine(WaitForAttack(animation_time));


    }
    IEnumerator WaitForAttack(float time)
    {
        yield return new WaitForSeconds(time);
        m_status = Status.RunningBack;
        camera_control.changeCamera("PlayerCamera");
    }
	// Update is called once per frame
	void Update () {
            //Debug.Log("dis" );
        if(m_status == Status.RunningToplayer)
        {
            if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Damaged"))
                Debug.Log("falling down");
            else
            {
                transform.LookAt(TargetPlayer.transform);
                float dis = Vector3.Distance(transform.position, TargetPlayer.transform.position);
                //  Debug.Log("dis" + dis);
                if (dis > 1)
                {
                    GetComponent<Animator>().SetBool("is_move", true);
                    transform.Translate(Vector3.forward * Speed * Time.deltaTime, Space.Self);
                }
                else
                {
                    GetComponent<Animator>().SetBool("is_move", false);
                    m_status = Status.Attacking;
                    LaunchAttack();
                }
            }
        }
        if(m_status == Status.RunningBack)
        {
            transform.LookAt(init_pos);
            float dis = Vector3.Distance(transform.position, init_pos);
            if(dis > 0.1)
            {

                transform.Translate(Vector3.forward * Speed * Time.deltaTime,Space.Self);
                GetComponent<Animator>().SetBool("is_move", true);
            }
            else
            {
                transform.LookAt(init_pos - Vector3.forward);
                //transform.LookAt(TargetPlayer.transform.position);
                m_status = Status.Waiting;
                Loader.Battle();

            }
        }
		
	}
}
