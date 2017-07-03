using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour {

    private GameObject player;
    public GameObject playerPrefab;

	// Use this for initialization
	void Start () {
        player = Instantiate(playerPrefab, player.transform, true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
