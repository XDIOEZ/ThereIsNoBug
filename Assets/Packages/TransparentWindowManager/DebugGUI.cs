using UnityEngine;
using System.Collections.Generic;

public class AutoDebugGUI : MonoBehaviour
{
    [Header("����")]
    public bool showOnStart = true;           // ����ʱ�Ƿ���ʾ
    public int maxLogCount = 20;              // �����ʾ��������־
    public int fontSize = 16;
    public Vector2 position = new Vector2(10, 10);
    public float maxWidth = 500f;
    public float maxHeight = 300f;

    private static bool _visible;
    private static List<LogEntry> _logs = new List<LogEntry>();
    private GUIStyle _boxStyle;
    private GUIStyle _labelStyle;

    [System.Serializable]
    private class LogEntry
    {
        public string message;
        public LogType type;
        public LogEntry(string msg, LogType t) { message = msg; type = t; }
    }

    void OnEnable()
    {
        _visible = showOnStart;
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            _visible = !_visible;
        }
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // ��ѡ�����˵�ĳЩ��־������ "Unload" �ȣ�
        // if (logString.Contains("SomeNoise")) return;

        _logs.Add(new LogEntry(logString, type));
        if (_logs.Count > maxLogCount)
            _logs.RemoveAt(0);
    }

    void OnGUI()
    {
        if (!_visible || _logs.Count == 0) return;

        if (_boxStyle == null)
        {
            _boxStyle = new GUIStyle(GUI.skin.box)
            {
                normal = { background = MakeTex(1, 1, new Color(0, 0, 0, 0.7f)) },
                padding = new RectOffset(10, 10, 10, 10)
            };

            _labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = fontSize,
                wordWrap = true,
                alignment = TextAnchor.UpperLeft
            };
        }

        // ������ʾ�ı�������ɫ��
        string fullText = "";
        foreach (var entry in _logs)
        {
            Color color = entry.type switch
            {
                LogType.Warning => Color.yellow,
                LogType.Error => Color.red,
                LogType.Assert => Color.red,
                _ => Color.white
            };

            // OnGUI ��֧�ָ��ı���ɫ������ͳһ�ð�ɫ����ֻ��ʾ���һ������ɫ��
            // �������Ǽ���ǰ׺��ʶ
            string prefix = entry.type switch
            {
                LogType.Warning => "[WARN] ",
                LogType.Error => "[ERROR] ",
                LogType.Assert => "[ASSERT] ",
                _ => ""
            };
            fullText += prefix + entry.message + "\n";
        }

        // ����߶ȣ��������
        float contentHeight = Mathf.Min(_labelStyle.CalcHeight(new GUIContent(fullText), maxWidth - 20), maxHeight);

        // ���Ʊ���
        GUI.Box(new Rect(position.x, position.y, maxWidth, contentHeight + 20), "", _boxStyle);

        // ��������
        GUI.Label(new Rect(position.x + 10, position.y + 5, maxWidth - 20, contentHeight), fullText, _labelStyle);
    }

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++)
            pix[i] = col;
        var result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}