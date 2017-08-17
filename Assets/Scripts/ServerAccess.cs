using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerAccess : MonoBehaviour {

	public IEnumerator Post(string url, Dictionary<string, string> post) {
		WWWForm form = new WWWForm();
		foreach (KeyValuePair<string, string> post_arg in post) {
			form.AddField(post_arg.Key, post_arg.Value);
		}
		WWW www = new WWW(url, form);

		yield return StartCoroutine(CheckTimeOut(www, 3f));	//TimeOutSecond = 3s;

		if (www.error != null) {
			Debug.Log("HttpPost NG: " + www.error);
			//そもそも接続ができていない時
		} else if (www.isDone) {
			Debug.Log("wwwtext:" + www.text + ";");
			//postDataProcess (www.text);
			yield return www.text;
		}
	}

	private IEnumerator CheckTimeOut( WWW www, float timeout ) {
		float requestTime = Time.time;

		while (!www.isDone) {
			if (Time.time - requestTime < timeout)
				yield return null;
			else {
				Debug.Log("TimeOut");
				break;
			}
		}
		yield return null;
	}
}
