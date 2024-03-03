using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Tactic : ActionCard {
    public string name() {
        return "Strategi";
    }

    public string description() {
        return "Pemain memainkan dadu dua kali di giliran ini.";
    }

    public Texture image() {
        return Resources.Load<Texture>("Images/Actions/Tactic");
    }

    public int cost() {
        return 8;
    }

    public void effect(Player user, Player target) {
        
    }
}