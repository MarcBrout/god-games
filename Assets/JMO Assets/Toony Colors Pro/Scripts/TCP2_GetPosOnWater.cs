// Toony Colors Pro+Mobile 2
// (c) 2014-2017 Jean Moreno

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// Script to get the water height from a specific world position
// Useful to easily make objects float on water for example

public class TCP2_GetPosOnWater : MonoBehaviour
{
	public Material WaterMaterial;

	[Tooltip("Will make the object stick to the water plane")]
	public bool followWaterHeight = true;
	[Tooltip("Y Position offset")]
	public float heightOffset;
	[Tooltip("Height scale, for example if the water mesh is scaled along its Y axis")]
	public float heightScale = 1f;

	[SerializeField, HideInInspector]
	bool isValid;
	[SerializeField, HideInInspector]
	int sineCount;

	float[] sinePosOffsetsX = new float[] { 1.0f, 2.2f, 2.7f, 3.4f, 1.4f, 1.8f, 4.2f, 3.6f };
	float[] sinePosOffsetsZ = new float[] { 0.6f, 1.3f, 3.1f, 2.4f, 1.1f, 2.8f, 1.7f, 4.3f };
	float[] sinePhsOffsetsX = new float[] { 1.0f, 1.3f, 0.7f, 1.75f,0.2f, 2.6f, 0.7f, 3.1f };
	float[] sinePhsOffsetsZ = new float[] { 2.2f, 0.4f, 3.3f, 2.9f, 0.5f, 4.8f, 3.1f, 2.3f };

#if UNITY_EDITOR
	//Verify that the material has a valid shader
	void OnValidate()
	{
		this.isValid = false;

		if (WaterMaterial == null)
			return;

		var shader = WaterMaterial.shader;
		if(shader == null)
		{
			WaterMaterial = null;
		}

		bool validMaterial = false;
		var assetImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(shader)) as ShaderImporter;
		if(assetImporter != null)
		{
			sineCount = 1;
			string[] userData = assetImporter.userData.Split(',');
			foreach(var ud in userData)
			{
				if(ud == "FVERTEX_SIN_WAVES")
					validMaterial = true;

				if(ud == "FVSW_8")
					sineCount = 8;

				if (ud == "FVSW_4")
					sineCount = 4;

				if (ud == "FVSW_2")
					sineCount = 2;
			}
		}

		if(!validMaterial)
		{
			WaterMaterial = null;
			Debug.LogWarning("Please use a material that has a generated TCP2 Water Shader!");
		}

		this.isValid = validMaterial;
	}
#endif

	void LateUpdate()
	{
		if(followWaterHeight)
		{
			this.transform.position = GetPositionOnWater(this.transform.position);
		}
	}

	//Returns a world space position on a water plane, based on its material
	public Vector3 GetPositionOnWater(Vector3 worldPosition)
	{
		if(!isValid)
		{
			Debug.LogWarning("Invalid Water Material, returning the same worldPosition");
			return worldPosition;
		}

		float freq = WaterMaterial.GetFloat("_WaveFrequency");
		float height = WaterMaterial.GetFloat("_WaveHeight") * heightScale;
		float speed = WaterMaterial.GetFloat("_WaveSpeed");

		float phase = Time.time * speed;
		float x = worldPosition.x * freq;
		float z = worldPosition.z * freq;

		float waveFactorX = 0f;
		float waveFactorZ = 0f;

		switch (sineCount)
		{
			case 1:
				waveFactorX = Mathf.Sin(sinePosOffsetsX[0] * x + sinePhsOffsetsX[0] * phase) * height;
				waveFactorZ = Mathf.Sin(sinePosOffsetsX[0] * x + sinePhsOffsetsX[0] * phase) * height;
				break;

			case 2:
				waveFactorX = (Mathf.Sin(sinePosOffsetsX[0] * x + sinePhsOffsetsX[0] * phase) + Mathf.Sin(sinePosOffsetsX[1] * x + sinePhsOffsetsX[1] * phase)) * height / 2f;
				waveFactorZ = (Mathf.Sin(sinePosOffsetsZ[0] * z + sinePhsOffsetsZ[0] * phase) + Mathf.Sin(sinePosOffsetsZ[1] * z + sinePhsOffsetsZ[1] * phase)) * height / 2f;
				break;

			case 4:
				waveFactorX = (Mathf.Sin(sinePosOffsetsX[0] * x + sinePhsOffsetsX[0] * phase) + Mathf.Sin(sinePosOffsetsX[1] * x + sinePhsOffsetsX[1] * phase) + Mathf.Sin(sinePosOffsetsX[2] * x + sinePhsOffsetsX[2] * phase) + Mathf.Sin(sinePosOffsetsX[3] * x + sinePhsOffsetsX[3] * phase)) * height / 4f;
				waveFactorZ = (Mathf.Sin(sinePosOffsetsZ[0] * z + sinePhsOffsetsZ[0] * phase) + Mathf.Sin(sinePosOffsetsZ[1] * z + sinePhsOffsetsZ[1] * phase) + Mathf.Sin(sinePosOffsetsZ[2] * z + sinePhsOffsetsZ[2] * phase) + Mathf.Sin(sinePosOffsetsZ[3] * z + sinePhsOffsetsZ[3] * phase)) * height / 4f;
				break;

			case 8:
				waveFactorX = (Mathf.Sin(sinePosOffsetsX[0] * x + sinePhsOffsetsX[0] * phase) + Mathf.Sin(sinePosOffsetsX[1] * x + sinePhsOffsetsX[1] * phase) + Mathf.Sin(sinePosOffsetsX[2] * x + sinePhsOffsetsX[2] * phase) + Mathf.Sin(sinePosOffsetsX[3] * x + sinePhsOffsetsX[3] * phase) + Mathf.Sin(sinePosOffsetsX[4] * x + sinePhsOffsetsX[4] * phase) + Mathf.Sin(sinePosOffsetsX[5] * x + sinePhsOffsetsX[5] * phase) + Mathf.Sin(sinePosOffsetsX[6] * x + sinePhsOffsetsX[6] * phase) + Mathf.Sin(sinePosOffsetsX[7] * x + sinePhsOffsetsX[7] * phase)) * height / 8f;
				waveFactorZ = (Mathf.Sin(sinePosOffsetsZ[0] * z + sinePhsOffsetsZ[0] * phase) + Mathf.Sin(sinePosOffsetsZ[1] * z + sinePhsOffsetsZ[1] * phase) + Mathf.Sin(sinePosOffsetsZ[2] * z + sinePhsOffsetsZ[2] * phase) + Mathf.Sin(sinePosOffsetsZ[3] * z + sinePhsOffsetsZ[3] * phase) + Mathf.Sin(sinePosOffsetsZ[4] * z + sinePhsOffsetsZ[4] * phase) + Mathf.Sin(sinePosOffsetsZ[5] * z + sinePhsOffsetsZ[5] * phase) + Mathf.Sin(sinePosOffsetsZ[6] * z + sinePhsOffsetsZ[6] * phase) + Mathf.Sin(sinePosOffsetsZ[7] * z + sinePhsOffsetsZ[7] * phase)) * height / 8f;
				break;

		}

		worldPosition.y = heightOffset + (waveFactorX + waveFactorZ);

		return worldPosition;
	}
}
