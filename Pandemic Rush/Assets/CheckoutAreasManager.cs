using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckoutAreasManager : MonoBehaviour
{
	public CheckoutArea[] checkoutAreas;
	public GameObject[] cartEnterParticles;

	public static CheckoutAreasManager instance;
	private void Awake() {
		if(instance == null) {
			instance = this;
		}
	}

	public Transform GetRandomCheckoutPoint() {
		int index = Random.Range(0, checkoutAreas.Length);
		return checkoutAreas[index].transform;
	}

	public GameObject GetCartEnterParticle() {
		int i = Random.Range(0, cartEnterParticles.Length);
		return cartEnterParticles[i];
	}
}
