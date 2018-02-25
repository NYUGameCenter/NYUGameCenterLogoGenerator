using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GradientRamp : MonoBehaviour {
	[Range(0f, 1f)]	public float amount = 1f;

	public Gradient gradientColors;
	public Gradient gradientColors2;

	Texture rampTexture;
	public Shader shader;
	Material _material;
	Material material {
		get {
			if (_material == null) {
				_material = new Material(shader);
				_material.hideFlags = HideFlags.HideAndDontSave;
			}

			return _material;
		}
	}
	void OnDisable() {
		if (_material)
			DestroyImmediate(_material);
	}

	public void UpdateGradient() {
		rampTexture = GradientToRamp(gradientColors) as Texture;
	}

	void Awake() {
		rampTexture = GradientToRamp(gradientColors) as Texture;
	}


	void OnRenderImage(RenderTexture source, RenderTexture destination) {
		if (rampTexture == null || amount == 0f) {
			Graphics.Blit(source, destination);
			return;
		}

		material.SetTexture("_RampTex", rampTexture);
		material.SetFloat("_Amount", amount);
		Graphics.Blit(source, destination, material);
	}

	Texture GradientToRamp(Gradient toconvert) {
		Texture2D texture = new Texture2D(256, 4, TextureFormat.RGB24, false, false);

		texture.wrapMode = TextureWrapMode.Clamp;

		Color[] colors = new Color[texture.width * texture.height];

		//SetPixels needs a continous set, laid out left to right, bottom to top:
		for (int j =0; j < texture.height; j++) {
			for (int i = 0; i < texture.width; i++) {
				colors[i + (texture.width * j)] = toconvert.Evaluate((float)i / (float)texture.width);
			}
		}

		texture.SetPixels(colors);
		texture.Apply();

		//var bytes = texture.EncodeToPNG();
		//File.WriteAllBytes("Assets/test.png", bytes);


		return texture;


	}


}
