using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class CalendarButton : MonoBehaviour {

	public CalendarManager calendar;
	public Button button;
	public Text text;

	//マスの日時
	[HideInInspector]public DateTime dateValue;
	//ボタン番号
	[HideInInspector]public int index;
	void Start() {

		calendar = FindObjectOfType<CalendarManager> ();
		button = GetComponent<Button> ();
		if (button != null) {
			text = button.GetComponentInChildren<Text> ();
		
			text.fontSize = 60;
			//gameObject.AddComponent<CalendarButton> ();
			this.ObserveEveryValueChanged (date => date.dateValue)
			.Subscribe (_ => {
				text.text = dateValue.isDefault() ? "" : dateValue.Day.ToString ();

				text.color = GetColorDayOfWeek (dateValue.DayOfWeek);
				if (dateValue == DateTime.Today) {
					//GetComponent<Image>().color = new Color(0f/255.0f, 200.0f/255.0f, 0f/255.0f, 255.0f/255.0f);
					//GetComponent<Image> ().enabled = true;
				}else{
					//GetComponent<Image>().color = new Color(255.0f/255.0f, 255.0f/255.0f, 255.0f/255.0f, 255.0f/255.0f);
					//GetComponent<Image> ().enabled = false;
				}
			});
			
			button.onClick.AddListener (() => {
				
				Debug.Log(text.text);
				//カレンダーの日をタップするとその日のスケジュールへ遷移

			});
		}
	}

	//色を指定
	Color GetColorDayOfWeek(DayOfWeek dayOfWeek)
	{
		if(dateValue.Month == calendar.current.Month)
		{
			switch (dayOfWeek)
			{

			case DayOfWeek.Saturday:
				return Color.blue;
			case DayOfWeek.Sunday:
				return Color.red;
			default:
				return Color.black;
			}
		}

		else
		{
			return Color.gray;
		}
	}
}
//DateTime拡張
public static class DateTimeExtension
{
	//デフォルトの0001/01/01が入る
	static DateTime Default = new DateTime();
	//デフォルトの値と比較
	public static bool isDefault(this DateTime d) { return d.Equals(Default); }
}