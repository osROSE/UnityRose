using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UI
{
	public class MessageBox
	{	
		public static void Show(GameObject messagebox, string message, string title, Action callback) {
			if(!messagebox){
				return;
			}
	
			Button btn = messagebox.transform.Find("TextBox/DissmisButton").gameObject.GetComponent<Button>();
			Text titleText = messagebox.transform.Find ("Titlebar/TitleText").gameObject.GetComponent<Text>();
			Text messageText = messagebox.transform.Find ("TextBox/MessageText").gameObject.GetComponent<Text>();

			titleText.text = title;
			messageText.text = message;
			
			btn.onClick.RemoveAllListeners();
			btn.onClick.AddListener(delegate {
				if(callback != null) {
					callback.Invoke();
				}
			});
		}
	}
}

