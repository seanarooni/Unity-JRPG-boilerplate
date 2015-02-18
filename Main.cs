using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	public Monster[] AllMonsters;
	private bool paused = false;

	private int InventorySlots = 6;

	private GameObject MenuCursor;
	private int cursorPosition = 0;
	private float cursorPadding = 120.0f;
	private float firstItemLeftPosition = -300.0f;
	private GameObject PauseMenuCanvas;

	void Start() {
		MenuCursor = GameObject.FindGameObjectWithTag ("MenuCursor");
		PauseMenuCanvas = GameObject.FindGameObjectWithTag ("PauseMenu");
		PauseMenuCanvas.SetActive (false);
	}

	void Update() {

		if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
			if (cursorPosition < InventorySlots - 1) {
				cursorPosition++;
			}
		}

		if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
			if (cursorPosition > 0)
				cursorPosition--;
		}

		MenuCursor.transform.localPosition = new Vector3 (firstItemLeftPosition + (cursorPadding * cursorPosition), 80.0f, 0);
	}

	public void Pause() {
		if (paused) {
			//unpause the game, hide pause menu
			PauseMenuCanvas.SetActive (false);
			paused = false;
			this.enabled = false;

		} else {
			//pause the game, show pause menu
			PauseMenuCanvas.SetActive (true);
			paused = true;
		}
	}
	
	public bool Paused() {
		if (paused)
			return true;
		
		return false;
	}


}
