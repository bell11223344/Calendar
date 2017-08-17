using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoteiDestroy : MonoBehaviour {

	public void OnClickButton(){
		GameObject obj = GameObject.Find ("GlayOutPanel(Clone)");
		Debug.Log (obj);
		Destroy (obj);

		obj = GameObject.Find ("YoteiPanel(Clone)");
		Destroy (obj);
		obj = null;
	}
}
