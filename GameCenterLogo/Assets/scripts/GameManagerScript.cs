using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManagerScript : MonoBehaviour {

	public const string LOGO_NAME = "LogoGrid";

	public float rotateTime;
	public float rotateSpeed;

	GameObject logoGrid;
	public float startTime = 0;

	public static bool rotateConnections = true;

	public bool rotateLogo = false;

	public Vector3[] initConnections;

	public static List<GameObject> nodes = new List<GameObject>();
	public static List<GameObject> connections = new List<GameObject>();

	public static GameManagerScript instance;

	//public RendCam rendcamlogo;

	// Use this for initialization
	void Start () {

		if(rotateLogo){
			rotateConnections = false;
		} else {
			rotateConnections = true;
		}

		instance = this;

		float sqrt2 = Mathf.Sqrt(2f);
	
		logoGrid = new GameObject(LOGO_NAME);
		logoGrid.transform.position = new Vector3(0,0,0);

		for(float x = -1; x <= 1; x++){
			for(float y = -1; y <= 1; y++){
				for(float z = -1; z <= 1; z++){
					Vector3 posIndex = new Vector3(x, y, z);

					GameObject sphere = Instantiate(Resources.Load("Sphere")) as GameObject;
					sphere.transform.parent = logoGrid.transform;
					sphere.transform.position = new Vector3(x * sqrt2, y * sqrt2, z * sqrt2);

					nodes.Add(sphere);

					for(int i = 0; i < initConnections.Length; i++){
						Vector3 conPos = initConnections[i];
//					foreach(Vector3 conPos in initConnections){
						if(conPos.Equals(posIndex)){

							GameObject connection = Instantiate(Resources.Load("Connection")) as GameObject;

							connection.transform.position = sphere.transform.position;

							connections.Add(connection);

							if(i >= 3){
								connection.GetComponent<ConnectionScript3D>().rot = true;
							}
						}
					}
				}
			}
		}

		transform.rotation = logoGrid.transform.rotation;

		RandomDir();
	}

	float timeDiff = 0;

	float prevSmoothDiff = 0;

	// Update is called once per frame
	void Update () {

		if(rotateLogo){
			if(!rotateConnections){

				timeDiff += Time.deltaTime/rotateSpeed;

				if (timeDiff <= 1)
				{
					//				Debug.Log(transform.eulerAngles  +  ": " + logoGrid.transform.eulerAngles);

					float smoothDiff = Mathf.SmoothStep(0, 1, timeDiff);

					logoGrid.transform.rotation = Quaternion.RotateTowards(
						logoGrid.transform.rotation, 
						transform.rotation, 
						(smoothDiff - prevSmoothDiff) * 90);

					prevSmoothDiff = smoothDiff;
				}

			} else if(ConnectionAnimationComplete()){
				rotateConnections = false;
			}

			if(timeDiff >= 1)
			{
				logoGrid.transform.rotation = transform.rotation;

				RandomDir();
				timeDiff = 0;
				prevSmoothDiff = 0;

				rotateConnections = true;
				setupConnectionsRotation();
			} 
		} else {
			if(ConnectionAnimationComplete()){
				//rendcamlogo.SnapRender();
				setupConnectionsRotation();
			}
		}
	}

	bool ConnectionAnimationComplete(){
		bool animComplete = true;

		foreach(GameObject con in connections){
			animComplete = animComplete && !con.GetComponent<ConnectionScript3D>().active;
		}

		return animComplete;
	}

	void setupConnectionsRotation(){
		print("setupConnectionsRotation");
		rotateConnections = true;
		foreach(GameObject con in connections){
			con.GetComponent<ConnectionScript3D>().SelectNodeToRotateTo();
		}
	}

	public void Run(){
		rotateLogo = true;
	}
	
	Quaternion RandomDir(){
		int i = Random.Range(0, 6);

//		Debug.Log (i);

		Vector3 result;

		switch(i){
		case 0:
			result = Vector3.forward;
			break;
		case 1:
			result =  Vector3.left;
			break;
		case 2:
			result =  Vector3.up;
			break;
		case 3:
			result =  Vector3.right;
			break;
		case 4:
			result =  Vector3.back;
			break;
		default:
			result =  Vector3.down;
			break;
		}

		transform.Rotate(result * 90);

		return transform.rotation;
	}
}
