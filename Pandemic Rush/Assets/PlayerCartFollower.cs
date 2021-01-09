using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCartFollower : MonoBehaviour
{
	public Transform player;
	public Transform playerPosition;

	private Rigidbody _playerRb;
	private PlayerMovement _playerMovement;
	private bool _isFollowing = false;

	void Start()
	{
		player = FindObjectOfType<PlayerMovement>().transform;
		_playerRb = player.GetComponent<Rigidbody>();
		_playerMovement = FindObjectOfType<PlayerMovement>();
	}

	void Update()
	{
		if(_isFollowing) {
			// disable rb
			if(!_playerRb.isKinematic) _playerRb.isKinematic = true;
			Debug.Log("DISABLING MOVEMENT");
			// disable movement
			if(!_playerMovement.disableMovement) _playerMovement.disableMovement = true;
			// follow position
			player.position = playerPosition.position;
		}
		else {
			// enable rb
			if(_playerRb.isKinematic) _playerRb.isKinematic = false;
			// enable movement
			if(_playerMovement.disableMovement) _playerMovement.disableMovement = false;
		}
	}

	public void StartFollwing() {
		player.position = playerPosition.position;
		_isFollowing = true;
	}

	public void StopFollowing() {
		_isFollowing = false;
	}
}
