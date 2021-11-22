using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class KeywordInput : MonoBehaviour
{
    public TimeOfDayManager timeManager;

    private string[] m_Keywords = {"amanecer", "día", "tarde", "anochecer", "puesta de sol", "noche"};
    private KeywordRecognizer m_Recognizer;

    void OnEnable()
    {
        m_Recognizer = new KeywordRecognizer(m_Keywords);
        m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;
        m_Recognizer.Start();
    }

    void OnDisable()
    {
        m_Recognizer.Stop();
        m_Recognizer.Dispose();
        PhraseRecognitionSystem.Shutdown();
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log(string.Format("{0}: {1}", args.text, args.confidence));
        switch (args.text)
        {
            case "amanecer":
                timeManager.time = 0.04f;
                break;
            case "día":
                timeManager.time = 0.15f;
                break;
            case "tarde":
                timeManager.time = 0.25f;
                break;
            case "puesta de sol":
            case "anochecer":
                timeManager.time = 0.35f;
                break;
            case "noche":
                timeManager.time = 0.4f;
                break;
            default:
                Debug.LogError("¡Caso no definido en el switch!");
                break;
        }
    }
}
