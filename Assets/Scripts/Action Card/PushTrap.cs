using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PushTrap : ActionCard {
    public string name() {
        return "Perangkap Kayu";
    }

    public string description() {
        return "Mendorong pemain lawan 5 petak mundur.";
    }

    public Texture image() {
        return Resources.Load<Texture>("Images/Actions/PushTrap");
    }

    public int cost() {
        return 6;
    }

    public void effect(Player user, Player target) {

    }
}