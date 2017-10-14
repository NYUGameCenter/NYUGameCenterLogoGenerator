using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendCam : MonoBehaviour {

	Camera actualcamera;
	public Material newlogomat;
	RenderTexture tex;

	void Start () {
		actualcamera = GetComponent<Camera>();
		InitializeCamera();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void InitializeCamera() {


		tex = new RenderTexture(Screen.width, Screen.width, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
		tex.antiAliasing = 1;
		tex.wrapMode = TextureWrapMode.Clamp;
		tex.filterMode = FilterMode.Bilinear;
		tex.Create();
		actualcamera.targetTexture = tex;
		SnapRender();
		newlogomat.SetTexture("_MainTex", tex);
		//camera_display.camera_livescreen.texture = tex;
	}

	public void SnapRender() {
		Debug.Log("click!");
		actualcamera.enabled = true;
		actualcamera.Render();
		actualcamera.enabled = false;

	}
}
