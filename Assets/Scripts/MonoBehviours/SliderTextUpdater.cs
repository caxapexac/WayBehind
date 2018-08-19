using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace MonoBehviours
{
	public class SliderTextUpdater : MonoBehaviour
	{
		private Slider _slider;
		private InputField _label;
		private string _target;
		private StartupSettings _startup;
	
		void Start ()
		{
			_startup = FindObjectOfType<StartupSettings>();
			_target = transform.parent.parent.name;
			
			_slider = GetComponentInParent<Slider>();
			_slider.value = _startup.GetValue(_target);
			_slider.onValueChanged.AddListener(ChangeListener);
			
			_label = GetComponent<InputField>();
			_label.text = _slider.value.ToString();
			_label.onEndEdit.AddListener(EditListener);
		}

		private void ChangeListener(float value)
		{
			_label.text = _slider.value.ToString();
			_startup.SetValue(_target, _slider.value);
		}

		private void EditListener(string value)
		{
			float result;
			if (float.TryParse(value, out result))
			{
				_slider.value = result;
				_startup.SetValue(_target, result);
			}
			else
			{
				_label.text = _slider.value.ToString();
			}
		}
	}
}
