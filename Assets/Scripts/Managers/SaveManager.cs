using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Cinemachine.DocumentationSortingAttribute;

[System.Serializable]
public class PlayerData
{
    public int health;
    public int experience;
    public int pickaxeAttackDamage;
    public float pickaxeAttackSpeed;
    public float[] position;
}

[System.Serializable]
public class EnemyData
{
    public int health;
    public float[] position;
}

[System.Serializable]
public class MapData
{
    public string mapName; // Nome do mapa (ex: "map1", "map2")
    public List<TileData> destroyedTiles; // Tiles destruídos do destructableTilemap
    public List<TileData> destroyedCrystalTiles; // Tiles destruídos do crystalsTilemap
}

[System.Serializable]
public class TileData
{
    public int x;
    public int y;
}

[System.Serializable]
public class GameData
{
    public string lastSaveTime;
    public int level;
    public float totalPlayTime; // Novo campo para o tempo total de jogo
    public PlayerData playerData;
    public List<EnemyData> enemies;
    public List<MapData> maps;
}

public class SaveManager : MonoBehaviour
{
    private static SaveManager _instance;
    private GameSceneManager gameSceneManager;
    private string filePath;

    public string FilePath { get; private set; }

    [SerializeField] private Player playerScript;  // Refer�ncia ao script do Player
    [SerializeField] private GameObject enemyPrefab;  // Prefab do inimigo

    public Tilemap destructableTilemap;
    public Tilemap crystalsTilemap;

    public Tilemap destructableTilemap2;
    public Tilemap crystalsTilemap2;


    private void Awake()
    {
        PlayerPrefs.SetString("Filename", "saveData.json");

        filePath = Path.Combine(Application.persistentDataPath, PlayerPrefs.GetString("Filename"));
        gameSceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<GameSceneManager>();

    }

    // M�todo para salvar os dados do jogador e inimigos
    public void SaveData()
    {
        // Dados do jogador
        PlayerData playerData = new PlayerData
        {
            health = playerScript.health,
            experience = playerScript.experience,
            pickaxeAttackSpeed = GameObject.FindGameObjectWithTag("Pickaxe").GetComponent<PickaxeController>().attackSpeed,
            pickaxeAttackDamage = GameObject.FindGameObjectWithTag("Pickaxe").GetComponent<PickaxeAttack>().attackDamage,
            position = new float[] { playerScript.transform.position.x, playerScript.transform.position.y, playerScript.transform.position.z }
        };

        // Inimigos
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();
        List<EnemyData> enemyDataList = new List<EnemyData>();
        foreach (var enemy in allEnemies)
        {
            enemyDataList.Add(new EnemyData
            {
                health = enemy.Health,
                position = new float[] { enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z }
            });
        }

        // Mapas
        List<MapData> mapDataList = new List<MapData>();

        // Adicione aqui todos os mapas
        SaveMapData("Map1", destructableTilemap, crystalsTilemap, mapDataList);
        SaveMapData("Map2", destructableTilemap2, crystalsTilemap2, mapDataList); // Exemplo para outro mapa

        string currentTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        float accumulatedPlayTime = gameSceneManager.TotalPlayTime;

        // Dados gerais
        GameData gameData = new GameData
        {
            playerData = playerData,
            enemies = enemyDataList,
            maps = mapDataList,
            lastSaveTime = currentTime,
            level = gameSceneManager.currentLevel,
            totalPlayTime = accumulatedPlayTime
        };

        // Salvar JSON
        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Game data saved! Filepath: " + filePath);
    }

    // M�todo para carregar os dados e instanciar inimigos nas posi��es salvas
    public void LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            GameData gameData = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Game data loaded!");

            // Dados do jogador
            playerScript.health = gameData.playerData.health;
            playerScript.experience = gameData.playerData.experience;
            GameObject.FindGameObjectWithTag("Pickaxe").GetComponent<PickaxeController>().attackSpeed = gameData.playerData.pickaxeAttackSpeed;
            GameObject.FindGameObjectWithTag("Pickaxe").GetComponent<PickaxeAttack>().attackDamage = gameData.playerData.pickaxeAttackDamage;
            playerScript.transform.position = new Vector3(gameData.playerData.position[0], gameData.playerData.position[1], gameData.playerData.position[2]);

            // Tempo total de jogo
            gameSceneManager.TotalPlayTime = gameData.totalPlayTime;
            gameSceneManager.currentLevel = gameData.level;

            // Inimigos
            foreach (var enemy in FindObjectsOfType<Enemy>())
            {
                Destroy(enemy.gameObject);
            }

            foreach (var enemyData in gameData.enemies)
            {
                GameObject enemyObj = Instantiate(enemyPrefab, new Vector3(enemyData.position[0], enemyData.position[1], enemyData.position[2]), Quaternion.identity);
                enemyObj.GetComponent<Enemy>().Health = enemyData.health;
            }

            // Mapas
            foreach (var mapData in gameData.maps)
            {
                if (mapData.mapName == "Map1")
                {
                    LoadMapData(mapData, destructableTilemap, crystalsTilemap);
                }
                else if (mapData.mapName == "Map2")
                {
                    LoadMapData(mapData, destructableTilemap2, crystalsTilemap2); // Exemplo para outro mapa
                }
            }
        }
        else
        {
            Debug.Log("No save file found!");
        }
    }

    // Função para carregar os dados de um único mapa
    private void LoadMapData(MapData mapData, Tilemap destructableTilemap, Tilemap crystalsTilemap)
    {
        foreach (var tile in mapData.destroyedTiles)
        {
            Vector3Int position = new Vector3Int(tile.x, tile.y, 0);
            destructableTilemap.SetTile(position, null);
        }

        foreach (var tile in mapData.destroyedCrystalTiles)
        {
            Vector3Int position = new Vector3Int(tile.x, tile.y, 0);
            crystalsTilemap.SetTile(position, null);
        }
    }

    // M�todo para atribuir o Player ao SaveManager
    public void SetPlayerReference(Player player)
    {
        playerScript = player;
    }

    private void SaveMapData(string mapName, Tilemap destructableTilemap, Tilemap crystalsTilemap, List<MapData> mapDataList)
    {
        List<TileData> destroyedTiles = new List<TileData>();
        foreach (Vector3Int position in destructableTilemap.cellBounds.allPositionsWithin)
        {
            if (destructableTilemap.GetTile(position) == null)
            {
                destroyedTiles.Add(new TileData { x = position.x, y = position.y });
            }
        }

        List<TileData> destroyedCrystalTiles = new List<TileData>();
        foreach (Vector3Int position in crystalsTilemap.cellBounds.allPositionsWithin)
        {
            if (crystalsTilemap.GetTile(position) == null)
            {
                destroyedCrystalTiles.Add(new TileData { x = position.x, y = position.y });
            }
        }

        mapDataList.Add(new MapData
        {
            mapName = mapName,
            destroyedTiles = destroyedTiles,
            destroyedCrystalTiles = destroyedCrystalTiles
        });
    }
}
