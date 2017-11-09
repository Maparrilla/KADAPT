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

    //public RootMotion.FinalIK.FullBodyBipedEffector DanielEffector;
    //public RootMotion.FinalIK.FullBodyBipedEffector RichardEffector;
    //public RootMotion.FinalIK.FullBodyBipedEffector EthanEffector;

    //public RootMotion.FinalIK.InteractionObject Sphere;

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
            Daniel.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(RichardPos,2.0f),
            new SequenceParallel(
                Daniel.GetComponent<BehaviorMecanim>().Node_OrientTowards(RichardPos),
                Richard.GetComponent<BehaviorMecanim>().Node_OrientTowards(DanielPos)));
    }

    protected Node ApproachAndOrientRE(Val<Vector3> RichardPos, Val<Vector3> EthanPos)
    {
        return new Sequence(
            Richard.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(EthanPos,2.0f),
            new SequenceParallel(
                Ethan.GetComponent<BehaviorMecanim>().Node_OrientTowards(RichardPos),
                Richard.GetComponent<BehaviorMecanim>().Node_OrientTowards(EthanPos)));
    }

    protected Node ApproachAndOrientDE(Val<Vector3> DanielPos, Val<Vector3> EthanPos)
    {
        return new Sequence(
            Ethan.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(DanielPos,2.0f),
            new SequenceParallel(
                Daniel.GetComponent<BehaviorMecanim>().Node_OrientTowards(EthanPos),
                Ethan.GetComponent<BehaviorMecanim>().Node_OrientTowards(DanielPos)));
    }

    protected Node GreetingTree()
    {
        Val<Vector3> DanielPos = Val.V(() => Daniel.transform.position);
        Val<Vector3> RichardPos = Val.V(() => Richard.transform.position);
        Val<Vector3> EthanPos = Val.V(() => Ethan.transform.position);

        return new Selector(
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
            Daniel.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("Breakdance",20000),
            Richard.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("Breakdance", 20000),
            Ethan.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("Breakdance", 20000));
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
            this.DanceTree(),
            this.ChoosePhase2(),
            this.GreetingTree());
    }

    protected Node ChoosePhase2()
    {
        if(Ball.GetComponent<Rigidbody>().velocity.magnitude > 0.5)
        {
            return this.ChaseBall();
        }
        else
        {
            return this.FightTree();
        }

    }

	protected Node ChaseBall()
	{		
			return new Race(
                Daniel.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(Val.V(() => Ball.transform.position), 1.0f),
                Richard.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(Val.V(() => Ball.transform.position), 1.0f),
                Ethan.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(Val.V(() => Ball.transform.position), 1.0f));	
	}

    protected Node FightTree()
    {
        Val<Vector3> DanielPos = Val.V(() => Daniel.transform.position);
        Val<Vector3> RichardPos = Val.V(() => Richard.transform.position);
        Val<Vector3> EthanPos = Val.V(() => Ethan.transform.position);

        return new Selector(
            this.FightDR(DanielPos,RichardPos),
            this.FightRE(EthanPos,RichardPos),
            this.FightDE(DanielPos,EthanPos));
    }

    protected Node FightDR(Val<Vector3> DanielPos, Val<Vector3> RichardPos)
    {
        return new Sequence(
            new SequenceParallel(
                Daniel.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(RichardPos, 2.0f),
                Richard.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(DanielPos, 2.0f)),
            new SequenceParallel(
                Daniel.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("Idle_Fight",2000),
                Richard.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("Idle_Fight", 2000)),
            new Sequence(
                Daniel.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Bash", 1000),
                Richard.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Bash", 1000),
                Daniel.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Bash", 1000),
                Richard.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Bash", 1000),
                Daniel.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Bash", 1000),
                Richard.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Bash", 1000)));
        
    }

    protected Node FightRE(Val<Vector3> EthanPos, Val<Vector3> RichardPos)
    {
        return new Sequence(
            new SequenceParallel(
                Ethan.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(RichardPos, 2.0f),
                Richard.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(EthanPos, 2.0f)),
            new SequenceParallel(
                Ethan.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("Idle_Fight", 2000),
                Richard.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("Idle_Fight", 2000)),
            new Sequence(
                Ethan.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Bash", 1000),
                Richard.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Bash", 1000),
                Ethan.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Bash", 1000),
                Richard.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Bash", 1000),
                Ethan.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Bash", 1000),
                Richard.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Bash", 1000)));
    }

    protected Node FightDE(Val<Vector3> DanielPos, Val<Vector3> EthanPos)
    {
        return new Sequence(
            new SequenceParallel(
                Daniel.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(EthanPos, 2.0f),
                Ethan.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(DanielPos, 2.0f)),
            new SequenceParallel(
                Daniel.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("Idle_Fight", 2000),
                Ethan.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("Idle_Fight", 2000)),
            new Sequence(
                Daniel.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Bash", 1000),
                Ethan.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Bash", 1000),
                Daniel.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Bash", 1000),
                Ethan.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Bash", 1000),
                Daniel.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Bash", 1000),
                Ethan.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Bash", 1000)));

    }

    //protected Node DanielPickup()
    //{
    //    return new Sequence(
    //        Daniel.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("Ground_Pickup_Right", 160),
    //        Daniel.GetComponent<BehaviorMecanim>().Node_StartInteraction(DanielEffector, Sphere));
    //}

    //protected Node RichardPickup()
    //{
    //    return new Sequence(
    //        Richard.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("Ground_Pickup_Right", 160),
    //        Richard.GetComponent<BehaviorMecanim>().Node_StartInteraction(RichardEffector, Sphere));
    //}

    //protected Node EthanPickup()
    //{
    //    return new Sequence(
    //        Ethan.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("Ground_Pickup_Right", 160),
    //        Ethan.GetComponent<BehaviorMecanim>().Node_StartInteraction(EthanEffector, Sphere));
    //}

    // Update is called once per frame
    void Update () {
	}
}
