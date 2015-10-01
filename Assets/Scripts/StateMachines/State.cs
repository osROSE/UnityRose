// <copyright file="BoneNode.cs" company="Wadii Bellamine">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Wadii Bellamine, Kiu</author>
// <date>2/25/2015 8:37 AM </date>

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;


//this is just for testing need to create a class with all needed enums for the whole game
//in original code all the enums are defined in a single file and can be used from the whole solution
public enum States
{
    STANDING = 0,
    TIRED,
    WALK,
    RUN,
    SIT,
    SITTING,
    STANDUP,
    WARNING,
    ATTACK1,
    ATTACK2,
    ATTACK3,
    HIT,
    FALL,
    DIE,
    RAISE,
    JUMP1,
    JUMP2,
    PICKUP,
    // CharSelect values
    SELECT,
    HOVERING,
};

/*
public struct StateParams
{
	public bool sitting;
	public bool standing;
	public bool walking;
	public bool targetLocked;
	public bool chase;
	public bool skill;
	public bool defaultAttack;
	public string nextSkill;
}
*/

public class StateConnection
{
    public States stateName;
	public State nextState;
	private string paramName;
	
	public bool condition(States state, States current)
	{

        if (state != current)
            return true;

        return false;
		//Type type = typeof(StateParams);
		//bool value = (bool)type.GetField(paramName).GetValue(stateParams);
		//return value;	


	} 
	
	public StateConnection(States stateName,State nextState, string parameter)
	{
        this.stateName = stateName;
		this.nextState = nextState;
		this.paramName = parameter;
		
	}
	
	public StateConnection(States stateName,State nextState)
	{
		this.stateName = stateName;
		this.nextState = nextState;
		this.paramName = "";
		
	}
}


public class State
{

    public States state;
 	public string Name { get; set; }
	public GameObject gameObject { get; set; }
	
	protected string clipName { get; set; }
	public List<StateConnection> connections { get; set; }
	protected Animation animation;
	protected WrapMode wrapMode;

    public State(States state, GameObject gameObject, WrapMode wrapMode = WrapMode.Loop)
    {
        this.state = state;
        clipName = Name = state.ToString().ToLower();
        this.gameObject = gameObject;
        this.wrapMode = wrapMode;
        connections = new List<StateConnection>();
        animation = gameObject.GetComponentInChildren<Animation>(); // TODO: add exception if no animation	
    }

    public virtual void Exit(bool crossFade = true)
	{
        if (!crossFade)
            animation.Stop(clipName);
        else
            animation.CrossFade(clipName);
	}
	
	public virtual void Entry(bool crossFade = true)
	{
		animation = gameObject.GetComponentInChildren<Animation>();
		if(animation)
		{
			animation.wrapMode = wrapMode;
			if(crossFade)
				animation.CrossFade(clipName);
			else
				animation.Play(clipName);
		}
	}
	
	public virtual State Evaluate(States s ) { 
		foreach(StateConnection connection in connections)
		{
            /*
			if(connection.condition(stateParams))
				return connection.nextState;
             */

            if (s != this.state && s == connection.stateName)
            {
                return connection.nextState;
            }
                
		}
		
		return this; 
	}
	
	
}

public class TransitionState : State
{
	private State nextState;
	public TransitionState(States activeState, State nextState, GameObject gameObject)
        : base(activeState, gameObject, WrapMode.Once)
	{
		this.nextState = nextState;
	}
	
	public override State Evaluate(States state)
	{
		if(animation.IsPlaying(clipName))
			return this;
		else
			return nextState;
	}
}

/*
public class MultiAnimationState : State
{
	private List<string> clips;
	int clipIndex;
	bool randomize;
	public MultiAnimationState( List<string> clips, GameObject gameObject, bool randomize = true)
		:base("", gameObject, WrapMode.Once)
	{
		this.clips = clips;
		this.randomize = randomize;
		
		if( randomize )
			clipIndex = UnityEngine.Random.Range(0, clips.Count);
		else
			clipIndex = 0;
			
		clipName = clips[clipIndex];
	}
	
	public override State Evaluate(ref StateParams stateParams)
	{
		State result = base.Evaluate(ref stateParams);
		if( result != this )
			return result;
			
		if( !animation.IsPlaying(clipName) )
		{
			if( randomize )
				clipIndex = UnityEngine.Random.Range(0, clips.Count);
			else
				clipIndex = (clipIndex + 1)%clips.Count;
				
			clipName = clips[clipIndex]; 
			animation.Play(clipName);
		}
		
		return this; 
	}
}

public class SkillState: State
{
	private Queue<string> skillQueue;
	private State nextState;
	
	public SkillState(string name, State nextState, GameObject gameObject)
	:base(name, gameObject, WrapMode.Once)
	{
		skillQueue = new Queue<string>();
		this.nextState = nextState;
	}
	
	public override void Entry( bool crossFade = true )
	{
			
	}
	
	public override State Evaluate(ref StateParams stateParams)
	{
		// if there is a skill pending
		if(stateParams.nextSkill != "")
		{
			// enqueue and consume it
			skillQueue.Enqueue(stateParams.nextSkill);
			stateParams.nextSkill = "";
		}
		
		// Once a skill has been commanded, it won't stop till the animation ends
		if( !animation.IsPlaying(clipName) )
		{
			// First, make sure we are still in this state
			State result = base.Evaluate(ref stateParams );
			
			if(result!=this)
				return result;
				
			// if we are in this state, check if there are still skills in the queue
			if( skillQueue.Count > 0 )
			{
				// Play the next skill in the queue (without crossfade)
				clipName = skillQueue.Dequeue();
				base.Entry(false);
			}
			else
			{
				return nextState;
			}

		}
		
		return this;
			
	}
	
	public override void Exit(bool crossFade = false )
	{
		skillQueue.Clear();
		animation.Stop (clipName);
	}
}

public class AttackMachine: State
{
	private Dictionary<string, State> states;
	private State currentState;
	
	public AttackMachine(string name, GameObject gameObject)
		:base( name, gameObject )	
	{
		states = new Dictionary<string, State>();
		List<string> defaultAttacks = new List<string>(3);
		defaultAttacks.Add ("attack1");
		defaultAttacks.Add ("attack2");
		defaultAttacks.Add("attack3");
		states.Add ("defaultAttack", new MultiAnimationState(defaultAttacks, gameObject));
		states.Add ("skill", new SkillState("skill", states["defaultAttack"], gameObject));
		states.Add ("chase", new State("run", gameObject));
	
		// connect every state to every other state
		states["defaultAttack"].connections.Add (new StateConnection(states["skill"], "skill"));
		states["defaultAttack"].connections.Add (new StateConnection(states["chase"], "chase"));
		states["skill"].connections.Add (new StateConnection( states["defaultAttack"], "defaultAttack"));
		states["skill"].connections.Add (new StateConnection( states["chase"], "chase"));
		states["chase"].connections.Add (new StateConnection( states["skill"], "skill"));
		states["chase"].connections.Add (new StateConnection( states["defaultAttack"], "defaultAttack"));
		
	}
	
	public override void Entry( bool crossFade = false )
	{
		currentState = states["defaultAttack"];
		currentState.Entry();
	}
	
	public override State Evaluate( ref StateParams stateParams )
	{
		State result = base.Evaluate( ref stateParams );
		if( result != this )
			return result;
		
		
		result = currentState.Evaluate( ref stateParams);
		
		if(result != currentState)
		{
			currentState.Exit(true);
			currentState = result;
			currentState.Entry(true);
		}	
		
		return this;
	}
	
	public override void Exit(bool crossFade = false)
	{
		currentState.Exit( crossFade );
	}
	
}
 * 
 * */

public class PlayerState : State
{ 
	private Dictionary<string, State> states;
	private State currentState;
	
	public PlayerState(States cState,string name, GameObject gameObject)
		:base(  cState, gameObject)
	{
		// Generate list of states
		states = new Dictionary<string, State>();
		states.Add ("STANDING", new State( States.STANDING, gameObject));
		states.Add ("sitStand", new TransitionState(States.STANDUP, states["STANDING"], gameObject));


		states.Add ("WALK", new State(States.WALK, gameObject));
		states.Add ("standWalk", new TransitionState(States.STANDUP, states["WALK"], gameObject));
		
        states.Add ("SITTING", new State(States.SITTING, gameObject));
		states.Add ("SIT", new TransitionState(States.SIT, states["SITTING"], gameObject));
 

        states.Add("RUN", new State(States.RUN, gameObject));
        states.Add("standRun", new TransitionState(States.STANDUP, states["RUN"], gameObject));

		//states.Add ("AttackMachine", new AttackMachine("attack", gameObject));
		//states.Add ("standAttack", new TransitionState(States.STANDUP,"standup", states["AttackMachine"], gameObject));
		
		
		states["STANDING"].connections.Add (new StateConnection(States.WALK, states["WALK"], "walking"));
		states["STANDING"].connections.Add (new StateConnection(States.SITTING,states["SIT"], "sitting"));
        states["STANDING"].connections.Add(new StateConnection(States.RUN, states["RUN"], "run"));
		//states["standing"].connections.Add (new StateConnection(states["AttackMachine"], "targetLocked"));
		
		states["SITTING"].connections.Add ( new StateConnection(States.WALK,states["standWalk"], "walking"));
        states["SITTING"].connections.Add(new StateConnection(States.RUN, states["standRun"], "run"));
        states["SITTING"].connections.Add ( new StateConnection(States.STANDING,states["sitStand"], "standing"));

		//states["sitting"].connections.Add ( new StateConnection(states["standAttack"], "targetLocked"));
		
		states["WALK"].connections.Add ( new StateConnection(States.STANDING,states["STANDING"], "standing"));

        states["RUN"].connections.Add(new StateConnection(States.STANDING, states["STANDING"], "standing"));
		//states["walk"].connections.Add ( new StateConnection(states["AttackMachine"], "targetLocked"));
		
		
		//states["AttackMachine"].connections.Add ( new StateConnection(states["standing"], "standing"));
		//states["AttackMachine"].connections.Add ( new StateConnection(states["walk"], "walking"));
		
	}


	public override void Entry(bool crossFade = false)
	{	
		currentState = states["STANDING"];
		currentState.Entry();
	}
	
	public override void Exit(bool crossFade = false)
	{
		currentState.Exit( crossFade );
	}
	
	public override State Evaluate(States s)
	{	
		State result = currentState.Evaluate(s);
		
		if(result != currentState)
		{
			currentState.Exit(true);
			currentState = result;
			currentState.Entry(true);
		}	
		
		return this;
	}
	
}


public class CharSelectState : State
{
    private Dictionary<string, State> states;
    private State currentState;
    private States initialState;

    public CharSelectState(States initialState, string name, GameObject gameObject)
        : base(initialState, gameObject)
    {
        this.initialState = initialState;
        // Generate list of states
        states = new Dictionary<string, State>();
        states.Add("HOVERING", new State(States.HOVERING, gameObject));
        states.Add("SELECT", new State(States.SELECT, gameObject, WrapMode.Once));

        states.Add("STANDING", new State(States.STANDING, gameObject));
        states.Add("STANDUP", new TransitionState(States.STANDUP, states["STANDING"], gameObject));

        states.Add("SITTING", new State(States.SITTING, gameObject));
        states.Add("SIT", new TransitionState(States.SIT, states["SITTING"], gameObject));

        // State connections:
        // Hovering -> standing -> select 
        //                |     |
        //                |     -> [sit -> sitting]
        //                |                      |
        //                <----------------standup

        states["HOVERING"].connections.Add(new StateConnection(States.STANDING, states["STANDING"]));

        states["STANDING"].connections.Add(new StateConnection(States.SELECT, states["SELECT"]));
        states["STANDING"].connections.Add(new StateConnection(States.SIT, states["SIT"]));

        states["SITTING"].connections.Add(new StateConnection(States.STANDUP, states["STANDUP"]));

        states["SELECT"].connections.Add(new StateConnection(States.STANDING, states["STANDING"]));

    }


    public override void Entry(bool crossFade = false)
    {
        currentState = states[initialState.ToString()];
        base.Entry();
    }

    public override void Exit(bool crossFade = false)
    {
        currentState.Exit(crossFade);
    }

    public override State Evaluate(States s)
    {
        State result = currentState.Evaluate(s);

        if (result != currentState)
        {
            currentState.Exit(true);
            currentState = result;
            currentState.Entry(true);
        }

        return this;
    }

}
