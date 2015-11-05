using UnityEngine;
using System.Collections;

public class CameraTrack : MonoBehaviour {
	private GameObject player;
	private Vector3 currPosition;
	private bool reposition = false;
	public bool frontView = true;
	private GameObject[] grounds;
	private Vector2[] vertices = new Vector2[30];

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		grounds = GameObject.FindGameObjectsWithTag ("Ground");
		currPosition = new Vector3 (player.transform.localPosition.x, player.transform.localPosition.y, this.transform.localPosition.z);
		this.transform.localPosition = currPosition;
		getGroundVertices ();
	}
		
	//void OnPostRender() {
	//	GL.Begin(GL.LINES);
	//	GL.PushMatrix ();
	//	// Set transformation matrix for drawing to
	//	// match our transform
	//	GL.MultMatrix (transform.localToWorldMatrix);
	//	//lineMat.SetPass(0);
	//	GL.Color(new Color(0f, 0f, 0f, 1f));
	//	Vector2 first = Casteljau (vertices [0], vertices [1], vertices [2], vertices [3], 0f);
	//	Vector2 second = Casteljau (vertices [0], vertices [1], vertices [2], vertices [3], .5f);
	//	print (first);
	//	print (second);
	//	GL.Vertex3(1, 1,0f);
	//	GL.Vertex3(3, 3, 1f);
	//	GL.End();
	//	GL.PopMatrix ();
	//}

	void getGroundVertices(){
		for (int i = 0; i < grounds.Length;i++){
			vertices[i] = grounds[i].GetComponent<ground>().getTR();
			//print (grounds[i].GetComponent<Transform>().localPosition);
			//print (vertices[i]);
		}
	}

	// Update is called once per frame
	void Update () { 	
		if (player != null) {
			//print("AHHAHA");
			if (frontView == true) {
				if (Mathf.Abs (currPosition.x - player.transform.localPosition.x) > 2) {
					//print ("STUFF");
					reposition = true;
				}
				if (Mathf.Abs (player.transform.localPosition.y - currPosition.y) > 2) {
					reposition = true;	
					//print ("STUFF");
				}	
			} else {
				if (Mathf.Abs (currPosition.z - player.transform.localPosition.z) > 1) {
					//print ("ZZZZZZZZZZZ");
					reposition = true;
				}
				if (Mathf.Abs (player.transform.localPosition.y - currPosition.y) > 1) {
					//print ("YYYYYYYYYYYYY");
					reposition = true;	
				}
		
			}
			if (reposition == true) {
				if ((player.transform.localPosition - this.transform.localPosition).magnitude < 1) {
					GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
					reposition = false;
				}
				GetComponent<Rigidbody> ().velocity = getVelocity ();
			}
			if (Input.GetKeyDown ("space")) {
				frontView = !frontView;
				this.transform.RotateAround (player.transform.localPosition, Vector3.up, 90);
				currPosition = this.transform.localPosition;

			}
		}

	}

	Vector3 getVelocity(){
		Vector3 retVec = (player.transform.localPosition - this.transform.localPosition ).normalized * 30;	
		if (frontView) {
			return new Vector3 (retVec.x, retVec.y, 0);
		} else {
			return new Vector3(0, retVec.y, retVec.z);
		}

	}

}
