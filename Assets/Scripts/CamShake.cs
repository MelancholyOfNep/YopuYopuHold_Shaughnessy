using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour
{
	public static CamShake Instance;

	[SerializeField]
	Transform camTransform; // Camera gameobject's transform component

	public float countdown = 1f; // modify this externally to start the timer

	[SerializeField]
	float force = 0.7f; // how much it shakes
	[SerializeField]
	float decay = 1.0f; // rate of decay of the shake

	Vector3 defaultpos; // where the camera should return to

	void Awake()
	{
		Instance = this;
		countdown = 0;
	}

	void OnEnable()
	{
		defaultpos = camTransform.localPosition;
	}

	void Update()
	{
		if (countdown > 0)
		{
			camTransform.localPosition = defaultpos + Random.insideUnitSphere * force;
			countdown -= Time.deltaTime * decay;
		}
		else
		{
			countdown = 0f;
			camTransform.localPosition = defaultpos;
		}
	}
}

