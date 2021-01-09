using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingCartManager : MonoBehaviour
{
  public GameObject cartPrefab;
	public CartController[] carts;
	public Vector3[] cartPositions;
	public Quaternion[] cartRotations;

	public static ShoppingCartManager instance;
	private void Awake() {
		if(instance == null) {
			instance = this;
		}
		carts = FindObjectsOfType<CartController>();
		cartPositions = new Vector3[carts.Length];
		cartRotations = new Quaternion[carts.Length];
		for (int i = 0; i < carts.Length; i++)
		{
			cartPositions[i] = carts[i].transform.position;
			cartRotations[i] = carts[i].transform.rotation;
		}
	}

	public void SpawnCartAtEmptyPosAfterDelay(float delay) {
		Invoke("SpawnCartAtEmptyPos", delay);
	}

	public void SpawnCartAtEmptyPos() {
		for (int i = 0; i < carts.Length; i++)
		{
			if(carts[i] == null) {
				Debug.Log("Spawned new cart");
				// spawn new cart
				GameObject spawnedCart = Instantiate(cartPrefab, cartPositions[i], cartRotations[i]);
				carts[i] = spawnedCart.GetComponent<CartController>();
			}
		}
	}
}
