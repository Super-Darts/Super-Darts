/*
 *  Author: Lewis Comstive
 *  Usage:Sets values in an AudioMixer based on slider values
 */
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace Assets.Scripts.UI
{
	/// <summary>
	/// Sets values in an AudioMixer based on slider values
	/// </summary>
	public class AudioSliders : MonoBehaviour
	{
		[SerializeField] private AudioMixer m_Mixer;
		[SerializeField] private Vector2 m_VolumeRange = new Vector2(-40, 0);

		[Space()]
		[Header("Exposed Properties")] // Names of exposed properties in AudioMixer
		[SerializeField] private string m_MasterVolumePropertyName = "Master Volume";
		[SerializeField] private string m_MusicVolumePropertyName = "Music Volume";
		[SerializeField] private string m_SFXVolumePropertyName = "SFX Volume";

		[Space()]
		[Header("UI")]
		[SerializeField] private Slider m_MasterVolumeSlider;
		[SerializeField] private Slider m_MusicVolumeSlider;
		[SerializeField] private Slider m_SFXVolumeSlider;

		[SerializeField] private Toggle m_MasterMuteToggle;
		[SerializeField] private Toggle m_MusicMuteToggle;
		[SerializeField] private Toggle m_SFXMuteToggle;

		private void Start()
		{
			PlayerData.Load();

			if(m_SFXMuteToggle)		m_SFXMuteToggle.isOn	= PlayerData.Data.SFXMuted;
			if(m_MusicMuteToggle)	m_MusicMuteToggle.isOn	= PlayerData.Data.MusicMuted;
			if(m_MasterMuteToggle)	m_MasterMuteToggle.isOn = PlayerData.Data.MasterMuted;

			// Listen to slider changes
			m_SFXVolumeSlider.onValueChanged.AddListener(OnSFXVolumeValueChanged);
			m_MusicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeValueChanged);
			m_MasterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeValueChanged);

			// Load slider values and set audiomixer properties
			OnSFXVolumeValueChanged(m_SFXVolumeSlider.value		  = PlayerData.Data.SFXVolume);
			OnMusicVolumeValueChanged(m_MusicVolumeSlider.value	  = PlayerData.Data.MusicVolume);
			OnMasterVolumeValueChanged(m_MasterVolumeSlider.value = PlayerData.Data.MasterVolume);
		}

		private void OnDestroy()
		{
			// Stop listening to slider changes
			m_SFXVolumeSlider.onValueChanged.RemoveListener(OnSFXVolumeValueChanged);
			m_MusicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeValueChanged);
			m_MasterVolumeSlider.onValueChanged.RemoveListener(OnMasterVolumeValueChanged);			

			PlayerData.Save();
		}

		/// <param name="name">Name of exposed property to alter</param>
		/// <param name="value">Percentage, ranging 0.0-1.0</param>
		public void SetProperty(string name, float value) => m_Mixer.SetFloat(name, PercentageToVolumeLevel(value));

		/// <summary>
		/// Converts <paramref name="value"/> to a value based on <see cref="m_VolumeRange"/>, conversion goes from percentage (0.0-1.0) to decibels (typically -80dB to +20dB).
		/// </summary>
		/// <param name="value">Percentage value, ranging 0.0-1.0</param>
		/// <returns>Decibel value, typically ranging -80dB to +20dB</returns>
		public float PercentageToVolumeLevel(float value) =>
			Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1.0f)) * (m_VolumeRange.y - m_VolumeRange.x) / 4 + m_VolumeRange.y;

		// Mute button callbacks //
		public void OnMuteSFX(bool mute) => SetProperty(m_MasterVolumePropertyName, (PlayerData.Data.MasterMuted = mute) ? 0 : m_SFXVolumeSlider.value);
		public void OnMuteMusic(bool mute) => SetProperty(m_MusicVolumePropertyName, (PlayerData.Data.MusicMuted = mute) ? 0 : m_MusicVolumeSlider.value);
		public void OnMuteMaster(bool mute) => SetProperty(m_SFXVolumePropertyName, (PlayerData.Data.SFXMuted = mute) ? 0 : m_MasterVolumeSlider.value);

		// Slider callbacks //
		private void OnMasterVolumeValueChanged(float value)
		{
			SetProperty(m_MasterVolumePropertyName, value);
			PlayerData.Data.MasterVolume = value;

			// If muted but volume slider changed, unmute
			if (m_MasterMuteToggle?.isOn ?? false && value != 0)
				PlayerData.Data.MasterMuted = m_MasterMuteToggle.isOn = false;
		}

		private void OnMusicVolumeValueChanged(float value)
		{
			SetProperty(m_MusicVolumePropertyName, value);
			PlayerData.Data.MusicVolume = value;

			// If muted but volume slider changed, unmute
			if (m_MusicMuteToggle?.isOn ?? false && value != 0)
				PlayerData.Data.MusicMuted = m_MusicMuteToggle.isOn = false;
		}

		private void OnSFXVolumeValueChanged(float value)
		{
			SetProperty(m_SFXVolumePropertyName, value);
			PlayerData.Data.SFXVolume = value;

			// If muted but volume slider changed, unmute
			if (m_SFXMuteToggle?.isOn ?? false && value != 0)
				PlayerData.Data.SFXMuted = m_SFXMuteToggle.isOn = false;
		}
	}
}