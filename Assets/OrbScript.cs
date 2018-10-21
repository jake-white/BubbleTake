using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

public class OrbScript : MonoBehaviour {
	private VRInteractiveItem gaze;
	private OrbManager manager;
	private float timeUntilDeath, timeCreated, redGracePeriod = 0.1f;
	private Renderer rend;
	private Color oldColor;
	public enum OrbColor {Green, Gold, Red};
	private OrbColor orbcolor;
	

	void Start () {
		manager = OrbManager.instance;
		timeCreated = Time.time;
        rend = GetComponent<Renderer>();
		oldColor = rend.material.GetColor("_Color");
	}
	
	void Update () {
		float timeLeft = (timeUntilDeath - (Time.time - timeCreated))/timeUntilDeath;
		Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, oldColor.a*timeLeft);
		rend.material.SetColor("_Color", newColor);
		if(Time.time > timeCreated + timeUntilDeath) {
			manager.deathOrb(this);
		}
	}

	private void OnEnable() {
		gaze = GetComponent<VRInteractiveItem>();
		gaze.OnOver += HandleOver;
	}

	void HandleOver(){
		if(orbcolor == OrbColor.Red && Time.time < timeCreated + redGracePeriod) {
			//do nothing
		}
		else {
			manager.touchOrb(this);
		}
	}

	public OrbColor GetOrbColor() {
		return orbcolor;
	}

	public void setDetails(OrbColor orbcolor, float lifetime) {
		this.orbcolor = orbcolor;
		this.timeUntilDeath = lifetime;
	}
}
