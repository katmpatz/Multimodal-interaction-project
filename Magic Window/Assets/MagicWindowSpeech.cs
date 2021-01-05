using System;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Collections.Generic;


namespace FrostweepGames.Plugins.GoogleCloud.SpeechRecognition
{
	public class MagicWindowSpeech : MonoBehaviour
	{
		
		private string actionType, seasonParameter;
		private DateTime weatherDate;
		
		private GCSpeechRecognition _speechRecognition;
		private ColorBlock theColor;

		public GameObject ARObject;
		public GameObject InstructionPanel;
		public Sprite Mike_Red, Mike_white;

		public SpriteRenderer sr;

		private Button _startRecordButton,
						_closeInstructions,
						_helpButton	;
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



			//make the cube fully transparent
			ARObject.GetComponent<Renderer>().material.color = new Color(0.0f, 0.0f, 0.8f, 0.3f);

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
			_closeInstructions= transform.Find("Canvas/Panel_HelpText/Button_close").GetComponent<Button>();
			_helpButton = transform.Find("Canvas/NameBoard_Panel/Button_help").GetComponent<Button>();
			_resultText = transform.Find("Canvas/PanelTest/GameObject/Texttest").GetComponent<Text>();

		//	_textTest = transform.Find("Canvas/PanelTest/GameObject/Texttest").GetComponent<Text>();
			//	_languageDropdown = transform.Find("Canvas/Dropdown_Language").GetComponent<Dropdown>();
			_microphoneDevicesDropdown = transform.Find("Canvas/Dropdown_MicrophoneDevices").GetComponent<Dropdown>();
			_voiceLevelImage = transform.Find("Canvas/Panel_VoiceLevel/Image_Level").GetComponent<Image>();

			_startRecordButton.onClick.AddListener(StartRecordButtonOnClickHandler);
			_startRecordButton.interactable = true;

			_closeInstructions.onClick.AddListener(CloseInstructionPanelOnClickHandler);
			_helpButton.onClick.AddListener(OpenHelpPanelOnClickHandler);

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
			//StartRecordButtonOnClickHandler();
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

			/*
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
					//_resultText.text = "yes sound is colour changing";
				}
				//_resultText.text = "yes sound is recording";
			}
			else
			{
				_voiceLevelImage.fillAmount = 0f;
				//_resultText.text = "\n Click on the Voice Button to speak";
			}

			*/
		}

		private void CloseInstructionPanelOnClickHandler()
		{
			InstructionPanel.SetActive(false);

		}

		private void OpenHelpPanelOnClickHandler()
		{
			InstructionPanel.SetActive(false);
			InstructionPanel.SetActive(true);
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
			_resultText.text = "Ask for a season you want to see or for the weather during the next week's days.";
		}

		private void StartRecordButtonOnClickHandler()
		{

			_startRecordButton.interactable = false;
			_startRecordButton.image.sprite = Mike_Red;
			ARObject.gameObject.SetActive(true);
			//pause the video 
			var videoPlayer = ARObject.GetComponent<UnityEngine.Video.VideoPlayer>();
			videoPlayer.Pause();


			//colour.a = 255f;
			//_startRecordButton.GetComponent<Image>().color = colour;


			//_resultText.text = string.Empty;
			_resultText.text = "Button clicked";
			_speechRecognition.StartRecord(true);
		}

		private void RecordFailedEventHandler()
		{
			_resultText.text = "<color=red>Start record Failed. Please check microphone device and try again.</color>";
			_startRecordButton.interactable = true;
		}

		private void BeginTalkigEventHandler()
		{
			_resultText.text = "\nI am listening";
			//_textTest.text= "<color=blue>Speech Began.</color>";
		}

		private void EndTalkigEventHandler(AudioClip clip, float[] raw)
		{
			_resultText.text = "\nWait a moment..";

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
			"spring" , "winter", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"}
		
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
			//_resultText.text = "Recognize Success.";
			InsertRecognitionResponseInfo(recognitionResponse);
		}

		private void InsertRecognitionResponseInfo(RecognitionResponse recognitionResponse)
		{
			if (recognitionResponse == null || recognitionResponse.results.Length == 0)
			{
				_resultText.text = "\nWords not detected. Try again";
			//	_startRecordButton.interactable = true;
			//	_startRecordButton.image.sprite = Mike_white;

			//	_speechRecognition.StopRecord();
				return;
			}

			_resultText.text = "You said: " + recognitionResponse.results[0].alternatives[0].transcript + "'";

			var words = recognitionResponse.results[0].alternatives[0].words;

			if (words != null)
			{
				string times = string.Empty;
				string phrase, today;
				actionType = "";
				seasonParameter = "";
				weatherDate = DateTime.Now; // dont know how to reset this!!!



				string[] dayofWeek = { "sunday", "monday", "tuesday", "wednesday", "thursday", "friday", "saturday" };

				foreach (var item in recognitionResponse.results[0].alternatives[0].words)
				{
					//times += "<color=green>" + item.word+ "\n";// + "</color> -  start: " + item.startTime + "; end: " + item.endTime + "\n";

					phrase = item.word;
					phrase = phrase.ToLower();
					//_resultText.text += "\n" + phrase;
					if (phrase.Contains("today") || phrase.Contains("tomorrow") || phrase.Contains("sunday") || phrase.Contains("monday") || phrase.Contains("tuesday") || phrase.Contains("wednesday") || phrase.Contains("thursday") || phrase.Contains("friday") || phrase.Contains("saturday"))
					{
						actionType = "weather";

						if (phrase.Contains("today"))
						{
							phrase = "today";
							weatherDate = DateTime.Now;

						}
						else if (phrase.Contains("tomorrow"))
						{
							phrase = "tomorrow";
								weatherDate = DateTime.Now.AddDays(1);
						}

						else
						{

							today = DateTime.Now.DayOfWeek.ToString();
							today = today.ToLower();
							phrase = phrase.Replace("?", String.Empty).Replace("'s",String.Empty).Replace(".", String.Empty).Replace(",",String.Empty);
							phrase = phrase.ToLower();

							int index1 = Array.IndexOf(dayofWeek, phrase);
							int indexToday = Array.IndexOf(dayofWeek, today);
							//_resultText.text += "\n index 1=" + index1 + "indexToday=" + indexToday + "today is: " + today;

							if (index1 > indexToday) { weatherDate = DateTime.Now.AddDays(index1 - indexToday); }
							else if (index1 < indexToday) { weatherDate = DateTime.Now.AddDays(index1 - indexToday + 7); } // if its a earlier day than today, calculate the next week's same day
							else { weatherDate = DateTime.Now; }
						}
					
					
					}
					else if (phrase.Contains("summer") || phrase.Contains("autumn") || phrase.Contains("spring") || phrase.Contains("sun") || phrase.Contains("sunny") || phrase.Contains("winter"))
					{
						phrase = phrase.Replace("?", String.Empty).Replace("'s", String.Empty).Replace(".", String.Empty).Replace(",", String.Empty);
						phrase = phrase.ToLower();

						actionType = "season";
						seasonParameter = phrase;

					}
				}

				// step 1 - Call getWeather(DateTime wdate) APIs to get weather status (return object would have a seasons parameter)
				// step 2 - call video player to change "showWeather" showWeather(string actionType, string seasonParameter)

				/*		times += "ActionType = " + actionType + "\n"
								+ "seasonParameter = " + seasonParameter + "\n"
								+ "weatherDate = " + weatherDate + "\n";

						_resultText.text += "\n" + times;
				*/
				//call this only if actionType=season, else call katerina's script
				if (actionType == "season")
				{
					getSeason(seasonParameter);
				}
				else if (actionType == "weather")
				{
					getWeather(weatherDate);
				}
				else {
					_resultText.text += "\n Ask for e.g what's the weather tomorrow";
				}

			
			}

			_startRecordButton.interactable = true;
			_startRecordButton.image.sprite = Mike_white;

			_speechRecognition.StopRecord();
			actionType = "";
			seasonParameter = "";



		}

		void getSeason(string parameter)
		{
			WeatherManager wd = gameObject.GetComponent<WeatherManager>();
			//wd.weatherPanel.gameObject.SetActive(false);
			wd.temperature.text = "";
			wd.dayFull.text = "";
			wd.description.text = "";

			//remove weather icon
			sr.sprite = null;

			//make the cube fully opaq
			ARObject.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			var videoPlayer = ARObject.GetComponent<UnityEngine.Video.VideoPlayer>();

			videoPlayer.clip = Resources.Load(parameter) as UnityEngine.Video.VideoClip;
		}

		//private WeatherManager accwth;
		 

		private void getWeather(DateTime wdate)
		{
			int dateToInt = 0;
			DateTime today = DateTime.Today;
			dateToInt = (wdate - today).Days;

			//remove the video
			var videoPlayer = ARObject.GetComponent<UnityEngine.Video.VideoPlayer>();
			videoPlayer.clip = null;
			//make the cube fully transparent
			ARObject.GetComponent<Renderer>().material.color = new Color(0.0f, 0.0f, 0.8f, 0.3f);


			WeatherManager accwth = gameObject.GetComponent<WeatherManager>();
			StartCoroutine(accwth.FetchWeatherDataFromApi(dateToInt));

		}




	}
}