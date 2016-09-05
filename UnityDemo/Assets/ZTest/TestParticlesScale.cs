using UnityEngine;
using System.Collections;

public class TestParticlesScale : MonoBehaviour 
{
	void OnWillRenderObject()
	{
		renderer.material.SetVector ("_Position", Camera.current.worldToCameraMatrix.MultiplyPoint (transform.root.position));
		renderer.material.SetVector("_Scale",transform.lossyScale);
        //renderer.material.SetVector("_Scale", new Vector3(transform.localScale.x * (transform.lossyScale.x < 0 ? -1.0f : 1.0f), transform.localScale.y * (transform.lossyScale.y < 0 ? -1.0f : 1.0f), transform.localScale.z * (transform.lossyScale.z < 0 ? -1.0f : 1.0f)));
	}
}
