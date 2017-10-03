using UnityEngine;
using System.Collections;

public class ConnectionScript : MonoBehaviour {
	
	const string SHADER_CENTER   = "_Center";
	Material mat;

	// Use this for initialization
	void Start () {
		mat = GetComponent<MeshRenderer>().material;
	}
	
	// Update is called once per frame
	void Update () {

		Vector4 vec4 = new Vector4(transform.position.x,
		                           transform.position.y,
		                           transform.position.z,
		                           0.71f);

		mat.SetVector(SHADER_CENTER, vec4);
	}
}
