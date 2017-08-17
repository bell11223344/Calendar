using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginCheck : MonoBehaviour {

	public InputField idInputField;
	public InputField passwordInputField;

	public Toggle idToggle;
	public Toggle passwordToggle;

	public Text errorText;

	public string ServerAddress = "https://nakap.sakura.ne.jp/php/calendar/logincheck.php";	//selecttest.phpを指定

	public void Start(){
		//IDが保存されていたらロードする
		if (PlayerPrefs.HasKey("Id")) {
			idInputField.text = PlayerPrefs.GetString ("Id");
		}
		//パスワードが保存されていたらロードする
		if (PlayerPrefs.HasKey("Password")) {
			passwordInputField.text = PlayerPrefs.GetString ("Password");
		}
		//IDチェックボックスが保存されていたらTRUE,されていなかったらFALSE
		if (PlayerPrefs.HasKey ("IdToggle")) {
			idToggle.isOn = true;
		} else {
			idToggle.isOn = false;
		}
		//パスワードチェックボックスが保存されていたらTRUE,されていなかったらFALSE
		if (PlayerPrefs.HasKey ("PasswordToggle")) {
			passwordToggle.isOn = true;
		} else {
			passwordToggle.isOn = false;
		}
	}
	public void OnClick(){

		Debug.Log ("ID:" + idInputField.text);
		Debug.Log ("PASSWORD:" + passwordInputField.text);

		Debug.Log ("ID_TOGGLE:" + idToggle.isOn);
		Debug.Log ("PASSWORD_TOGGLE:" + passwordToggle.isOn);

		Dictionary<string, string> dic = new Dictionary<string, string>();
		dic.Add("Id", idInputField.text.ToString());
		dic.Add("Password", passwordInputField.text.ToString());

		//phpへのアクセス, Dictionary（ID,PASSWORD）を送る
		StartCoroutine(Post(ServerAddress, dic));

	}

	private IEnumerator Post(string url, Dictionary<string, string> post) {
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
			//送られてきたデータをテキストに反映

			if (www.text == "Success") {
				Debug.Log ("ログイン成功");
				//トグルにチェックがある場合は保存する,外れている場合はキーを削除
				if (idToggle.isOn) {
					PlayerPrefs.SetString ("Id", idInputField.text);
					PlayerPrefs.SetInt ("IdToggle", 1);

				} else {
					PlayerPrefs.DeleteKey ("Id");
					PlayerPrefs.DeleteKey ("IdToggle");
				}
				if (passwordToggle.isOn) {
					PlayerPrefs.SetString ("Password", passwordInputField.text);
					PlayerPrefs.SetInt ("PasswordToggle", 1);
				} else {
					PlayerPrefs.DeleteKey ("Password");
					PlayerPrefs.DeleteKey ("PasswordToggle");
				}

				SceneManager.LoadScene("Calendar", LoadSceneMode.Single);
			} else {
				errorText.text = "IDまたはPASSWORDが違います";
			}
			Debug.Log("wwwtext:" + www.text + ";");

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
