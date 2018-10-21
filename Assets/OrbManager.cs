using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrbManager : MonoBehaviour {

	public static OrbManager instance = null;
	public List<OrbScript> greenOrbs;
	public List<OrbScript> redOrbs;
	public GameObject greenOrbPrefab, redOrbPrefab, goldOrbPrefab, wall;
	public int redAmountGoal, blueAmountGoal;
	public float minY = 0, maxY = 5, minZ = -5f, maxZ = 5f;
	private int score = 0;
	public TextMesh scoreText, timeText;
	public Gameover gameover;
	public Win win;
	private float orbLife = 2, gameStarted = 0;
	private bool paused = false;
	public List<AudioClip> pop;
	public AudioClip victory, goldPop;
	public AudioClip balloon;
	public int endTimeTrial = 100, increaseDifficulty, currentDifficulty;
	public AudioSource music;

	void Awake() {
		if (instance == null) {
            instance = this;
		}
	}
	
	void Start () {
		gameStarted = Time.time;
		greenOrbs = new List<OrbScript>();
		redOrbs = new List<OrbScript>();
	}

	public void Restart() {
		music.Play();
		gameStarted = Time.time;
		score = 0;
		scoreText.text = ""+score;
	}

	public string GetTime() {
		float elapsedTime = Time.time - gameStarted;
		string minutes = Mathf.Floor(elapsedTime / 60).ToString("0");
		string seconds = Mathf.Floor(elapsedTime % 60).ToString("00");
		return minutes+":"+seconds;
	}

	public int GetScore() {
		return score;
	}

	public void Pause() {
		paused = true;
	}

	public void Unpause() {
		paused = false;
	}

	void KillAllOrbs() {
		foreach(OrbScript orb in greenOrbs) {
			orb.GetComponent<Outline>().OutlineWidth = 0;
		}
		foreach(OrbScript orb in redOrbs) {
			orb.GetComponent<Outline>().OutlineWidth = 0;
		}
	}
	
	void Update () {
		float timeElapsed = Time.time - gameStarted;
		blueAmountGoal = 3 + (int) System.Math.Floor(timeElapsed/15.0f);
		// if(score >= endTimeTrial && !paused) {
		// 	KillAllOrbs();
		// 	win.EndGame();
		// }
		if(!paused) {
			if(greenOrbs.Count < redAmountGoal) {
				createOrb(true);
			}
			if(redOrbs.Count < blueAmountGoal) {
				createOrb(false);
			}
		}

		//timeText.text = GetTime();
	}

	public void touchOrb(OrbScript orb) {
		if(!paused) {
			if(orb.GetOrbColor() == OrbScript.OrbColor.Red) {
				GetComponent<AudioSource>().clip = balloon;
				GetComponent<AudioSource>().Play();
				KillAllOrbs();
				gameover.EndGame();
				music.Stop();
			}
			else {
				if(orb.GetOrbColor() == OrbScript.OrbColor.Gold) {
					GetComponent<AudioSource>().clip = goldPop;
					GetComponent<AudioSource>().Play();
				}
				else {
					int randomSound = Random.Range(0, 2);
					GetComponent<AudioSource>().clip = pop[randomSound];
					GetComponent<AudioSource>().Play();
				}
				if(orb.GetOrbColor() == OrbScript.OrbColor.Gold) {
					score+=5;
				}
				else {
					score++;
				}
				scoreText.text = ""+score;
			}
			deathOrb(orb);
		}
	}

	public void deathOrb(OrbScript orb) {
		if(orb.GetOrbColor() != OrbScript.OrbColor.Red) {
			greenOrbs.Remove(orb);
		}
		else {
			redOrbs.Remove(orb);
		}
		Destroy(orb.gameObject);
	}

	private void createOrb(bool isRed) {
		float randomZ = Random.Range(minZ, maxZ);
		float randomY = Random.Range(minY, maxY);
		float x = wall.transform.position.x;
		if(isRed) {
			x += 0.05f;
		}
		Vector3 randomSpot = new Vector3(x-0.2f, randomY, randomZ);
		if(isRed) {
			float randomGold = Random.Range(0.0f,1.0f);
			GameObject orb;
			if(randomGold < 0.05f) {
				orb = Instantiate(goldOrbPrefab, randomSpot, Quaternion.identity);
				orb.GetComponent<OrbScript>().setDetails(OrbScript.OrbColor.Gold, orbLife/2);				
			}
			else {
				orb = Instantiate(greenOrbPrefab, randomSpot, Quaternion.identity);
				orb.GetComponent<OrbScript>().setDetails(OrbScript.OrbColor.Green, orbLife);
			}
			greenOrbs.Add(orb.GetComponent<OrbScript>());
		}
		else {
			GameObject orb = Instantiate(redOrbPrefab, randomSpot, Quaternion.identity);
			orb.GetComponent<OrbScript>().setDetails(OrbScript.OrbColor.Red, orbLife);
			redOrbs.Add(orb.GetComponent<OrbScript>());
		}
	}


}
