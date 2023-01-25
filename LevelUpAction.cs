using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelUpAction : MonoBehaviour
{
    public abstract void onLevelUp(PlayerObject playerObject);
}
