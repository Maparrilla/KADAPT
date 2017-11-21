using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TreeSharpPlus;

public class InvokeEventB4 : MonoBehaviour {

	public GameObject Daniel;
	public GameObject R1;
	public GameObject R2;
	public RootMotion.FinalIK.FullBodyBipedEffector hand;
	public RootMotion.FinalIK.InteractionObject shroom;

	public GameObject Point1;
	public GameObject Point2;

	private BehaviorAgent behaviorAgent;

	// Use this for initialization
	void Start () {
		behaviorAgent = new BehaviorAgent(this.walk());
		BehaviorManager.Instance.Register(behaviorAgent);
		behaviorAgent.StartBehavior();
	}

	protected Node StartStory()
	{
		return new SequenceParallel (
			EatShroom (),
			Crowd ()
		);
	}

	protected Node EatShroom()
	{
		return new Sequence (
			Daniel.GetComponent<BehaviorMecanim> ().ST_PlayFaceGesture("EAT",1000));
	}

	protected Node walk()
	{
		Val<Vector3> r1p = Val.V(() => Point1.transform.position);
		Val<Vector3> r2p = Val.V(() => Point2.transform.position);

		return R1.GetComponent<BehaviorMecanim> ().Node_FaceAnimation ("Acknowledge", true);
	}

	protected Node Crowd()
	{
		Val<Vector3> r1p = Val.V(() => Point1.transform.position);
		Val<Vector3> r2p = Val.V(() => Point2.transform.position);

		return new Sequence (
			new SequenceParallel(
				R1.GetComponent<BehaviorMecanim>().Node_GoTo(r1p),
				R2.GetComponent<BehaviorMecanim>().Node_GoTo(r2p)
			),
			new SequenceParallel(
				R1.GetComponent<BehaviorMecanim>().ST_TurnToFace(r2p),
				R2.GetComponent<BehaviorMecanim>().ST_TurnToFace(r1p)
			),
			new DecoratorLoop(
				new SequenceParallel(
					new SequenceShuffle(
						R1.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("ACKNOWLEDGE", 1000),
						R1.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("HEADNOD", 1000),
						R1.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("LOOKAWAY", 1000),
						R1.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("SPEW", 1000)
					),
					new SequenceShuffle(
						R2.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("ACKNOWLEDGE", 1000),
						R2.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("HEADNOD", 1000),
						R2.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("LOOKAWAY", 1000),
						R2.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("SPEW", 1000)
					)
				)
			)

		);


	}

	// Update is called once per frame
	void Update () {
		Daniel.GetComponent<BehaviorMecanim> ().Node_StartInteraction (Val.V (() => hand), Val.V (() => shroom));
	}
}
