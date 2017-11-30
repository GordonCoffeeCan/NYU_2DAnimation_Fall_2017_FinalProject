using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReskinGroundTile : Tile {

	//an array of all the sprites that could be put on our groundTile as alternates for variation
	public Sprite[] altSprites;

	//probability we won't change sprite (0 is always change, 1 is never change)
	//public float probablityDefaultSprite;


	// Use this for initialization
	void Start () {
        /*float rand = Random.value;

		if (rand > probablityDefaultSprite) {
			int tileToApply = Random.Range (0, altSprites.Length);
			GetComponent<SpriteRenderer>().sprite = altSprites [tileToApply];
		}*/

        GetComponent<SpriteRenderer>().sprite = altSprites[Random.Range(0, altSprites.Length)];
    }
}
