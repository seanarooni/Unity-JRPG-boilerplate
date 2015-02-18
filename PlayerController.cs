using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private Vector3 startPoint;
	private Vector3 endPoint;
	private float moveSpeed = 4.0f;
	private float increment;
	private bool isMoving = false;
	private bool disableMovement = false;
	private Vector3 AXIS;
	private bool animating = false;
	private Vector3 directionFacing;
	private Main MainScript;

	Animator anim;
	private GameObject CameraMain;

	void Start() {
		anim = this.GetComponent<Animator> ();
		startPoint = transform.position;
		endPoint = transform.position;
		CameraMain = GameObject.FindGameObjectWithTag ("MainCamera");
		MainScript = CameraMain.GetComponent<Main> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Escape)) {
			MainScript.enabled = true;
			MainScript.Pause();
		}

		if (MainScript.Paused ()) {

			return;
		}
	


		if (increment <= 1 && isMoving) {
			increment += moveSpeed / 100;
		//	print ("Moving");
		} else {
			isMoving = false;
		//	print ("Stopped");
		}

		if (animating) {
			//check if we've reached the endPoint, then return
			transform.position = Vector3.Lerp(startPoint, endPoint, increment);

			if (transform.position == endPoint)
				animating = false;

			return;
		}

		if (isMoving) {

			if (!PathClear(AXIS))
				return;

			if (animating)
				return;

			Move (startPoint, endPoint, increment);
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			ActionButton();
		}



		float V = Input.GetAxis ("Vertical");
		float H = Input.GetAxis ("Horizontal");
		float aV = Mathf.Abs (V);
		float aH = Mathf.Abs (H);

		if (aV > 0 || aH > 0 ) {
			if (aV > 0 && aH>0) 
				return;

			if (!isMoving && !disableMovement) {
				AXIS = new Vector3(H, V, 0).normalized;
				increment = 0;
				isMoving = true;
				startPoint = transform.position;
				endPoint = MovementHelper(transform.position, AXIS);
			}
		}
	}

	void Pause() {

	}

	void ActionButton() {
		RaycastHit hit;
		if (Physics.Raycast (transform.position, AXIS, out hit, 0.6f)) {
			if (hit.collider.tag == "NPC") {
			//talk
				print ("talk");
				return;
			}
		} else {
			//fire
			if (Physics.Raycast (transform.position, AXIS, out hit, 10.0f)) {
				if (hit.collider.tag == "enemy") {
					//talk
					print ("hit");
					return;
				}
			}
		}
	}

	bool PathClear(Vector3 dir) {
		float d = .52f; //these values probably need to be tweaked slightly in the hundreths place
		//, or a better system needs to be created

		//determine raycast distance
	//	if (dir.y < 0)
	//		d = 0.6f;
	//	if (dir.x != 0)
	//		d = 0.5f;

		RaycastHit hit; //need to convert this to 2D raycast.
		if (Physics.Raycast (transform.position, dir, out hit, d)) {
			if (hit.collider.tag == "wall") {
				print ("wall");
				isMoving = false;
				return false;
			}
			if (hit.collider.tag == "NPC") {
				isMoving = false;
				return false;
			}
			if (hit.collider.tag == "door") {
				Teleport(hit.collider.gameObject);
				return false;
			}
		}
		isMoving = true;
		return true;
	}

	void Teleport(GameObject door) {
		print ("door");
		Entrance es = door.GetComponent<Entrance> ();
		if (es.target.transform.position == null) 
			return;


		startPoint = es.target.transform.position;
		//transform.position = Vector3.Lerp
		increment = 0;
		isMoving = true;
		animating = true;
		endPoint = MovementHelper(startPoint, es.target.GetComponent<Entrance>().exitDirection);
		return;
	}

	void Move(Vector3 sp, Vector3 ep, float inc) {
		transform.position = Vector3.Lerp(startPoint, endPoint, increment);
	}

	Vector3 MovementHelper(Vector3 start, Vector3 input) {
		//determine endpoint, switch to control animation.
		int n=0;
		if (input.x > 0)
			n = 6;
		if (input.x < 0) 
			n = 4;
		if (input.y > 0)
			n = 8;
		if (input.y < 0)
			n = 2;

		anim.SetInteger ("direction", n);
		directionFacing = input;



		return new Vector3(start.x + input.x, start.y + input.y, 0);
	}
}
