using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.arcade;
using Items;
using LevelEditor.Rule;
using Terrain;
using UI;
using Units;
using Units.PathFinding;
using UnityEngine;
using Utility;
using Utility.PoolManager;
using Random = System.Random;
using rand = UnityEngine.Random;

namespace Game {
    [Serializable]
    public class EnemySpawn {
        public float spawnTime;
        public EntityType entityType;
        public Vector2 position;
    }

    public class Level : MonoBehaviour {
        private Shop _shop;
        private ShopManager _shopManager;
        private SystemUnit _systemUnit;

        public Rule rule;

        public TerrainMeshBuilder terrainBuilder;

        [Header("Shop content")] public List<Consumable> consumablesList = new List<Consumable>();
        public List<Equipment> equipmentsList = new List<Equipment>();
        public List<StoreUnit> unitList = new List<StoreUnit>();

        [Header("Enemies")] public List<EnemySpawn> enemySpawns;
        public List<Transform> livingEnemies;

        private List<PlayerUnit> _playerUnits;
        private bool _levelStarted;

        private List<Tween> _enemySpawnsDelayedCalls;
        private List<Tween> _spawningEnemies;
        private AiUnit _lastUnit;
        private Entity _lastEntity;
        private bool _checkForCinematic;
        private bool _isWeak;
        private bool _cinematicPlayed;
        private bool _mageCheck;

        private bool _skipCutscene;

        // Initializes level with terrain, a*, shop
        public void Init() {
            _systemUnit = FindObjectOfType<SystemUnit>();

            //Player player = GameSingleton.Instance.GetPlayer();
            GameSingleton.Instance.aStarHandler = transform.GetComponentInChildren<AStarHandler>();

            StartCoroutine(terrainBuilder.Init(InitAStar, rule));

            if (rule != null) LoadEnnemiesRule();

            _shop = Shop.Instance;
            _shop.ClearShop();
            _shopManager = ShopManager.instance;

            foreach (Consumable cons in consumablesList) {
                _shop.AddConsummable(cons);
            }

            foreach (Equipment equip in equipmentsList) {
                _shop.AddEquipment(equip);
            }

            foreach (StoreUnit storeUnit in unitList) {
                _shop.AddStoreUnit(storeUnit);
            }

            _shopManager.UpdateUi(_shop);

            _enemySpawnsDelayedCalls = new List<Tween>();
            _spawningEnemies = new List<Tween>();
        }

        // Initializes a*, called after terrain generation
        private void InitAStar() {
            GameSingleton.Instance.aStarHandler.Init(terrainBuilder.terrainOptions);

            var offset = Vector3.right * (TerrainGrid.Width / 2f) +
                         Vector3.forward * (TerrainGrid.Height / 2f);
            terrainBuilder.transform.position += offset;
            _systemUnit.cam.transform.position += offset;

            //DOVirtual.DelayedCall(1f, () => {
            if (GameSingleton.Instance.GetPlayer().gamemode == Player.Gamemode.ARCADE)
                RespawnEnemies();
            //});

            ShowNextSpawns();
            GameSingleton.Instance.ResumeGame();
        }

        // CUSTOM LEVELS
        private void LoadEnnemiesRule() {
            Random rando = new Random();

            foreach (var spawn in rule.localSpawnDifficulty) {
                if (rand.Range(0, 4 * 128) <= spawn.Value * 128 && (int) spawn.Key.X % 3 == 0 &&
                    (int) spawn.Key.Z % 3 == 0) {
                    EnemySpawn es = new EnemySpawn();

                    es.position = new Vector2(spawn.Key.X - rule.size / 2f, spawn.Key.Z - rule.size / 2f);
                    //es.spawnTime = counter == 0 ? 0 : Random.Range(0, 60);

                    Array values = Enum.GetValues(typeof(EntityType));
                    es.entityType = (EntityType) values.GetValue(rando.Next(values.Length));


                    es.entityType = GenRandomParam.SoftEntityType(rando, es.entityType, 0.25f);
                    enemySpawns.Add(es);
                }
            }
        }

        private bool _gameEnded;

        private void FixedUpdate() {
            if (!_levelStarted) return;

            // getting active units, TODO refactor to events on add/remove unit
            _playerUnits.Clear();
            foreach (var unit in _systemUnit.units) {
                if (unit.GetType() == typeof(PlayerUnit)) {
                    _playerUnits.Add((PlayerUnit) unit);
                }
            }

            if (enemySpawns.Count == 0 && livingEnemies.Count == 0 && !_gameEnded) {
                Time.timeScale = 1;
                _gameEnded = true;
                GameSingleton.Instance.EndGame(1); // WIN
            }
            else if (_playerUnits.Count == 0 && !_gameEnded) {
                Time.timeScale = 1;
                _gameEnded = true;
                GameSingleton.Instance.EndGame(0);
            }

            for (int i = 0; i < livingEnemies.Count; i++) {
                if (livingEnemies[i] == null) {
                    livingEnemies.RemoveAt(i);
                    break;
                }
            }

            for (int i = 0; i < _playerUnits.Count; i++) {
                if (_playerUnits[i] == null || !_playerUnits[i].gameObject.activeSelf) {
                    _playerUnits.RemoveAt(i);
                    break;
                }
            }

            if (_skipCutscene) return;

            if (!_checkForCinematic) {
                if (!_mageCheck) {
                    _mageCheck = true;
                    bool onlyMages = true;
                    foreach (var unit in _playerUnits) {
                        EntityType type = unit.GetEntityType();
                        if (type != EntityType.Mage && type != EntityType.Bard && type != EntityType.BlackMage &&
                            type != EntityType.Demonist && type != EntityType.WhiteMage && type != EntityType.RedMage) {
                            onlyMages = false;
                            break;
                        }
                    }

                    if (onlyMages) {
                        _skipCutscene = true;
                        return;
                    }
                }

                if (enemySpawns.Count == 0 && livingEnemies.Count == 1 && !_gameEnded) {
                    //Debug.Log("start checking for cinematic");
                    _lastUnit = livingEnemies[0].GetComponent<AiUnit>();
                    _lastEntity = _lastUnit.entities[0];
                    _checkForCinematic = true;
                    if (_playerUnits.Count < 3) {
                        _isWeak = true;
                    }
                }
            }

            if (_cinematicPlayed) return;
            if (!_checkForCinematic) return;
            if (_lastUnit.GetNumberAlive() == 1) {
                if (_isWeak) {
                    if (_lastEntity.GetLife() < _lastEntity.GetMaxLife() / 2) {
                        _cinematicPlayed = true;
                        GameSingleton.Instance.cameraController.PlayCinematic(_lastEntity);
                    }
                }
                else {
                    if (_lastEntity.GetLife() < _lastEntity.GetMaxLife()) {
                        _cinematicPlayed = true;
                        GameSingleton.Instance.cameraController.PlayCinematic(_lastEntity);
                    }
                }
            }
        }

        public IEnumerator StartLevel() {
            for (int i = _waitingSpawners.Count - 1; i >= 0; --i) {
                _waitingSpawners[i].SetAnimating(false);
                _waitingSpawners[i].DeInit();
            }

            _waitingSpawners.Clear();
            _playerUnits = new List<PlayerUnit>();
            Transform newUnit;
            for (int i = enemySpawns.Count - 1; i >= 0; i--) {
                EnemySpawn current = enemySpawns[0];
                var offset = Vector3.right * (TerrainGrid.Width / 2f) +
                             Vector3.forward * (TerrainGrid.Height / 2f);
                Vector3 position = new Vector3(current.position.x, SystemUnit.YPos, current.position.y) + offset;

                // enemy spawn (can be delayed)
                if (current.spawnTime > 0) {
                    // todo update delayed calls when finished
                    _enemySpawnsDelayedCalls.Add(DOVirtual.DelayedCall(
                        Mathf.Max(0f, current.spawnTime - Spawner.timeToSpawn), () => {
                            Spawner spawner = (Spawner) PoolManager.Instance().GetPoolableObject(typeof(Spawner));
                            spawner.transform.position = position;
                            spawner.Init(current.entityType);

                            _spawningEnemies.Add(DOVirtual.DelayedCall(Spawner.timeToSpawn, () => {
                                spawner.DeInit();
                                newUnit = _systemUnit.SpawnUnit(current.entityType, _systemUnit.aiUnitPrefab,
                                    position);
                                livingEnemies.Add(newUnit);
                            }).OnUpdate(() => { spawner.UpdateTime(); }));
                        }));
                }
                else {
                    newUnit = _systemUnit.SpawnUnit(current.entityType, _systemUnit.aiUnitPrefab,
                        position);
                    livingEnemies.Add(newUnit);
                }

                enemySpawns.Remove(current);

                yield return null;
            }

            GameSingleton.Instance.ResumeGame();
            _levelStarted = true;
        }

        private List<Spawner> _waitingSpawners = new List<Spawner>();

        private void ShowNextSpawns() {
            for (int i = enemySpawns.Count - 1; i >= 0; --i) {
                EnemySpawn current = enemySpawns[i];
                if (Math.Abs(current.spawnTime) < 0.001f) {
                    var offset = Vector3.right * (TerrainGrid.Width / 2f) +
                                 Vector3.forward * (TerrainGrid.Height / 2f);
                    Vector3 position = new Vector3(current.position.x, SystemUnit.YPos, current.position.y) + offset;
                    Spawner spawner = (Spawner) PoolManager.Instance().GetPoolableObject(typeof(Spawner));
                    spawner.transform.position = position;
                    spawner.Init(current.entityType);
                    spawner.SetAnimating(true);
                    _waitingSpawners.Add(spawner);
                }
            }
        }

        public void PauseDelayedSpawns() {
            for (int i = _enemySpawnsDelayedCalls.Count - 1; i >= 0; --i) {
                _enemySpawnsDelayedCalls[i].Pause();
            }

            for (int i = _spawningEnemies.Count - 1; i >= 0; --i) {
                _spawningEnemies[i].Pause();
            }
        }

        public void ResumeDelayedSpawns() {
            for (int i = _enemySpawnsDelayedCalls.Count - 1; i >= 0; --i) {
                _enemySpawnsDelayedCalls[i].Play();
            }

            for (int i = _spawningEnemies.Count - 1; i >= 0; --i) {
                _spawningEnemies[i].Pause();
            }
        }

        private void RespawnEnemies() {
            rand.InitState(GameSingleton.Instance.GetPlayer().currentSeed);
            var w = terrainBuilder.terrainOptions.width;
            var h = terrainBuilder.terrainOptions.height;
            Vector2 offset = new Vector2(TerrainGrid.Width / 2f, TerrainGrid.Height / 2f);

            foreach (var enemy in enemySpawns) {
                Vector3 position;
                RaycastHit hitInfo;
                bool isOk;
                do {
                    isOk = true;
                    position = new Vector2(rand.Range(1.5f, w - 0.5f), rand.Range(1.5f, h - 0.5f)) - offset;

                    for (int x = -1; x <= 1; x++) {
                        for (int y = -1; y <= 1; y++) {
                            Vector3 toCast = new Vector3(x+position.x, 0, y+position.y);
                            Physics.Raycast(toCast + Vector3.up * 50f, Vector3.down, out hitInfo, 100f, 1 << 8);    // Raycast ground
                            if (Mathf.Abs(hitInfo.point.y) > 0.2f) {
                                isOk = false;
                                break;
                            }
                        }
                    }
                } while (!isOk);

                enemy.position = position;
            }
        }

        private void OnDrawGizmos() {
            if (Application.isPlaying) return;

            // Enemy spawns
            Gizmos.color = Color.red;
            int enemyLen = enemySpawns.Count;
            for (int i = 0; i < enemyLen; ++i) {
                Vector3 position = new Vector3(enemySpawns[i].position.x, SystemUnit.YPos, enemySpawns[i].position.y);
                Gizmos.DrawSphere(position, 0.5f);
            }
        }
    }
}