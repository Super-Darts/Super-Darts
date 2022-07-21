/*
 *  Author: Lewis Comstive
 *  Usage: Moves an object back to a particular position, resetting any rigidbody velocity
 */
using UnityEngine;

namespace Assets.Scripts.Helpers
{
	public class RespawnAtStartPosition : MonoBehaviour
	{
		private Vector3 m_SpawnPoint;
		private Rigidbody m_Rigidbody;

		private void Start()
		{
			m_SpawnPoint = transform.position;
			m_Rigidbody = GetComponent<Rigidbody>();
		}

		public void Respawn()
		{
			gameObject.SetActive(false);

			transform.position = m_SpawnPoint;
			m_Rigidbody.velocity = Vector3.zero;
			m_Rigidbody.angularVelocity = Vector3.zero;

			gameObject.SetActive(true);
		}
	}
}