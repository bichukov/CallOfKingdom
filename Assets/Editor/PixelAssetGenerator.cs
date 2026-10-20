using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class LevelBuilder : Editor
{
    [MenuItem("Tools/Сгенерировать уровень")]
    public static void BuildLevel()
    {
        // 1. Создаем сетку и тайлмап
        GameObject gridObj = new GameObject("Grid", typeof(Grid));
        GameObject tilemapObj = new GameObject("Ground", typeof(Tilemap), typeof(TilemapRenderer));
        tilemapObj.transform.SetParent(gridObj.transform);

        Tilemap tilemap = tilemapObj.GetComponent<Tilemap>();
        tilemap.ClearAllTiles();

        // 2. Загружаем нашу траву и делаем из нее Тайл
        Sprite grassSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/PixelPack/grass_tile.png");
        if (grassSprite == null)
        {
            Debug.LogError("Не найден Assets/PixelPack/grass_tile.png! Убедись, что ты нажал 'Tools -> Сгенерировать пиксель-ассеты'");
            return;
        }

        Tile grassTile = ScriptableObject.CreateInstance<Tile>();
        grassTile.sprite = grassSprite;
        grassTile.colliderType = Tile.ColliderType.Grid;

        // 3. Застилаем поле 20x20 тайлами
        int size = 20;
        for (int x = -size / 2; x < size / 2; x++)
        {
            for (int y = -size / 2; y < size / 2; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), grassTile);
            }
        }

        // 4. Добавляем коллайдер на землю, чтобы рыцарь не проваливался
        TilemapCollider2D collider = tilemapObj.AddComponent<TilemapCollider2D>();
        tilemapObj.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        // 5. Ищем или создаем героя
        GameObject hero = GameObject.Find("Hero");
        if (hero == null)
        {
            hero = new GameObject("Hero");
            hero.AddComponent<SpriteRenderer>();
            hero.AddComponent<Rigidbody2D>().gravityScale = 0; // Отключаем гравитацию для вида сверху
            hero.AddComponent<BoxCollider2D>();
        }

        // Ставим первого попавшегося рыцаря
        Sprite knightSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/PixelPack/knight_walk_0.png");
        if (knightSprite != null) hero.GetComponent<SpriteRenderer>().sprite = knightSprite;
        hero.transform.position = new Vector3(0, 0, -1); // Чуть ближе к камере

        // 6. Расставляем окружение (камни и деревья)
        Sprite[] rocks = LoadSprites("Assets/PixelPack/rock.png");
        Sprite[] trees = LoadSprites("Assets/PixelPack/tree.png");
        
        for (int i = 0; i < 10; i++)
        {
            SpawnObject(rocks, Random.Range(-8, 8), Random.Range(-8, 8));
            SpawnObject(trees, Random.Range(-9, 9), Random.Range(-9, 9));
        }

        // 7. Настраиваем камеру
        Camera.main.orthographicSize = 7;
        Camera.main.transform.position = new Vector3(0, 0, -10);
        Camera.main.backgroundColor = new Color32(30, 30, 46, 255); // Темный фон

        Debug.Log("Уровень сгенерирован! Нажми Play.");
    }

    static Sprite[] LoadSprites(string path)
    {
        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);
        System.Collections.Generic.List<Sprite> sprites = new System.Collections.Generic.List<Sprite>();
        foreach (var a in assets) if (a is Sprite s) sprites.Add(s);
        return sprites.ToArray();
    }

    static void SpawnObject(Sprite[] sprites, float x, float y)
    {
        if (sprites.Length == 0) return;
        GameObject obj = new GameObject("Decor");
        obj.transform.position = new Vector3(x, y, 0);
        obj.AddComponent<SpriteRenderer>().sprite = sprites[0];
    }
}