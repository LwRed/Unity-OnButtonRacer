﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour {

	public Color lineColor;

	private List<Transform> nodes = new List<Transform>();

	void OnDrawGizmos() {
		Gizmos.color = lineColor;

		// Get child transforms
		Transform[] pathTransforms = GetComponentsInChildren<Transform>();
		nodes = new List<Transform> ();

		// create list skipping the parent transform
		for (int i = 0; i < pathTransforms.Length; i++) {
			if (pathTransforms [i] != transform) {
				nodes.Add (pathTransforms [i]);
			}
		}

		// draw lines between nodes
		for (int i = 0; i < nodes.Count; i++) {
			Vector3 currentNode = nodes[i].position;
			Vector3 previousNode = Vector3.zero;

			if (i == 0 && nodes.Count > 1) {
				previousNode = nodes[nodes.Count - 1].position;
			} else {
				previousNode = nodes[i - 1].position;
			}

			Gizmos.DrawLine (previousNode,currentNode);
			Gizmos.DrawWireSphere (currentNode, 0.3f);
		}
	}
}
