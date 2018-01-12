using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBullet  {

	// Interface for every object that can damage enemies
	// General tag for objects inheriting this is "Bullet"

	float GetDamage ();
}
