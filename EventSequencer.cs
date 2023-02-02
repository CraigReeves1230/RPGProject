using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class EventSequencer : EventSequence
{
    [PropertySpace(SpaceAfter = 0, SpaceBefore = 15)]
    public List<MovingEntity> movingEntities;
    
    [PropertySpace(SpaceAfter = 0, SpaceBefore = 15), TextArea(1, 1)]
    public List<string> eventSteps;

    private MovingEntity getMovingEntity(string arg)
    {
        if (arg.Contains("party-", StringComparison.OrdinalIgnoreCase))
        {
            var indxString = arg[6].ToString();
            var indx = int.Parse(indxString);
            return GameManager.instance.party[indx];
        }

        return movingEntities[int.Parse(arg)];
    }
    
    
    public override void run()
    {
        if (eventSteps.Count > 0)
        {
            foreach (var eventStep in eventSteps)
            {
                var commandAndArgs = getCommandAndArguments(eventStep);
                var cmdArgsMsg = getCommandAndArgsFromMSG(eventStep);
                
                if (commandAndArgs[0].Equals("stopAllFollowing", StringComparison.OrdinalIgnoreCase))
                {
                    stopAllFollowing();
                }
                
                if (commandAndArgs[0].Equals("followTheLeader", StringComparison.OrdinalIgnoreCase))
                {
                    if (commandAndArgs.Length > 1)
                    {
                        followTheLeader(bool.Parse(commandAndArgs[1]));
                    }
                    else
                    {
                        followTheLeader();
                    }
                }
                
                if (commandAndArgs[0].Equals("setExitsEnabled", StringComparison.OrdinalIgnoreCase))
                {
                    setExitsEnabled(bool.Parse(commandAndArgs[1]));
                }
                
                if (commandAndArgs[0].Equals("RunEast", StringComparison.OrdinalIgnoreCase))
                {
                    var me = getMovingEntity(commandAndArgs[1]);
                    var distance = float.Parse(commandAndArgs[2]);
                    runEast(me, distance);
                }
                
                if (commandAndArgs[0].Equals("RunNorth", StringComparison.OrdinalIgnoreCase))
                {
                    var me = getMovingEntity(commandAndArgs[1]);
                    var distance = float.Parse(commandAndArgs[2]);
                    runNorth(me, distance);
                }
                
                if (commandAndArgs[0].Equals("RunSouth", StringComparison.OrdinalIgnoreCase))
                {
                    var me = getMovingEntity(commandAndArgs[1]);
                    var distance = float.Parse(commandAndArgs[2]);
                    runSouth(me, distance);
                }
                
                if (commandAndArgs[0].Equals("RunWest", StringComparison.OrdinalIgnoreCase))
                {
                    var me = getMovingEntity(commandAndArgs[1]);
                    var distance = float.Parse(commandAndArgs[2]);
                    runWest(me, distance);
                }

                if (commandAndArgs[0].Equals("RunNE", StringComparison.OrdinalIgnoreCase) ||
                    commandAndArgs[0].Equals("RunNorthEast", StringComparison.OrdinalIgnoreCase))
                {
                    var me = getMovingEntity(commandAndArgs[1]);
                    var distance = float.Parse(commandAndArgs[2]);
                    runNE(me, distance);
                }
                
                if (commandAndArgs[0].Equals("RunNW", StringComparison.OrdinalIgnoreCase) ||
                    commandAndArgs[0].Equals("RunNorthWest", StringComparison.OrdinalIgnoreCase))
                {
                    var me = getMovingEntity(commandAndArgs[1]);
                    var distance = float.Parse(commandAndArgs[2]);
                    runNW(me, distance);
                }
                
                if (commandAndArgs[0].Equals("RunSW", StringComparison.OrdinalIgnoreCase) ||
                    commandAndArgs[0].Equals("RunSouthWest", StringComparison.OrdinalIgnoreCase))
                {
                    var me = getMovingEntity(commandAndArgs[1]);
                    var distance = float.Parse(commandAndArgs[2]);
                    runSW(me, distance);
                }
                
                if (commandAndArgs[0].Equals("RunSE", StringComparison.OrdinalIgnoreCase) ||
                    commandAndArgs[0].Equals("RunSouthEast", StringComparison.OrdinalIgnoreCase))
                {
                    var me = getMovingEntity(commandAndArgs[1]);
                    var distance = float.Parse(commandAndArgs[2]);
                    runSE(me, distance);
                }

                if (commandAndArgs[0].Equals("WalkEast", StringComparison.OrdinalIgnoreCase))
                {
                    var me = getMovingEntity(commandAndArgs[1]);
                    var distance = float.Parse(commandAndArgs[2]);
                    walkEast(me, distance);
                }

                if (commandAndArgs[0].Equals("WalkWest", StringComparison.OrdinalIgnoreCase))
                {
                    var me = getMovingEntity(commandAndArgs[1]);
                    var distance = float.Parse(commandAndArgs[2]);
                    walkWest(me, distance);
                }
                
                if (commandAndArgs[0].Equals("WalkNorth", StringComparison.OrdinalIgnoreCase))
                {
                    var me = getMovingEntity(commandAndArgs[1]);
                    var distance = float.Parse(commandAndArgs[2]);
                    walkNorth(me, distance);
                }
                
                if (commandAndArgs[0].Equals("WalkSouth", StringComparison.OrdinalIgnoreCase))
                {
                    var me = getMovingEntity(commandAndArgs[1]);
                    var distance = float.Parse(commandAndArgs[2]);
                    walkSouth(me, distance);
                }
                
                if (commandAndArgs[0].Equals("Delay", StringComparison.OrdinalIgnoreCase))
                {
                    var time = float.Parse(commandAndArgs[1]);
                    delay(time);
                }
                
                if (commandAndArgs[0].Equals("StealControl", StringComparison.OrdinalIgnoreCase))
                {
                    stealControl();
                }
                
                if (commandAndArgs[0].Equals("FaceNorth", StringComparison.OrdinalIgnoreCase))
                {
                    var me = getMovingEntity(commandAndArgs[1]);
                    faceNorth(me);
                }
                
                if (commandAndArgs[0].Equals("FaceSouth", StringComparison.OrdinalIgnoreCase))
                {
                    var me = getMovingEntity(commandAndArgs[1]);
                    faceSouth(me);
                }
                
                if (commandAndArgs[0].Equals("FaceEast", StringComparison.OrdinalIgnoreCase))
                {
                    var me = getMovingEntity(commandAndArgs[1]);
                    faceEast(me);
                }
                
                if (commandAndArgs[0].Equals("FaceWest", StringComparison.OrdinalIgnoreCase))
                {
                    var me = getMovingEntity(commandAndArgs[1]);
                    faceWest(me);
                }
                
                if (commandAndArgs[0].Equals("ReturnControl", StringComparison.OrdinalIgnoreCase))
                {
                    returnControl();
                }
                
                if (commandAndArgs[0].Equals("msg", StringComparison.OrdinalIgnoreCase))
                {
                    if (cmdArgsMsg.Length < 2)
                    {
                        msg(cmdArgsMsg[0]);
                    }
                    else
                    {
                        msg(cmdArgsMsg[0], cmdArgsMsg[1]);
                    }
                }
                
                if (commandAndArgs[0].Equals("wait", StringComparison.OrdinalIgnoreCase))
                {
                    wait();
                }
                
                if (commandAndArgs[0].Equals("msgCls", StringComparison.OrdinalIgnoreCase))
                {
                    msgCls();
                }
            }
        }
    }
    
    private string[] getCommandAndArguments(string line)
    {
        return line.Split(" ");
    }

    private string[] getCommandAndArgsFromMSG(string line)
    {
        var lineList = new List<string>();
        var currentLine = 0;
        var lineAlive = false;
        var lineToAdd = "";
        foreach (var character in line)
        {
            // start first line which is a quote "
            if (character == 34 && !lineAlive)    // ASCII code
            {
                currentLine++;
                lineAlive = true;
                continue;
            } 
            
            if (character == 34 && lineAlive)
            {
                lineAlive = false;
                lineList.Add(lineToAdd);
                lineToAdd = "";
                continue;
            }

            if (!lineAlive && character != 34)
            {
                continue;
            }

            if (lineAlive && character != 34)
            {
                // write to lineToAdd string
                lineToAdd += character;
            }
        }

        return lineList.ToArray();
    }
}
