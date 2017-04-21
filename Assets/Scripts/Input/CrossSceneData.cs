using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSceneData {

	private static CrossSceneData instance;

	private CrossSceneData() { }

	public static CrossSceneData Instance {
		get {
			if (instance == null) {
				instance = new CrossSceneData();
			}
			return instance;
		}
	}

	List<int> activeControllers;

	public List<int> GetActiveControllers() {
		return activeControllers;
	}

	public void SetActiveControllers(List<int> conts) {
		activeControllers = conts;
	}
	
}
