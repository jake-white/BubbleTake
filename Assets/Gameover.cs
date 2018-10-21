using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class Gameover : MonoBehaviour {

	public GameObject plane, sphere;
	public TextMesh text;
	public List<int> leaderboard;
	public TextMesh leaderboardText;

	// Use this for initialization
	void Start () {		
        string path = "Assets/Data/leaderboard.txt";

        StreamReader reader = new StreamReader(path); 
		string score;
		while((score = reader.ReadLine()) != null)  
		{  
			if(score!= String.Empty) {
				Debug.Log(score);
				leaderboard.Add(Convert.ToInt32(score));
			}
		}
        reader.Close();
		DisplayLeaderboard();
		
	}
	
	void Update () {
		
	}

	public void EndGame() {
		plane.GetComponent<Animator>().Play("fadeIn");
		text.gameObject.SetActive(true);
		sphere.gameObject.SetActive(true);
		OrbManager.instance.Pause();
		UpdateLeaderboard(OrbManager.instance.GetScore());
	}

	public void RestartGame() {
		plane.GetComponent<Animator>().Play("fadeOut");
		text.gameObject.SetActive(false);
		sphere.gameObject.SetActive(false);
		OrbManager.instance.Restart();
		OrbManager.instance.Unpause();
	}

	void DisplayLeaderboard() {
		leaderboard.Sort();
		string text = "";
		for(int i = leaderboard.Count - 1; i >= 0; --i) {
			text+=leaderboard[i]+"\n";
		}
		leaderboardText.text = text;
	}

	void UpdateLeaderboard(int newScore) {
		leaderboard.Sort();
		bool notInserted = true;
		if(leaderboard.Count < 10) {
			leaderboard.Add(newScore);
		}
		else {
			for(int i = leaderboard.Count - 1; i > 0; --i) {
				if(newScore > leaderboard[i] && notInserted) {
					leaderboard.Insert(i, newScore);
					if(leaderboard.Count > 10) {
						leaderboard.RemoveAt(0); // remove the last one
					}
					notInserted = false;
				}
			}
		}
		string path = "Assets/Data/leaderboard.txt";

        //Write some text to the test.txt file
		File.WriteAllText("Assets/Data/leaderboard.txt", String.Empty);
        StreamWriter writer = new StreamWriter(path, true);
		foreach(int score in leaderboard) {
        	writer.WriteLine(score+"\n");
		}
        writer.Close();
		DisplayLeaderboard();
	}
}
