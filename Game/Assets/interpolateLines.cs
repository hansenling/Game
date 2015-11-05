using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class interpolateLines : MonoBehaviour {
	private LineRenderer lines;
	public List<GameObject> grounds = new List<GameObject>();
	private List<List<Vector2>> vertices = new List<List<Vector2>> ();
	private GameObject[] tempgrounds;
	private bool thing = false;
	private float lastposition = -10;
	private float bottom = -5;
	public GameObject groundobject;
	public GameObject boundaryobject;
	private GameObject camera;
	private EdgeCollider2D boundary;
	public GameObject[] ground;
	private bool initialized = false;
	public List<GameObject> removeground;
	void Start () {
		boundary = GetComponent<EdgeCollider2D> ();
		camera = GameObject.FindGameObjectWithTag ("MainCamera");
		lines = this.GetComponent<LineRenderer> ();
		initializeGround ();
		generateTerrain ();
		ground = GameObject.FindGameObjectsWithTag ("Ground");
		print (ground.Length);
		drawGround();

	}
	Vector2 Casteljau(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, float x){
		
		Vector2 ap1 = Vector2.Lerp(p1, p2, x);
		Vector2 ap2 = Vector2.Lerp(p2, p3, x);
		Vector2 ap3 = Vector2.Lerp(p3, p4, x);
		
		Vector2 bp1 = Vector2.Lerp(ap1, ap2, x);
		Vector2 bp2 = Vector2.Lerp(ap2, ap3, x);
		Vector2 bp3 = Vector2.Lerp(bp1, bp2, x);
		// Place a vertex at p
		return bp3;
	}
	void generateTerrain(){
		for (int i = 0;i < 10;i++){
			float height = Random.Range (50f, 100f);
			float width = Random.Range(1f, 100f);
			GameObject curr = (GameObject) Instantiate (groundobject, new Vector3 (lastposition + .5f*width, bottom+.5f*height, -5), Quaternion.identity);
			GameObject currbound = (GameObject)Instantiate (boundaryobject, new Vector3(lastposition + .5f * width, bottom, -5), Quaternion.identity);
			Vector2 temp = curr.GetComponent<Transform>().localScale;
			Vector2 tempbound = currbound.GetComponent<Transform>().localScale;
			tempbound.x = width;
			currbound.GetComponent<Transform>().localScale = tempbound;
			temp.x = width;
			temp.y = height;
			curr.GetComponent<Transform>().localScale = temp;
			lastposition = lastposition + width;
			grounds.Add(curr);
		}

		drawGround ();
	}
	private void initializeGround(){
		grounds = new List<GameObject> ();
		tempgrounds = new GameObject[GameObject.FindGameObjectsWithTag ("Ground").Length];
		tempgrounds = GameObject.FindGameObjectsWithTag ("Ground");
		
		for (int i = 0; i < tempgrounds.Length;i++){
			grounds.Add(tempgrounds[i]);
			
		}
	}
	public void drawGround(){
		//Destroy (GetComponent<LineRenderer>());
		//gameObject.AddComponent<LineRenderer>();
		//lines = GetComponent < LineRenderer >();
		grounds.Sort (comparetopleft);
		organizeGround (grounds);
		if (!initialized){
			initializeGround();
			initialized = true;
		}

		vertices = new List<List<Vector2>> ();
		lines.SetVertexCount (0);
		lines.SetColors (new Color(1f, 0f, 0f), new Color(1f, 0f, 0f));
		thing = true;

		//print (grounds.Count);	


		for (int i = 0; i < grounds.Count-1; i++) {
			List<Vector2> temp = new List<Vector2>(4);
			if (i == 0){
				temp.Add( grounds [i].GetComponent<ground> ().getTL ());
				temp.Add (grounds [i].GetComponent<ground> ().getTR ());
				temp.Add(grounds[i+1].GetComponent<ground>().getTL ());
				Vector2 tempTL = grounds[i+1].GetComponent<ground>().getTL ();
				Vector2 tempTR = grounds[i+1].GetComponent<ground>().getTR ();
				temp.Add(new Vector2((tempTL.x + tempTR.x) /2, tempTL.y));
				vertices.Add (temp);
			} else {
				Vector2 tempTL = grounds[i].GetComponent<ground>().getTL ();	
				Vector2 tempTR = grounds[i].GetComponent<ground>().getTR ();
				temp.Add(new Vector2((tempTL.x + tempTR.x) /2, tempTL.y));
				temp.Add (tempTR);
				temp.Add (grounds[i+1].GetComponent<ground>().getTL ());
				Vector2 tempTL2 = grounds[i+1].GetComponent<ground>().getTL ();
				Vector2 tempTR2 = grounds[i+1].GetComponent<ground>().getTR ();
				temp.Add(new Vector2((tempTL2.x+tempTR2.x)/2, tempTL2.y));
				vertices.Add (temp);
			}
		}

		lines.SetWidth (.1f, .1f);
		lines.SetVertexCount ((vertices.Count) * 20); //can change 20 to a constant according to the for loop below
		Vector2[] temp2 = new Vector2[(int)Mathf.Floor((vertices.Count * 20)/4)];
		//Vector2[] temp2 = new Vector2 [(int)Mathf.Floor((vertices.Count * 20)/4)];

		int count = 0;
		for (int i = 0; i < vertices.Count; i++){

			Vector2 p1 = vertices[i][0];
			Vector2 p2 = vertices[i][1];
			Vector2 p3 = vertices[i][2];
			Vector2 p4 = vertices[i][3];

			for (float j = 0.0f; j < 1f; j+=.05f) {
				//print (vertices[i]);
				Vector2 ret = Casteljau(p1, p2, p3, p4, j);
				//print (ret);
				lines.SetPosition (count, new Vector3 (ret.x, ret.y - 3, -6));
				if (count %4 ==0){
					temp2[count/4] = new Vector2(ret.x, ret.y -3);
					//boundary.points[count/8] =  ret;

				}
				//print (count);
				//boundary.points[count] = ret;
				//print (count);
				count++;
			}
			 //chang	

		}
		boundary.points = temp2;
		Vector2 offsettemp = new Vector2 (-4.3f, -1.1f);
		boundary.offset = offsettemp;
		//boundary.points.SetValue (new Vector2(0, 0), 0);//[0] = new Vector2(0, 0);

	}
	void organizeGround(List<GameObject> groundList){
		removeground = new List<GameObject>();
		for (int i = 0; i < groundList.Count-1;i++){
			ground firstquad = groundList[i].GetComponent<ground>();
			ground secondquad = groundList[i+1].GetComponent<ground> ();
			if (firstquad.getTL().x == secondquad.getTL().x && firstquad.getTR ().x == secondquad.getTR().x){
				int j = i+2;
				while (j < groundList.Count){
					secondquad = groundList[j].GetComponent<ground>();
					if (firstquad.getTL().x == secondquad.getTL().x && firstquad.getTR ().x == secondquad.getTR().x){
						j++;
						continue;
					} else {
						break;
					}

				}
				for (int k = i; k < j;k++){
					removeground.Add(groundList[k]);
				}
				float currheight = float.MinValue;
				GameObject currhighest = removeground[0];
				for (int l = 0; l < removeground.Count;l++){
					if (removeground[l].GetComponent<ground>().getTL().y > currheight){
						currhighest = removeground[l];
						currheight = currhighest.GetComponent<ground>().getTL().y;
					}
				}
				removeground.Remove(currhighest);
				for (int m = 0; m < removeground.Count;m++){
					groundList.Remove(removeground[m]);
				}
			}  
		}
  
	}

	int comparetopleft(GameObject first, GameObject second){

		return first.GetComponent<ground> ().getTL ().x.CompareTo (second.GetComponent<ground> ().getTL ().x);

	}

	// Update is called once per frame
	void Update () {
		if (Mathf.Abs(camera.GetComponent<Transform>().localPosition.x - lastposition) < 20f){
			generateTerrain();
		}
	}
}