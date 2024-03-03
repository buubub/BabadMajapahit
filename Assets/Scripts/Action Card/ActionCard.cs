using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ActionCard {
	string name();
	string description();
	Texture image();
    int cost();
	void effect(Player user, Player target);
}