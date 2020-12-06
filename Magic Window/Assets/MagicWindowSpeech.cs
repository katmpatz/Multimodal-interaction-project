using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


namespace FrostweepGames.Plugins.GoogleCloud.SpeechRecognition
{
	public class MagicWindowSpeech : MonoBehaviour
	{
		

		private GCSpeechRecognition _speechRecognition;

	

		private Button _startRecordButton;
					//   _stopRecordButton,
					//   _getOperationButton,
					//   _getListOperationsButton,
					//   _detectThresholdButton,
					//   _cancelAllRequestsButton,
					//   _recognizeButton,
					//   _refreshMicrophonesButton;

		//private Image _speechRecognitionState;

		private Text _resultText;

		//private Toggle _voiceDetectionToggle,
		//			   _recognizeDirectlyToggle,
		//			   _longRunningRecognizeToggle;

		private Dropdown// _languageDropdown,
						 _microphoneDevicesDropdown;

		//private InputField _contextPhrasesInputField,
		//				   _operationIdInputField;

		private Image _voiceLevelImage;

/*		List<string> m_DropOptions = new List<string> {
			"en_AU",
			"en_CA",
			"en_GH",
			"en_GB",
			"en_IN",
			"en_IE",
			"en_KE",
			"en_NZ",
			"en_NG",
			"en_PH",
			"en_ZA",
			"en_TZ",
			"en_US" };

		List<string> m_speechContext = new List<string> {
			"weather",
			"tomorrow",
			"today",
			"summer",
			"next day",
			"autumn",
			"spring"};
*/


		// Start is called before the first frame update
		void Start()
		{
			_speechRecognition = GCSpeechRecognition.Instance;
			_speechRecognition.RecognizeSuccessEvent += RecognizeSuccessEventHandler;
			_speechRecognition.RecognizeFailedEvent += RecognizeFailedEventHandler;
			_speechRecognition.FinishedRecordEvent += FinishedRecordEventHandler;
			_speechRecognition.StartedRecordEvent += StartedRecordEventHandler;
			_speechRecognition.RecordFailedEvent += RecordFailedEventHandler;

			_speechRecognition.BeginTalkigEvent += BeginTalkigEventHandler;
			_speechRecognition.EndTalkigEvent += EndTalkigEventHandler;

			//_contextPhrasesInputField = transform.Find("Canvas/InputField_SpeechContext").GetComponent<InputField>();
			_startRecordButton = transform.Find("Canvas/Button_StartRecord").GetComponent<Button>();
			_resultText = transform.Find("Canvas/Panel_ContentResult/Text_Area/Text_Result").GetComponent<Text>();
		//	_languageDropdown = transform.Find("Canvas/Dropdown_Language").GetComponent<Dropdown>();
			_microphoneDevicesDropdown = transform.Find("Canvas/Dropdown_MicrophoneDevices").GetComponent<Dropdown>();
			_voiceLevelImage = transform.Find("Canvas/Panel_VoiceLevel/Image_Level").GetComponent<Image>();

			_startRecordButton.onClick.AddListener(StartRecordButtonOnClickHandler);
			_startRecordButton.interactable = true;
			_microphoneDevicesDropdown.onValueChanged.AddListener(MicrophoneDevicesDropdownOnValueChangedEventHandler);

			//Loading the language options
			//_languageDropdown.ClearOptions();

			//_languageDropdown.AddOptions(m_DropOptions);
		/*	for (int i = 0; i < Enum.GetNames(typeof(Enumerators.LanguageCode)).Length; i++)
			{
				_languageDropdown.options.Add(new Dropdown.OptionData(((Enumerators.LanguageCode)i).Parse()));
			}
		*/
		//	_languageDropdown.value = _languageDropdown.options.IndexOf(_languageDropdown.options.Find(x => x.text == Enumerators.LanguageCode.en_US.Parse()));

			RefreshMicsButtonOnClickHandler();

		}

		private void OnDestroy()
		{
			_speechRecognition.RecognizeSuccessEvent -= RecognizeSuccessEventHandler;
			_speechRecognition.RecognizeFailedEvent -= RecognizeFailedEventHandler;
			_speechRecognition.FinishedRecordEvent -= FinishedRecordEventHandler;
			_speechRecognition.StartedRecordEvent -= StartedRecordEventHandler;
			_speechRecognition.RecordFailedEvent -= RecordFailedEventHandler;
			_speechRecognition.BeginTalkigEvent -= BeginTalkigEventHandler;
			_speechRecognition.EndTalkigEvent -= EndTalkigEventHandler;
		}

		// Update is called once per frame
		void Update()
		{
			if (_speechRecognition.IsRecording)
			{
				if (_speechRecognition.GetMaxFrame() > 0)
				{
					float max = (float)_speechRecognition.configs[_speechRecognition.currentConfigIndex].voiceDetectionThreshold;
					float current = _speechRecognition.GetLastFrame() / max;

					if (current >= 1f)
					{
						_voiceLevelImage.fillAmount = Mathf.Lerp(_voiceLevelImage.fillAmount, Mathf.Clamp(current / 2f, 0, 1f), 30 * Time.deltaTime);
					}
					else
					{
						_voiceLevelImage.fillAmount = Mathf.Lerp(_voiceLevelImage.fillAmount, Mathf.Clamp(current / 2f, 0, 0.5f), 30 * Time.deltaTime);
					}

					_voiceLevelImage.color = current >= 1f ? Color.green : Color.red;
				//	_resultText.text = "yes sound is colour changing";
				}
				//_resultText.text = "yes sound is recording";
			}
			else
			{
				_voiceLevelImage.fillAmount = 0f;
				//_resultText.text = "No sound detected";
			}
		}

		private void RefreshMicsButtonOnClickHandler()
		{
			_speechRecognition.RequestMicrophonePermission(null);

			_microphoneDevicesDropdown.ClearOptions();

			for (int i = 0; i < _speechRecognition.GetMicrophoneDevices().Length; i++)
			{
				_microphoneDevicesDropdown.options.Add(new Dropdown.OptionData(_speechRecognition.GetMicrophoneDevices()[i]));
			}

			//smart fix of dropdowns
			_microphoneDevicesDropdown.value = 1;
			_microphoneDevicesDropdown.value = 0;
		}


		private void MicrophoneDevicesDropdownOnValueChangedEventHandler(int value)
		{
			if (!_speechRecognition.HasConnectedMicrophoneDevices())
				return;
			_speechRecognition.SetMicrophoneDevice(_speechRecognition.GetMicrophoneDevices()[value]);
		}

		private void StartedRecordEventHandler()
		{
			_resultText.text = "StartedRecordEventHandler";
		}

		private void StartRecordButtonOnClickHandler()
		{
			_startRecordButton.interactable = false;
			_resultText.text = string.Empty;
			_speechRecognition.StartRecord(true);
		}

		private void RecordFailedEventHandler()
		{
			_resultText.text = "<color=red>Start record Failed. Please check microphone device and try again.</color>";
			_startRecordButton.interactable = true;
		}

		private void BeginTalkigEventHandler()
		{
			_resultText.text = "<color=blue>Speech Began.</color>";
		}

		private void EndTalkigEventHandler(AudioClip clip, float[] raw)
		{
			_resultText.text += "\n<color=blue>Speech Ended.</color>";

			FinishedRecordEventHandler(clip, raw);
		}

		private void FinishedRecordEventHandler(AudioClip clip, float[] raw)
		{

			if (clip == null)
				return;

			RecognitionConfig config = RecognitionConfig.GetDefault();
			//config.languageCode = ((Enumerators.LanguageCode)_languageDropdown.value).Parse();
			config.languageCode = "en_US";

			config.speechContexts = new SpeechContext[]
			{
				new SpeechContext()
				{
					phrases = new string[] {"weather",
			"tomorrow",
			"today",
			"summer",
			"next day",
			"autumn",
			"spring" }
		
		//phrases = _contextPhrasesInputField.text.Replace(" ", string.Empty).Split(',')
				}
			};


			config.audioChannelCount = clip.channels;
			// configure other parameters of the config if need

			GeneralRecognitionRequest recognitionRequest = new GeneralRecognitionRequest()
			{
				audio = new RecognitionAudioContent()
				{
					content = raw.ToBase64()
				},
				//audio = new RecognitionAudioUri() // for Google Cloud Storage object
				//{
				//	uri = "gs://bucketName/object_name"
				//},
				config = config
			};

			
				_speechRecognition.Recognize(recognitionRequest);
			
		}


		private void RecognizeFailedEventHandler(string error)
		{
			_resultText.text = "Recognize Failed: " + error;
		}

		private void RecognizeSuccessEventHandler(RecognitionResponse recognitionResponse)
		{
			_resultText.text = "Recognize Success.";
			InsertRecognitionResponseInfo(recognitionResponse);
		}

		private void InsertRecognitionResponseInfo(RecognitionResponse recognitionResponse)
		{
			if (recognitionResponse == null || recognitionResponse.results.Length == 0)
			{
				_resultText.text = "\nWords not detected.";
				return;
			}

			_resultText.text += "\n" + recognitionResponse.results[0].alternatives[0].transcript;

			var words = recognitionResponse.results[0].alternatives[0].words;

			if (words != null)
			{
				string times = string.Empty;

				foreach (var item in recognitionResponse.results[0].alternatives[0].words)
				{
					times += "<color=green>" + item.word + "</color> -  start: " + item.startTime + "; end: " + item.endTime + "\n";
				}

				_resultText.text += "\n" + times;
			}

			
		}

	}
}