using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public TextMeshProUGUI currentModeText;
    public TextMeshProUGUI helpText;
    public TextMeshPro m_Recognitions;

    private bool isDictation = false;
    private string currentModeTemplate = "Current mode: {0}";
    private string helpString = "Pruebe a decir 'día', 'noche', etc.";
    private KeywordInput keywordInput;
    private DictationInput dictationInput;

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        keywordInput = GetComponent<KeywordInput>();
        dictationInput = GetComponent<DictationInput>();
    }

    void Start()
    {
        m_Recognitions.SetText("");
        helpText.SetText(helpString);
        currentModeText.SetText(string.Format(currentModeTemplate, "KeywordRecognizer"));
        keywordInput.enabled = true;
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if (isDictation)
            {
                dictationInput.enabled = false;
                keywordInput.enabled = true;
                isDictation = false;
                m_Recognitions.SetText("");
                helpText.SetText(helpString);
                currentModeText.SetText(string.Format(currentModeTemplate, "KeywordRecognizer"));
            } 
            else
            {
                keywordInput.enabled = false;
                dictationInput.enabled = true;
                isDictation = true;
                helpText.SetText("");
                currentModeText.SetText(string.Format(currentModeTemplate, "DictationRecognizer"));
            }
        }    
    }
}
