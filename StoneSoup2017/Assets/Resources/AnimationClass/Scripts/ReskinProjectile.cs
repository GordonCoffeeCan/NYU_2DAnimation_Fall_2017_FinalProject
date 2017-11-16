using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Example of a tile that can be picked up.
// A simple rock that can be thrown by the player and enemies alike
public class ReskinProjectile : Tile {


	// How much force we apply to a target when we deal damage. 
	public float damageForce = 1000;

	// We keep track of the tile that threw us so we don't collide with it immediately.
	public Tile tileThatShotUs = null;

	// Keep track of whether we're in the air and whether we were JUST thrown
	protected bool _isInAir = false;
	protected float _afterThrowCounter;
	public float afterThrowTime = 0.2f;

	// Like walls, rocks need explosive damage to be hurt.
	public override void takeDamage(Tile tileDamagingUs, int amount, DamageType damageType) {
		if (damageType == DamageType.Explosive) {
			base.takeDamage(tileDamagingUs, amount, damageType);
		}
	}

	void Update() {
			if (_afterThrowCounter > 0) {
				_afterThrowCounter -= Time.deltaTime;
			}
	}

	// When we collide with something in the air, we try to deal damage to it.
	public virtual void OnCollisionEnter2D(Collision2D collision) {
		Tile otherTile = collision.gameObject.GetComponent<Tile>();
		if (otherTile == null) {
			Destroy (gameObject);
			return;
		}
		if (otherTile != tileThatShotUs && !otherTile.hasTag(TileTags.Gun)) {
			otherTile.takeDamage (this, 1);
			if (otherTile.GetComponent<Rigidbody2D> ()) {
				otherTile.addForce (GetComponent<Rigidbody2D> ().velocity.normalized * damageForce);
			}
			Destroy (gameObject);
		}
	}


	public void setShooter(Tile _tileThatShotUs){
		tileThatShotUs = _tileThatShotUs;
		Physics2D.IgnoreCollision(tileThatShotUs.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);

	}



}
