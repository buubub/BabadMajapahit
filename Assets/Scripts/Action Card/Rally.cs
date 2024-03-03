using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rally : ActionCard {
    public string name() {
        return "Berkumpul";
    }

    public string description() {
        return "Pemain melompat menuju Surya Majapahit terdekat.";
    }

    public Texture image() {
        return Resources.Load<Texture>("Images/Actions/Rally");
    }

    public int cost() {
        return 15;
    }

    public void effect(Player user, Player target) {
        
    }
}
