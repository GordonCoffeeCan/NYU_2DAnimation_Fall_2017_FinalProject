using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Example of a tile that can be picked up.
// A simple rock that can be thrown by the player and enemies alike
public class ReskinProjectileSpawner : Tile {


	public GameObject projectile;

	// Sound that's played when we're used;
	public AudioClip fireSound;

	// How much force to add when thrown
	public float shootForce = 3000f;

	// How much force we apply to a target when we deal damage. 
	public float damageForce = 1000;

	// We keep track of the tile that used us so we don't collide with it immediately.
	protected Tile _tileThatUsedUs = null;

	// Like walls, need explosive damage to be hurt.
	public override void takeDamage(Tile tileDamagingUs, int amount, DamageType damageType) {
		if (damageType == DamageType.Explosive) {
			base.takeDamage(tileDamagingUs, amount, damageType);
		}
	}


	// The idea is that we get thrown when we're used
	public override void useAsItem(Tile tileUsingUs) {
		if (_tileHoldingUs != tileUsingUs) {
			return;
		}

		AudioManager.playAudio(fireSound);

		// We're thrown in the aim direction specified by the object throwing us.
		Vector2 shootDir = tileUsingUs.aimDirection.normalized;



		// Finally, here's where we get the throw force.
		GameObject myProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
		myProjectile.GetComponent<ReskinProjectile> ().setShooter (_tileHoldingUs);
		myProjectile.GetComponent<Rigidbody2D>().AddForce(shootDir*shootForce);
	}



}
