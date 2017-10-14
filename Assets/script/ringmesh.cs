using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ringmesh : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        GetComponent<MeshFilter>().mesh = CreateRingMesh(1.0f,2.0f,360/10);
	}
    private void Start()
    {
        Awake();
    }
    List<Vector3> Vertex;
    List<Vector2> uv;
    List<int> triangles;
    Mesh CreateRingMesh(float inrad ,float outrad,int angle)
    {
        float tt = 2 * 3.141592653f / angle;
        Mesh m = new Mesh();
        m.name = "ScriptRingMesh";
        Vertex = new List<Vector3>();
        uv = new List<Vector2>();
        triangles = new List<int>();
        for(int i = 0;i<angle;++i)
        {
            float sin_t = Mathf.Sin(i * tt);
            float cos_t = Mathf.Cos(i * tt);
            Vector3 tt1 = new Vector3(inrad * sin_t+0.6f,0f,inrad * cos_t);
            Vector3 tt2 = new Vector3(outrad * sin_t, 0f, outrad * cos_t);
            Vertex.Add(tt1);Vertex.Add(tt2);
            uv.Add(new Vector2(0.0f,1.0f*i/angle));
            uv.Add(new Vector2(1.0f,1.0f*i/angle));
        }
        for(int i = 0;i<angle;++i)
        {
                triangles.Add(i * 2 + 1);
            if (i != angle - 1)
                triangles.Add((i + 1) * 2 + 1);
            else triangles.Add(1);
                triangles.Add(i * 2);

                triangles.Add(i * 2 );
                if (i != 0)
                    triangles.Add((i - 1) * 2 );
                else triangles.Add((angle - 1) * 2 );
                triangles.Add(i * 2 +1);
        }
        m.vertices = Vertex.ToArray();
        m.uv = uv.ToArray();
        m.triangles = triangles.ToArray();
        m.RecalculateNormals();
        m.RecalculateBounds();
        return m;
    }

	
	// Update is called once per frame
	void Update () {
		
	}
}
