using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
	public Rigidbody2D rb;
	public int speed = 100;
	public int numjumps = 4;
	private int jump = 1;
	public GameObject camera;
	// Use this for initialization
	void Start () {
		jump = numjumps;
		rb = GetComponent<Rigidbody2D> ();
		camera = GameObject.FindGameObjectWithTag ("MainCamera");
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey ("right")) {
			rb.AddForce (new Vector2(speed, 0));
			//print ("ADGADLGN");

		}
		
		if (Input.GetKey ("left")) {
			rb.AddForce (new Vector2(-speed, 0));
			//print("LEFT");
		}

		if (Input.GetKeyDown("up") && jump > 0){
			rb.velocity = new Vector2((float)(rb.velocity.x * .5), (float) rb.velocity.y);
			rb.AddForce(new Vector2(0, speed * 50));

			jump -= 1;
		}

		//if (GetComponent<Transform> ().localPosition.y < ground) {
		//	jump = numjumps;
		//	rb.velocity = new Vector2(rb.velocity.x, 0);
		//	rb.transform.localPosition = new Vector3(rb.transform.localPosition.x, ground, rb.transform.localPosition.z);
		//}

	}

	void OnCollisionEnter2D(Collision2D other){
		//print ("gggggg");
		//print (other.gameObject.tag);
		if (other.gameObject.tag == "Controller"){
			jump = numjumps;
		}
	}


}
