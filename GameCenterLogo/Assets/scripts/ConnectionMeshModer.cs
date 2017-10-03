using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionMeshModer : MonoBehaviour {

	public Vector3 pos;
	public Vector3 endCap;
	public float dist = 1;
	public float step = 0.1f;

	// Use this for initialization
	void Start () {		
//		Mesh mesh = GetComponent<MeshFilter>().mesh;
//		Vector3[] vertices = mesh.vertices;
//		int i = 0;
//
//		while (i < vertices.Length) {
//			vertices[i] += Vector3.up * Time.deltaTime;
//			i++;
//		}
//		mesh.vertices = vertices;
//		mesh.RecalculateBounds();
	}
	
	// Update is called once per frame
	void Update () {

//		Mesh mesh = GetComponent<MeshFilter>().mesh;
//		Vector3[] vertices = mesh.vertices;
//		int i = 0;
//
//		while (i < vertices.Length) {
//			vertices[i] += Vector3.up * Time.deltaTime;
//			i++;
//		}
//		mesh.vertices = vertices;
//		mesh.RecalculateBounds();



		MeshFilter meshFilter = GetComponent<MeshFilter>();
		Mesh mesh = meshFilter.mesh;
		Vector3[] vertices = mesh.vertices;

		int i = 0;

		float mag = pos.magnitude;

		while (i < vertices.Length) {

//			foreach(Vector3 pos in posList){
//			for(int k = 0; k < posList.Length; k++){
//				Vector3 pos = posList[k];

				int j = 0;

				for(float f = 0; f < Mathf.PI*2; f+= Mathf.PI*2/64f){

					pos.x = Mathf.Sin(f) * mag;
					pos.z = Mathf.Cos(f) * mag;

//					Debug.Log("0rot pos: " + pos);
//					Debug.Log("x: " + Mathf.Sin(f));
//					Debug.Log("y: " + Mathf.Cos(f));

//				Vector2 temp1 = new Vector2(pos.x, pos.y);
//				Vector2 temp2 = new Vector2(vertices[i].x, vertices[i].y);

//				if(Vector2.Distance(temp1, temp2) < dist && j < 10){
				while(Vector3.Distance(vertices[i], pos) < dist){

						Vector3 dir = (Vector3.zero  + vertices[i]).normalized;

//						Debug.Log("frame: " + Time.frameCount + " i: " + i + " "  + dir + " : " + vertices[i]);

						vertices[i].Set(
							vertices[i].x - dir.x * step, 
							vertices[i].y - dir.y * step, 
							vertices[i].z - dir.z * step);

						j++;

					}
			}

			i++;
		}

//		endPa.x = Mathf.Sin(90) * mag;
//		pos.y = Mathf.Cos(90) * mag;

//		i = 0;
//		while (i < vertices.Length) {
//			if(Vector3.Distance(vertices[i], endCap) < dist){
//
//				Vector3 dir = (Vector3.zero  + vertices[i]).normalized;
//
//				vertices[i].Set(
//					vertices[i].x - dir.x * step, 
//					vertices[i].y - dir.y * step, 
//					vertices[i].z - dir.z * step);
//			}
//		}

		mesh.vertices = vertices;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
	}
}
