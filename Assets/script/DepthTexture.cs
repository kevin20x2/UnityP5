using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof(Camera))]
public class DepthTexture : MonoBehaviour {

    // Use this for initialization
    private Camera myCamera;
    public Camera camera
    {
        get
        {
            if (myCamera == null)
                myCamera = GetComponent<Camera>();
            return myCamera;
        }
    }
    private Transform myCameraTransform;
    public Transform cameraTransform
    {
        get
        {
            if(myCameraTransform == null)
            {
                myCameraTransform = camera.transform;
            }
            return myCameraTransform;
        }
    }
    public Shader shader;
    private Material MyMaterial;
    public Material material
    {
        get
        {
            if(MyMaterial == null && shader != null)
            {
                MyMaterial = new Material(shader);
            }
            return MyMaterial;
        }
        set
        {
            MyMaterial = value;
        }
    }
    private Matrix4x4 _CurrentViewMatrix;
    private Matrix4x4 _PreviousViewMatrix;
    [Range(0.0f,0.001f)]
    public float blurSize = 0.005f;
    [Range(1, 10)]
    public int blurLevel = 1;
    [Range(0.0f, 1.0f)]
    public float _du = 0.5f;
    [Range(0.0f, 1.0f)]
    public float _dv = 0.5f;
	void Start () {
		
	}
    private void OnEnable()
    {
        camera.depthTextureMode |= DepthTextureMode.Depth;
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(material != null)
        {
            material.SetFloat("_BlurSize",blurSize);
            material.SetInt("_Level", blurLevel);
            material.SetFloat("_Du", _du);
            material.SetFloat("_Dv", _dv);

            Matrix4x4 frustumCorners = Matrix4x4.identity;

            float fov = camera.fieldOfView;
            float near = camera.nearClipPlane;
            float far = camera.farClipPlane;
            float aspect = camera.aspect;
            float halfHeight = near * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
            Vector3 toRight = cameraTransform.right * halfHeight * aspect;
            Vector3 toTop = cameraTransform.up * halfHeight;

            Vector3 topLeft = cameraTransform.forward * near + toTop - toRight;
            float scale = topLeft.magnitude / near;

            topLeft.Normalize();
            topLeft *= scale;

            Vector3 topRight = cameraTransform.forward * near + toTop + toRight;
            topRight.Normalize();
            topRight *= scale;

            Vector3 bottomLeft = cameraTransform.forward * near - toTop - toRight;
            bottomLeft.Normalize();
            bottomLeft *= scale;

            Vector3 bottomRight = cameraTransform.forward * near - toTop + toRight;
            bottomRight.Normalize();
            bottomRight *= scale;

            frustumCorners.SetRow(0,bottomLeft);
            frustumCorners.SetRow(1,bottomRight);
            frustumCorners.SetRow(2,topRight);
            frustumCorners.SetRow(3,topLeft);
            

            material.SetMatrix("_FrustumCornersRay", frustumCorners);
            _CurrentViewMatrix = (camera.projectionMatrix * camera.worldToCameraMatrix);
            material.SetMatrix("_PreviousViewProjectionMatrix", _PreviousViewMatrix);
            _PreviousViewMatrix = _CurrentViewMatrix;



            Graphics.Blit(source, destination,material);

            
        }
        else 
        Graphics.Blit(source, destination);
        
       
    }

    // Update is called once per frame
    void Update () {
		
	}
}
