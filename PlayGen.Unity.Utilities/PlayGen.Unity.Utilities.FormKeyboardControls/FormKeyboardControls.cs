﻿using System.Linq;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PlayGen.Unity.Utilities.FormKeyboardControls
{
	public class FormKeyboardControls : MonoBehaviour
	{
		/// <summary>
		/// Button to be triggered on pressing return key. Can be left null if not neeeded or wanted.
		/// </summary>
		[Tooltip("Button to be triggered on pressing return key. Can be left null if not neeeded or wanted.")]
		[SerializeField]
		protected Button _returnButton;
		/// <summary>
		/// Button to be triggered on pressing escape key. Can be left null if not neeeded or wanted.
		/// </summary>
		[Tooltip("Button to be triggered on pressing escape key. Can be left null if not neeeded or wanted.")]
		[SerializeField]
		protected Button _escapeButton;
		protected bool _selectOnEnable;

		protected virtual void OnEnable()
		{
			EventSystem.current.SetSelectedGameObject(null);
			_selectOnEnable = true;
		}

		protected virtual void Update()
		{
			if (_selectOnEnable && !Application.isMobilePlatform)
			{
				if (GetComponentsInChildren<InputField>().Any())
				{
					SetOnPointerClick(GetComponentsInChildren<InputField>().First());
				}
				_selectOnEnable = false;
			}
			if (Input.GetKeyDown(KeyCode.Tab))
			{
				var back = Input.GetKey(KeyCode.LeftShift);
				var nextObj = EventSystem.current.currentSelectedGameObject;
				var next = nextObj ?
							back ?
							(nextObj.GetComponent<Selectable>().navigation.selectOnUp ?? nextObj.GetComponent<Selectable>().FindSelectableOnUp()) ?? (nextObj.GetComponent<Selectable>().navigation.selectOnLeft ?? nextObj.GetComponent<Selectable>().FindSelectableOnLeft()) :
							(nextObj.GetComponent<Selectable>().navigation.selectOnDown ?? nextObj.GetComponent<Selectable>().FindSelectableOnDown()) ?? (nextObj.GetComponent<Selectable>().navigation.selectOnRight ?? nextObj.GetComponent<Selectable>().FindSelectableOnRight()) :
							null;

				if (!next)
				{
					if (GetComponentsInChildren<InputField>().Any())
					{
						next = back ? GetComponentsInChildren<InputField>().Last() : GetComponentsInChildren<InputField>().First();
					}
					else
					{
						return;
					}
				}
				SetOnPointerClick(next);
			}
			else if (Input.GetKeyDown(KeyCode.Return) && _returnButton)
			{
				var nextObj = EventSystem.current.currentSelectedGameObject;
				if (!nextObj || (nextObj && nextObj.GetComponent<InputField>()))
				{
					_returnButton.onClick.Invoke();
				}
			}
			else if (Input.GetKeyDown(KeyCode.Escape) && _escapeButton)
			{
				_escapeButton.onClick.Invoke();
			}
		}

		protected virtual void SetOnPointerClick(Selectable select)
		{
			var inputfield = select.GetComponentInChildren<InputField>();
			if (inputfield)
			{
				inputfield.OnPointerClick(new PointerEventData(EventSystem.current));
			}
			select.Select();
		}
	}
}