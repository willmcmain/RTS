using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour {
	public List<GameObject> selected;
	[SerializeField] private Vector2 destination;
	private Vector2 mouseDown;

	void Awake() {
		selected = new List<GameObject>();
	}

	void Update() {
		if (Input.GetButtonDown("Select")) {
			mouseDown = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}

		if (Input.GetButtonUp("Select")) {
			// Clear old selection
			foreach (GameObject old in selected) {
				old.transform.FindChild("SelectBox").gameObject.SetActive(false);
			}
			selected = new List<GameObject>();

			Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			// Box selection
			if ((mouseDown - mouse).magnitude > 0.1f) {
				Rect box = new Rect(mouseDown.x, mouseDown.y, mouse.x - mouseDown.x, mouse.y - mouseDown.y);
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
		}

		if (Input.GetButtonDown("Order")) {
			destination = Camera.main.ViewportToWorldPoint(
				Camera.main.ScreenToViewportPoint(Input.mousePosition));

			foreach (GameObject unit in selected) {
				Move move = unit.GetComponent<Move>();
				move.MoveTo(destination);
			}
		}
	}
}