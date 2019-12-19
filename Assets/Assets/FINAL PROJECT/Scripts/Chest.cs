using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : ActionItem
{
    public override void Interact()
    {
        base.Interact();

        print("Interacting with Chest!");
    }
	
}
