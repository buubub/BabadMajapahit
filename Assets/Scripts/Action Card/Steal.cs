using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Steal : ActionCard {
	public string name() {
		return "Curi";
	}

	public string description() {
		return "Mencuri 1 dari setiap sumber daya lawan giliran selanjutnya.";
    }

    public Texture image() {
        return Resources.Load<Texture>("Images/Actions/Steal");
    }

    public int cost() {
        return 5;
    }

    public void effect(Player user, Player target) {
        user.resource.food++;
        user.resource.intel++;
        user.resource.weapon++;
        target.resource.food--;
        target.resource.intel--;
        target.resource.weapon--;
        if(target.resource.food < 0) {
            target.resource.food = 0;
        }
        if (target.resource.intel < 0) {
            target.resource.intel = 0;
        }
        if (target.resource.weapon < 0) {
            target.resource.weapon = 0;
        }
    }
}
