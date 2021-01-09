using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Doozy.Engine.Soundy;

public class MaskBlaster : MonoBehaviour
{
	public GameObject bulletPrefab;
	public Transform gunCamera;
	public Transform shootPosition;
	public float startFireCooldown;
	public Ease gunRotationEase;
	public GameObject shootParticle;

	private float _fireCooldown;
	private Animator _anim;

	private void Start()
	{
		_anim = GetComponent<Animator>();
		_fireCooldown = 0f;
	}

	private void Update()
	{
		if(PauseManager.instance.isGamePaused)
			return;

		RaycastHit _hit;
		if (Physics.Raycast(gunCamera.position, gunCamera.forward, out _hit, 2000f))
		{
			if (Input.GetButtonDown("Fire1") && _fireCooldown <= 0f)
			{
				Vector3 _shootDir = (_hit.point - shootPosition.position).normalized;
				// instantiate bullet
				GameObject bullet = Instantiate(bulletPrefab, shootPosition.position, Quaternion.identity);
				bullet.transform.GetComponent<MaskBullet>().Setup(_shootDir, _hit.point);
				ShootFX();
				// reset fire cooldown
				_fireCooldown = startFireCooldown;
			}
		}

		_fireCooldown -= Time.deltaTime;
	}

	void ShootFX()
	{
		GameObject _shootParticle = Instantiate(shootParticle, shootPosition.position, Quaternion.identity);
		Destroy(_shootParticle, 1f);
		// sound
		SoundyManager.Play("General", "Shoot", shootPosition.position);
		// animation
		//_anim.SetTrigger("Shoot");
		Vector3 gunEndRot = transform.localEulerAngles + new Vector3(0f, 0f, 360f);
		transform.DOLocalRotate(gunEndRot, startFireCooldown-0.05f, RotateMode.FastBeyond360).SetEase(gunRotationEase);
		// rotate magazine
		Transform magazine = transform.GetChild(0);
		Vector3 endRot = magazine.localEulerAngles + new Vector3(0f, 0f, 35f);
		magazine.DOLocalRotate(endRot, 0.3f);
	}
}
