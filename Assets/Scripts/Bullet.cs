﻿using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
	public GameObject explosion;
	public AudioClip[] bulletSounds;
	public AudioClip[] rewardSounds;

	private GameController gameController;

	void Start ()
	{
		gameController = GameObject.FindWithTag ("GameController").GetComponent<GameController>();

		audio.pitch = Random.Range (0.9f, 1.1f);
		if (bulletSounds.Length > 0) {
			audio.PlayOneShot(bulletSounds [Random.Range(0, bulletSounds.Length)]);
		}
	}

	void OnCollisionEnter2D (Collision2D collision)
	{
		if (collision.gameObject.tag == "asteroid") {
			gameController.AddPoints(1);
			if (rewardSounds.Length > 0) {
				audio.PlayOneShot(rewardSounds[Random.Range(0, rewardSounds.Length)]);
			}
			Destroy(collision.gameObject);
			GameObject explosionInstance = (GameObject)Instantiate (explosion, collision.contacts[0].point, Quaternion.identity);
			Destroy (explosionInstance, 3f);
			Destroy (gameObject, audio.clip.length);
		}
	}
}
