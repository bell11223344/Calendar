using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTouch : MonoBehaviour {

	//CalendarManagerスクリプト
	public CalendarManager calendar;

	private Vector3 touchStartPos;
	private Vector3 touchEndPos;

	void Update() {
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			touchStartPos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
		}
		if (Input.GetKeyUp (KeyCode.Mouse0)) {
			touchEndPos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
			GetDirection ();
		}
	}

	void GetDirection() {
		float directionX = touchEndPos.x - touchStartPos.x;
		float directionY = touchEndPos.y - touchStartPos.y;
		string Direction = "";

		if (Mathf.Abs (directionY) < Mathf.Abs (directionX)) {
			if (30 < directionX) {
				//右向きにフリック
				Direction = "right";
			} else if (-30 > directionX) {
				//左向きにフリック
				Direction = "left";
			}
		} else if (Mathf.Abs (directionX) < Mathf.Abs (directionY)) {
			if (30 < directionY) {
				//上向きにフリック
				Direction = "up";
			} else if (-30 > directionY) {
				//下向きのフリック
				Direction = "down";
			} else {
				//タッチを検出
				Direction = "touch";
			}
		}
			
		switch (Direction){
			case "up":
				//上フリックされた時の処理
				Debug.Log("上フリック");
				break;
			case "down":
				//下フリックされた時の処理
				Debug.Log("下フリック");
				break;
			case "right":
				//右フリックされた時の処理
				Debug.Log("右フリック");
				calendar.current = calendar.current.AddMonths (-1);
				calendar.SetCalendar ();
				break;
			case "left":
				//左フリックされた時の処理
				Debug.Log("左フリック");
				calendar.current = calendar.current.AddMonths (1);
				calendar.SetCalendar ();
				break;
			case "touch":
				//タッチされた時の処理
				Debug.Log("タッチ");
				break;
		}
	}
			

	/*
	private float minSwipeDistX;
	private float minSwipeDistY;

	private Vector2 startPos;
	private Vector2 endPos;

	void Start () {

		minSwipeDistX = 30;
		minSwipeDistY = 30;

	}
	
	void Update () {

		if(Input.touchCount > 0){
				
			//タッチを取得
			Touch touch = Input.touches [0];
			//タッチフェーズによって場合分け
			switch (touch.phase) {
			//タッチ開始時 
			case TouchPhase.Began:
				startPos = touch.position;
				break;
			//タッチ終了時
			case TouchPhase.Ended:
				endPos = touch.position;
				float swipeDistX = (new Vector3 (endPos.x, 0, 0) - new Vector3 (startPos.x, 0, 0)).magnitude;
				float swipeDistY = (new Vector3 (0, endPos.y, 0) - new Vector3 (0, startPos.y, 0)).magnitude;

				Debug.Log("X" + minSwipeDistX.ToString ());
				Debug.Log("Y" + minSwipeDistY.ToString ());

				if (swipeDistX > swipeDistY && swipeDistX > minSwipeDistX) {
					float SignValueX = Mathf.Sign (endPos.x - startPos.x);
					if (SignValueX > 0){
						//右にスワイプした時
						Debug.Log("右にスワイプ");
					} else if (SignValueX < 0) {
						//左にスワイプした時
						Debug.Log("左にスワイプ");
					}
				} else if (swipeDistY > minSwipeDistY) {
					float SignValueY = Mathf.Sign ( endPos.y - startPos.y );
					if (SignValueY > 0) {
						//上にスワイプした時
						Debug.Log("上にスワイプ");
					} else if (SignValueY < 0){
						//下にスワイプした時
						Debug.Log("下にスワイプ");
					}
				}
				if (swipeDistX < minSwipeDistX && swipeDistY < minSwipeDistY) {
					//タップした時
				}
				break;
			}
		}
		
	}*/
}
