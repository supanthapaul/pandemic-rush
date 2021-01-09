using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine.Soundy;

public class CheckoutArea : MonoBehaviour
{
	// public static CheckoutArea instance;
	// private void Awake() {
	// 	if(instance == null) {
	// 		instance = this;
	// 	}
	// }
	private void OnTriggerEnter(Collider other) {
		if(other.CompareTag("Cart")) {
			CartController cart = other.GetComponent<CartController>();
			// stop using the cart
			cart.StopUsingCart();
			// add to score
			TimedRush.instance.IncrementScore(cart.cartScore);
			// reset game state
			TimedRush.instance.SetGameState(GameStates.FindItems);
			// Cart complete sound
			SoundyManager.Play("General", "CartComplete", transform.position);
			// cart enter particle
			GameObject cartEnterPart = Instantiate(CheckoutAreasManager.instance.GetCartEnterParticle(), other.transform.position, Quaternion.identity);
			Destroy(cartEnterPart, 2f);
			float newCartDelay = 0.5f;
			// destroy cart after some time
			Destroy(cart.gameObject, newCartDelay);
			// spawn new cart
			ShoppingCartManager.instance.SpawnCartAtEmptyPosAfterDelay(newCartDelay);
		}
	}
}
