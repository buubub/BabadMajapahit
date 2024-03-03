using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FireTrap : ActionCard {
	public string name() {
		return "Perangkap Api";
	}
	
	public string description() {
		return "Membakar 2 dari setiap sumber daya yang dimiliki lawan.";
    }

    public Texture image() {
        return Resources.Load<Texture>("Images/Actions/FireTrap");
    }

    public int cost() {
        return 6;
    }

	public void effect(Player user, Player target) {
		
	}
}