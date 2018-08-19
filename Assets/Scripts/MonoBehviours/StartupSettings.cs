using System.Reflection;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MonoBehviours
{
	public class StartupSettings : MonoBehaviour
	{
		[SerializeField]private SettingsObject _settings;

		public void SetValue(string target, float value)
		{
			FieldInfo field = typeof(SettingsObject).GetField(target);
			//if (field == null) return;
			if (field.FieldType == typeof(float))
			{
				field.SetValue(_settings, value);
			}
			else if(field.FieldType == typeof(int))
			{
				field.SetValue(_settings, (int)(value + 0.1f));
			}
			else if(field.FieldType == typeof(bool))
			{
				field.SetValue(_settings, value > 0.5f);
			}
		}

		public float GetValue(string target)
		{
			FieldInfo field = typeof(SettingsObject).GetField(target);
			//if (field == null) return 4.04f;
			if (field.FieldType == typeof(float))
			{
				return (float)field.GetValue(_settings);
			}
			else if(field.FieldType == typeof(int))
			{
				return (int)field.GetValue(_settings);
			}
			else if(field.FieldType == typeof(bool))
			{
				return (bool)field.GetValue(_settings) ? 1f : 0f;
			}

			return 4.04f;
		}

		public void Play()
		{
			SceneManager.LoadScene("WorldScene");
		}
	}
}