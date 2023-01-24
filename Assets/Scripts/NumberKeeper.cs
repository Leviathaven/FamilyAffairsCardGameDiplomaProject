using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NumberKeeper : NetworkBehaviour
{
    [SyncVar]
    public int number;

    [SyncVar]
    public int type;

}
