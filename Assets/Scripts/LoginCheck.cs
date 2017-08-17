using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginCheck : MonoBehaviour {

	public ServerAccess serverAccess;

	public InputField idInputField;
	public InputField passwordInputField;

	public Toggle idToggle;
	public Toggle passwordToggle;

	public Text errorText;

	public string ServerAddress = "https://nakap.sakura.ne.jp/php/calendar/logincheck.php";	//selecttest.phpを指定

	public void Start(){

		ServerAccess serverAccess = GetComponent<ServerAccess> ();

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
		StartCoroutine(postDataProcess(dic));

	}

	private IEnumerator postDataProcess (Dictionary<string, string> dic) {

		//サーバーへアクセスするIEnumerator Post()を外部スクリプトから参照する
		var coroutine = serverAccess.Post (ServerAddress, dic);
		yield return StartCoroutine (coroutine);
		var wwwText = (string)coroutine.Current;

		//送られてきたデータをテキストに反映
		if (wwwText.ToString() == "Success") {
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

	}

}
