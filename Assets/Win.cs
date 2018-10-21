using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour {

	public GameObject plane, sphere;
	public TextMesh text, timerText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void EndGame() {
		plane.GetComponent<Animator>().Play("fadeIn");
		text.gameObject.SetActive(true);
		timerText.gameObject.SetActive(true);
		timerText.text = OrbManager.instance.GetTime();
		sphere.gameObject.SetActive(true);
		OrbManager.instance.Pause();
	}

	public void RestartGame() {
		plane.GetComponent<Animator>().Play("fadeOut");
		text.gameObject.SetActive(false);
		timerText.gameObject.SetActive(false);
		sphere.gameObject.SetActive(false);
		OrbManager.instance.Restart();
		OrbManager.instance.Unpause();
	}
}
