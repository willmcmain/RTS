using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour {
	public GameObject selected;
	[SerializeField] private Vector2 destination;

	void Update() {
		if (Input.GetButtonDown ("Order")) {
			destination = Camera.main.ViewportToWorldPoint (
				Camera.main.ScreenToViewportPoint (Input.mousePosition));

			//selected.transform.position = destination;

			Move move = selected.GetComponent<Move> ();
			move.MoveTo (destination);
		}
	}
}
