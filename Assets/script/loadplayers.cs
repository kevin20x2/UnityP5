using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//[ExecuteInEditMode]
public class loadplayers : MonoBehaviour {
    public int player_number = 1;
    public int enemy_number ;
    private List <GameObject> player_list;
    private List <GameObject> enemy_list;
    private List <GameObject> unit_list;
    private int player_z_pos = -3;
    private int enemy_z_pos = 3;
    private CameraControl camera_controller;

	// Use this for initialization
	void Start () {
        camera_controller = GameObject.Find("CameraController").GetComponent<CameraControl>();
        camera_controller.changeCamera("PlayerCamera");
        init_players();
		
	}

    void init_players()
    {
        player_list = new List<GameObject>();
        enemy_list = new List<GameObject>();
        unit_list = new List<GameObject>();

        if(GameObject.Find("unitychan"))
        {
            player_list.Add(GameObject.Find("unitychan"));
            unit_list.Add(GameObject.Find("unitychan"));
        }

        foreach(GameObject m_player in player_list)
        {
            m_player.transform.position += new Vector3(0,0,player_z_pos);
            m_player.GetComponent<PlayerTurn>().init_pos = m_player.transform.position;
            //m_player.transform.position += new Vect
            
        }
        bool first = true;
        for(int i = 0;i<enemy_number;++i)
        {
            //Transform enemy_trans = ;
            //enemy_trans.position = new Vector3(0,0,enemy_z_pos);
            GameObject enemy;
            if (first)
            {
                enemy = GameObject.Find("zonbie");
                first = false;
            }
            else enemy = Instantiate(GameObject.Find("zonbie"));
            enemy.transform.position = new Vector3(i*1.5f, 0, enemy_z_pos);
            enemy.transform.forward = new Vector3(0,0,-1);
            enemy.GetComponent<EnemyAI>().init_pos = enemy.transform.position;
            enemy_list.Add(enemy);
            unit_list.Add(enemy);
        }
        //foreach(GameObject t in unit_list.OrderBy())
        unit_list.Sort(new BattleUnitComparer());
        Battle();
        
    }
    public void Battle()
    {
        GameObject[] t_player_list = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] t_enemy_list = GameObject.FindGameObjectsWithTag("enemy");
        if(0 == t_player_list.Length )
        {
            Debug.Log("player none !");
            return;
        }
        if(0 == t_enemy_list.Length)
        {
            Debug.Log("enemy none !");
            return;
        }
        GameObject current_object = unit_list[0];
        Debug.Log("current object : "+current_object.name + " tag" + current_object.tag);
        unit_list.Remove(current_object);
        BattleUnit status = current_object.GetComponent<BattleUnit>();
        if(!status.get_status()) // 没有阵亡
        {
            unit_list.Add(current_object);
            if (current_object.tag == "Player")
            {
                PlayerTurn turn = current_object.GetComponent<PlayerTurn>();
                turn.process_turn(t_enemy_list);
                //Battle();

            }
            else if("enemy" == current_object.tag)
            {
                // Debug.Log("enemy object : " + current_object.name + " active" + current_object.activeSelf);
                EnemyAI ai = current_object.GetComponent<EnemyAI>();
              //  ai.enabled = true;
                ai.process_turn(t_player_list);
                //Battle();
                
            }
            
        }
        else
        {

            // Battle(); 
            Battle();
        }

    }
    void find_target()
    {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
