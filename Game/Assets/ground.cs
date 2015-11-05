using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ground : MonoBehaviour {
	public Vector2 topLeft;
	public Vector2 topRight;
	public Vector2 bottomRight;
	public Vector2 bottomLeft;
	public GameObject quad; 
	public string getTag(){
		return gameObject.tag;
	}
	// Use this for initialization
	void Start () {
		float x = transform.localPosition.x;
		float y = transform.localPosition.y;
		float scalex = transform.localScale.x;
		float scaley = transform.localScale.y;
		topLeft = new Vector2 (x - .5f * scalex, y + .5f * scaley);
		topRight = new Vector2 (x + .5f * scalex, y + .5f * scaley);
		bottomRight = new Vector2 (x + .5f * scalex, y - .5f * scaley);
		bottomLeft = new Vector2 (x - .5f * scalex, y - .5f * scaley);
		if (GetComponent<Transform> ().localScale.x <= .06 || GetComponent<Transform>().localScale.y<=.06) {
			GameObject control = GameObject.FindGameObjectWithTag("Controller");
			control.GetComponent<interpolateLines> ().grounds.Remove (this.gameObject);
			Destroy(this.gameObject);
		}
	}
	public Vector2 getTL(){
		float x = transform.localPosition.x;
		float y = transform.localPosition.y;
		float scalex = transform.localScale.x;
		float scaley = transform.localScale.y;
		return new Vector2 (x - .5f * scalex, y + .5f * scaley);
	}
	public Vector2 getTR(){
		float x = transform.localPosition.x;
		float y = transform.localPosition.y;
		float scalex = transform.localScale.x;
		float scaley = transform.localScale.y;
		return new Vector2 (x + .5f * scalex, y + .5f * scaley);
	}
	public Vector2 getBR(){
		float x = transform.localPosition.x;
		float y = transform.localPosition.y;
		float scalex = transform.localScale.x;
		float scaley = transform.localScale.y;
		return new Vector2 (x + .5f * scalex, y - .5f * scaley);
	}
	public Vector2 getBL(){
		float x = transform.localPosition.x;
		float y = transform.localPosition.y;
		float scalex = transform.localScale.x;
		float scaley = transform.localScale.y;
		return new Vector2 (x - .5f * scalex, y - .5f * scaley);
	}

	// Update is called once per frame
	void Update () {
	
	}
	private Vector2[] initializeSelectBox(Vector2 anchor, float width, float height){
		Vector2 selectTL = new Vector2 (anchor.x, anchor.y + height);
		Vector2 selectTR = new Vector2 (anchor.x + width, anchor.y + height);
		Vector2 selectBR = new Vector2 (anchor.x + width, anchor.y);
		Vector2 selectBL = new Vector2 (anchor.x, anchor.y);
		if (width >= 0 && height >= 0) {
			selectTL = new Vector2 (anchor.x, anchor.y + height);
			selectTR = new Vector2 (anchor.x + width, anchor.y + height);
			selectBR = new Vector2 (anchor.x + width, anchor.y);
			selectBL = new Vector2 (anchor.x, anchor.y);
		}
		if (width >= 0 && height < 0) {
			selectTL = new Vector2 (anchor.x, anchor.y);
			selectTR = new Vector2 (anchor.x + width, anchor.y);
			selectBR = new Vector2 (anchor.x + width, anchor.y + height);
			selectBL = new Vector2 (anchor.x, anchor.y + height);
		}
		if (width < 0 && height < 0) {
			selectTL = new Vector2 (anchor.x + width, anchor.y);
			selectTR = new Vector2 (anchor.x, anchor.y);
			selectBR = new Vector2 (anchor.x, anchor.y + height);
			selectBL = new Vector2 (anchor.x + width, anchor.y + height);
		}
		if (width < 0 && height >= 0) {
			selectTL = new Vector2 (anchor.x + width, anchor.y + height);
			selectTR = new Vector2 (anchor.x, anchor.y + height);
			selectBR = new Vector2 (anchor.x, anchor.y);
			selectBL = new Vector2 (anchor.x + width, anchor.y);
		} 
		Vector2[] ret = new Vector2[4];
		ret [0] = selectTL;
		ret [1] = selectTR;
		ret [2] = selectBR;
		ret [3] = selectBL;
		return ret;

	}
	public int deleteGround(Vector2 anchor, float width, float height){
		Vector2[] temp = initializeSelectBox (anchor, width, height);
		Vector2 selectTL = temp [0];
		Vector2 selectTR = temp [1];
		Vector2 selectBR = temp [2];
		Vector2 selectBL = temp [3];
		if (selectTL.x > topLeft.x && selectTR.x < topRight.x && (selectTL.y < bottomLeft.y || selectBL.y > topLeft.y)) { //maybe could check that selectTL.x < topright.x and such
			GameObject left = (GameObject)Instantiate (this.gameObject, new Vector2 (topLeft.x + .5f * (selectTL.x - topLeft.x), transform.localPosition.y), Quaternion.identity);
			GameObject middle = (GameObject)Instantiate (this.gameObject, new Vector2 (selectTL.x + .5f * (selectTR.x - selectTL.x), transform.localPosition.y), Quaternion.identity);
			GameObject right = (GameObject)Instantiate (this.gameObject, new Vector2 (topRight.x - .5f * (topRight.x - selectTR.x), transform.localPosition.y), Quaternion.identity);
			Vector2 templ = left.GetComponent<Transform> ().localScale;
			Vector2 tempm = middle.GetComponent<Transform> ().localScale;
			Vector2 tempr = right.GetComponent<Transform> ().localScale;
			templ.x = selectTL.x - topLeft.x;
			tempm.x = selectTR.x - selectTL.x;
			tempr.x = topRight.x - selectTR.x;
			left.GetComponent<Transform> ().localScale = templ;
			middle.GetComponent<Transform> ().localScale = tempm;
			right.GetComponent<Transform> ().localScale = tempr;
			print (left.GetComponent<Transform>().position);
			print (middle.GetComponent<Transform>().position);
			print (right.GetComponent<Transform>().position);
			//could changed which sections are added to grounds
			GameObject control =  GameObject.FindGameObjectWithTag("Controller");
			control.GetComponent<interpolateLines>().grounds.Add(left);
			control.GetComponent<interpolateLines>().grounds.Add(middle);
			control.GetComponent<interpolateLines>().grounds.Add(right);

			Destroy(this.gameObject);
			return 1;
		} else if (selectTL.x > topLeft.x && selectTL.x < topRight.x && (selectTL.y < bottomLeft.y || selectBL.y > topLeft.y)) {
			print ("2");
			GameObject left = (GameObject)Instantiate (this.gameObject, new Vector2 (topLeft.x + .5f * (selectTL.x - topLeft.x), transform.localPosition.y), Quaternion.identity);
			GameObject right = (GameObject)Instantiate (this.gameObject, new Vector2 (topRight.x - .5f * (topRight.x - selectTL.x), transform.localPosition.y), Quaternion.identity);
			Vector2 templ = left.GetComponent<Transform> ().localScale;
			Vector2 tempr = right.GetComponent<Transform> ().localScale;
			templ.x = selectTL.x - topLeft.x;
			tempr.x = topRight.x - selectTL.x;
			left.GetComponent<Transform> ().localScale = templ;
			right.GetComponent<Transform> ().localScale = tempr;

			//could changed which sections are added to grounds
			GameObject control =  GameObject.FindGameObjectWithTag("Controller");
			control.GetComponent<interpolateLines>().grounds.Add(left);
			control.GetComponent<interpolateLines>().grounds.Add(right);
			print (left.GetComponent<Transform>().position);
			print (right.GetComponent<Transform>().position);
			//endGround (right, new List<GameObject> (){left});
			Destroy (this.gameObject);
			return 1;
		} else if (selectTR.x > topLeft.x && selectTR.x < topRight.x && (selectTL.y < bottomLeft.y || selectBL.y > topLeft.y)) {
			print ("3");
			GameObject left = (GameObject)Instantiate (this.gameObject, new Vector2 (topLeft.x + .5f * (selectTR.x - topLeft.x), transform.localPosition.y), Quaternion.identity);
			GameObject right = (GameObject)Instantiate (this.gameObject, new Vector2 (topRight.x - .5f * (topRight.x - selectTR.x), transform.localPosition.y), Quaternion.identity);
			Vector2 templ = left.GetComponent<Transform> ().localScale;
			Vector2 tempr = right.GetComponent<Transform> ().localScale;
			templ.x = selectTR.x - topLeft.x;
			tempr.x = topRight.x - selectTR.x;
			left.GetComponent<Transform> ().localScale = templ;
			right.GetComponent<Transform> ().localScale = tempr;
			print (left.GetComponent<Transform>().position);
			print (right.GetComponent<Transform>().position);
			//could changed which sections are added to grounds
			GameObject control =  GameObject.FindGameObjectWithTag("Controller");
			control.GetComponent<interpolateLines>().grounds.Add(left);
			control.GetComponent<interpolateLines>().grounds.Add(right);

			//endGround (left, new List<GameObject> (){right});
			Destroy (this.gameObject);
			return 1;
		} else {
			return 0;
		}

	}	
	public GameObject splitGround(Vector2 anchor, float width, float height){
		Vector2[] temp = initializeSelectBox (anchor, width, height);
		Vector2 selectTL = temp [0];
		Vector2 selectTR = temp [1];
		Vector2 selectBR = temp [2];
		Vector2 selectBL = temp [3];
		if (selectTL.y < topRight.y && selectTL.y > bottomLeft.y && selectTR.y < topRight.y && selectTR.y > bottomRight.y && selectTL.x > topLeft.x && selectTL.x < topRight.x && selectTR.x > topLeft.x && selectTR.x < topRight.x){
			GameObject left = (GameObject)Instantiate (this.gameObject, new Vector2 (topLeft.x + .5f * (selectTL.x - topLeft.x), transform.localPosition.y), Quaternion.identity);
			GameObject up = (GameObject)Instantiate (this.gameObject, new Vector2 (selectTL.x + .5f * (selectTR.x - selectTL.x), topLeft.y-.5f *(topLeft.y - selectTL.y)), Quaternion.identity);
			GameObject middle = (GameObject)Instantiate (this.gameObject, new Vector2 (selectTL.x + .5f * (selectTR.x - selectTL.x), selectTL.y-.5f *(selectTL.y - selectBL.y)), Quaternion.identity);
			GameObject down = (GameObject)Instantiate (this.gameObject, new Vector2 (selectTL.x + .5f * (selectTR.x - selectTL.x), bottomLeft.y + .5f * (selectBL.y - bottomLeft.y)), Quaternion.identity);
			GameObject right = (GameObject)Instantiate (this.gameObject, new Vector2 (topRight.x - .5f * (topRight.x - selectTR.x), transform.localPosition.y), Quaternion.identity);
			Vector2 templ = left.GetComponent<Transform> ().localScale;
			Vector2 tempu= up.GetComponent<Transform> ().localScale;
			Vector2 tempm = middle.GetComponent<Transform> ().localScale;
			Vector2 tempd = down.GetComponent<Transform> ().localScale;
			Vector2 tempr = right.GetComponent<Transform> ().localScale;

			templ.x = selectTL.x - topLeft.x;
			tempu.x = selectTR.x - selectTL.x;
			tempu.y = topLeft.y - selectTL.y;
			tempm.x = selectTR.x - selectTL.x;
			tempm.y = selectTR.y - selectBR.y;
			tempd.x = selectTR.x - selectTL.x;
			tempd.y = selectBL.y - bottomLeft.y;
			tempr.x = topRight.x - selectTR.x;
			left.GetComponent<Transform> ().localScale = templ;
			up.GetComponent<Transform> ().localScale = tempu;
			middle.GetComponent<Transform>().localScale = tempm;
			down.GetComponent<Transform> ().localScale = tempd;
			right.GetComponent<Transform> ().localScale = tempr;
			left.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			up.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			middle.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			down.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			right.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));

			endGround(middle, new List<GameObject> {left, up, right});
			Destroy (this.gameObject);
			return middle;
		}
		else if (selectTR.y < topRight.y && selectTR.y > bottomRight.y && selectTL.x > topLeft.x && selectTL.x < topRight.x && selectTR.x > topLeft.x && selectTR.x < topRight.x){
			print ("2");
			GameObject left = (GameObject)Instantiate (this.gameObject, new Vector2 (topLeft.x + .5f * (selectTL.x - topLeft.x), transform.localPosition.y), Quaternion.identity);
			GameObject up = (GameObject)Instantiate (this.gameObject, new Vector2 (selectTL.x + .5f * (selectTR.x - selectTL.x), topLeft.y-.5f *(topLeft.y - selectTL.y)), Quaternion.identity);
			GameObject down = (GameObject)Instantiate (this.gameObject, new Vector2 (selectTL.x + .5f * (selectTR.x - selectTL.x), bottomLeft.y + .5f * (selectTL.y - bottomLeft.y)), Quaternion.identity);
			GameObject right = (GameObject)Instantiate (this.gameObject, new Vector2 (topRight.x - .5f * (topRight.x - selectTR.x), transform.localPosition.y), Quaternion.identity);
			Vector2 templ = left.GetComponent<Transform> ().localScale;
			Vector2 tempu= up.GetComponent<Transform> ().localScale;
			Vector2 tempd = down.GetComponent<Transform> ().localScale;
			Vector2 tempr = right.GetComponent<Transform> ().localScale;
			templ.x = selectTL.x - topLeft.x;
			tempu.x = selectTR.x - selectTL.x;
			tempu.y = topLeft.y - selectTL.y;
			tempd.x = selectTR.x - selectTL.x;
			tempd.y = selectTL.y - bottomLeft.y;
			tempr.x = topRight.x - selectTR.x;
			left.GetComponent<Transform> ().localScale = templ;
			up.GetComponent<Transform> ().localScale = tempu;
			down.GetComponent<Transform> ().localScale = tempd;
			right.GetComponent<Transform> ().localScale = tempr;
			left.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			up.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			down.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			right.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));

			endGround(down, new List<GameObject>(){left, up, right});
			Destroy (this.gameObject);
			return down;
		}else if(selectTL.x > topLeft.x && selectTL.x < topRight.x && selectTL.y < topLeft.y && selectTL.y > bottomLeft.y && selectBL.y < topLeft.y && selectBL.y > bottomLeft.y){
			
			GameObject left = (GameObject)Instantiate (this.gameObject, new Vector2 (topLeft.x + .5f * (selectTL.x - topLeft.x), transform.localPosition.y), Quaternion.identity);
			GameObject up = (GameObject)Instantiate (this.gameObject, new Vector2 (topRight.x - .5f * (topRight.x - selectTL.x), topLeft.y-.5f *(topLeft.y - selectTL.y)), Quaternion.identity);
			GameObject middle = (GameObject) Instantiate (this.gameObject, new Vector2 (topRight.x - .5f * (topRight.x - selectTL.x), selectTL.y-.5f *(selectTL.y - selectBL.y)), Quaternion.identity);
			
			GameObject down = (GameObject)Instantiate (this.gameObject, new Vector2 (topRight.x - .5f * (topRight.x - selectTL.x), bottomLeft.y + .5f * (selectBL.y - bottomLeft.y)), Quaternion.identity);
			Vector2 templ = left.GetComponent<Transform> ().localScale;
			Vector2 tempu= up.GetComponent<Transform> ().localScale;
			Vector2 tempm = middle.GetComponent<Transform> ().localScale;
			Vector2 tempd = down.GetComponent<Transform> ().localScale;
			templ.x = selectTL.x - topLeft.x;
			tempu.x = topRight.x - selectTL.x;
			tempu.y = topRight.y - selectTL.y;
			tempm.x = topRight.x - selectTL.x;
			tempm.y = selectTL.y - selectBL.y;
			tempd.x = topRight.x - selectTL.x;
			tempd.y = selectBL.y - bottomLeft.y;
			left.GetComponent<Transform> ().localScale = templ;
			up.GetComponent<Transform> ().localScale = tempu;
			middle.GetComponent<Transform> ().localScale = tempm;
			down.GetComponent<Transform> ().localScale = tempd;


			left.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			up.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			middle.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			down.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));

			endGround(middle, new List<GameObject>(){left, up});
			Destroy (this.gameObject);
			return middle;
		}else if(selectTR.x > topLeft.x && selectTR.x < topRight.x && selectTL.y < topLeft.y && selectTL.y > bottomLeft.y && selectBL.y < topLeft.y && selectBL.y > bottomLeft.y){
			GameObject right = (GameObject)Instantiate (this.gameObject, new Vector2 (topRight.x - .5f * (topRight.x - selectTR.x), transform.localPosition.y), Quaternion.identity);
			GameObject up = (GameObject)Instantiate (this.gameObject, new Vector2 (topLeft.x + .5f * (selectTR.x - topLeft.x), topLeft.y-.5f *(topLeft.y - selectTL.y)), Quaternion.identity);
			GameObject middle = (GameObject) Instantiate (this.gameObject, new Vector2 (topLeft.x + .5f * (selectTR.x - topLeft.x), selectTL.y-.5f *(selectTL.y - selectBL.y)), Quaternion.identity);
			GameObject down = (GameObject)Instantiate (this.gameObject, new Vector2 (topLeft.x + .5f * (selectTR.x - topLeft.x), bottomLeft.y + .5f * (selectBL.y - bottomLeft.y)), Quaternion.identity);
			Vector2 tempr = right.GetComponent<Transform> ().localScale;
			Vector2 tempu= up.GetComponent<Transform> ().localScale;
			Vector2 tempm = middle.GetComponent<Transform> ().localScale;
			Vector2 tempd = down.GetComponent<Transform> ().localScale;
			tempr.x = topRight.x - selectTR.x;
			tempu.x = selectTR.x - topLeft.x;
			tempu.y = topRight.y - selectTL.y;
			tempm.x = selectTR.x - topLeft.x;
			tempm.y = selectTL.y - selectBL.y;
			tempd.x = selectTR.x - topLeft.x;
			tempd.y = selectBL.y - bottomLeft.y;
			right.GetComponent<Transform> ().localScale = tempr;
			up.GetComponent<Transform> ().localScale = tempu;
			middle.GetComponent<Transform> ().localScale = tempm;
			down.GetComponent<Transform> ().localScale = tempd;
			
			right.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			up.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			down.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			middle.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			endGround(middle, new List<GameObject>(){up, right});
			Destroy (this.gameObject);
			return middle;
		}else if(selectBR.y > bottomRight.y && selectBR.y < topRight.y && selectTL.x > topLeft.x && selectTL.x < topRight.x && selectTR.x > topLeft.x && selectTR.x < topRight.x){
			GameObject left = (GameObject)Instantiate (this.gameObject, new Vector2 (topLeft.x + .5f * (selectTL.x - topLeft.x), transform.localPosition.y), Quaternion.identity);
			GameObject up = (GameObject)Instantiate (this.gameObject, new Vector2 (selectTL.x + .5f * (selectTR.x - selectTL.x), topLeft.y-.5f *(topLeft.y - selectBL.y)), Quaternion.identity);
			GameObject down = (GameObject)Instantiate (this.gameObject, new Vector2 (selectTL.x + .5f * (selectTR.x - selectTL.x), bottomLeft.y + .5f * (selectBL.y - bottomLeft.y)), Quaternion.identity);
			GameObject right = (GameObject)Instantiate (this.gameObject, new Vector2 (topRight.x - .5f * (topRight.x - selectTR.x), transform.localPosition.y), Quaternion.identity);
			Vector2 templ = left.GetComponent<Transform> ().localScale;
			Vector2 tempu= up.GetComponent<Transform> ().localScale;
			Vector2 tempd = down.GetComponent<Transform> ().localScale;
			Vector2 tempr = right.GetComponent<Transform> ().localScale;
			templ.x = selectTL.x - topLeft.x;
			tempu.x = selectTR.x - selectTL.x;
			tempu.y = topLeft.y - selectBL.y;
			tempd.x = selectTR.x - selectTL.x;
			tempd.y = selectBL.y - bottomLeft.y;	
			tempr.x = topRight.x - selectTR.x;
			left.GetComponent<Transform> ().localScale = templ;
			up.GetComponent<Transform> ().localScale = tempu;
			down.GetComponent<Transform> ().localScale = tempd;
			right.GetComponent<Transform> ().localScale = tempr;
			left.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			up.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			down.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			right.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			Destroy (this.gameObject);
			endGround(up, new List<GameObject>(){left, down, right});
			return up;
		} else if (selectTL.x > topLeft.x && selectTL.x < topRight.x && selectTR.x > topLeft.x && selectTR.x < topRight.x) {
			GameObject left = (GameObject)Instantiate (this.gameObject, new Vector2 (topLeft.x + .5f * (selectTL.x - topLeft.x), transform.localPosition.y), Quaternion.identity);
			GameObject middle = (GameObject)Instantiate (this.gameObject, new Vector2 (selectTL.x + .5f * (selectTR.x - selectTL.x), transform.localPosition.y), Quaternion.identity);
			GameObject right = (GameObject)Instantiate (this.gameObject, new Vector2 (topRight.x - .5f * (topRight.x - selectTR.x), transform.localPosition.y), Quaternion.identity);
			Vector2 templ = left.GetComponent<Transform> ().localScale;
			Vector2 tempm = middle.GetComponent<Transform> ().localScale;
			Vector2 tempr = right.GetComponent<Transform> ().localScale;
			templ.x = selectTL.x - topLeft.x;
			tempm.x = selectTR.x - selectTL.x;
			tempr.x = topRight.x - selectTR.x;
			left.GetComponent<Transform> ().localScale = templ;
			middle.GetComponent<Transform> ().localScale = tempm;
			right.GetComponent<Transform> ().localScale = tempr;
			left.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			middle.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			right.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			endGround(middle, new List<GameObject>(){left, right});
			Destroy (this.gameObject);
			return middle;
		} else if (selectTL.y < topLeft.y && selectTL.y > bottomLeft.y && selectBL.y < topLeft.y && selectBL.y > bottomLeft.y) {
			GameObject up = (GameObject)Instantiate (this.gameObject, new Vector2 (transform.localPosition.x, topLeft.y - .5f * (topLeft.y - selectTL.y)), Quaternion.identity);
			GameObject middle = (GameObject)Instantiate (this.gameObject, new Vector2 (transform.localPosition.x, selectTL.y - .5f * (selectTL.y - selectBL.y)), Quaternion.identity);
			GameObject down = (GameObject)Instantiate (this.gameObject, new Vector2 (transform.localPosition.x, bottomLeft.y + .5f * (selectBL.y - bottomLeft.y)), Quaternion.identity);
			Vector2 tempu = up.GetComponent<Transform> ().localScale;	
			Vector2 tempm = middle.GetComponent<Transform> ().localScale;
			Vector2 tempd = down.GetComponent<Transform> ().localScale;
			tempu.y = topLeft.y - selectTL.y;
			tempm.y = selectTL.y - selectBL.y;
			tempd.y = selectBL.y - bottomLeft.y;
			up.GetComponent<Transform> ().localScale = tempu;
			middle.GetComponent<Transform> ().localScale = tempm;
			down.GetComponent<Transform> ().localScale = tempd;
			up.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			middle.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			down.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			endGround(middle,new List<GameObject>(){up} );
			Destroy (this.gameObject);
			return middle;
			//first corner
		} else if (selectTL.x > topLeft.x && selectTL.x < topRight.x && selectTL.y < topLeft.y && selectTL.y > bottomLeft.y) {												
			GameObject up = (GameObject)Instantiate (this.gameObject, new Vector2 (topRight.x - .5f * (topRight.x - selectTL.x), topRight.y - .5f * (topRight.y - selectTR.y)), Quaternion.identity);
			GameObject down = (GameObject)Instantiate (this.gameObject, new Vector2 (topRight.x - .5f * (topRight.x - selectTL.x), bottomRight.y + .5f * (selectTR.y - bottomRight.y)), Quaternion.identity);
			GameObject left = (GameObject)Instantiate (this.gameObject, new Vector2 (topLeft.x + .5f * (selectTL.x - topLeft.x), transform.localPosition.y), Quaternion.identity);
			Vector2 tempu = up.GetComponent<Transform> ().localScale;	
			Vector2 tempd = down.GetComponent<Transform> ().localScale;
			Vector2 templ = left.GetComponent<Transform> ().localScale;
			tempu.x = topRight.x - selectTL.x;
			tempu.y = topRight.y - selectTL.y;
			tempd.x = topRight.x - selectTL.x;
			tempd.y = selectTL.y - bottomLeft.y;
			templ.x = selectTL.x - topLeft.x;
			up.GetComponent<Transform> ().localScale = tempu;
			down.GetComponent<Transform> ().localScale = tempd;
			left.GetComponent<Transform> ().localScale = templ;
			up.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			down.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			left.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			endGround(down, new List<GameObject>(){left, up});
			Destroy (this.gameObject);
			return down;
			//second corner
		} else if (selectTR.x > topLeft.x && selectTR.x < topRight.x && selectTL.y < topLeft.y && selectTL.y > bottomLeft.y) {
			GameObject up = (GameObject)Instantiate (this.gameObject, new Vector2 (topLeft.x + .5f * (selectTR.x - topLeft.x), topRight.y - .5f * (topRight.y - selectTR.y)), Quaternion.identity);
			GameObject down = (GameObject)Instantiate (this.gameObject, new Vector2 (topLeft.x + .5f * (selectTR.x - topLeft.x), bottomRight.y + .5f * (selectTR.y - bottomRight.y)), Quaternion.identity);
			GameObject right = (GameObject)Instantiate (this.gameObject, new Vector2 (topRight.x - .5f * (topRight.x - selectTR.x), transform.localPosition.y), Quaternion.identity);
			Vector2 tempu = up.GetComponent<Transform> ().localScale;	
			Vector2 tempd = down.GetComponent<Transform> ().localScale;
			Vector2 templ = right.GetComponent<Transform> ().localScale;
			tempu.x = selectTR.x - topLeft.x;
			tempu.y = topLeft.y - selectTR.y;
			tempd.x = selectTR.x - topLeft.x;
			tempd.y = selectTR.y - bottomLeft.y;
			templ.x = topRight.x - selectTR.x;
			up.GetComponent<Transform> ().localScale = tempu;
			down.GetComponent<Transform> ().localScale = tempd;
			right.GetComponent<Transform> ().localScale = templ;
			up.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			down.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			right.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			endGround(down, new List<GameObject>(){up, right});
			Destroy (this.gameObject);
			return down;
			//third corner
		} else if (selectTR.x > topLeft.x && selectTR.x < topRight.x && selectBL.y < topLeft.y && selectBL.y > bottomLeft.y) {
			GameObject up = (GameObject)Instantiate (this.gameObject, new Vector2 (topLeft.x + .5f * (selectTR.x - topLeft.x), topLeft.y - .5f * (topRight.y - selectBR.y)), Quaternion.identity);
			GameObject down = (GameObject)Instantiate (this.gameObject, new Vector2 (topLeft.x + .5f * (selectTR.x - topLeft.x), bottomLeft.y + .5f * (selectBR.y - bottomLeft.y)), Quaternion.identity);
			GameObject left = (GameObject)Instantiate (this.gameObject, new Vector2 (topRight.x - .5f * (topRight.x - selectTR.x), transform.localPosition.y), Quaternion.identity);
			Vector2 tempu = up.GetComponent<Transform> ().localScale;	
			Vector2 tempd = down.GetComponent<Transform> ().localScale;
			Vector2 templ = left.GetComponent<Transform> ().localScale;
			tempu.x = selectBR.x - topLeft.x;
			tempu.y = topLeft.y - selectBR.y;
			tempd.x = selectBR.x - topLeft.x;
			tempd.y = selectBR.y - bottomLeft.y;
			templ.x = topRight.x - selectBR.x;
			up.GetComponent<Transform> ().localScale = tempu;
			down.GetComponent<Transform> ().localScale = tempd;
			left.GetComponent<Transform> ().localScale = templ;
			up.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			down.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			left.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			endGround(up, new List<GameObject>(){left, down});
			Destroy (this.gameObject);
			return up;
			//fourth corner
		} else if (selectTL.x > topLeft.x && selectTL.x < topRight.x && selectBL.y < topLeft.y && selectBL.y > bottomLeft.y) {
			GameObject up = (GameObject)Instantiate (this.gameObject, new Vector2 (topRight.x - .5f * (topRight.x - selectBL.x), topRight.y - .5f * (topRight.y - selectBR.y)), Quaternion.identity);
			GameObject down = (GameObject)Instantiate (this.gameObject, new Vector2 (topRight.x - .5f * (topRight.x - selectBL.x), bottomRight.y + .5f * (selectBR.y - bottomRight.y)), Quaternion.identity);
			GameObject right = (GameObject)Instantiate (this.gameObject, new Vector2 (topLeft.x + .5f * (selectTL.x - topLeft.x), transform.localPosition.y), Quaternion.identity);
			Vector2 tempu = up.GetComponent<Transform> ().localScale;	
			Vector2 tempd = down.GetComponent<Transform> ().localScale;
			Vector2 templ = right.GetComponent<Transform> ().localScale;
			tempu.x = topRight.x - selectBL.x;
			tempu.y = topRight.y - selectBL.y;
			tempd.x = topRight.x - selectBL.x;
			tempd.y = selectBL.y - bottomLeft.y;
			templ.x = selectBL.x - topLeft.x;
			up.GetComponent<Transform> ().localScale = tempu;
			down.GetComponent<Transform> ().localScale = tempd;
			right.GetComponent<Transform> ().localScale = templ;
			up.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			down.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			right.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			endGround(up, new List<GameObject>(){down, right});
			Destroy (this.gameObject);
			return up;
		} else if (selectTL.x > topLeft.x && selectTL.x < topRight.x) {
			GameObject left = (GameObject)Instantiate (this.gameObject, new Vector2 (topLeft.x + .5f * (selectTL.x - topLeft.x), transform.localPosition.y), Quaternion.identity);
			GameObject right = (GameObject)Instantiate (this.gameObject, new Vector2 (topRight.x - .5f * (topRight.x - selectTL.x), transform.localPosition.y), Quaternion.identity);
			Vector2 templ = left.GetComponent<Transform> ().localScale;
			Vector2 tempr = right.GetComponent<Transform> ().localScale;
			templ.x = selectTL.x - topLeft.x;
			tempr.x = topRight.x - selectTL.x;
			left.GetComponent<Transform> ().localScale = templ;
			right.GetComponent<Transform> ().localScale = tempr;
			left.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			right.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			endGround(right, new List<GameObject>(){left});
			Destroy (this.gameObject);
			return right;
		} else if (selectTR.x > topLeft.x && selectTR.x < topRight.x) {
			GameObject left = (GameObject)Instantiate (this.gameObject, new Vector2 (topLeft.x + .5f * (selectTR.x - topLeft.x), transform.localPosition.y), Quaternion.identity);
			GameObject right = (GameObject)Instantiate (this.gameObject, new Vector2 (topRight.x - .5f * (topRight.x - selectTR.x), transform.localPosition.y), Quaternion.identity);
			Vector2 templ = left.GetComponent<Transform> ().localScale;
			Vector2 tempr = right.GetComponent<Transform> ().localScale;
			templ.x = selectTR.x - topLeft.x;
			tempr.x = topRight.x - selectTR.x;
			left.GetComponent<Transform> ().localScale = templ;
			right.GetComponent<Transform> ().localScale = tempr;
			left.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			right.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			endGround(left, new List<GameObject>(){right});
			Destroy(this.gameObject);
			return left;
		} else if (selectTL.y < topLeft.y && selectTL.y > bottomLeft.y) {
			GameObject up = (GameObject)Instantiate (this.gameObject, new Vector2 (transform.localPosition.x, topLeft.y - .5f * (topLeft.y - selectTL.y)), Quaternion.identity);
			GameObject down = (GameObject)Instantiate (this.gameObject, new Vector2 (transform.localPosition.x, bottomLeft.y + .5f * (selectTL.y - bottomLeft.y)), Quaternion.identity);
			Vector2 tempu = up.GetComponent<Transform> ().localScale;
			Vector2 tempd = down.GetComponent<Transform> ().localScale;
			tempu.y = topLeft.y - selectTL.y;
			tempd.y = selectTL.y - bottomLeft.y;
			up.GetComponent<Transform> ().localScale = tempu;
			down.GetComponent<Transform> ().localScale = tempd;
			up.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			down.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			endGround(down, new List<GameObject>(){up});
			Destroy (this.gameObject);
			return down;
		} else if (selectBL.y < topLeft.y && selectBL.y > bottomLeft.y) {
			GameObject up = (GameObject)Instantiate (this.gameObject, new Vector2 (transform.localPosition.x, topLeft.y - .5f * (topLeft.y - selectBL.y)), Quaternion.identity);
			GameObject down = (GameObject)Instantiate (this.gameObject, new Vector2 (transform.localPosition.x, bottomLeft.y + .5f * (selectBL.y - bottomLeft.y)), Quaternion.identity);
			Vector2 tempu = up.GetComponent<Transform> ().localScale;	
			Vector2 tempd = down.GetComponent<Transform> ().localScale;
			tempu.y = topLeft.y - selectBL.y;
			tempd.y = selectBL.y - bottomLeft.y;
			up.GetComponent<Transform> ().localScale = tempu;
			down.GetComponent<Transform> ().localScale = tempd;
			up.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			down.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));
			endGround(up, new List<GameObject>(){down});

			Destroy (this.gameObject);
			return up;
		} 
		endGround (gameObject, new List<GameObject>());
		return this.gameObject;
	}
	public void invokeStop(){
		Invoke ("stopRise", 3);
	}
	void stopRise(){
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
	}
	void endGround(GameObject retQuad, List<GameObject> groundparts){
		gameObject.tag = "Untagged";
		retQuad.gameObject.tag = "floatingGround";
		retQuad.gameObject.GetComponent<Rigidbody2D> ().gravityScale = 0;
		retQuad.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 10);
		retQuad.gameObject.layer = 11;
		//Destroy (retQuad); // eventually gonna throw this, not destroy
		retQuad.GetComponent<ground> ().invokeStop ();
		GameObject control = GameObject.FindGameObjectWithTag("Controller");
		control.GetComponent<interpolateLines> ().grounds.Remove (this.gameObject);
		for (int i= 0; i < groundparts.Count;i++){
			control.GetComponent<interpolateLines>().grounds.Add(groundparts[i]);
		}
	}

}
