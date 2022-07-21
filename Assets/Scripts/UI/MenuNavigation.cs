/*
 *  Author: Lewis Comstive
 *  Usage: Switches between various "pages" of UI navigation, can be shown & hidden w/ user input
 */
using UnityEngine;
using UnityEngine.Events;
using Assets.Scripts.Tweens;
using System.Threading.Tasks;
using UnityEngine.InputSystem;
using Assets.Scripts.Extensions;

namespace Assets.Scripts.UserInterface
{
	/// <summary>
	/// Switches between various "pages" of UI navigation, can be shown & hidden w/ user input
	/// </summary>
	public class MenuNavigation : MonoBehaviour
	{
		// Inspector Variables //
		
		/// <summary>
		/// The input action to show and hide the menu
		/// </summary>
		[SerializeField] private InputActionReference m_ToggleAction;

		/// <summary>
		/// Pages that can be shown.
		/// First item is always shown when UI is initially shown
		/// </summary>
		[SerializeField, Tooltip("Pages that can be shown. First item is always shown when UI is initially shown")]
		private GameObject[] m_Pages;

		/// <summary>
		/// Value to set timescale to while any menu is open
		/// </summary>
		[SerializeField, Tooltip("Timescale to use while menus are open")]
		private float m_Timescale = 0.05f;

		/// <summary>
		/// Event when UI is activated
		/// </summary>
		[SerializeField]
		private UnityEvent m_OnUIShown;

		/// <summary>
		/// Event when UI is de-activated and becomes hidden
		/// </summary>
		[SerializeField]
		private UnityEvent m_OnUIClosed;

		/// <summary>
		/// Current visibility state of menu UI
		/// </summary>
		public bool IsShowing { get; private set; } = false;

		/// <summary>
		/// Currently active menu page.
		/// Is one of <see cref="m_Pages"/>
		/// </summary>
		private GameObject m_ActiveTarget;

		private void Start()
		{
			// Iterate over all pages and set their values
			//  based on any found tweener's Rewind().
			// This ensures all tween-modified properties are at their initial values
			foreach (GameObject target in m_Pages)
				// Disable object
				target.SetActive(false);

			// Listen to input action
			if (m_ToggleAction)
				m_ToggleAction.action.started += OnToggleMenu;
			else
				ShowPage(0);
		}

		/// <summary>
		/// This script has been destroyed. Resets any modified state and listeners.
		/// </summary>
		private void OnDestroy()
		{
			Time.timeScale = 1.0f;
			if(m_ToggleAction)
				m_ToggleAction.action.started -= OnToggleMenu;
		}

		/// <summary>
		/// Callback for <see cref="m_ToggleAction"/> being pressed.
		/// Toggles visibility of UI menu
		/// </summary>
		private void OnToggleMenu(InputAction.CallbackContext _) => ShowUI(!IsShowing);

		/// <summary>
		/// Shows or hides <see cref="target"/>
		/// </summary>
		private async void ShowUI(GameObject target, bool show)
		{
			if (show)
			{
				target.SetActive(true);
				m_ActiveTarget = target;
			}

			// Iterate over all tweeners and play or rewind them all
			float tweenLength = 0;
			ITweener[] tweeners = target.GetComponents<ITweener>();
			foreach (ITweener tweener in tweeners)
			{
				// Store the length of the longest tween,
				// this is used to wait for it so that script execution
				// doesn't interrupt any tween animations
				tweenLength = Mathf.Max(tweenLength, tweener.Duration);

				if (!show)
					tweener.Rewind();
			}

			// Wait for tween to finish
			if (!show)
			{
				await Task.Delay(Mathf.RoundToInt(tweenLength * 1000));
				target.SetActive(false);
			}
		}

		/// <summary>
		/// Shows UI of <see cref="m_Pages"/> at <see cref="index"/>
		/// </summary>
		public void ShowPage(int index)
		{
			if (m_Pages.Length == 0)
				return; // Nothing to change
			
			// Hide active page
			if (m_ActiveTarget)
				ShowUI(m_ActiveTarget, false);

			// Show new page
			index = Mathf.Clamp(index, 0, m_Pages.Length - 1);
			ShowUI(m_Pages[index], true);
		}

		/// <summary>
		/// Shows UI and navigates to first <see cref="m_Pages"/>
		/// </summary>
		public void ShowUI(bool show)
		{
			if(show == IsShowing)
				return; // No change

			if (show) // Show UI, navigate to first page
			{
				ShowPage(0);
				m_OnUIShown.Invoke();
			}
			else // Hide UI
			{
				ShowUI(m_ActiveTarget, false);
				m_ActiveTarget = null;
				m_OnUIClosed.Invoke();
			}
			IsShowing = show;

			// Set timescale based on UI visibility
			Time.timeScale = IsShowing ? m_Timescale : 1.0f;
		}
	}
}