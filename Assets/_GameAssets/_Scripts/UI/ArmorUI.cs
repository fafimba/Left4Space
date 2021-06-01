using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorUI : MonoBehaviour {

    [SerializeField]
    SpaceShip ship;
    [SerializeField]
     Image lifeBar;
    [SerializeField]
    Image shieldBar;


	void Update ()
    {
        lifeBar.fillAmount = ship.armor.Life.Actual / ship.armor.Life.Max;
        shieldBar.fillAmount = ship.armor.Shield.Actual / ship.armor.Shield.Max;
    }
}
