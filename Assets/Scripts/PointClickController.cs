using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* PointClickController script handles click inputs and sends them to
 * the interactive narrative tree */
public class PointClickController : MonoBehaviour {

	public float raycastDistance;
	public GameObject updater; // The gameobject that handles the story's behavior tree

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

//		// ignore clicks on Event System
//		if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject ())
//			return;
//
//		if (Input.GetButtonDown ("Fire1"))
//		{
//			// Handle I/O Pointer Events
//			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//			RaycastHit hit;
//
//			if (Physics.Raycast (ray, out hit, raycastDistance))
//			{
//				if (hit.collider.CompareTag ("Interactable"))
//				{
//					GameObject selectedGameObject = hit.transform.gameObject;
//					updater.SendMessage ("Interact", selectedGameObject);
//				}
//				else
//				{
//					updater.SendMessage ("GoTo", hit.point);
//				}
//			}
//		}
	}
}
