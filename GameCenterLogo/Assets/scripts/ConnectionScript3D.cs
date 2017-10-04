using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionScript3D : MonoBehaviour {

	public float capPosition;

	public Transform connectionHolder;

	Transform end1;
	Transform end2;

	Vector3 startRotation;
	Vector3 endRotation;

	GameObject logoGrid;

	public bool active = false;

	float lerp = 0;

	public static int index = 0;
	public bool rot = false;

	// Use this for initialization
	void Start () {

		end1 = (Instantiate(Resources.Load("Sphere")) as GameObject).transform;
		end1.name = "ConnectionEnd1-" + index;
		end2 = (Instantiate(Resources.Load("Sphere")) as GameObject).transform;
		end2.name = "ConnectionEnd2-" + index;

		//The end caps don't need colliders right?
		end1.GetComponent<SphereCollider>().enabled = false;
		end2.GetComponent<SphereCollider>().enabled = false;


		end1.transform.position = transform.position;

		transform.parent = end1.transform;
		end2.parent = end1.transform;

		transform.localPosition = new Vector3(0, capPosition, 0);
		end2.localPosition = new Vector3(0, capPosition * 2, 0);

		logoGrid = GameObject.Find(GameManagerScript.LOGO_NAME);

		end1.transform.parent = logoGrid.transform;

		if(rot){
			end1.transform.localEulerAngles = new Vector3(0, 0, 90);
		}
			
		startRotation = end1.rotation.eulerAngles;
		endRotation = end1.rotation.eulerAngles;

		index++;

//		Invoke(
//			"SelectNodeToRotateTo",
//			GameManagerScript.instance.startTime);
	}
	
	// Update is called once per frame
	void Update () {
		if(GameManagerScript.rotateConnections){
			
			if (lerp <= 1){

				lerp += Time.deltaTime/GameManagerScript.instance.rotateSpeed;

				float lerpPer = Mathf.SmoothStep(0, 1, lerp);

				Quaternion newQuat = new Quaternion();
				newQuat.eulerAngles = Vector3.Lerp(startRotation, endRotation, lerpPer);

				end1.rotation = newQuat;
			} else {
				active = false;
				ToggleRendering(false);
			}
		}
	}


	void ToggleRendering(bool toggle) {
		end1.GetComponent<MeshRenderer>().enabled = toggle;
		end2.GetComponent<MeshRenderer>().enabled = toggle;
	}

	float MapInterval(float val, float srcMin, float srcMax, float dstMin, float dstMax) {
		if (val>=srcMax) return dstMax;
		if (val<=srcMin) return dstMin;
		return dstMin + (val-srcMin) / (srcMax-srcMin) * (dstMax-dstMin);
	}

	public void SelectNodeToRotateTo(){
		active = true;
		ToggleRendering(true);

		if (Random.Range(0, 4) < 1){
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

		lerp = 0;
	}

	bool HitNode(Vector3 rotation){
		end1.Rotate(rotation);

		bool hitNode = false;

		foreach(GameObject go in GameManagerScript.nodes){
			if(Vector3.Distance(end2.transform.position, go.transform.position) < 0.01f){
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

		end1.parent = logoGrid.transform;
	}
}
