using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// F1로 여닫는 개발자 패널. 에디터와 Development Build에서만 동작한다.
public sealed class DebugPanelController : MonoBehaviour
{
    // 기본 TMP 폰트는 한글을 지원하지 않아 UI 문자열은 영어로 표시한다.
    private const string NotImplemented = "Not implemented";

    [Header("References")]
    [SerializeField] private GameObject _panelRoot;
    [SerializeField] private DebugEnemySpawner _spawner;
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private Transform _player;
    [SerializeField] private PlayerAttack _playerAttack;

    [Header("Game Info")]
    [SerializeField] private TMP_Text _fpsText;
    [SerializeField] private TMP_Text _waveText;
    [SerializeField] private TMP_Text _elapsedText;
    [SerializeField] private TMP_Text _enemyCountText;
    [SerializeField] private TMP_Text _stateText;

    [Header("Player Status")]
    [SerializeField] private TMP_Text _positionText;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _xpText;

    [Header("Enemy Spawner")]
    [SerializeField] private TMP_Dropdown _enemyTypeDropdown;
    [SerializeField] private Toggle[] _spawnPointToggles;
    [SerializeField] private TMP_InputField _countInput;
    [SerializeField] private Button _add1Button;
    [SerializeField] private Button _add5Button;
    [SerializeField] private Button _add10Button;
    [SerializeField] private Button _spawnButton;
    [SerializeField] private TMP_Text _resultText;

    [SerializeField, Min(0.05f)] private float _refreshInterval = 0.25f;

    private InputAction _toggleAction;
    private float _refreshTimer;
    private float _fpsAccumulator;
    private int _fpsFrames;

    private void Awake()
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        _toggleAction = new InputAction("ToggleDebugPanel", InputActionType.Button, "<Keyboard>/f1");
        SetPanelOpen(false);
        InitStaticTexts();
#else
        Destroy(gameObject);
#endif
    }

    private void OnEnable()
    {
        if (_toggleAction == null) return;

        _toggleAction.performed += OnTogglePerformed;
        _toggleAction.Enable();
        _add1Button.onClick.AddListener(OnAdd1);
        _add5Button.onClick.AddListener(OnAdd5);
        _add10Button.onClick.AddListener(OnAdd10);
        _spawnButton.onClick.AddListener(OnSpawnClicked);
    }

    private void OnDisable()
    {
        if (_toggleAction == null) return;

        _toggleAction.performed -= OnTogglePerformed;
        _toggleAction.Disable();
        _add1Button.onClick.RemoveListener(OnAdd1);
        _add5Button.onClick.RemoveListener(OnAdd5);
        _add10Button.onClick.RemoveListener(OnAdd10);
        _spawnButton.onClick.RemoveListener(OnSpawnClicked);
        SetFireBlocked(false);
    }

    private void OnDestroy()
    {
        _toggleAction?.Dispose();
    }

    private void Update()
    {
        if (_toggleAction == null || !_panelRoot.activeSelf) return;

        _fpsAccumulator += Time.unscaledDeltaTime;
        _fpsFrames++;
        _refreshTimer += Time.unscaledDeltaTime;
        if (_refreshTimer < _refreshInterval) return;
        _refreshTimer = 0f;

        RefreshTexts();
    }

    private void OnTogglePerformed(InputAction.CallbackContext context)
    {
        SetPanelOpen(!_panelRoot.activeSelf);
    }

    private void SetPanelOpen(bool open)
    {
        _panelRoot.SetActive(open);
        SetFireBlocked(open);

        if (!open) return;

        _fpsAccumulator = 0f;
        _fpsFrames = 0;
        _refreshTimer = 0f;
        RefreshTexts();
    }

    private void SetFireBlocked(bool blocked)
    {
        if (_playerAttack != null) _playerAttack.IsFireBlocked = blocked;
    }

    // 미구현 시스템은 값을 만들지 않고 미구현으로 표시한다.
    private void InitStaticTexts()
    {
        _waveText.text = "Wave: " + NotImplemented;
        _elapsedText.text = "Elapsed: " + NotImplemented;
        _stateText.text = "State: " + NotImplemented;
        _healthText.text = "HP: " + NotImplemented;
        _levelText.text = "Level: " + NotImplemented;
        _xpText.text = "XP: " + NotImplemented;
    }

    private void RefreshTexts()
    {
        float fps = _fpsAccumulator > 0f ? _fpsFrames / _fpsAccumulator : 0f;
        _fpsAccumulator = 0f;
        _fpsFrames = 0;

        _fpsText.SetText("FPS: {0:1}", fps);
        _enemyCountText.SetText("Enemies: {0}", _enemyManager.ActiveEnemyCount);

        Vector3 position = _player.position;
        _positionText.SetText("Position X: {0:1}  Z: {1:1}", position.x, position.z);
    }

    private void OnAdd1() => AddCount(1);
    private void OnAdd5() => AddCount(5);
    private void OnAdd10() => AddCount(10);

    private void AddCount(int amount)
    {
        int current = TryReadCount(out int value) ? value : 0;
        _countInput.text = (current + amount).ToString();
    }

    private bool TryReadCount(out int count)
    {
        return int.TryParse(_countInput.text, out count) && count >= 1;
    }

    private int GetSelectedSpawnPoint()
    {
        for (int i = 0; i < _spawnPointToggles.Length; i++)
        {
            if (_spawnPointToggles[i].isOn) return i;
        }

        return -1;
    }

    private void OnSpawnClicked()
    {
        var type = (DebugEnemyType)_enemyTypeDropdown.value;

        if (!_spawner.IsEnemyTypeImplemented(type))
        {
            _resultText.text = type + " enemy: " + NotImplemented;
            return;
        }

        if (!TryReadCount(out int count))
        {
            _resultText.text = "Count must be an integer >= 1.";
            return;
        }

        int pointIndex = GetSelectedSpawnPoint();
        if (pointIndex < 0)
        {
            _resultText.text = "Select a spawn point.";
            return;
        }

        int spawned = _spawner.SpawnAround(type, pointIndex, count);
        _resultText.text = "Requested " + count + " / Spawned " + spawned;
        RefreshTexts();
    }
}
