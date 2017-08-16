using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;


public class CalendarManager : MonoBehaviour {

	//ボタンの親オブジェクト
	public GameObject calendarParent;
	//来月へ
	public Button nextButton;
	//先月へ
	public Button prevButton;
	//今月へ
	public Button nowButton;
	//カレンダーの日時
	public DateTime current;
	//Buttonオブジェクト
	GameObject[] objDays = new GameObject[42];
	//カレンダーの日付マス
	CalendarButton[] Days = new CalendarButton[42];
	//
	public Text yearText;
	public Text monthText;

	void Start(){

		InitCalendarTime ();

		InitCalendarComponent ();
		SetCalendar ();

		if (nextButton != null) {

			//押されたら起動
			nextButton.onClick.AsObservable ()
				.Subscribe (_ => {
					//一つ月を進める
					current = current.AddMonths (1);
					SetCalendar ();
				});
		}
		if (prevButton != null) {
			prevButton.onClick.AsObservable ()
				.Subscribe (_ => {
					current = current.AddMonths (-1);
					SetCalendar ();
				});
		}
		if (nowButton != null) {
			nowButton.onClick.AsObservable ()
				.Subscribe (_ => {
					InitCalendarTime ();
					SetCalendar ();
			});
		}

	}

	public void OnClick(Button button)
	{
		//Debug.Log (button.index);
	}

	//コンポーネントの取得、設定
	void InitCalendarComponent()
	{

		for (int i = 0; i < calendarParent.transform.childCount; i++) {
			//子オブジェクトを保存
			objDays [i] = calendarParent.transform.GetChild (i).gameObject;
			//コンポーネントを設定、取得
			Days [i] = objDays [i].AddComponent<CalendarButton> ();
			Days [i].index = i + 1;

		}
	}

	//カレンダーに日付をセット
	public void SetCalendar()
	{
		int day = 1;

		//今月の１日目
		var first = new DateTime (current.Year, current.Month, day);

		//来月
		var nextMonth = current.AddMonths (1);
		int nextMonthDay = 1;
		//先月
		var prevMonth = current.AddMonths (-1);
		//先月の場合は後ろから数える。
		int prevMonthDay = DateTime.DaysInMonth (prevMonth.Year, prevMonth.Month) - (int)first.DayOfWeek + 1;

		yearText.GetComponent<Text>().text = current.Year.ToString();
		monthText.GetComponent<Text> ().text = current.Month.ToString()+"月";
		//yearText.text = System.DateTime.Now.Year.ToString();
		//Debug.Log(System.DateTime.Now.Year.ToString());
			
		foreach (var cDay in Days) 
		{
			//今月の１日より前のマスには先月の日にちを入れる。
			if (cDay.index <= (int)first.DayOfWeek) {
				cDay.dateValue = new DateTime (prevMonth.Year, prevMonth.Month, prevMonthDay);
				prevMonthDay++;
			}
			//今月の最終日より後ろのマスには来月の日にちを入れる。
			else if (day > DateTime.DaysInMonth (current.Year, current.Month)) 
			{
				cDay.dateValue = new DateTime (nextMonth.Year, nextMonth.Month, nextMonthDay);
				nextMonthDay++;
			}
			//今月の日にちをマスに入れる。
			else {
				cDay.dateValue = new DateTime (current.Year, current.Month, day);
				day++;
			}
		}
	}

	//日付リセット
	void InitCalendarTime(){
		current = new DateTime (DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
	}

	public int GetcurrentYear(){
		return current.Year;
	}
	public int GetcurrentMonth(){
		return current.Month;
	}
}
