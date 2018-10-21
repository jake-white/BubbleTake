using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

public class GameoverRestartMenu : MonoBehaviour {
	private VRInteractiveItem gaze;
	private int secondsToHold = 1;
	private Renderer rend;
	private Color oldColor;
	public Gameover gameover;
	private float timeEntered;


	void Start () {
        rend = GetComponent<Renderer>();
		oldColor = rend.material.GetColor("_Color");
	}
	
	void Update () {
		if(gaze.IsOver && Time.time > timeEntered + secondsToHold) {
			gameover.RestartGame();
		}
		else if(gaze.IsOver) {			
			float timeUntilSelected = (secondsToHold - (Time.time - timeEntered))/secondsToHold;
			timeUntilSelected*=255;
			Color newColor = new Color(oldColor.r * 255/timeUntilSelected, oldColor.g * timeUntilSelected/255, oldColor.b * timeUntilSelected/255);
			rend.material.SetColor("_Color", newColor);
		}
	}	

	private void OnEnable() {
		gaze = GetComponent<VRInteractiveItem>();
		gaze.OnOver += HandleOver;
		gaze.OnOut += HandleOut;
	}

	void HandleOver() {
		timeEntered = Time.time;
	}

	void HandleOut() {
		rend.material.SetColor("_Color", oldColor);
	}
}
