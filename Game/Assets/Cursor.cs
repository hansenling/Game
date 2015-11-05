using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cursor : MonoBehaviour {
	private Vector3 mouseposition;
	public GameObject boxsprite;
	public GameObject currBox;
	private Vector3 start;
	private Vector3 end;
	private GameObject box;
	private List<GameObject> boxes;
	//private Shader shader = Shader.Find("Hidden/Internal-Colored");
	// Use this for initialization
	void Start () {

	}


	
	//public Material lineMat = new Material(shader);

	// Update is called once per frame
	void Update () {
		mouseposition = Camera.main.ScreenToWorldPoint(new Vector3 (Input.mousePosition.x,Input.mousePosition.y,0));
		transform.localPosition = new Vector2(mouseposition.x, mouseposition.y);
		//print (mouseposition);
		if (Input.GetMouseButtonDown (0)) {
			start = transform.localPosition;
			box = (GameObject)Instantiate (currBox, transform.localPosition, Quaternion.identity);
			box.GetComponent<RectTransform>().anchoredPosition = transform.localPosition;
			box.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0) ;
			//box.GetComponent<RectTransform>().rect = new Rect(new Vector2(start.x, start.y), 1, 1);
		}
		if (Input.GetMouseButton (0)) {
			end = transform.localPosition;
			RectTransform rect = box.GetComponent<RectTransform> ();
			//currBox.xMax = transform.localPosition.x;
			float x = transform.localPosition.x - start.x;
			float y = transform.localPosition.y - start.y;
			if (x < 0) {
				x = -x;
				Vector3 temp = rect.localScale;
				temp.x = -1;
				rect.localScale = temp;

			} else {
				Vector3 temp = rect.localScale;
				temp.x = 1;
				rect.localScale = temp;
			}
			if (y < 0) {
				y = -y;
				Vector3 temp = rect.localScale;
				temp.y = -1;
				rect.localScale = temp;
			} else {
				Vector3 temp = rect.localScale;
				temp.y = 1;
				rect.localScale = temp;	
			}

			box.GetComponent<RectTransform>().sizeDelta = new Vector2(x, y);


		}

		if (Input.GetMouseButtonUp(0)){
			boxes = box.GetComponent<selectbox>().checkHit();
			Destroy (box);
			//for (int i = 0; i < boxes.Count; i++){
			//	boxes[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0, 10);
			//}


		}

	}

}
