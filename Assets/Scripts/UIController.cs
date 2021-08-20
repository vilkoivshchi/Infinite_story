using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

namespace Infinite_story
{
    public class UIController : MonoBehaviour
    {
        public GameObject MainMenu;

        public Button
            MainMenuButton,
            ContinueButton,
            NewGameButton,
            SaveButton,
            LoadButton,
            ExitButton;

        [Tooltip("This marker will shown if player not in screen")] public GameObject PlayerMarker;

        public static Action SaveFileEvent;
        public static Action<string> SendTimeToSaveFile;
        public static Action LoadFileAction;

        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _time;
        private int _score = 0;
        private float _timeScale = 0;
        private string _timeText;
        private bool _isPlayerVisible = true;
        //private Camera _mainCam;

        //private Vector3 _playerOnScreenPos;

        // ссылка на игрока
        //private GameObject _player;

        private void Awake()
        {
            PlayerController.currentScore += OnScoreChange;
            PlayerController.IsPlayerVisible += IsPlayerVisible;
            //PlayerController.SetPlayer += SetPlayer;
            //PlayerController.SetCamera += SetMainCam;
        }

        private void OnDestroy()
        {
            PlayerController.currentScore -= OnScoreChange;
            PlayerController.IsPlayerVisible -= IsPlayerVisible;
            //PlayerController.SetPlayer -= SetPlayer;
            //PlayerController.SetCamera -= SetMainCam;
        }

        private void Start()
        {
            // Добавляем к кнопкам события
            MainMenu.SetActive(false);
            ContinueButton.onClick.AddListener(HideMainMenu);
            NewGameButton.onClick.AddListener(NewGame);
            ExitButton.onClick.AddListener(GameExit);
            MainMenuButton.onClick.AddListener(ShowMainMenu);
            SaveButton.onClick.AddListener(SaveFile);
            LoadButton.onClick.AddListener(LoadFile);
            // нормальный ход времени
            _timeScale = Time.timeScale;
        }
        // Событие, которое вызывает другое событие в скрипте FloorScroller,
        // которое передаёт List<GameObject> в скрипт сохранинения
        void SaveFile()
        {
            SaveFileEvent?.Invoke();
            SendTimeToSaveFile?.Invoke(_timeText);
        }
        /*
        void SetMainCam(Camera cam)
        {
            _mainCam = cam;
        }
        */
        void HideMainMenu()
        {
            MainMenu.SetActive(false);
            MainMenuButton.gameObject.SetActive(true);
            Time.timeScale = _timeScale;
        }
        /*
        void SetPlayer(GameObject Player)
        {
            _player = Player;
        }
        */

        void LoadFile()
        {
                LoadFileAction?.Invoke();
        }

        void GameExit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        void NewGame()
        {
            Time.timeScale = _timeScale;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        void ShowMainMenu()
        {
            MainMenu.SetActive(true);
            Time.timeScale = 0;
            MainMenuButton.gameObject.SetActive(false);
        }

        void OnScoreChange(int score)
        {
            _score = score;
        }

        void IsPlayerVisible(bool PlayerVis)
        {
            _isPlayerVisible = PlayerVis;
        }

        void Update()
        {
            _scoreText.text = $"Score: {_score}";
            _time.text = $"{System.TimeSpan.FromSeconds((int)Time.timeSinceLevelLoad)}";
            _timeText = _time.text;
            /*
            _playerOnScreenPos = _mainCam.WorldToScreenPoint(_player.transform.position);
            if (!_isPlayerVisible || !_player.GetComponent<Renderer>().isVisible)
            {
                PlayerMarker?.SetActive(false);
            }
            else
            {
                PlayerMarker?.SetActive(true);
                PlayerMarker.transform.position = Vector3.MoveTowards(
                    new Vector3(
                        PlayerMarker.transform.position.x,
                    _player.transform.position.x,
                    0),
                    new Vector3(
                        PlayerMarker.transform.position.y,
                    _player.transform.position.y,
                    0), 1.0f);
                PlayerMarker.transform.position = new Vector3(
                    Mathf.Clamp(_playerOnScreenPos.x, 0, Screen.width), 
                    Mathf.Clamp(_playerOnScreenPos.y, 0, Screen.height), 
                    0);
                
                //PlayerMarker.transform.LookAt(_player.transform);
            }
            
            */
        }
#if UNITY_EDITOR
        void OnGUI()
        {
            GUI.Label(new Rect(Screen.width - 300, 200, 100, 100), $"{_isPlayerVisible}");
        }
#endif
    }
}