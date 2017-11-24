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
	public GameObject Harry;
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
	private bool cheer = false;
	private bool hgrown = true;
	private bool cry = false;

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
			Evil(),
			new Sequence(
				walk(),
				Direct()
			)
		);
	}

	protected Node Direct()
	{
		System.Random rnd = new System.Random ();
		int num = rnd.Next (0, 2);
		if (num == 1) {
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
						new LeafInvoke(() => Daniel.transform.GetChild (7).gameObject.SetActive (true)),
						GetRedChest()
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
						new LeafInvoke (() => ateGreen = true),
						GetGreenChest()
					)
				)
			);
		}
	}

	protected Node GetRedChest()
	{
		return new Sequence (
			Daniel.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => Boulder.transform.position - new Vector3 (0f, 2f, 2.5f))),
			Daniel.GetComponent<BehaviorMecanim> ().ST_TurnToFace (Val.V (() => Tom.transform.position)),
			Daniel.GetComponent<BehaviorMecanim> ().ST_PlayHandGesture ("CALLOVER", 1000),
			Daniel.GetComponent<BehaviorMecanim> ().ST_TurnToFace (Val.V (() => Boulder.transform.position)),
			Daniel.GetComponent<BehaviorMecanim> ().ST_PlayBodyGesture ("TALKING ON PHONE", 2000),
			Daniel.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => RedChest.transform.position - (RedChest.transform.position - Daniel.transform.position).normalized)),
			new SequenceParallel (
				new Sequence (
					new LeafWait (1500),
					new LeafInvoke (() => RedChest.SetActive (false)),
					new LeafInvoke (() => Harry.transform.position = new Vector3 (15, 0, 21))
				),
				Daniel.GetComponent<BehaviorMecanim> ().ST_PlayBodyGesture ("PICKUPLEFT", 1000)
			),
			new LeafInvoke (() => RedChest.transform.position = new Vector3 (0f, 0f, 11.5f)),
			ShowChest ()
		);
	}

	protected Node GetGreenChest()
	{
		return new Sequence (
			Daniel.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => GreenChest.transform.position - new Vector3(0,4,0) - (GreenChest.transform.position - new Vector3(0,4,0) - Daniel.transform.position).normalized*3f)),
			new SequenceParallel(
				new Sequence(
					new LeafWait(1000),
					new LeafInvoke(() => GreenChest.SetActive(false)),
					new LeafInvoke (() => Harry.transform.position = new Vector3 (-15, 0, 21))
				),
				Daniel.GetComponent<BehaviorMecanim> ().ST_TurnToFace(Val.V(() => GreenChest.transform.position)),
				Daniel.GetComponent<BehaviorMecanim> ().ST_PlayBodyGesture ("DUCK", 1000)
			),
			new LeafInvoke (() => GreenChest.transform.position = new Vector3 (0f, 0f, 11.5f)),
			ShowChest()
		);
	}

	protected Node ShowChest()
	{
		//Vector3 temp = new Vector3 (0f, 0f, 11.5f);
		return new Sequence (
			Daniel.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => new Vector3 (0f, 0f, 11.5f) - (new Vector3 (0f, 0f, 11.5f) - Daniel.transform.position).normalized * 1.5f)),
			new SequenceParallel (
				new Sequence (
					new LeafWait (1000),
					new LeafInvoke (() => RedChest.SetActive (true)),
					new LeafInvoke (() => GreenChest.SetActive (true)),
					new LeafInvoke (() => cheer = true)
				),
				Daniel.GetComponent<BehaviorMecanim> ().ST_PlayBodyGesture ("PICKUPLEFT", 1000)
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

		Func<bool> cheerAssert = () => (!cheer);
		Func<bool> cryAssert = () => (!cry);

		Node talking = new DecoratorLoop (
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
		               );
		Node TalkLoop = new DecoratorLoop (new SequenceParallel (new DecoratorLoop (new LeafAssert (cheerAssert)), talking));
		Node PreCry = new DecoratorLoop (new LeafAssert (cryAssert));

		return new Sequence (
			new SequenceParallel (
				R1.GetComponent<BehaviorMecanim> ().Node_GoTo (r1p),
				R2.GetComponent<BehaviorMecanim> ().Node_GoTo (r2p)
			),
			new SequenceParallel (
				R1.GetComponent<BehaviorMecanim> ().ST_TurnToFace (r2p),
				R2.GetComponent<BehaviorMecanim> ().ST_TurnToFace (r1p)
			),
			new DecoratorForceStatus (RunStatus.Success, TalkLoop),
			Celebrate(),
			new DecoratorForceStatus(RunStatus.Success, PreCry),
			new SequenceParallel (
				R1.GetComponent<BehaviorMecanim> ().Node_HandAnimation ("Cry", true),
				R2.GetComponent<BehaviorMecanim> ().Node_HandAnimation ("Cry", true)
			)
		);
	}

	protected Node Celebrate()
	{
		return new Sequence (
			new SequenceParallel (
				R1.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => new Vector3 (0f, 0f, 11.5f) - (new Vector3 (0f, 0f, 11.5f) - R1.transform.position).normalized * 3f)),
				R2.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => new Vector3 (0f, 0f, 11.5f) - (new Vector3 (0f, 0f, 11.5f) - R2.transform.position).normalized * 3f))
			),
			new SequenceParallel (
				R1.GetComponent<BehaviorMecanim> ().Node_HandAnimation ("CHEER", true),
				R2.GetComponent<BehaviorMecanim> ().Node_HandAnimation ("CHEER", true)
			)
		);
	}

	protected Node Evil()
	{
		Func<bool> act = () => (!cheer);
		Node actLoop = new DecoratorForceStatus (RunStatus.Success, new DecoratorLoop (new LeafAssert (act)));

		return new Sequence (
			actLoop,
			HarryPower(),
			HarryKill(),
			HarryRun()
		);



	}

	protected Node HarryPower()
	{
		Func<bool> RedAssert = () => (ateRed);
		Func<bool> GreenAssert = () => (ateGreen);
		Node RedTrigger = new DecoratorLoop (new LeafAssert (RedAssert));
		Node GreenTrigger = new DecoratorLoop (new LeafAssert (GreenAssert));


		return new Selector (
			new Sequence (
				new LeafInvoke (() => print (RedAssert ())),
				new LeafAssert (RedAssert),
				new LeafInvoke (() => Harry.SetActive (true)),
				Harry.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => RedShroom.transform.position - (RedShroom.transform.position - Harry.transform.position).normalized)),
				Harry.GetComponent<BehaviorMecanim> ().ST_PlayFaceGesture ("EAT", 1000),
				new LeafInvoke (() => Harry.transform.GetChild (4).gameObject.SetActive (true))
			),
			new Sequence (
				new LeafInvoke (() => print (GreenAssert ())),
				new LeafAssert (GreenAssert),
				new LeafInvoke (() => Harry.SetActive (true)),
				Harry.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => GreenShroom.transform.position - (GreenShroom.transform.position - Harry.transform.position).normalized)),
				Harry.GetComponent<BehaviorMecanim> ().ST_PlayFaceGesture ("EAT", 1000),
				new LeafInvoke (() => hgrown = false)
			)
		);
	}

	protected Node HarryKill()
	{
		return new Sequence (
			Harry.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => Daniel.transform.position - (Daniel.transform.position - Harry.transform.position).normalized * 2)),
			Daniel.GetComponent<BehaviorMecanim> ().ST_TurnToFace (Val.V (() => Harry.transform.position)),
			Harry.GetComponent<BehaviorMecanim> ().ST_PlayFaceGesture ("SPEW", 700),
			Daniel.GetComponent<BehaviorMecanim> ().Node_BodyAnimation ("DYING", true),
			new LeafInvoke (() => cry = true)
		);
	}

	protected Node HarryRun()
	{
		Func<bool> RedAssert = () => (ateRed);
		Func<bool> GreenAssert = () => (ateGreen);
		Node RedTrigger = new DecoratorLoop (new LeafAssert (RedAssert));
		Node GreenTrigger = new DecoratorLoop (new LeafAssert (GreenAssert));
			

		return new Selector (
			new Sequence (
				new LeafInvoke (() => print (RedAssert ())),
				new LeafAssert (RedAssert),
				Harry.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => new Vector3(16,0,21))),
				new LeafInvoke (() => Harry.SetActive(false))
			),
			new Sequence (
				new LeafInvoke (() => print (GreenAssert ())),
				new LeafAssert (GreenAssert),
				Harry.GetComponent<BehaviorMecanim> ().Node_GoTo (Val.V (() => new Vector3(-16,0,21))),
				new LeafInvoke (() => Harry.SetActive(false))
			)
		);
	}

	// Update is called once per frame
	void Update () {
		if (ateRed) {
			Daniel.transform.Find ("Light").GetComponent<Light> ().intensity = Mathf.Abs (Time.frameCount % 20 - 10);
			Harry.transform.Find ("Light").GetComponent<Light> ().intensity = Mathf.Abs (Time.frameCount % 20 - 10);
		} else if (ateGreen && !grown) {
			Daniel.transform.localScale += new Vector3 (0.1f, 0.1f, 0.1f);
			if (Daniel.transform.localScale.x > 2.0f) {
				grown = true;
			}
		} else if (ateGreen && !hgrown) {
			Harry.transform.localScale += new Vector3 (0.1f, 0.1f, 0.1f);
			if (Harry.transform.localScale.x > 2.0f) {
				hgrown = true;
			}
		}
	}
}
