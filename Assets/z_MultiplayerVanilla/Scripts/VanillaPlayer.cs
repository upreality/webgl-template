using System.Collections;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;

public class VanillaPlayer : MonoBehaviour
{
    private string playerId;

    public bool FetchPlayerId(out string id)
    {
        id = playerId;
        return !playerId.IsEmpty();
    }
    
}
