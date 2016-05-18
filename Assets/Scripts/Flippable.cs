using UnityEngine;

public class Flippable : MonoBehaviour {
	public Vector3 originalPosition { get; private set; }
	public Vector3 originalRotation { get; private set; }
	public Vector3 originalScale { get; private set; }
	public GameObject previewObject { get; private set; }

	void Start()
	{
		originalPosition = transform.localPosition;
		originalRotation = transform.localEulerAngles;
		originalScale = transform.localScale;

		previewObject = new GameObject("Preview of " + name);
		previewObject.transform.SetParent(transform.parent);
		previewObject.transform.localPosition = transform.localPosition;
		previewObject.transform.localRotation = transform.localRotation;
		previewObject.transform.localScale = transform.localScale;
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		if (sr)
		{
			SpriteRenderer psr = previewObject.AddComponent<SpriteRenderer>();
			psr.sprite = sr.sprite;
			psr.material = sr.material;
			psr.color = Color.clear;
			if (GetComponent<TileSprite>())
			{
				previewObject.AddComponent<TileSprite>();
			}
			psr.sortingLayerName = "Preview Layer";
		}
		Collider2D col = GetComponent<Collider2D>();
		if (col)
		{
			Collider2D pcol = (Collider2D)previewObject.AddComponent(col.GetType());
			pcol.isTrigger = true;
		}
		previewObject.tag = "WallPreview";
		previewObject.layer = LayerMask.NameToLayer("WallPreview");
	}
}
