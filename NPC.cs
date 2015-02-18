using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour {

	public string[] talkMessage;
	private string displayText;
	private bool showText;
	private int messageLocation = 0;

	void OnGUI() {
		if (showText) {
			GUI.Label(new Rect(10,10,500,20), "" + displayText);
		}
	}

	public void interact() {
		showText = true;
	
		if (messageLocation +1 > talkMessage.Length) {
			showText = false;
			messageLocation = 0;
			return;
		} else {
			displayText = talkMessage [messageLocation];

			messageLocation++;
		}
	}

}
