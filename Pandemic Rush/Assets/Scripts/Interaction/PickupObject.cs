using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour, IInteractible
{
	public PickupTypes itemType;
	public bool takenByEnemy = false;

	private Rigidbody _rb;

	private void Start() {
		// disable outline on start
		_rb = GetComponent<Rigidbody>();
		Invoke("DisableOutline", 0.5f);
	}

	public void Interact()
	{
		// TODO: add to inventory
		Debug.Log("Adding item to inventory");
		bool added = Inventory.instance.Add(itemType);
		if(added)
			Destroy(gameObject);
	}

	public void DisableInteraction() {
		_rb.isKinematic = true;
		gameObject.layer = LayerMask.NameToLayer("Default");
	}
	public void EnableInteraction() {
		_rb.isKinematic = false;
		gameObject.layer = LayerMask.NameToLayer("Interactible");
	}

	public void DisableInteractionAfterDelay(float delay) {
		Invoke("DisableInteraction", delay);
	}

	void DisableOutline() {
		GetComponentInChildren<Outline>().enabled = false;
	}
}
