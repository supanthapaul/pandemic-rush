using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine.Soundy;

public interface IInteractible
{
	void Interact();
}

public class InteractionSystem : MonoBehaviour
{
	public LayerMask interactLayer;
	public float interactDistance = 20f;

	private Transform _interactingObject;

	private void Update() {

		// Resetting the outline
		if(_interactingObject != null) {
			//Debug.Log("Resetting");
			_interactingObject.GetComponentInChildren<Outline>().enabled = false;
			_interactingObject = null;
		}
		RaycastHit hit;
		Outline targetOutline = null;
		// Chehcking raycast
		if(Physics.Raycast(transform.position, transform.forward, out hit, interactDistance, interactLayer)) {
			//Debug.Log("hit");
			Transform interactingObject = hit.transform;
			if(!targetOutline)
				targetOutline = interactingObject.GetComponentInChildren<Outline>();
			// enable the outline
			targetOutline.enabled = true;
			// wait for interaction input
			if(Input.GetButtonDown("Interact")) {
				IInteractible interactible = hit.collider.GetComponent<IInteractible>();
				if(interactible != null) {
					SoundyManager.Play("General", "Grab");
					interactible.Interact();
				}
			}
			_interactingObject = interactingObject;
		}

	}
}
