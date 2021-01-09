using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine.UI;
using TMPro;

public enum PickupTypes
	{
		ToiletPaper,
		Sanitizer,
		Chips
	}
public class Inventory : MonoBehaviour
{
	// Event for communicating changes in inventory
	public delegate void OnInventoryChange();
	public static event OnInventoryChange onInventoryChange;

	public int maxInventorySize = 20;
	public UIView inventoryPanel;
	public Transform inventoryItemsParent;
	public GameObject inventoryItemPrefab;
	// offset by which dropped items spawn
	public Vector3 itemSpawnOffset;

	Dictionary<int, int> items = new Dictionary<int, int>();

	// Static instance
	public static Inventory instance;
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		InitializeInventory();
		PrintInventory();
	}

	private void Start()
	{

	}
	// Initialize inventory with having 0 of each item
	void InitializeInventory()
	{
		foreach (PickupTypes pickupType in Enum.GetValues(typeof(PickupTypes))) {
			items.Add((int)pickupType, 0);
		}
		// Populate inventory UI
		foreach (KeyValuePair<int, int> item in items)
		{
			GameObject inventoryItem = Instantiate(inventoryItemPrefab, inventoryItemsParent, false);
			// set item name
			inventoryItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + (PickupTypes) item.Key;
			// setup inventory item type
			inventoryItem.GetComponent<InventoryItemManager>().itemType = (PickupTypes) item.Key;
		}

		// update inventory UIs
		if(onInventoryChange != null)
			onInventoryChange();
	}

	private void Update() {
		// TODO: toggle inventory panel
		if(Input.GetButton("Inventory")) {
			inventoryPanel.Show();
			LockUnlockCursor(true);
		}
		else if(inventoryPanel.IsVisible) {
			inventoryPanel.Hide();
			LockUnlockCursor(false);
		}
	}

	// add to inventory
	public bool Add(PickupTypes pickupType) {
		// add item to specified slot if inventory isn't full
		if(GetInventorySize() < maxInventorySize) {
			items[(int)pickupType]++;
			Debug.Log("1 item added to: " + pickupType);
			// Fire onInventoryChange event
			if(onInventoryChange != null) {
				onInventoryChange();
			}
			PrintInventory();
			return true;
		}
		return false;
	}

	public bool Remove(PickupTypes pickupType,int count=1, bool addToManager=true) {
		// remove count from specified slot if it's more than 0
		if(items[(int)pickupType] >= count) {
			items[(int)pickupType] -= count;
			Debug.Log(count +" items removed from: " + pickupType);
			// spawn the dropped items in front of player
			Transform orientation = transform.GetChild(0);
			for (int i = 0; i < count; i++)
			{
				SpawnItemByType(pickupType,orientation, addToManager);
			}
			// Fire onInventoryChange event
			if(onInventoryChange != null) {
				onInventoryChange();
			}
			PrintInventory();
			return true;
		}
		return false;
	}

	// removes all items from inventory and returns number of items
	public int DropInventory(Transform[] spawnPositions) {
		int count = GetInventorySize();
		List<int> keys = new List<int>(items.Keys);
		for (int i = 0; i < keys.Count; i++)
		{
			int currValue= items[keys[i]];
			for (int j = 0; j < currValue; j++)
			{
				items[keys[i]]--;
				Transform randomSpawnPos = spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Length)];
				GameObject item = SpawnItemByType((PickupTypes)keys[i],randomSpawnPos, false, randomSpawnPos);

				//item.GetComponent<Rigidbody>().isKinematic = true;
			}
			//Remove((PickupTypes)keys[i], items[keys[i]], false, true);
		}
		// Fire onInventoryChange event
			if(onInventoryChange != null) {
				onInventoryChange();
			}
		return count;
	}

	// spawns a item in front of player
	GameObject SpawnItemByType(PickupTypes itemType, Transform targetPos, bool addToManager=true, Transform parent=null) {
		GameObject itemToSpawn = ItemsManager.instance.GetItemByType(itemType);
		// spawn location
		//Transform orientation = transform.GetChild(0);
		Vector3 pos = targetPos.position + (targetPos.forward * itemSpawnOffset.z);
		pos.y += itemSpawnOffset.y;
		GameObject item;
		if(parent) {
			item = Instantiate(itemToSpawn, parent);
			item.transform.localPosition = Vector3.zero;
			if(!addToManager) {
				item.layer = LayerMask.NameToLayer("Default");
				Destroy(item.GetComponent<Rigidbody>());
				Destroy(item.GetComponent<Collider>());
			}
		}
		else {
			item = Instantiate(itemToSpawn, pos, UnityEngine.Random.rotation);
		}
		// re-add lost item reference to the items manager
		if(addToManager) ItemsManager.instance.ReAddItem(item);
		return item;
	}

	// returns the count of a specific item
	public int GetItemCount(PickupTypes itemType) {
		return items[(int)itemType];
	}

	public int GetInventorySize() {
		int size = 0;
		foreach (KeyValuePair<int, int> item in items)
		{
			size += item.Value;
		}
		return size;
	}

	public bool IsFull() {
		return GetInventorySize() >= maxInventorySize;
	}
	// prints the whole inventory
	void PrintInventory() {
		foreach (KeyValuePair<int, int> kvp in items)
		{
			Debug.Log("Key = "+kvp.Key+ ", Value = "+kvp.Value);
		}
	}

	public void LockUnlockCursor(bool unlock) {
		if(unlock) {
			// unlock cursor
			if(Cursor.lockState == CursorLockMode.Locked) {
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}
		else {
			// lock cursor
			if(Cursor.lockState == CursorLockMode.None) {
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
		}
	}

	private void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		// draw item spawn location sphere
		Gizmos.DrawSphere(transform.position + itemSpawnOffset, 0.1f);
	}
}
