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
    public List<TileData> destroyedTiles;
    public List<TileData> destroyedCrystalTiles;
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


    private void Awake()
    {
        PlayerPrefs.SetString("Filename", "saveData.json");

        filePath = Path.Combine(Application.persistentDataPath, PlayerPrefs.GetString("Filename"));
        gameSceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<GameSceneManager>();

    }

    // M�todo para salvar os dados do jogador e inimigos
    public void SaveData()
    {
        // Criar PlayerData com as informa��es do jogador
        PlayerData playerData = new PlayerData
        {
            health = playerScript.health,
            experience = playerScript.experience,
            pickaxeAttackSpeed = GameObject.FindGameObjectWithTag("Pickaxe").GetComponent<PickaxeController>().attackSpeed,
            pickaxeAttackDamage = GameObject.FindGameObjectWithTag("Pickaxe").GetComponent<PickaxeAttack>().attackDamage,
            position = new float[] { playerScript.transform.position.x, playerScript.transform.position.y, playerScript.transform.position.z }
        };

        // Buscar todos os inimigos na cena
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();

        // Criar a lista de EnemyData
        List<EnemyData> enemyDataList = new List<EnemyData>();
        foreach (var enemy in allEnemies)
        {
            EnemyData enemyData = new EnemyData
            {
                health = enemy.Health,  // Supondo que o inimigo tenha um m�todo 'Health()'
                position = new float[] { enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z }
            };
            enemyDataList.Add(enemyData);
        }

        List<TileData> destroyedTiles = new List<TileData>();

        foreach (Vector3Int position in destructableTilemap.cellBounds.allPositionsWithin)
        {
            // Verifica se o tile est� vazio
            if (destructableTilemap.GetTile(position) == null)
            {
                destroyedTiles.Add(new TileData { x = position.x, y = position.y });
            }
        }

        List<TileData> destroyedCrystalTiles = new List<TileData>();

        foreach (Vector3Int position in crystalsTilemap.cellBounds.allPositionsWithin)
        {
            // Verifica se o tile est� vazio
            if (crystalsTilemap.GetTile(position) == null)
            {
                destroyedCrystalTiles.Add(new TileData { x = position.x, y = position.y });
            }
        }

        string currentTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        float accumulatedPlayTime = gameSceneManager.TotalPlayTime;

        // Criar GameData e adicionar PlayerData e inimigos
        GameData gameData = new GameData
        {
            playerData = playerData,
            enemies = enemyDataList,
            destroyedTiles = destroyedTiles,
            destroyedCrystalTiles = destroyedCrystalTiles,
            lastSaveTime = currentTime,
            level = gameSceneManager.currentLevel,
            totalPlayTime = accumulatedPlayTime
        };

        // Salvar os dados no arquivo JSON
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
            Debug.Log("Player and enemies data loaded!");

            // Atualizar os dados do jogador com os dados carregados
            playerScript.health = gameData.playerData.health;
            playerScript.experience = gameData.playerData.experience;
            GameObject.FindGameObjectWithTag("Pickaxe").GetComponent<PickaxeController>().attackSpeed = gameData.playerData.pickaxeAttackSpeed;
            GameObject.FindGameObjectWithTag("Pickaxe").GetComponent<PickaxeAttack>().attackDamage = gameData.playerData.pickaxeAttackDamage;

            // Atualizar a posi��o do jogador
            playerScript.transform.position = new Vector3(gameData.playerData.position[0], gameData.playerData.position[1], gameData.playerData.position[2]);

            // Atualizar o tempo total de jogo
            gameSceneManager.TotalPlayTime = gameData.totalPlayTime; // Carregar o tempo total de jogo
            gameSceneManager.currentLevel = gameData.level;

            // Destruir inimigos existentes
            Enemy[] allEnemies = FindObjectsOfType<Enemy>();
            foreach (var enemy in allEnemies)
            {
                Destroy(enemy.gameObject);
            }

            // Instanciar novos inimigos
            foreach (var enemyData in gameData.enemies)
            {
                // Instanciar o inimigo na posi��o guardada
                GameObject enemyObj = Instantiate(enemyPrefab, new Vector3(enemyData.position[0], enemyData.position[1], enemyData.position[2]), Quaternion.identity);
                Enemy enemyScript = enemyObj.GetComponent<Enemy>();

                if (enemyScript != null)
                {
                    enemyScript.Health = enemyData.health;
                }
            }

            // Remove os tiles destru�dos
            foreach (var tile in gameData.destroyedTiles)
            {
                Vector3Int position = new Vector3Int(tile.x, tile.y, 0);
                destructableTilemap.SetTile(position, null);
            }

            // Remove os tiles destru�dos
            foreach (var tile in gameData.destroyedCrystalTiles)
            {
                Vector3Int position = new Vector3Int(tile.x, tile.y, 0);
                crystalsTilemap.SetTile(position, null);
            }
        }
        else
        {
            Debug.Log("No save file found!");
        }
    }

    // M�todo para atribuir o Player ao SaveManager
    public void SetPlayerReference(Player player)
    {
        playerScript = player;
    }
}
