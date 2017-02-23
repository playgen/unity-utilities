using System.Linq;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PlayGen.Unity.Utilities.FormKeyboardControls
{

	public class FormKeyboardControls : MonoBehaviour
	{

		[SerializeField]
		private Button _returnButton;
		[SerializeField]
		private Button _escapeButton;
		private bool _selectOnEnable;

		private void OnEnable()
		{
			EventSystem.current.SetSelectedGameObject(null);
			_selectOnEnable = true;
		}

		private void Update()
		{
			if (_selectOnEnable)
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

		private void SetOnPointerClick(Selectable select)
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