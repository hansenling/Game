using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class selectbox : MonoBehaviour {
	private GameObject controller;
	private GameObject[] ground;
	private List<GameObject> boxes = new List<GameObject>();
	private List<GameObject> none = new List<GameObject>();
	// Use this for initialization
	void Start () {
		controller = GameObject.FindGameObjectWithTag ("Controller");

	}
	
	// Update is called once per frame
	void Update () {
		//print (transform.localPosition);
		Vector2 temp = GetComponent<BoxCollider2D> ().offset;
		temp.x = GetComponent<RectTransform> ().sizeDelta.x * .5f;
		temp.y = GetComponent<RectTransform> ().sizeDelta.y * .5f;
		Vector2 temp2 = GetComponent<BoxCollider2D> ().size;
		temp2.x = GetComponent<RectTransform> ().sizeDelta.x;
		temp2.y = GetComponent<RectTransform> ().sizeDelta.y;
		GetComponent<BoxCollider2D> ().offset = temp;
		GetComponent<BoxCollider2D> ().size = temp2; 

	}
	public List<GameObject> checkHit(){
		bool touch = false;
		List<int> notTouched = new List<int> ();
		if (Input.GetMouseButtonUp(0)){
			//ground = controller.GetComponent<interpolateLines> ().grounds;
			RectTransform rec = GetComponent<RectTransform>();
			ground = GameObject.FindGameObjectsWithTag("Ground");
			for (int i = 0; i < ground.Length; i++){
				if (GetComponent<Collider2D>().IsTouching(ground[i].GetComponent<Collider2D>())){
					//ground[i].GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f));
					GameObject temp = (ground[i].GetComponent<ground>().splitGround(rec.anchoredPosition, rec.sizeDelta.x * rec.localScale.x, rec.sizeDelta.y * rec.localScale.y));
					boxes.Add(temp);
					controller.GetComponent<interpolateLines> ().grounds.Remove (ground[i]);
					touch = true;
				} else{
					notTouched.Add(i);
				}
			}
			if (touch == false){
				return boxes;
			}
			for (int i = 0; i < notTouched.Count; i++){
					int retval = ground[notTouched[i]].GetComponent<ground>().deleteGround(rec.anchoredPosition, rec.sizeDelta.x * rec.localScale.x, rec.sizeDelta.y * rec.localScale.y);
					if (retval == 1){ //delete ground returns 1 if the ground object has been split
						controller.GetComponent<interpolateLines> ().grounds.Remove (ground[notTouched[i]]);		
					}
			}
			//RectTransform rec = GetComponent<RectTransform>();
			//float x1 = rec.anchoredPosition.x;
			//float x2 = rec.anchoredPosition.x + rec.sizeDelta.x * rec.localScale.x;

			GameObject.FindGameObjectWithTag("Controller").GetComponent<interpolateLines>().drawGround();
			return boxes;
		}
		return boxes;

	}
}
