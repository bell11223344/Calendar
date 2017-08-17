using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class YoteiCatch : MonoBehaviour {

	public Text ResultText_;
	public string ServerAddress = "https://nakap.sakura.ne.jp/php/calendar/selecttest.php";	//selecttest.phpを指定

	public CalendarManager calendarManager;								//CalendarManagerの関数、変数が使いたいので
	public CalendarButton calendarButton;								//CalendarButtonの関数、変数が使いたいので

	//カレンダーの日時
	public DateTime current;

	void Start(){

		calendarManager = FindObjectOfType<CalendarManager> ();
		calendarButton = FindObjectOfType<CalendarButton> ();

		StartCoroutine("Access");	//コルーチン

	}

	private IEnumerator Access() {

		//起点となる今月の１日目
		var first = new DateTime (calendarManager.GetcurrentYear(), calendarManager.GetcurrentMonth(), 1);

		current = first;												//currentに日付を作成
		string buttonNum = PlayerPrefs.GetString("Index");				//押したボタンのインデックスナンバー
		int day = int.Parse(PlayerPrefs.GetString("Day"))-1;			//押したボタンのテキスト（日にち）

		current = current.AddDays(day);

		Debug.Log("currentDay: "+current.Day);

		Dictionary<string, string> dic = new Dictionary<string, string>();

		//先月の日付なら月-1, 来月の日付なら月+1
		if (int.Parse (buttonNum) <= (int)first.DayOfWeek) {
			Debug.Log ("先月");
			current = current.AddMonths (-1);
		} else if (current.Day > DateTime.DaysInMonth (current.Year, current.Month)) {
			Debug.Log ("来月");
			current = current.AddMonths (1);
		}

		//Dictionaryに格納します
		dic.Add("Year", current.Year.ToString());
		dic.Add("Month", current.Month.ToString());
		dic.Add("Day", current.Day.ToString());

		//Dictionary内の確認
		Debug.Log (dic["Year"]+dic["Month"]+dic["Day"]);

		//phpへのアクセス, Dictionary（年月日）を送る
		StartCoroutine(Post(ServerAddress, dic));

		yield return 0;
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

			if (www.text != "") {
				ResultText_.GetComponent<Text> ().text = www.text;
			} else {
				Debug.Log ("予定がありません");
				ResultText_.GetComponent<Text> ().text = "予定はありません。";
			}
			Debug.Log("wwwtext:" + www.text + ";");
			//ResultText_.text = www.text.ToString();
			//Debug.Log(ResultText_);
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
