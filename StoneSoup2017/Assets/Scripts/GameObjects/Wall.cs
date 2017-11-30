using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Tile {

    [SerializeField] private Sprite[] wallSprites;

    private SpriteRenderer spriteRenderer;

    private void Start() {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = wallSprites[Random.Range(0, wallSprites.Length)];
    }

    // Walls only take explosive damage.
    public override void takeDamage(Tile tileDamagingUs, int amount, DamageType damageType) {
		if (damageType == DamageType.Explosive) {
			base.takeDamage(tileDamagingUs, amount, damageType);
		}
	}
}
