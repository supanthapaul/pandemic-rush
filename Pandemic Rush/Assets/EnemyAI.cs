using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public enum EnemyStates {
	FindTarget,
	ChaseTarget,
	TakeTarget,
	GoCheckout,
	Idle
}
public interface IEnemy {
	void TakeHit();
}

public class EnemyAI : MonoBehaviour, IEnemy
{
	public Vector2 speedRange;
	public GameObject maskObj;
	public float idleWaitDuration = 3f;
	public Transform grabPosition;
	public PickupObject target;

	NavMeshAgent _agent;
	EnemyStates state;
	Animator _anim;
	Transform checkoutPoint;

	private void Start() {
		checkoutPoint = CheckoutAreasManager.instance.GetRandomCheckoutPoint();
		maskObj.SetActive(false);
		_anim = GetComponentInChildren<Animator>();
		_agent = GetComponent<NavMeshAgent>();
		_agent.speed = Random.Range(speedRange.x, speedRange.y);
	}

	private void Update() {
		switch(state) {
			case EnemyStates.FindTarget:
			{
				_anim.SetBool("IsRunning", false);
				_anim.SetBool("IsReturning", false);
				if(target == null || target.takenByEnemy)
					target = ItemsManager.instance.GetRandomTarget();
				if(target != null) {
					state = EnemyStates.ChaseTarget;
				}
				break;
			}
			case EnemyStates.ChaseTarget: {
				if(target == null || target.takenByEnemy) {
					state = EnemyStates.FindTarget;
					break;
				}
				_anim.SetBool("IsRunning", true);
				_anim.SetBool("IsReturning", false);
				_agent.SetDestination(target.transform.position);
				// check if reached target
				float distanceFromItem = Vector3.Magnitude(target.transform.position - transform.position);
				if(distanceFromItem <= _agent.stoppingDistance + 0.8f) {
					// enemy reached target, take it
					state = EnemyStates.TakeTarget;
					break;
				}
				break;
			}
			case EnemyStates.TakeTarget: {
				if(target == null)
					break;
				// disable interaction of target
				target.DisableInteraction();
				// set target's takenByEnemy
				target.takenByEnemy = true;
				// position and parent target
				target.transform.position = grabPosition.position;
				target.transform.rotation = grabPosition.rotation;
				target.transform.parent = grabPosition;
				state = EnemyStates.GoCheckout;
				break;
			}
			case EnemyStates.GoCheckout: {
				// animation
				_anim.SetBool("IsRunning", false);
				_anim.SetBool("IsReturning", true);
				// return to checkout area
				Vector3 checkoutPos = checkoutPoint.position;
				_agent.SetDestination(checkoutPos);
				// check if reached checkout area
				float distanceFromCheckout = Vector3.Magnitude(checkoutPos - transform.position);
				if(distanceFromCheckout <= _agent.stoppingDistance + 1f) {
					// reached checkout area, drop item
					DropTarget();
					state = EnemyStates.FindTarget;
					break;
				}
				break;
			}
			case EnemyStates.Idle:
			{
				_anim.SetBool("IsRunning", false);
				_anim.SetBool("IsReturning", false);
				_agent.isStopped = true;
				break;
			}
		}
	}

	public void TakeHit() {
		StartCoroutine(StartIdle());
	}
	IEnumerator StartIdle() {
		state = EnemyStates.Idle;
		maskObj.SetActive(true);
		DropTarget();
		yield return new WaitForSeconds(idleWaitDuration);
		// disable mask
		maskObj.SetActive(false);
		// return to find new item
		_agent.isStopped = false;
		state = EnemyStates.FindTarget;
	}

	void DropTarget() {
		if(target != null) {
			target.transform.parent = null;
					target.EnableInteraction();
					target = null;
		}
	}
#if UNITY_EDITOR
	private void OnDrawGizmos() {
		Handles.Label(transform.position, state.ToString());
	}
#endif
}



