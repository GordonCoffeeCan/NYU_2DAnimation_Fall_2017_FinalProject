using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItemText : MonoBehaviour {

    [SerializeField] private Sprite[] itemSprites;
    private Image uiImage;

	//protected Text _text;

	// Use this for initialization
	void Start () {
        //_text = GetComponent<Text>();
        uiImage = this.GetComponent<Image>();

    }
		
	// Update is called once per frame
	void Update () {
		if (Player.instance != null) {
			if (Player.instance.tileWereHolding != null) {
                //_text.text = string.Format("Item: {0}", Player.instance.tileWereHolding.tileName);
                uiImage.color = new Color32(255, 255, 255, 255);
                
                switch (Player.instance.tileWereHolding.tileName) {
                    case "Swing Thing":
                        uiImage.sprite = itemSprites[0];
                        uiImage.SetNativeSize();
                        break;
                    case "thing2throw":
                        uiImage.sprite = itemSprites[1];
                        uiImage.SetNativeSize();
                        break;
                    case "bow":
                        uiImage.sprite = itemSprites[2];
                        uiImage.SetNativeSize();
                        break;
                    case "throwUp":
                        uiImage.sprite = itemSprites[3];
                        uiImage.SetNativeSize();
                        break;
                }
			}
			else {
                //_text.text = "Item: None";
                uiImage.color = new Color32(255, 255, 255, 0);
            }
		}
	}
}
