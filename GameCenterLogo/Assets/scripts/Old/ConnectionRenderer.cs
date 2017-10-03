using UnityEngine;
using System.Collections;

public class ConnectionRenderer : MonoBehaviour {

//	Material mat;

	public Transform connectionHolder;

	public Transform end1;
	public Transform end2;

	GameObject[] nodes;

	Vector3 startRotation;
	Vector3 endRotation;

	float lerp = 0;
	const float CORE_TRANS_TIME = 0.5f;
	const float transitionTime = 8.5f;
	const float timeBetweenAnimations = transitionTime + CORE_TRANS_TIME;

	// Use this for initialization
	void Start () {

//		var shader = Shader.Find ("Custom/2DConnectionShader");
//		mat = new Material (shader);
//		mat.hideFlags = HideFlags.HideAndDontSave;
//
//		GetComponent<MeshRenderer>().material = mat;

		nodes = GameObject.FindGameObjectsWithTag("Nodes");

		startRotation = end1.rotation.eulerAngles;
		endRotation = end1.rotation.eulerAngles;
//		SelectNodeToRotateTo();

		InvokeRepeating(
			"SelectNodeToRotateTo",
			0,//timeBetweenAnimations - transitionTime,
			timeBetweenAnimations);
	}
	
	// Update is called once per frame
	void Update () {

		lerp += Time.deltaTime/transitionTime * Mathf.PI;

		lerp = Mathf.Clamp(lerp, -Mathf.PI/2f, Mathf.PI/2f);

		float lerpPer = MapInterval(Mathf.Sin(lerp), -1, 1, 0, 1);

		Quaternion newQuat = new Quaternion();
		newQuat.eulerAngles = Vector3.Lerp(startRotation, endRotation, lerpPer);

		end1.rotation = newQuat;
	}
//
//	bool t = false;
//	float prevLerpPer = 0;
//
	void SelectNodeToRotateTo(){
//		transitionTime = Random.value * CORE_TRANS_TIME + CORE_TRANS_TIME; 

//		prevLerpPer = Time.realtimeSinceStartup;
////		Debug.Log("---------start: " + Time.realtimeSinceStartup);
//		t = true;

		if(Random.Range(0, 4) < 1){
			return;
		}

		lerp = -Mathf.PI/2f;

		if(Random.Range(0, 2) < 1){
			SwapEnds();
		}

		Vector3 rotation = new Vector3(0, 0, 90);

		if(Random.Range(0, 3) < 1){
			rotation *= -1;
		}

		if(!HitNode(rotation)){
			rotation *= -1;
		}

		Vector3 newRotation = end1.rotation.eulerAngles + rotation;

		startRotation = end1.rotation.eulerAngles;
		endRotation = newRotation;
	}

	bool HitNode(Vector3 rotation){
		end1.Rotate(rotation);

		bool hitNode = false;

		foreach(GameObject go in nodes){
			if(Vector3.Distance(end2.transform.position, go.transform.position) < 0.1f){
				hitNode = true;
			}
		}
			
		end1.Rotate(-rotation);

		return hitNode;
	}

	void SwapEnds(){
		transform.parent = end2;
		end2.parent = connectionHolder;
		end1.parent = end2;

		Transform temp = end1;
		end1 = end2;
		end2 = temp;
	}

	Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles){
		Vector3 dir = point - pivot; // get point direction relative to pivot
		dir = Quaternion.Euler(angles) * dir; // rotate it
		point = dir + pivot; // calculate rotated point
		return point; // return it
	}

	float MapInterval(float val, float srcMin, float srcMax, float dstMin, float dstMax) {
		if (val>=srcMax) return dstMax;
		if (val<=srcMin) return dstMin;
		return dstMin + (val-srcMin) / (srcMax-srcMin) * (dstMax-dstMin);
	}
}

