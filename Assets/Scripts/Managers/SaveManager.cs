using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class PlayerData
{
    public int health;
    public int experience;
    public int pickaxeLevel;
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
    public PlayerData playerData;
    public List<EnemyData> enemies;
    public List<TileData> destroyedTiles;
    public List<TileData> destroyedCrystalTiles;
}

public class SaveManager : MonoBehaviour
{
    private static SaveManager _instance;
    private string filePath;

    public string FilePath { get; private set; }

    [SerializeField] private Player playerScript;  // Referência ao script do Player
    [SerializeField] private GameObject enemyPrefab;  // Prefab do inimigo


    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, PlayerPrefs.GetString("Filename"));
    }

    // Método para salvar os dados do jogador e inimigos
    public void SaveData(Tilemap destructableTilemap, Tilemap crystalsTilemap)
    {
        // Criar PlayerData com as informações do jogador
        PlayerData playerData = new PlayerData
        {
            health = playerScript.health,
            experience = playerScript.experience,
            pickaxeLevel = playerScript.pickaxeLevel,
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
                health = enemy.Health,  // Supondo que o inimigo tenha um método 'Health()'
                position = new float[] { enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z }
            };
            enemyDataList.Add(enemyData);
        }

        List<TileData> destroyedTiles = new List<TileData>();

        foreach (Vector3Int position in destructableTilemap.cellBounds.allPositionsWithin)
        {
            // Verifica se o tile está vazio
            if (destructableTilemap.GetTile(position) == null)
            {
                destroyedTiles.Add(new TileData { x = position.x, y = position.y });
            }
        }

        List<TileData> destroyedCrystalTiles = new List<TileData>();

        foreach (Vector3Int position in crystalsTilemap.cellBounds.allPositionsWithin)
        {
            // Verifica se o tile está vazio
            if (crystalsTilemap.GetTile(position) == null)
            {
                destroyedCrystalTiles.Add(new TileData { x = position.x, y = position.y });
            }
        }


        // Criar GameData e adicionar PlayerData e inimigos
        GameData gameData = new GameData
        {
            playerData = playerData,
            enemies = enemyDataList,
            destroyedTiles = destroyedTiles,
            destroyedCrystalTiles= destroyedCrystalTiles
        };

        // Salvar os dados no arquivo JSON
        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Game data saved! Filepath: " + filePath);
    }

    // Método para carregar os dados e instanciar inimigos nas posições salvas
    public void LoadData(Tilemap destructableTilemap, Tilemap crystalTilemap)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            GameData gameData = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Player and enemies data loaded!");

            // Atualizar os dados do jogador com os dados carregados
            playerScript.health = gameData.playerData.health;
            playerScript.experience = gameData.playerData.experience;
            playerScript.pickaxeLevel = gameData.playerData.pickaxeLevel;

            // Atualizar a posição do jogador
            playerScript.transform.position = new Vector3(gameData.playerData.position[0], gameData.playerData.position[1], gameData.playerData.position[2]);

            // Destruir inimigos existentes
            Enemy[] allEnemies = FindObjectsOfType<Enemy>();
            foreach (var enemy in allEnemies)
            {
                Destroy(enemy.gameObject);
            }

            // Instanciar novos inimigos
            foreach (var enemyData in gameData.enemies)
            {
                // Instanciar o inimigo na posição salva
                GameObject enemyObj = Instantiate(enemyPrefab, new Vector3(enemyData.position[0], enemyData.position[1], enemyData.position[2]), Quaternion.identity);
                Enemy enemyScript = enemyObj.GetComponent<Enemy>();

                if (enemyScript != null)
                {
                    // Atualizar a saúde do inimigo
                    enemyScript.Health = enemyData.health;  // Supondo que o inimigo tenha um método 'SetHealth'
                }
            }

            // Remove os tiles destruídos
            foreach (var tile in gameData.destroyedTiles)
            {
                Vector3Int position = new Vector3Int(tile.x, tile.y, 0);
                destructableTilemap.SetTile(position, null);
            }

            // Remove os tiles destruídos
            foreach (var tile in gameData.destroyedCrystalTiles)
            {
                Vector3Int position = new Vector3Int(tile.x, tile.y, 0);
                crystalTilemap.SetTile(position, null);
            }
        }
        else
        {
            Debug.Log("No save file found!");
        }
    }

    // Método para atribuir o Player ao SaveManager
    public void SetPlayerReference(Player player)
    {
        playerScript = player;
    }

}
