﻿using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {

	public GameObject explosion;
	public GameObject bullet;
	public int bulletForce = 300;

	public AudioClip[] crashSounds;

	[HideInInspector]
	public bool canShoot = false;
	[HideInInspector]
	public bool canMove = false;

	private GameObject[] spawns;

	private Transform bulletSpawnTransform;
	private GameController gameController;
	private float tempo;
	private SpawnController spawnController;

	private int currPos = 1;

	void Start ()
	{
		bulletSpawnTransform = GameObject.Find ("BulletSpawnPoint").transform;

		gameController = GameObject.Find ("GameController").GetComponent<GameController> ();
		tempo = gameController.GetComponent<GameController>().tempo;

		spawnController = GameObject.Find ("SpawnController").GetComponent<SpawnController> ();
		spawns = spawnController.spawns;
	}

	void Shoot ()
	{
		if (canShoot) {
			GameObject bulletInstance = (GameObject)Instantiate (bullet, bulletSpawnTransform.position, Quaternion.identity);
			bulletInstance.rigidbody2D.AddForce(Vector2.up * bulletForce, ForceMode2D.Force);
		
			if (SpawnController.canSpawn == false) {
				SpawnController.canSpawn = true;
			}
		}
	}

	void Update ()
	{
#if UNITY_EDITOR
		// get input from keyboard
		if (Input.GetKeyDown("left")) {
			MoveLeft();
		}	
		else if (Input.GetKeyDown("right")) {
			MoveRight();
		}
#endif
	}
	
	public void MoveLeft ()
	{
		//only move 
		if (currPos > 0) {
			currPos--;
			//do the movement
			Move ();
		}
	}
	
	public void MoveRight ()
	{
		if (currPos < spawns.Length-1) {
			currPos++;
			//do the movement
			Move ();
		}
	}
	
	void Move ()
	{
		if (canMove) {
			iTween.MoveTo (this.gameObject, iTween.Hash (
				"position", new Vector3 (spawns [currPos].transform.position.x, transform.position.y, transform.position.z)
				,"easetype", iTween.EaseType.spring
				,"time", tempo
				,"onstart", "Shoot"
				,"onstarttarget", gameObject
				));
		}
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		if (collision.gameObject.CompareTag("asteroid")) {
			gameController.LoseHeart();
			if (crashSounds.Length > 0) {
				audio.PlayOneShot(crashSounds[Random.Range(0, crashSounds.Length)]);
			}
		}
		
		Destroy(collision.gameObject);
		GameObject explosionInstance = (GameObject)Instantiate (explosion, collision.contacts[0].point, Quaternion.identity);
		Destroy (explosionInstance, 3f);
	}

}
