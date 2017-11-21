using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TreeSharpPlus;

public class InvokeEventB4 : MonoBehaviour {

	public GameObject Daniel;
	public GameObject R1;
	public GameObject R2;
	public GameObject Tom;
	public RootMotion.FinalIK.InteractionObject RedShroom;
	public RootMotion.FinalIK.InteractionObject GreenShroom;
	public RootMotion.FinalIK.FullBodyBipedEffector hand;
	public GameObject Boulder;
	public GameObject RedChest;
	public GameObject GreenChest;


	public GameObject Point1;
	public GameObject Point2;

	private BehaviorAgent behaviorAgent;
	private bool ateRed = false;
	private bool ateGreen = false;
	private bool grown = false;

	// Use this for initialization
	void Start () {
		behaviorAgent = new BehaviorAgent(this.StartStory());
		BehaviorManager.Instance.Register(behaviorAgent);
		behaviorAgent.StartBehavior();
	}

	protected Node StartStory()
	{
		return new SequenceParallel (
			Crowd(),
			new Sequence(
				walk(),
				Direct(),
				GoToRoom()
			)
		);
	}

	protected Node Direct()
	{
		System.Random rnd = new System.Random ();
		int num = rnd.Next (0, 2);
		if (1 == 1) {
			return new Sequence (
				Tom.GetComponent<BehaviorMecanim> ().ST_TurnToFace (Val.V (() => RedShroom.transform.position)),
				new SequenceParallel (
					new Sequence (
						Tom.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture ("Pointing", 2000)
					),
					new Sequence (
						new LeafWait (1000),
						Daniel.GetComponent<BehaviorMecanim> ().ST_TurnToFace (Val.V (() => RedShroom.transform.position)),
						Daniel.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture ("Surprised", 1000),
						Daniel.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => RedShroom.transform.position - (RedShroom.transform.position - Daniel.transform.position).normalized)),
						Daniel.GetComponent<BehaviorMecanim> ().ST_PlayFaceGesture ("EAT", 1000),
						new LeafInvoke(() => ateRed = true),
						new LeafInvoke(() => Daniel.transform.GetChild (7).gameObject.SetActive (true))
					)
				)
			);
		} else {
			return new Sequence (
				Tom.GetComponent<BehaviorMecanim> ().ST_TurnToFace (Val.V (() => GreenShroom.transform.position)),
				new SequenceParallel (
					new Sequence (
						Tom.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture ("Pointing", 2000)
					),
					new Sequence (
						new LeafWait (1000),
						Daniel.GetComponent<BehaviorMecanim> ().ST_TurnToFace (Val.V (() => GreenShroom.transform.position)),
						Daniel.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture ("Surprised", 1000),
						Daniel.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => GreenShroom.transform.position - (GreenShroom.transform.position - Daniel.transform.position).normalized)),
						Daniel.GetComponent<BehaviorMecanim> ().ST_PlayFaceGesture ("EAT", 1000),
						new LeafInvoke (() => ateGreen = true)
					)
				)
			);
		}
	}

	protected Node GoToRoom()
	{
		Func<bool> act1 = () => (ateRed);
		Func<bool> act2 = () => (ateGreen);

		return new Selector (
			new SequenceParallel (
				new LeafAssert (act1),
				new Sequence (
					Daniel.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => Boulder.transform.position - new Vector3 (0f, 2f, 2.5f))),
					Daniel.GetComponent<BehaviorMecanim>().ST_TurnToFace(Val.V(() => Tom.transform.position)),
					Daniel.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture ("CALLOVER", 1000),
					Daniel.GetComponent<BehaviorMecanim>().ST_TurnToFace(Val.V(() => Boulder.transform.position)),
					PushBoulder ()
				)
			),
			new SequenceParallel (
				new LeafAssert (act2),
				Daniel.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => new Vector3 (-5, 0, 16)))
			)
		);
	}

	protected Node PushBoulder()
	{
		return new Sequence (
			Daniel.GetComponent<BehaviorMecanim> ().ST_PlayBodyGesture ("TALKING ON PHONE", 2000),
			GetRedChest ()
		);
	}

	protected Node GetRedChest()
	{
		return new Sequence (
			Daniel.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => RedChest.transform.position - (RedChest.transform.position - Daniel.transform.position).normalized)),
			new SequenceParallel(
				new Sequence(
					new LeafWait(1000),
					new LeafInvoke(() => RedChest.SetActive(false))
				),
				Daniel.GetComponent<BehaviorMecanim> ().ST_PlayBodyGesture ("PICKUPLEFT", 1000)
			),
			ShowChest()
		);
	}

	protected Node ShowChest()
	{
		//Vector3 temp = new Vector3 (0f, 0f, 11.5f);
		return new Sequence (
			Daniel.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => new Vector3 (0f, 0f, 11.5f) - (new Vector3 (0f, 0f, 11.5f)- Daniel.transform.position).normalized * 1.5f)),
			new SequenceParallel (
				new Sequence (
					new LeafWait (1000),
					new LeafInvoke (() => RedChest.SetActive (true)),
					new LeafInvoke(() => RedChest.transform.position = new Vector3 (0f, 0f, 11.5f))
				),
				Daniel.GetComponent<BehaviorMecanim> ().ST_PlayBodyGesture ("PICKUPLEFT", 2000)
			)
		);
	}

	protected Node walk()
	{
		return new SequenceParallel (
			Daniel.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => Tom.transform.position - new Vector3 (0, 0, 1.5f))),
			Tom.GetComponent<BehaviorMecanim> ().ST_TurnToFace (Val.V (() => Daniel.transform.position))
		);
	}

	protected Node Crowd()
	{
		Val<Vector3> r1p = Val.V(() => Point1.transform.position);
		Val<Vector3> r2p = Val.V(() => Point2.transform.position);

		return new Sequence (
			new SequenceParallel (
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
						R1.GetComponent<BehaviorMecanim> ().ST_PlayFaceGesture ("LOOKAWAY", 1000),
						R1.GetComponent<BehaviorMecanim> ().ST_PlayFaceGesture ("SPEW", 1000)
					),
					new SequenceShuffle (
						R2.GetComponent<BehaviorMecanim> ().ST_PlayFaceGesture ("ACKNOWLEDGE", 1000),
						R2.GetComponent<BehaviorMecanim> ().ST_PlayFaceGesture ("HEADNOD", 1000),
						R2.GetComponent<BehaviorMecanim> ().ST_PlayFaceGesture ("LOOKAWAY", 1000),
						R2.GetComponent<BehaviorMecanim> ().ST_PlayFaceGesture ("SPEW", 1000)
					)
				)
			)

		);


	}

	// Update is called once per frame
	void Update () {
		if (ateRed) {
			Daniel.transform.Find ("Light").GetComponent<Light> ().intensity = Mathf.Abs(Time.frameCount % 20 - 10);
		} else if (ateGreen && !grown) {
			Daniel.transform.localScale += new Vector3 (0.1f, 0.1f, 0.1f);
			if (Daniel.transform.localScale.x > 2.0f) {
				grown = true;
			}
		}
	}
}
