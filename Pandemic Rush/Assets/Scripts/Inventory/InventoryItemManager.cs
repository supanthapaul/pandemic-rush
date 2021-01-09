using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryItemManager : MonoBehaviour
{
	public PickupTypes itemType;
	public string prefix;

	public TextMeshProUGUI itemCountText;

	private void Start()
	{
		Inventory.onInventoryChange += UpdateItemCount;
		UpdateItemCount();
	}

	void UpdateItemCount()
	{
		itemCountText.text = "" + Inventory.instance.GetItemCount(itemType);
	}

	public void RemoveItem() {
		Inventory.instance.Remove(itemType);
	}
}
