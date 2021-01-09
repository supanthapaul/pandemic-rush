using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartController : MonoBehaviour, IInteractible
{
	private float hInput;
	private float vInput;
	private float currSteeringAngle;
	private float currBreakForce;
	private bool isBreaking;

	public int cartScore = 0;
	[SerializeField] private Vector3 centerOfMass;
	[SerializeField] private float motorForce;
	[SerializeField] private float breakForce;
	[SerializeField] private float maxSteeringAngle;

	[SerializeField] private WheelCollider fLCollider;
	[SerializeField] private WheelCollider fRCollider;
	[SerializeField] private WheelCollider rLCollider;
	[SerializeField] private WheelCollider rRCollider;

	[SerializeField] private Transform fLTrandform;
	[SerializeField] private Transform fRTrandform;
	[SerializeField] private Transform rLTrandform;
	[SerializeField] private Transform rRTrandform;
	[SerializeField] private Transform[] itemSpawns;

	private bool _isBeingUsed = false;
	private PlayerCartFollower _playerCartFollower;
	private Outline outline;

	private void Start()
	{
		outline = GetComponentInChildren<Outline>();
		Invoke("DisableOutline", 0.5f);
		_playerCartFollower = GetComponent<PlayerCartFollower>();
		// set com
		GetComponent<Rigidbody>().centerOfMass = centerOfMass;
		fLCollider.ConfigureVehicleSubsteps(5, 12, 15);
		fRCollider.ConfigureVehicleSubsteps(5, 12, 15);
		rLCollider.ConfigureVehicleSubsteps(5, 12, 15);
		rRCollider.ConfigureVehicleSubsteps(5, 12, 15);
	}
	private void FixedUpdate()
	{
		if(!_isBeingUsed)
			return;

		GetInput();
		HandleMotor();
		HandleSteering();
		UpdateWheels();
	}

	private void Update() {
		// stop using the cart
		// if(_isBeingUsed && Input.GetButtonDown("Interact")) {
		// 	_isBeingUsed = false;
		// 	_playerCartFollower.StopFollowing();
		// 	// re-add interactivity
		// 	Invoke("MakeInteractible", 1f);
		// }
	}
	void MakeInteractible() {
		gameObject.layer = LayerMask.NameToLayer("Interactible");
	}
	public void Interact() {
		if(!Inventory.instance.IsFull()) {
			// TODO: popup - get more items
			return;
		}
		_isBeingUsed = true;
		// remove interactivity
		gameObject.layer = LayerMask.NameToLayer("Default");
		outline.enabled = false;
		// make player follow cart
		_playerCartFollower.StartFollwing();
		// add to cart score
		cartScore = Inventory.instance.GetInventorySize() * TimedRush.instance.scorePerItem;
		// drop inventory
		Inventory.instance.DropInventory(itemSpawns);
	}

	public void StopUsingCart() {
		if(_isBeingUsed) {
			_isBeingUsed = false;
			_playerCartFollower.StopFollowing();
			// re-add interactivity
			//Invoke("MakeInteractible", 1f);
		}
	}

	private void GetInput()
	{
		hInput = Input.GetAxis("Horizontal");
		vInput = Input.GetAxis("Vertical");
		//isBreaking = Input.GetKey(KeyCode.LeftShift);
	}

	private void HandleMotor()
	{
		rLCollider.motorTorque = vInput * motorForce;
		rRCollider.motorTorque = vInput * motorForce;
		currBreakForce = isBreaking ? breakForce : 0f;
		if (isBreaking)
		{
			ApplyBreaking();
		}
	}

	private void ApplyBreaking()
	{
		fLCollider.brakeTorque = currBreakForce;
		fRCollider.brakeTorque = currBreakForce;
		rLCollider.brakeTorque = currBreakForce;
		rRCollider.brakeTorque = currBreakForce;
	}

	private void HandleSteering()
	{
		currSteeringAngle = maxSteeringAngle * hInput;
		fLCollider.steerAngle = currSteeringAngle;
		fRCollider.steerAngle = currSteeringAngle;
	}

	private void UpdateWheels()
	{
		UpdateSingleWheel(fLCollider, fLTrandform);
		UpdateSingleWheel(fRCollider, fRTrandform);
		UpdateSingleWheel(rLCollider, rLTrandform);
		UpdateSingleWheel(rRCollider, rRTrandform);
	}

	private void UpdateSingleWheel(WheelCollider wCollider, Transform wTrandform)
	{
		Vector3 pos;
		Quaternion rot;
		wCollider.GetWorldPose(out pos, out rot);
		wTrandform.position = pos;
		wTrandform.rotation = rot;
	}


	void DisableOutline() {
		outline.enabled = false;
	}

	private void OnDrawGizmosSelected() {
		Vector3 pos = transform.position + centerOfMass;
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(pos, 0.1f);
	}
}
