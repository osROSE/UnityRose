using UnityEngine;
using System.Collections.Generic;
using System;

public class NetworkMonoBehaviour : MonoBehaviour {

    protected Queue<Action> funcQueue;

    // Use this for initialization
    protected void Init () {
        funcQueue = new Queue<Action>();
    }
	
	// Update is called once per frame
	protected void ProcessPackets () {
        if (funcQueue != null)
            while (funcQueue.Count > 0)
                funcQueue.Dequeue().Invoke();
    }
}
