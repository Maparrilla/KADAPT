using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TreeSharpPlus;

public class InvokeEventB3 : MonoBehaviour {

    public GameObject Daniel;
    public GameObject Richard;
    public GameObject Ethan;
	public GameObject Ball;

    public GameObject Point1;
    public GameObject Point2;
    public GameObject Point3;
    public GameObject Point4;
    public GameObject Point5;
    public GameObject Point6;

    private BehaviorAgent behaviorAgent;

    protected Node DanRichGreeting()
    {
        return new Sequence(
            Daniel.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 3500),
            Richard.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 3500),
            Daniel.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("BeingCocky", 1633),
            Richard.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("LookAway", 2000),
            Richard.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 3500),
            Daniel.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 3500));
    }

    protected Node RichEthanGreeting()
    {
        return new Sequence(
            Richard.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 3500),
            Ethan.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 3500),
            Richard.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("BeingCocky", 1633),
            Ethan.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("LookAway", 2000),
            Ethan.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 3500),
            Richard.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 3500));
    }

    protected Node DanEthanGreeting()
    {
        return new Sequence(
            Ethan.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 3500),
            Daniel.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 3500),
            Ethan.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("BeingCocky", 1633),
            Daniel.GetComponent<BehaviorMecanim>().ST_PlayFaceGesture("LookAway", 2000),
            Daniel.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 3500),
            Ethan.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 3500));
    }

    protected Node ApproachAndOrientDR(Val<Vector3> DanielPos, Val<Vector3> RichardPos)
    {
        return new Sequence(
            Daniel.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(RichardPos,1.0f),
            new SequenceParallel(
                Daniel.GetComponent<BehaviorMecanim>().Node_OrientTowards(RichardPos),
                Richard.GetComponent<BehaviorMecanim>().Node_OrientTowards(DanielPos)));
    }

    protected Node ApproachAndOrientRE(Val<Vector3> RichardPos, Val<Vector3> EthanPos)
    {
        return new Sequence(
            Richard.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(EthanPos,1.0f),
            new SequenceParallel(
                Ethan.GetComponent<BehaviorMecanim>().Node_OrientTowards(RichardPos),
                Richard.GetComponent<BehaviorMecanim>().Node_OrientTowards(EthanPos)));
    }

    protected Node ApproachAndOrientDE(Val<Vector3> DanielPos, Val<Vector3> EthanPos)
    {
        return new Sequence(
            Ethan.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(DanielPos,1.0f),
            new SequenceParallel(
                Daniel.GetComponent<BehaviorMecanim>().Node_OrientTowards(EthanPos),
                Ethan.GetComponent<BehaviorMecanim>().Node_OrientTowards(DanielPos)));
    }

    protected Node GreetingTree()
    {
        Val<Vector3> DanielPos = Val.V(() => Daniel.transform.position);
        Val<Vector3> RichardPos = Val.V(() => Richard.transform.position);
        Val<Vector3> EthanPos = Val.V(() => Ethan.transform.position);

        return new SequenceShuffle(
            new Sequence(
                this.ApproachAndOrientDR(DanielPos, RichardPos),
                this.DanRichGreeting()),
            new Sequence(
                this.ApproachAndOrientRE(RichardPos, EthanPos),
                this.RichEthanGreeting()),
            new Sequence(
                this.ApproachAndOrientDE(DanielPos, EthanPos),
                this.DanEthanGreeting()));
    }

    protected Node GoToSpotAndOrient(Val<Vector3> point1, Val<Vector3> point2, Val<Vector3> point3, Val<Vector3> point4, Val<Vector3> point5, Val<Vector3> point6)
    {
        return new Sequence(
            new SequenceParallel(
                Daniel.GetComponent<BehaviorMecanim>().Node_GoTo(point1),
                Richard.GetComponent<BehaviorMecanim>().Node_GoTo(point2),
                Ethan.GetComponent<BehaviorMecanim>().Node_GoTo(point3)),
            new SequenceParallel(
                Daniel.GetComponent<BehaviorMecanim>().Node_OrientTowards(point4),
                Richard.GetComponent<BehaviorMecanim>().Node_OrientTowards(point5),
                Ethan.GetComponent<BehaviorMecanim>().Node_OrientTowards(point6)));
    }

    protected Node AllDance()
    {
        return new SequenceParallel(
            Daniel.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("Breakdance",10000),
            Richard.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("Breakdance", 10000),
            Ethan.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("Breakdance", 10000));
    }

    protected Node DanceTree()
    {
        Val<Vector3> point1 = Val.V(() => Point1.transform.position);
        Val<Vector3> point2 = Val.V(() => Point2.transform.position);
        Val<Vector3> point3 = Val.V(() => Point3.transform.position);
        Val<Vector3> point4 = Val.V(() => Point4.transform.position);
        Val<Vector3> point5 = Val.V(() => Point6.transform.position);
        Val<Vector3> point6 = Val.V(() => Point6.transform.position);

        return new Sequence(
            this.GoToSpotAndOrient(point1,point2,point3,point4,point5,point6),
            this.AllDance());
    }

    // Use this for initialization
    void Start () {
		behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
		print (Vector3.zero);
    }

    protected Node BuildTreeRoot()
    {
        return new Sequence(
        	this.GreetingTree(),
            this.DanceTree());

    }

	protected Node ChaseBall()
	{
		Func<bool> act = () => (Ball.GetComponent<Rigidbody>().velocity.magnitude > 0.5);
		Node trigger = new DecoratorLoop (new LeafAssert (act));
		Node chasing = new DecoratorLoop (
			new SequenceParallel(
				Daniel.GetComponent<BehaviorMecanim>().Node_GoTo(Val.V(() => Ball.transform.position)),
				Richard.GetComponent<BehaviorMecanim>().Node_GoTo(Val.V(() => Ball.transform.position)),
				Ethan.GetComponent<BehaviorMecanim>().Node_GoTo(Val.V(() => Ball.transform.position))));
		Node root = new DecoratorLoop (new DecoratorForceStatus (RunStatus.Success, new SequenceParallel(trigger, chasing)));
		return root;
	}

	// Update is called once per frame
	void Update () {
	}
}
