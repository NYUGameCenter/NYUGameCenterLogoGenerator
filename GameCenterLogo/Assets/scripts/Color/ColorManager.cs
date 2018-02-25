using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour {

	public GradientRamp gradientrampeffect;


	public Gradient randomGradient;
	public float timeMultiplier = 1f;

	System.DateTime timebegan;
	float timeelapsed;

	// Use this for initialization
	void Start () {
		//https://medium.com/@bantic/hand-coding-a-color-wheel-with-canvas-78256c9d7d43

		timebegan = System.DateTime.Now;
		

	}




	[Range(0f,2f*Mathf.PI)] public float rand = 0;
	[Range(0f, Mathf.PI)]
	public float offset = 0;
	[Range(0f, 1f)]
	public float satur = 0;
	[Range(0f, 1f)]
	public float valu = 0;



	float[] randAngles = new float[4];

	void ColorsCalculate() {
		timeelapsed = timeMultiplier * (float)(System.DateTime.Now - timebegan).TotalSeconds;

		randAngles[0] = ((timeelapsed % (60f * 60f * 24f * 7f)) / (60f * 60f * 24f * 7f)) * (Mathf.PI * 2f);  //Random.Range(0, 2f * Mathf.PI);
		randAngles[1] = Mathf.Repeat((randAngles[0] + Mathf.PI), (2f * Mathf.PI));
		randAngles[2] = Mathf.Repeat((randAngles[1] + offset), (2f * Mathf.PI));
		randAngles[3] = Mathf.Repeat((randAngles[1] - offset), (2f * Mathf.PI));

		//randAngles[2] = (randAngles[1] + (Mathf.PI / 3f)) % (2f * Mathf.PI);
		//randAngles[3] = (randAngles[1] - (Mathf.PI / 3f)) % (2f * Mathf.PI);



		GradientCalculation();
	}

	void GradientCalculation() {
			Gradient g;
		GradientColorKey[] gck;
		GradientAlphaKey[] gak;
		g = new Gradient();
		gck = new GradientColorKey[4];

		gck[0].color = Color.black;
		gck[0].time = 0.0F;

		gck[1].color = new ColorExt.ColorHSV(randAngles[2] / (2f * Mathf.PI), satur, valu).ToColor();
		gck[1].time = 0.33f;

		gck[2].color = new ColorExt.ColorHSV(randAngles[3] / (2f * Mathf.PI), satur, valu).ToColor();
		gck[2].time = 0.66f;

		gck[3].color = new ColorExt.ColorHSV(randAngles[0] / (2f * Mathf.PI), satur, valu).ToColor();
		gck[3].time = 1.0F;



		gak = new GradientAlphaKey[2];
		gak[0].alpha = 1.0F;
		gak[0].time = 0.0F;
		gak[1].alpha = 1.0F;
		gak[1].time = 1.0F;
		g.SetKeys(gck, gak);


		randomGradient = g;
		gradientrampeffect.gradientColors = g;
		gradientrampeffect.UpdateGradient();
	}



	void Update() {
		ColorsCalculate();
	}


	//void OnDrawGizmos() {

	//	ColorExt.ColorHSV[] colors = new ColorExt.ColorHSV[4];
	//	for (int i = 0; i < 4; i++) {
	//		ColorExt.ColorHSV newcolor = new ColorExt.ColorHSV(randAngles[i] / (2f * Mathf.PI), satur, valu);
	//		Gizmos.color = newcolor.ToColor();
	//		Gizmos.DrawCube(new Vector3((float)i, 0, 0), Vector3.one);
	//		//if (i == 0) i++;
	//	}

	//}


	//Texture2D drawimage;
	//public MeshRenderer rend;

	//void DrawCircle() {

	//	drawimage = new Texture2D(500, 500, TextureFormat.ARGB32, false);
	//	//drawimage.filterMode = FilterMode.Point;
	//	drawimage.name = "DrawingDirectly";

	//	Color defaultcolor = Color.clear;


	//	Color[] defaultgrid = new Color[500 * 500];
	//	for (int i = 0; i < defaultgrid.Length; i++) {
	//		defaultgrid[i] = defaultcolor;

	//	}

	//	int radius = 250;
	//	for (int x = -radius; x < radius; x++) {
	//		for (int y = -radius; y < radius; y++) {
	//			Vector2 coord = xy2polarcoord(x, y);
	//			if (coord.x > radius) {
	//				continue;
	//			}
	//			float deg = coord.y * Mathf.Rad2Deg;
	//			int rowlength = 2 * radius;
	//			int adjustedx = x + radius; // convert x from [-50, 50] to [0, 100] (the coordinates of the image data array)
	//			int adjustedy = y + radius; // convert y from [-50, 50] to [0, 100] (the coordinates of the image data array)
	//			int pixelwidth = 1; // each pixel requires 4 slots in the data array
	//			int index = (adjustedx + (adjustedy * rowlength)) * pixelwidth;

	//			ColorExt.ColorHSV newcol = new ColorExt.ColorHSV((coord.y + Mathf.PI )/ (2f*Mathf.PI), coord.x / (float)radius, 1f);


	//			defaultgrid[index] = newcol.ToColor();
	//		}

	//	}



	//	drawimage.SetPixels(defaultgrid);
	//	drawimage.Apply();

	//	rend.material.SetTexture("_MainTex", drawimage);

	//}


	//Vector2 xy2polarcoord(int x, int y) {
	//	float r = Mathf.Sqrt((x * x) + (y * y));
	//	float phi = Mathf.Atan2(y, x);
	//	return new Vector2(r, phi);

	//}



}
