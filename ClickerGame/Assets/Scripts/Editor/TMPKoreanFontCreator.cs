// Assets/Editor/TMPKoreanFontCreator.cs
using UnityEditor;
using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.TextCore.LowLevel;

public class TMPKoreanFontCreator : EditorWindow
{
	[Header("Required")]
	public Font sourceFont;

	[Header("Font Asset Options")]
	public int samplingPointSize = 90;          // ���� ���ø� ����Ʈ
	public int atlasPadding = 4;                // ���� �� �е�
	public int atlasWidth = 4096;               // ��Ʋ�� ����
	public int atlasHeight = 4096;              // ��Ʋ�� ����
	public GlyphRenderMode renderMode = GlyphRenderMode.SDFAA; // ��Ƽ�ٸ���� ���� SDF
	public bool dynamicAtlas = true;            // ����(��Ÿ�ӿ� ���� �߰� ����)
	public bool setAsTMPDefault = false;        // TMP �⺻ ��Ʈ�� ���

	[MenuItem("Tools/TMP/Create Korean Font Asset")]
	public static void ShowWindow()
	{
		GetWindow<TMPKoreanFontCreator>("Korean TMP Font Creator");
	}

	private void OnGUI()
	{
		GUILayout.Label("Korean TMP Font Asset Creator", EditorStyles.boldLabel);
		sourceFont = (Font)EditorGUILayout.ObjectField("Source Font (.ttf/.otf)", sourceFont, typeof(Font), false);

		GUILayout.Space(6);
		GUILayout.Label("Asset Options", EditorStyles.boldLabel);
		samplingPointSize = EditorGUILayout.IntField("Sampling Point Size", samplingPointSize);
		atlasPadding = EditorGUILayout.IntField("Atlas Padding", atlasPadding);
		renderMode = (GlyphRenderMode)EditorGUILayout.EnumPopup("Render Mode", renderMode);

		GUILayout.Space(4);
		atlasWidth = EditorGUILayout.IntField("Atlas Width", atlasWidth);
		atlasHeight = EditorGUILayout.IntField("Atlas Height", atlasHeight);
		dynamicAtlas = EditorGUILayout.Toggle(new GUIContent("Dynamic Atlas (runtime addable)"), dynamicAtlas);
		setAsTMPDefault = EditorGUILayout.Toggle(new GUIContent("Set As TMP Default Font"), setAsTMPDefault);

		GUILayout.Space(10);
		if (GUILayout.Button("Create"))
		{
			CreateFontAsset();
		}
	}

	void CreateFontAsset()
	{
		if (sourceFont == null)
		{
			EditorUtility.DisplayDialog("Error", "Source Font �� �����ϼ��� (.ttf / .otf)", "OK");
			return;
		}

		// 1) ��Ʈ ���� ���� (����/���� ����)
		var populationMode = dynamicAtlas ? AtlasPopulationMode.Dynamic : AtlasPopulationMode.Static;
		var fontAsset = TMP_FontAsset.CreateFontAsset(
			sourceFont,
			samplingPointSize,
			atlasPadding,
			renderMode,
			atlasWidth,
			atlasHeight,
			populationMode
		);

		if (fontAsset == null)
		{
			EditorUtility.DisplayDialog("Error", "TMP_FontAsset ���� ����", "OK");
			return;
		}

		// 2) �⺻ ASCII + �ѱ�(��~�R) �̸� ä���
		string charset = BuildAscii() + BuildHangulSyllables();
		fontAsset.TryAddCharacters(charset, out string missing);

		// 3) ���� ����
		string srcPath = AssetDatabase.GetAssetPath(sourceFont);
		string dir = System.IO.Path.GetDirectoryName(srcPath);
		string name = sourceFont.name + "_Korean_TMP.asset";
		string savePath = System.IO.Path.Combine(dir, name).Replace("\\", "/");

		AssetDatabase.CreateAsset(fontAsset, savePath);
		EditorUtility.SetDirty(fontAsset);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		// 4) TMP �⺻ ��Ʈ�� ����(����)
		if (setAsTMPDefault && TMP_Settings.instance != null)
		{
			TMP_Settings.defaultFontAsset = fontAsset;
			EditorUtility.SetDirty(TMP_Settings.instance);
			AssetDatabase.SaveAssets();
		}

		// 5) ��� �ȳ�
		var msg = $"���� �Ϸ�!\n\n- Path: {savePath}\n- Missing chars: {(string.IsNullOrEmpty(missing) ? "����" : missing.Length.ToString() + "��")}\n" +
				  $"- Atlas: {atlasWidth}x{atlasHeight}, Dynamic: {dynamicAtlas}\n" +
				  $"- Render: {renderMode}, PointSize: {samplingPointSize}, Padding: {atlasPadding}";
		EditorUtility.DisplayDialog("Success", msg, "OK");
		Selection.activeObject = fontAsset;
	}

	static string BuildAscii()
	{
		// ����/����/�� + 32~126 �⺻ ASCII
		StringBuilder sb = new StringBuilder();
		sb.Append(' ');
		sb.Append('\n');
		sb.Append('\r');
		sb.Append('\t');
		for (int i = 32; i <= 126; i++)
			sb.Append((char)i);
		return sb.ToString();
	}

	static string BuildHangulSyllables()
	{
		// ��(AC00) ~ �R(D7A3) �ϼ��� �ѱ�
		StringBuilder sb = new StringBuilder();
		for (int code = 0xAC00; code <= 0xD7A3; code++)
			sb.Append((char)code);
		return sb.ToString();
	}
}
