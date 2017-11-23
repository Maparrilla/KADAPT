using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using TreeSharpPlus;

public class InvokeEventB4_P2 : MonoBehaviour {

	public GameObject Daniel;
	public GameObject R1;
	public GameObject R2;
	public GameObject Tom;
	public GameObject Harry;
	public GameObject RedMushroom;
	public GameObject GreenMushroom;
	public RootMotion.FinalIK.InteractionObject RedShroom;
	public RootMotion.FinalIK.InteractionObject GreenShroom;
	public RootMotion.FinalIK.FullBodyBipedEffector hand;
	public GameObject Boulder;
	public GameObject RedChest;
	public GameObject GreenChest;


	public GameObject Point1;
	public GameObject Point2;


	private float raycastDistance = 100.0f;
	private BehaviorAgent behaviorAgent;
	private bool ateRed = false;
	private bool ateGreen = false;
	private bool grown = false;
	private bool cheer = false;
	private bool hgrown = true;
	private bool cry = false;
	private Camera mainCamera;
	private bool uncheckedClickReceived;

	// Use this for initialization
	void Start () {
		mainCamera = Camera.main;
		uncheckedClickReceived = false;
		behaviorAgent = new BehaviorAgent(this.InteractiveBehaviorTree());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();
	}

	private string currentArc = "Intro";
	private string eatenMushroom = "None";
	private GameObject selectedObject;
	private Vector3 selectedPoint;

	protected Node InteractiveBehaviorTree()
	{
		return new SequenceParallel (
			Story(),
			MonitorInput()
		);
	}

	protected Node Story()
	{
		return new DecoratorLoop (
			new Selector(
				SelectArc_Intro(),
				SelectArc_AteMushroom(),
				SelectArc_CarryOut(),
				SelectArc_EvilWins()
			)
		);
	}


	protected Node MonitorInput()
	{
		Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
		RaycastHit hit;
		Physics.Raycast (ray, out hit, raycastDistance);

		return new DecoratorLoop (
			new DecoratorForceStatus (
				RunStatus.Success,
				new Sequence (
					// Runs if a click input has been received
					new LeafAssert(() => uncheckedClickReceived),
					new LeafInvoke(() => uncheckedClickReceived = false),
					new LeafInvoke(() => Debug.Log("CLICK RECEIVED")),
					new LeafInvoke(() => ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward)),
					new Selector (
						new Sequence (
							new LeafAssert(() => Physics.Raycast (ray, out hit, raycastDistance)),
							new Selector (
								// Interact with selected object
								new Sequence (
									new LeafAssert(() => hit.collider.CompareTag ("Interactable")),
									new LeafInvoke(() => Debug.Log(hit.transform.name)),
									new LeafInvoke(() => selectedObject = hit.transform.gameObject),
									new Selector (
										new Sequence (
											new LeafAssert(() => selectedObject == Tom),
											new LeafInvoke(() => Debug.Log("TOM SELECTED")),
											TalkToTom()
										),
										new Sequence (
											new LeafAssert(() => selectedObject == RedMushroom),
											new LeafInvoke(() => Debug.Log("RED MUSHROOM SELECTED")),
											EatRedMushroom()
										),
										new Sequence (
											new LeafAssert(() => selectedObject == GreenMushroom),
											new LeafInvoke(() => Debug.Log("GREEN MUSHROOM SELECTED")),
											EatGreenMushroom()
										),
										new Sequence (
											new LeafAssert(() => selectedObject == Boulder),
											new LeafInvoke(() => Debug.Log("BOULDER SELECTED")),
											TryBoulder()
										),
										new Sequence (
											new LeafAssert(() => selectedObject == RedChest),
											new LeafInvoke(() => Debug.Log("RED CHEST SELECTED")),
											PickUpRedChest()
										),
										new Sequence (
											new LeafAssert(() => selectedObject == GreenChest),
											new LeafInvoke(() => Debug.Log("GREEN CHEST SELECTED")),
											PickUpGreenChest()
										)
									)
								),
								new Sequence (
									Daniel.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => hit.point))
								)
							)
						),
						new Sequence (
							new LeafInvoke (() => Debug.Log("Click not registered"))
						)
					)
				)
			)
		);
	}

	protected Node TalkToTom()
	{
		System.Random rnd = new System.Random ();
		int num = rnd.Next (0, 2);
		if (num == 1) {
			return new Sequence (
				Daniel.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => Tom.transform.position - new Vector3 (0, 0, 1.5f))),
				Daniel.GetComponent<BehaviorMecanim> ().ST_TurnToFace (Val.V (() => Tom.transform.position)),
				Tom.GetComponent<BehaviorMecanim> ().ST_TurnToFace (Val.V (() => RedShroom.transform.position)),
				Tom.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture ("Pointing", 2000)
			);
		} else {
			return new Sequence (
				Daniel.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => Tom.transform.position - new Vector3 (0, 0, 1.5f))),
				Daniel.GetComponent<BehaviorMecanim> ().ST_TurnToFace (Val.V (() => Tom.transform.position)),
				Tom.GetComponent<BehaviorMecanim> ().ST_TurnToFace (Val.V (() => GreenShroom.transform.position)),
				Tom.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture ("Pointing", 2000)
			);
		}
	}

	protected Node EatRedMushroom()
	{
		return new Sequence (
			Daniel.GetComponent<BehaviorMecanim> ().ST_TurnToFace (Val.V (() => RedShroom.transform.position)),
			Daniel.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture ("Surprised", 1000),
			Daniel.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => RedShroom.transform.position - (RedShroom.transform.position - Daniel.transform.position).normalized)),
			Daniel.GetComponent<BehaviorMecanim> ().ST_PlayFaceGesture ("EAT", 1000),
			new LeafInvoke (() => currentArc = "AteMushroom"),
			new LeafInvoke (() => Debug.Log(currentArc)),
			new LeafInvoke (() => eatenMushroom = "Red"),
			new LeafInvoke (() => Daniel.transform.GetChild (7).gameObject.SetActive (true))
		);
	}

	protected Node EatGreenMushroom()
	{
		return new Sequence (
			Daniel.GetComponent<BehaviorMecanim> ().ST_TurnToFace (Val.V (() => GreenShroom.transform.position)),
			Daniel.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture ("Surprised", 1000),
			Daniel.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => GreenShroom.transform.position - (GreenShroom.transform.position - Daniel.transform.position).normalized)),
			Daniel.GetComponent<BehaviorMecanim> ().ST_PlayFaceGesture ("EAT", 1000),
			new LeafInvoke (() => currentArc = "AteMushroom"),
			new LeafInvoke (() => eatenMushroom = "Green")
		);
	}

	protected Node TryBoulder()
	{
		return new Selector (
			// Has super strength (ate red mushroom)
			new Sequence (
				new LeafAssert (() => currentArc == "AteMushroom"),
				new LeafAssert (() => eatenMushroom == "Red"),
				Daniel.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => Boulder.transform.position - new Vector3 (0f, 2f, 2.5f))),
				Daniel.GetComponent<BehaviorMecanim> ().ST_TurnToFace (Val.V (() => Tom.transform.position)),
				Tom.GetComponent<BehaviorMecanim> ().ST_TurnToFace (Val.V (() => Daniel.transform.position)),
				Daniel.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture ("CALLOVER", 1000),
				Daniel.GetComponent<BehaviorMecanim> ().ST_TurnToFace (Val.V (() => Boulder.transform.position)),
				new LeafInvoke(() => Boulder.GetComponent<NavMeshObstacle> ().carving = false),
				new LeafInvoke(() => Boulder.GetComponent<NavMeshObstacle> ().radius = 0.1f),
				Daniel.GetComponent<BehaviorMecanim> ().ST_PlayBodyGesture ("TALKING ON PHONE", 3000),
				new LeafInvoke(() => Boulder.GetComponent<NavMeshObstacle> ().carving = true),
				new LeafInvoke(() => Boulder.GetComponent<NavMeshObstacle> ().radius = 0.5f)
			),
			// Doesn't have super strength
			new Sequence (
				new LeafInvoke (() => Debug.Log ("That looks heavy. With super-strength, I might be able to move it."))
			)
		);
	}

	protected Node PickUpRedChest()
	{
		return new Selector (
			new Sequence (
				new LeafAssert (() => currentArc == "AteMushroom"),
				new LeafAssert (() => eatenMushroom == "Red"),
				Daniel.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => RedChest.transform.position - (RedChest.transform.position - Daniel.transform.position).normalized)),
				new Sequence (
					Daniel.GetComponent<BehaviorMecanim> ().ST_PlayBodyGesture ("PICKUPLEFT", 1000),
					new LeafWait (500),
					new LeafInvoke (() => RedChest.SetActive (false)),
					new LeafInvoke (() => Harry.transform.position = new Vector3 (15, 0, 21))
				),
				new LeafInvoke (() => RedChest.transform.position = new Vector3 (0f, 0f, 11.5f)),
				new LeafInvoke (() => currentArc = "CarryOut") // carry chest out
			),
			new Sequence (
				new LeafInvoke (() => Debug.Log ("This looks a bit heavy for me..."))
			)
		);
	}

	protected Node PickUpGreenChest()
	{
		return new Selector (
			new Sequence (
				new LeafAssert(() => currentArc == "AteMushroom"),
				new LeafAssert(() => eatenMushroom == "Green"),
				Daniel.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => GreenChest.transform.position - new Vector3(0,4,0) - (GreenChest.transform.position - new Vector3(0,4,0) - Daniel.transform.position).normalized*3f)),
				new Sequence (
					Daniel.GetComponent<BehaviorMecanim> ().ST_TurnToFace(Val.V(() => GreenChest.transform.position)),
					Daniel.GetComponent<BehaviorMecanim> ().ST_PlayBodyGesture ("DUCK", 1000),
					new LeafWait (500),
					new LeafInvoke(() => GreenChest.SetActive(false)),
					new LeafInvoke (() => Harry.transform.position = new Vector3 (-15, 0, 21))
				),
				new LeafInvoke (() => GreenChest.transform.position = new Vector3 (0f, 0f, 11.5f)),
				new LeafInvoke (() => currentArc = "CarryOut") // carry chest out
			),
			new Sequence (
				new LeafInvoke (() => Debug.Log ("That looks a little too high for me..."))
			)
		);
	}



	protected Node SelectArc_Intro()
	{
		Func<bool> arc = () => (currentArc == "Intro");
		Val<Vector3> r1p = Val.V(() => Point1.transform.position);
		Val<Vector3> r2p = Val.V(() => Point2.transform.position);
		
		return new SequenceParallel (
			new DecoratorLoop (new LeafAssert (arc)),
			new Sequence (
				new LeafAssert(arc),
				new LeafInvoke(() => Debug.Log ("Running Intro arc")),
				new SequenceParallel (
					Tom.GetComponent<BehaviorMecanim> ().ST_TurnToFace (Val.V (() => Daniel.transform.position)),
					R1.GetComponent<BehaviorMecanim> ().Node_GoTo (r1p),
					R2.GetComponent<BehaviorMecanim> ().Node_GoTo (r2p)
				),
				new SequenceParallel (
					R1.GetComponent<BehaviorMecanim> ().ST_TurnToFace (r2p),
					R2.GetComponent<BehaviorMecanim> ().ST_TurnToFace (r1p)
				),
				new DecoratorLoop (
					new SequenceParallel (
						new SequenceShuffle (
							R1.GetComponent<BehaviorMecanim> ().ST_PlayFaceGesture ("ACKNOWLEDGE", 1000),
							R1.GetComponent<BehaviorMecanim> ().ST_PlayFaceGesture ("HEADNOD", 1000),
							R1.GetComponent<BehaviorMecanim> ().ST_PlayFaceGesture ("LOOKAWAY", 1000)
						),
						new SequenceShuffle (
							R2.GetComponent<BehaviorMecanim> ().ST_PlayFaceGesture ("ACKNOWLEDGE", 1000),
							R2.GetComponent<BehaviorMecanim> ().ST_PlayFaceGesture ("HEADNOD", 1000),
							R2.GetComponent<BehaviorMecanim> ().ST_PlayFaceGesture ("LOOKAWAY", 1000)
						)
					)
				)
			)
		);
	}

	protected Node SelectArc_AteMushroom()
	{
		Func<bool> arc = () => (currentArc == "AteMushroom");

		return new SequenceParallel (
			new DecoratorLoop (new LeafAssert (arc)),
			new Sequence (
				new LeafAssert (arc),
				new LeafInvoke (() => Debug.Log ("Running AteMushroom arc")),
				new SequenceParallel (
					R1.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => new Vector3 (0f, 0f, 11.5f) - (new Vector3 (0f, 0f, 11.5f) - R1.transform.position).normalized * 3f)),
					R2.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => new Vector3 (0f, 0f, 11.5f) - (new Vector3 (0f, 0f, 11.5f) - R2.transform.position).normalized * 3f))
				)
			)
		);
	}

	protected Node SelectArc_CarryOut()
	{
		Func<bool> arc = () => (currentArc == "CarryOut");

		return new SequenceParallel (
			new DecoratorLoop (new LeafAssert (arc)),
			new Sequence (
				new LeafAssert(arc),
				new LeafInvoke (() => Debug.Log ("Running CarryOut arc")),
				Daniel.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => new Vector3 (0f, 0f, 11.5f) - (new Vector3 (0f, 0f, 11.5f) - Daniel.transform.position).normalized * 1.5f)),
				Daniel.GetComponent<BehaviorMecanim> ().ST_PlayBodyGesture ("PICKUPLEFT", 1000),
				new LeafInvoke (() => RedChest.SetActive (true)),
				new LeafInvoke (() => GreenChest.SetActive (true)),
				new SequenceParallel (
					R1.GetComponent<BehaviorMecanim> ().Node_HandAnimation ("CHEER", true),
					R2.GetComponent<BehaviorMecanim> ().Node_HandAnimation ("CHEER", true)
				),
				new LeafInvoke(() => currentArc = "EvilWins")
			)
		);
	}

	protected Node SelectArc_EvilWins()
	{
		Func<bool> arc = () => (currentArc == "EvilWins");

		return new SequenceParallel (
			new DecoratorLoop (new LeafAssert (arc)),
			new Sequence (
				new LeafAssert (arc),
				new LeafInvoke (() => Debug.Log ("Running EvilWins arc")),
				// Harry Gets Powerful
				new Selector (
					new Sequence (
						new LeafAssert (() => eatenMushroom == "Red"),
						new LeafInvoke (() => Harry.SetActive (true)),
						Harry.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => RedShroom.transform.position - (RedShroom.transform.position - Harry.transform.position).normalized)),
						Harry.GetComponent<BehaviorMecanim> ().ST_PlayFaceGesture ("EAT", 1000),
						new LeafInvoke (() => Harry.transform.GetChild (4).gameObject.SetActive (true))
					),
					new Sequence (
						new LeafAssert (() => eatenMushroom == "Green"),
						new LeafInvoke (() => Debug.Log("DO SELECTORS EVER REACH ALTERNATIVE?")),
						new LeafInvoke (() => Harry.SetActive (true)),
						Harry.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => new Vector3 (-5.0f, 0.0f, 12.0f))),
						// Harry.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => GreenShroom.transform.position - (GreenShroom.transform.position - Harry.transform.position).normalized)),
						Harry.GetComponent<BehaviorMecanim> ().ST_PlayFaceGesture ("EAT", 1000),
						new LeafInvoke (() => hgrown = false)
					)
				),
				// Harry Fuckin Rocks Daniel... The crowd is sad.
				new Sequence (
					Harry.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => Daniel.transform.position - (Daniel.transform.position - Harry.transform.position).normalized * 2)),
					Daniel.GetComponent<BehaviorMecanim> ().ST_TurnToFace (Val.V (() => Harry.transform.position)),
					Harry.GetComponent<BehaviorMecanim> ().ST_PlayFaceGesture ("SPEW", 700),
					Daniel.GetComponent<BehaviorMecanim> ().Node_BodyAnimation ("DYING", true),
					new SequenceParallel (
						R1.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture ("Surprised", 1000),
						R2.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture ("Surprised", 1000)
					),
					new SequenceParallel (
						R1.GetComponent<BehaviorMecanim> ().Node_HandAnimation ("Cry", true),
						R2.GetComponent<BehaviorMecanim> ().Node_HandAnimation ("Cry", true)
					)
				),
				// Harry Walks Away, the evil bastard he is
				new Selector (
					new Sequence (
						new LeafAssert (() => eatenMushroom == "Red"),
						Harry.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => new Vector3(16,0,21))),
						new LeafInvoke (() => Harry.SetActive(false))
					),
					new Sequence (
						new LeafAssert (() => eatenMushroom == "Green"),
						Harry.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => new Vector3(-16,0,21))),
						new LeafInvoke (() => Harry.SetActive(false))
					)
				)
			)
		);
	}


	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1"))
			uncheckedClickReceived = true;

		if (eatenMushroom == "Red") {
			Daniel.transform.Find ("Light").GetComponent<Light> ().intensity = Mathf.Abs (Time.frameCount % 20 - 10);
			Harry.transform.Find ("Light").GetComponent<Light> ().intensity = Mathf.Abs (Time.frameCount % 20 - 10);
		} else if (eatenMushroom == "Green" && !grown) {
			Daniel.transform.localScale += new Vector3 (0.1f, 0.1f, 0.1f);
			if (Daniel.transform.localScale.x > 2.0f) {
				grown = true;
			}
		} else if (eatenMushroom == "Green" && !hgrown) {
			Harry.transform.localScale += new Vector3 (0.1f, 0.1f, 0.1f);
			if (Harry.transform.localScale.x > 2.0f) {
				hgrown = true;
			}
		}
	}
}
