using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBox : MonoBehaviour {
	public int boxWidth = 3;
	public Color color = Color.green;

	new private SpriteRenderer renderer;
	private SpriteRenderer selectBoxRenderer;
	private GameObject selectBox;

	// Generate a selection box for this object and add it as a child
	void Awake() {
		renderer = GetComponent<SpriteRenderer>();

		// Create child gameobject and add box sprite to it
		selectBox = new GameObject("SelectBox");
		selectBox.SetActive(false);
		selectBox.transform.SetParent(this.transform, false);
		selectBoxRenderer = selectBox.AddComponent<SpriteRenderer>();
		selectBoxRenderer.sortingOrder = 1;
		selectBoxRenderer.sprite = CreateBoxSprite();

		selectBox.transform.localScale = new Vector3(selectBox.transform.localScale.x, 
			transform.localScale.x / transform.localScale.y, 
			selectBox.transform.localScale.z);
	}

	private Sprite CreateBoxSprite() {
		// calculate Screen space (pixel) size of the object
		var origin = (Vector2)Camera.main.WorldToScreenPoint(renderer.bounds.min);
		var extents = (Vector2)Camera.main.WorldToScreenPoint(renderer.bounds.max);
		Vector2 size = new Vector2(extents.x - origin.x, extents.y - origin.y);

		Texture2D tex = new Texture2D(Mathf.RoundToInt(size.x), Mathf.RoundToInt(size.y));
		for (int x = 0; x < size.x; x++) {
			for (int y = 0; y < size.y; y++) {
				tex.SetPixel(x, y, new Color(0.0f, 0.0f, 0.0f, 0.0f));
			}
		}
		for (int x = 0; x < size.x; x++) {
			for (int y = 0; y < size.y; y++) {
				if (x < boxWidth || x > size.x - boxWidth || y < boxWidth || y > size.y - boxWidth) {
					tex.SetPixel(x, y, color);
				}
			}
		}
		tex.alphaIsTransparency = true;
		tex.filterMode = FilterMode.Point;
		tex.Apply();

		return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), tex.width);
	}
}
