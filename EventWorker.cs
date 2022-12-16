using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventWorker : MonoBehaviour
{
    private List<Command> commandList;
    private EventEngine eventEngine;
    private int eventsInQueue;
    private bool paused;
    private bool enabled = true;
    private bool waitingForKey;
    
    public void storeInQueue(Command command)
    {
        if (enabled)
        commandList.Add(command);
    }

    private void Start()
    {
        commandList = new List<Command>();
        eventEngine = gameObject.AddComponent<EventEngine>();
    }

    void Update()
    {
        eventsInQueue = commandList.Count;
        if (eventsInQueue > 0 && !paused && !waitingForKey)
        {
            // work only the oldest queue in the stack
            workCommand(commandList.First());
        }

        if (isWaitingForKey())
        {
            if (GameManager.instance.getMainFireKeyUp())
            {
                waitingForKey = false;
            }
        }
    }

    void workCommand(Command command)
    {
        // run method
        if (eventEngine.start(command))
        {
            // move to next command
            commandList.Remove(command);
        }
    }
    
    // pauses event queue
    public void pauseNow()
    {
        paused = true;
    }

    // resumes event queue
    public void resume()
    {
        paused = false;
    }

    public void setWaitForKey(bool setting)
    {
        waitingForKey = setting;
    }
    
    // cancels event queue
    public void cancelEventQueue()
    {
        commandList.Clear();
        enabled = false;
    }

    public int numberOfEventsInQueue()
    {
        return eventsInQueue;
    }

    public bool isPaused()
    {
        return paused;
    }

    public bool isWaitingForKey()
    {
        return waitingForKey;
    }
}
