using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSequence : EventSequence
{
    public List<string> eventSteps;
    private MovingEntity me;
    private EventSequence es;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        me = gameObject.GetComponent<MovingEntity>();
        es = gameObject.GetComponent<EventSequence>();
        es.setLoop(true);
        es.setIsAutotrigger(true);
    }


    public override void run()
    {
        if (eventSteps.Count > 0)
        {
            foreach (var eventStep in eventSteps)
            {
                var commandAndArgs = getCommandAndArguments(eventStep);
                
                if (commandAndArgs[0].Equals("WalkEast"))
                {
                    var distance = float.Parse(commandAndArgs[1]);
                    walkEast(me, distance);
                }
                
                if (commandAndArgs[0].Equals("WalkWest"))
                {
                    var distance = float.Parse(commandAndArgs[1]);
                    walkWest(me, distance);
                }
                
                if (commandAndArgs[0].Equals("WalkNorth"))
                {
                    var distance = float.Parse(commandAndArgs[1]);
                    walkNorth(me, distance);
                }
                
                if (commandAndArgs[0].Equals("WalkSouth"))
                {
                    var distance = float.Parse(commandAndArgs[1]);
                    walkSouth(me, distance);
                }
                
                if (commandAndArgs[0].Equals("Delay"))
                {
                    var time = float.Parse(commandAndArgs[1]);
                    delay(time);
                }
            }
        }
    }

    private string[] getCommandAndArguments(string line)
    {
        return line.Split(" ");
    }
}
