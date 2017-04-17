using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitController : MonoBehaviour {
	public GameObject dragBoxPrefab;
	public Canvas uiCanvas;
	private List<GameObject> selected = new List<GameObject>();
	private Vector2 wMouseDown;
	private bool isMouseDown = false;
	private bool dragging = false;

	void Awake() {
		selected = new List<GameObject>();
	}

	private IEnumerator DragBoxRoutine(Vector2 sSpawnPos) {
		GameObject dragBox = GameObject.Instantiate(dragBoxPrefab, sSpawnPos, Quaternion.identity, uiCanvas.transform);
		Debug.Log(sSpawnPos);
		dragging = true;

		while (Input.GetButton("Select")) {
			Rect rect = Rect.MinMaxRect(
				Mathf.Min(sSpawnPos.x, Input.mousePosition.x),
				Mathf.Min(sSpawnPos.y, Input.mousePosition.y),
				Mathf.Max(sSpawnPos.x, Input.mousePosition.x),
				Mathf.Max(sSpawnPos.y, Input.mousePosition.y)
			);
			RectTransform rt = dragBox.GetComponent<RectTransform>();
			rt.position = rect.position;
			rt.sizeDelta = new Vector2(rect.width, rect.height);
			yield return null;
		}

		Destroy(dragBox);
		dragging = false;
	}

	private bool IsMouseDrag() {
		Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		return (wMouseDown - mouse).magnitude > 0.1f;
	}

	void Update() {
		if (isMouseDown && IsMouseDrag() && dragging == false) {
			StartCoroutine(DragBoxRoutine(Camera.main.WorldToScreenPoint(wMouseDown)));
		}

		if (Input.GetButtonDown("Select")) {
			isMouseDown = true;
			wMouseDown = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}

		if (Input.GetButtonUp("Select")) {
			// Clear old selection
			foreach (GameObject old in selected) {
				old.transform.FindChild("SelectBox").gameObject.SetActive(false);
			}
			selected = new List<GameObject>();

			// Box selection
			Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (IsMouseDrag()) {
				Rect box = new Rect(wMouseDown.x, wMouseDown.y, mouse.x - wMouseDown.x, mouse.y - wMouseDown.y);
				foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Unit")) {
					if (box.Contains(unit.transform.position, true)) {
						selected.Add(unit);
						unit.transform.FindChild("SelectBox").gameObject.SetActive(true);
					}
				}
			}
			// Single selection
			else {
				var hit = Physics2D.Raycast(mouse, Vector2.zero, 0f);
				if (hit.transform != null && hit.transform.tag == "Unit") {
					selected.Add(hit.transform.gameObject);
					hit.transform.FindChild("SelectBox").gameObject.SetActive(true);
				}
			}

			isMouseDown = false;
		}

		if (Input.GetButtonDown("Order")) {
			Vector2 destination = Camera.main.ViewportToWorldPoint(
				Camera.main.ScreenToViewportPoint(Input.mousePosition));

			foreach (GameObject unit in selected) {
				Move move = unit.GetComponent<Move>();
				move.MoveTo(destination);
			}
		}
	}
}