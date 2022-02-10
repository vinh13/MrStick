#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TexturePostProcessor : AssetPostprocessor
{
	void OnPostprocessTexture (Texture2D texture)
	{
//		TextureImporter importer = assetImporter as TextureImporter;
//		importer.anisoLevel = 0;
//		importer.filterMode = FilterMode.Bilinear;
//		importer.SetPlatformTextureSettings ("Android", 2048, TextureImporterFormat.ASTC_RGBA_6x6, 50, false);
//		importer.SetPlatformTextureSettings ("iPhone", 2048, TextureImporterFormat.ASTC_RGBA_6x6, 50, false);
//		Object asset = AssetDatabase.LoadAssetAtPath (importer.assetPath, typeof(Texture2D));
//		if (asset) {
//			EditorUtility.SetDirty (asset);
//		} else {
//			texture.anisoLevel = 0;
//			texture.filterMode = FilterMode.Bilinear;
//		}
//		AssetDatabase.Refresh ();
	}

	void OnPostprocessAudio (AudioClip clip)
	{
	}
}
#endif
