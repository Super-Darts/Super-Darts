/*
 *  Author: Lewis Comstive
 *  Usage: Used to modify player save data
 */
using UnityEngine;
using UnityEditor;

namespace Assets.Scripts.Editor
{
	public class PlayerDataEditor : EditorWindow
	{
		[MenuItem("Tools/Save Data Editor")]
		public static void OpenWindow() => GetWindow<PlayerDataEditor>().Show();

		private void OnEnable() => PlayerData.Load();

		private void OnGUI()
		{
			titleContent = new GUIContent("Player Save Data Editor");
			EditorGUILayout.LabelField($"Path: {PlayerData.DefaultSavePath}");
			EditorGUILayout.Space(20);

			// Save Data //

			PlayerData.Data.Highscore = Mathf.Max(0, EditorGUILayout.IntField("Highscore", PlayerData.Data.Highscore));
			PlayerData.Data.UnlockedLevelIndex = (byte)Mathf.Clamp(EditorGUILayout.IntField("Unlocked Levels", PlayerData.Data.UnlockedLevelIndex), 0, 255);

			// Unlocked Prizes //
			EditorGUILayout.LabelField("Prizes");
			for(int i = 0; i < PlayerData.Data.UnlockedPrizes.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();

				PlayerData.Data.UnlockedPrizes[i] = EditorGUILayout.TextField(PlayerData.Data.UnlockedPrizes[i]);
				if(GUILayout.Button("-"))
				{
					PlayerData.Data.UnlockedPrizes.RemoveAt(i);
					i--;
				}

				EditorGUILayout.EndHorizontal();
			}
			if(GUILayout.Button("+"))
				PlayerData.Data.UnlockedPrizes.Add("Prize Name");

			// Volumes //
			EditorGUILayout.LabelField("Volume Settings");
			EditorGUI.indentLevel++;

			EditorGUILayout.Space(10);
			PlayerData.Data.MasterVolume = EditorGUILayout.Slider(PlayerData.Data.MasterVolume, 0.0f, 1.0f);
			PlayerData.Data.MasterMuted = EditorGUILayout.Toggle("Mute", PlayerData.Data.MasterMuted);

			EditorGUILayout.Space(10);
			PlayerData.Data.MusicVolume = EditorGUILayout.Slider(PlayerData.Data.MusicVolume, 0.0f, 1.0f);
			PlayerData.Data.MusicMuted = EditorGUILayout.Toggle("Mute", PlayerData.Data.MusicMuted);

			EditorGUILayout.Space(10);
			PlayerData.Data.SFXVolume = EditorGUILayout.Slider(PlayerData.Data.SFXVolume, 0.0f, 1.0f);
			PlayerData.Data.SFXMuted = EditorGUILayout.Toggle("Mute", PlayerData.Data.SFXMuted);

			EditorGUI.indentLevel--;

			EditorGUILayout.Space(20);

			// Save & Load //
			if(GUILayout.Button("Save"))
				PlayerData.Save();
			if (GUILayout.Button("Reload"))
				PlayerData.Load();
			
			EditorGUILayout.Space(20);

			// Reset //
			if (GUILayout.Button("Reset") &&
				EditorUtility.DisplayDialog("Reset save data", "Are you sure you want to reset the save data to defaults?", "Yes", "No"))
			{
				PlayerData.Data = new PlayerData.SaveData();
				PlayerData.Save();
			}
		}
	}
}