# Práctica 5: Reconocimiento de voz en Unity3D <!-- omit in toc -->

> El objetivo de esta práctica es aprender a utilizar las herramientas de reconocimiento de voz que Unity3D ofrece para Windows 10.

* Javier Correa Marichal (alu0101233598)
* Nerea Rodríguez Hernández (alu0101215693)
* Interfaces Inteligentes 21/22
* Universidad de La Laguna

### Tabla de contenidos <!-- omit in toc -->
- [KeywordRecognizer](#keywordrecognizer)
- [DictationRecognizer](#dictationrecognizer)
- [GameManager](#gamemanager)
- [Funcionamiento](#funcionamiento)

-----

### KeywordRecognizer

El uso de la clase ``KeywordRecognizer`` es muy sencillo. Para empezar, solo debemos de crear un objeto de este tipo, pasando como parámetro la lista de palabras claves que queremos reconocer en nuestro programa. A la instancia obtenida se le asigna un *callback* a la función que contiene los pasos a seguir cuando se reconoce una palabra clave, y se activa el sistema de reconocimiento:

```csharp
    private string[] m_Keywords = {"amanecer", "día", "tarde", "anochecer", "puesta de sol", "noche"};
    private KeywordRecognizer m_Recognizer;

    void OnEnable()
    {
        m_Recognizer = new KeywordRecognizer(m_Keywords);
        m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;
        m_Recognizer.Start();
    }
```

El *callback* utiliza el atributo `PhraseRecognizedEventArgs.text` para obtener la coincidencia procesada y, a través de un switch, establece en el gestor horario la hora que corresponde con la palabra clave capturada:

```csharp
    public TimeOfDayManager timeManager;

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
```

Cuando el usuario cambia de modo a `DictationRecognizer`, es necesario liberar los recursos del sistema utilizados por el método de captura de voz. Para ello, se incluye el siguiente código en el método `OnDisable()`:

```csharp
    void OnDisable()
    {
        m_Recognizer.Stop();
        m_Recognizer.Dispose();
        PhraseRecognitionSystem.Shutdown();
    }
```

### DictationRecognizer

Al igual que con la clase ``KeywordRecognizer``, la configuración a realizar para el funcionamiento de ``DictationRecognizer`` es muy directo. Tan solo hemos de crear un nuevo objeto de este tipo y asignarle 4 *callbacks* distintos, las cuales serán invocadas según sucedan ciertos eventos definidos por la API de Windows. Para los propósitos de esta práctia, tan solo es de interés definir la función `DictationResult`, puesto que se trata la función que devuelve el texto completo reconocido:

```csharp
        m_Recognitions.SetText("");
        m_DictationRecognizer = new DictationRecognizer();

        m_DictationRecognizer.DictationResult += (text, confidence) =>
        {
            Debug.LogFormat("Dictation result: {0}", text);
            m_Recognitions.SetText(text);
        };
```

Como se hizo con la instancia de `KeywordRecognizer` en el apartado anterior, es importante liberar los recursos utilizados por la API de dictado. Para ello, basta con invocar los siguientes métodos cuando se desactive la funcionalidad:

```csharp
    void OnDisable()
    {
        m_DictationRecognizer.Stop();
        m_DictationRecognizer.Dispose();
    }
```

### GameManager

Para controlar el cambio entre un modo de funcionamiento y otro, desarrollamos una clase `GameManager` para que controlase esta lógica dentro del juego. Este script se ocupa de comprobar en cada frame si se ha pulsado la barra espaciadora; y en caso positivo, se habilita el componente de entrada que se desee utilizar y se deshabilita el actual.

```csharp
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
```

### Funcionamiento

El funcionamiento del código desarollado puede verse en el siguiente vídeo:

[![Miniatura del vídeo](img/img1.png)](https://youtu.be/JhAJ4pX4dks)