using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
	public PickupObject[] items;

	[Header("Prefab references")]
	public GameObject[] toiletPapers;
	public GameObject[] sanitizers;
	public GameObject[] chips;

	public static ItemsManager instance;
	private void Awake()
	{
		if (instance == null)
			instance = this;
		items = FindObjectsOfType<PickupObject>();
	}

	// re-adds a lost item reference to the items array
	public void ReAddItem(GameObject item)
	{
		for (int i = 0; i < items.Length; i++)
		{
			if (items[i] == null)
			{
				items[i] = item.GetComponent<PickupObject>();
				break;
			}
		}
	}

	public PickupObject GetRandomTarget() {
		bool _targetAvailable = targetAvailable();
		if(!_targetAvailable)
			return null;

		PickupObject target = items[Random.Range(0, items.Length)];
		while(target == null || target.takenByEnemy) {
			target = items[Random.Range(0, items.Length)];
		}
		return target;
	}
	// returns true if a target that can still be taken from environment
	bool targetAvailable() {
		for (int i = 0; i < items.Length; i++)
		{
			if(items[i] != null && !items[i].takenByEnemy) {
				return true;
			}
		}
		return false;
	}
	public GameObject GetItemByType(PickupTypes itemType)
	{
		switch (itemType)
		{
			case PickupTypes.ToiletPaper:
				return GetRandomArrayEl(toiletPapers);
			case PickupTypes.Sanitizer:
				return GetRandomArrayEl(sanitizers);
			case PickupTypes.Chips:
				return GetRandomArrayEl(chips);
		}
		return null;
	}

	GameObject GetRandomArrayEl(GameObject[] arr)
	{
		int index = Random.Range(0, arr.Length);
		return arr[index];
	}
}
