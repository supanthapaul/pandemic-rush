using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine.Soundy;

public class MaskBullet : MonoBehaviour
{
	public float shootSpeed = 100f;
	public GameObject hitParticle;

	private Vector3 _shootDir;
	private Rigidbody _rb;

	public void Setup(Vector3 shootDir, Vector3 shootPoint) {
		_rb = GetComponent<Rigidbody>();
		this._shootDir = shootDir;
		//transform.eulerAngles = new Vector3(GetAngleFromVector(shootDir), GetAngleFromVector(shootDir), 0f);
		Quaternion _lookRotation = Quaternion.LookRotation(_shootDir);
		transform.rotation = _lookRotation;
		Destroy(gameObject, 20f);
	}

	private void FixedUpdate() {
		//transform.position += _shootDir * shootForce * Time.deltaTime;
		_rb.MovePosition(transform.position + _shootDir * shootSpeed * Time.fixedDeltaTime);
		//_rb.AddForce(_shootDir * shootForce * Time.fixedDeltaTime, ForceMode.Impulse);
	}

	private void OnTriggerEnter(Collider other) {
		Debug.Log("Hit something");
		if(other.CompareTag("Enemy")) {
			Debug.Log("Hit Enemy");
			IEnemy enemy = other.GetComponent<IEnemy>();
			SoundyManager.Play("General", "Hit");
			Vector3 particleSpawnPos = other.transform.position + new Vector3(0f, 1.5f, 0f);
			GameObject _hitPart = Instantiate(hitParticle, particleSpawnPos, Quaternion.identity);
			Destroy(_hitPart, 1f);
			enemy.TakeHit();
		}
		Destroy(gameObject);
	}

	float GetAngleFromVector(Vector3 dir) {
		dir = dir.normalized;
		float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		if(n < 0) n += 360;

		return n;
	}
}
