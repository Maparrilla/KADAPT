using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TreeSharpPlus;

public class InvokeEventB3 : MonoBehaviour {

    public GameObject Daniel;
    public GameObject Richard;
    public GameObject Ethan;

    public Vector3 offset = new Vector3(0.1f, 0, -0.1f);

    private BehaviorAgent behaviorAgent;

    protected Node DanRichGreeting()
    {
        return new Sequence(
            Daniel.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 3500),
            Richard.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 3500),
            Daniel.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("being_cocky", 1633),
            Richard.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("look_away", 2000),
            Richard.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 3500),
            Daniel.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 3500));
    }

    protected Node RichEthanGreeting()
    {
        return new Sequence(
            Richard.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 3500),
            Ethan.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 3500),
            Richard.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("being_cocky", 1633),
            Ethan.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("look_away", 2000),
            Ethan.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 3500),
            Richard.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 3500));
    }

    protected Node DanEthanGreeting()
    {
        return new Sequence(
            Ethan.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 3500),
            Daniel.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 3500),
            Ethan.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("being_cocky", 1633),
            Daniel.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("look_away", 2000),
            Daniel.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 3500),
            Ethan.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 3500));
    }

    protected Node ApproachAndOrientDR(Val<Vector3> DanielPos, Val<Vector3> RichardPos)
    {
        return new Sequence(
            Daniel.GetComponent<BehaviorMecanim>().Node_GoTo(RichardPos),
            new SequenceParallel(
                Daniel.GetComponent<BehaviorMecanim>().Node_OrientTowards(RichardPos),
                Richard.GetComponent<BehaviorMecanim>().Node_OrientTowards(DanielPos)));
    }

    protected Node ApproachAndOrientRE(Val<Vector3> RichardPos, Val<Vector3> EthanPos)
    {
        return new SequenceParallel(
            Richard.GetComponent<BehaviorMecanim>().Node_GoTo(EthanPos),
            new SequenceParallel(
                Ethan.GetComponent<BehaviorMecanim>().Node_OrientTowards(RichardPos),
                Richard.GetComponent<BehaviorMecanim>().Node_OrientTowards(EthanPos)));
    }

    protected Node ApproachAndOrientDE(Val<Vector3> DanielPos, Val<Vector3> EthanPos)
    {
        return new SequenceParallel(
            Ethan.GetComponent<BehaviorMecanim>().Node_GoTo(DanielPos),
            new SequenceParallel(
                Daniel.GetComponent<BehaviorMecanim>().Node_OrientTowards(EthanPos),
                Ethan.GetComponent<BehaviorMecanim>().Node_OrientTowards(DanielPos)));
    }

    protected Node GreetingTree()
    {
        Val<Vector3> DanielPos = Val.V(() => Daniel.transform.position + offset);
        Val<Vector3> RichardPos = Val.V(() => Richard.transform.position + offset);
        Val<Vector3> EthanPos = Val.V(() => Ethan.transform.position + offset);

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

    // Use this for initialization
    void Start () {
        behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
    }

    protected Node BuildTreeRoot()
    {
        return new Sequence(
            this.GreetingTree());
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
