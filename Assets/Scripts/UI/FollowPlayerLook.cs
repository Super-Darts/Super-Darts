/*
 *  Author: Charlie O'Regan
 *  Usage: Rotates a transform to follow the player's head rotation, also keeping the position at a desired offset
 */

using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.UI
{
	public class FollowPlayerLook : MonoBehaviour
	{
		[SerializeField] private Transform m_FollowTransform;

		[SerializeField] private float m_PositionOffset = 2.0f;
		[SerializeField] private float m_FollowPositionSpeed = 5.0f;

		private Vector3 m_FollowVelocity;

		private void Update()
		{
			// Follow position
			Vector3 desiredPosition = m_FollowTransform.TransformPoint(Vector3.forward * m_PositionOffset); // Get offset relative to follow transform
			transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref m_FollowVelocity, Time.deltaTime * m_FollowPositionSpeed);

			// Rotate towards follow transform
			transform.LookAt(2 * transform.position - m_FollowTransform.position);
		}
	}
}