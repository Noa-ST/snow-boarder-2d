using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public class Terrain : MonoBehaviour
{
    [SerializeField] private GameObject finishLine;
    [SerializeField] private GameObject starPrefabs;

    // THÊM: Prefabs cho Power-up
    [SerializeField] private GameObject speedBoostPrefab;
    [SerializeField] private GameObject invincibilityPrefab;

    [SerializeField] private List<GameObject> environmentElementsList = new List<GameObject>();
    [SerializeField] private List<GameObject> cloudsList = new List<GameObject>();

    // Cấu hình cho từng level
    [System.Serializable]
    public class LevelConfig
    {
        public int numberOfPoints = 50;        // Số điểm tạo terrain
        public int distanceBetweenPoints = 10;  // Khoảng cách giữa các điểm
        public float pointMinHeight = 3f;       // Độ cao tối thiểu
        public float pointMaxHeight = 20f;      // Độ cao tối đa
        public int pointIndex = 2;              // Bắt đầu từ điểm thứ mấy
        public float tangentMinWidth = 1f;      // Độ rộng tối thiểu của tangent
        public float tangentMaxWidth = 5f;      // Độ rộng tối đa của tangent
        public float minSplineHeight = 1f;      // Độ cao tối thiểu của spline
        public float maxSplineHeight = 4f;      // Độ cao tối đa của spline
        public int cloudMinSize = 1;            // Kích thước tối thiểu của đám mây
        public int cloudMaxSize = 5;            // Kích thước tối đa của đám mây
        public int cloudMinHeight = 10;         // Độ cao tối thiểu của đám mây
        public int cloudMaxHeight = 16;         // Độ cao tối đa của đám mây
        public float rampChance = 0.2f;         // Xác suất tạo ramp
        public float obstacleDensity = 0.6f;    // Mật độ chướng ngại vật (0-1)
        public float collectibleChance = 0.7f;  // ĐỔI TÊN: Thành xác suất chung cho tất cả vật phẩm thu thập
        public float powerUpRatio = 0.3f;       // THÊM: Tỷ lệ Power-up so với tổng số vật phẩm thu thập (0.3 = 30%)
    }

    [SerializeField]
    private List<LevelConfig> levelConfigs = new List<LevelConfig>
    {
        // THAY ĐỔI: snowflakeChance thành collectibleChance, thêm powerUpRatio
        new LevelConfig { numberOfPoints = 30, distanceBetweenPoints = 12, pointMaxHeight = 12f, rampChance = 0.1f, obstacleDensity = 0.5f, collectibleChance = 0.6f, powerUpRatio = 0.2f }, // Level 1
        new LevelConfig { numberOfPoints = 35, distanceBetweenPoints = 11, pointMaxHeight = 14f, rampChance = 0.12f, obstacleDensity = 0.55f, collectibleChance = 0.65f, powerUpRatio = 0.25f }, // Level 2
        new LevelConfig { numberOfPoints = 40, distanceBetweenPoints = 10, pointMaxHeight = 16f, rampChance = 0.14f, obstacleDensity = 0.6f, collectibleChance = 0.7f, powerUpRatio = 0.3f }, // Level 3
        new LevelConfig { numberOfPoints = 45, distanceBetweenPoints = 10, pointMaxHeight = 18f, rampChance = 0.16f, obstacleDensity = 0.65f, collectibleChance = 0.75f, powerUpRatio = 0.35f }, // Level 4
        new LevelConfig { numberOfPoints = 50, distanceBetweenPoints = 10, pointMaxHeight = 20f, rampChance = 0.18f, obstacleDensity = 0.7f, collectibleChance = 0.8f, powerUpRatio = 0.4f } // Level 5
    };

    private SpriteShapeController shape;
    private int currentLevelIndex;

    private void Start()
    {
        shape = GetComponent<SpriteShapeController>();
        if (shape == null)
        {
            Debug.LogError("SpriteShapeController not found on this GameObject!");
            return;
        }

        currentLevelIndex = SceneManager.GetActiveScene().buildIndex - 1;
        if (currentLevelIndex < 0 || currentLevelIndex >= levelConfigs.Count)
        {
            Debug.LogWarning("Invalid level index, using default config (Level 1).");
            currentLevelIndex = 0;
        }

        LevelConfig config = levelConfigs[currentLevelIndex];
        for (int i = config.pointIndex; i < config.numberOfPoints; i++)
        {
            float xPosition = i * config.distanceBetweenPoints;
            float yPosition = Random.Range(config.pointMinHeight, config.pointMaxHeight);

            // Logic tạo ramp (giữ nguyên)
            if (Random.value < config.rampChance && i < config.numberOfPoints - 5)
            {
                yPosition += 5f * (i / 10f);
                shape.spline.SetHeight(i, Random.Range(config.minSplineHeight * 1.2f, config.maxSplineHeight * 1.2f));
            }

            shape.spline.InsertPointAt(i, new Vector3(xPosition, yPosition, 0));
            shape.spline.SetTangentMode(i, ShapeTangentMode.Continuous);
            shape.spline.SetLeftTangent(i, new Vector3(-Random.Range(config.tangentMinWidth, config.tangentMaxWidth), 0, 0));
            shape.spline.SetRightTangent(i, new Vector3(Random.Range(config.tangentMinWidth, config.tangentMaxWidth), 0, 0));
            shape.spline.SetHeight(i, Random.Range(config.minSplineHeight, config.maxSplineHeight));

            if (i == config.numberOfPoints - 1)
            {
                finishLine.transform.position = new Vector3(xPosition, yPosition + config.pointIndex, 0);
            }
            else
            {
                SetRandomEnvironmentObject(xPosition, yPosition, config.obstacleDensity);
                SetRandomCloud(xPosition, yPosition, config.cloudMinSize, config.cloudMaxSize, config.cloudMinHeight, config.cloudMaxHeight);
                // ĐỔI TÊN: Gọi phương thức chung để spawn vật phẩm thu thập
                SetRandomCollectible(xPosition, yPosition, config.collectibleChance, config.powerUpRatio);
            }
        }
    }

    private void SetRandomEnvironmentObject(float xPosition, float yPosition, float obstacleDensity)
    {
        if (Random.value > obstacleDensity - (currentLevelIndex * 0.05f))
        {
            GameObject environmentElement = environmentElementsList[Random.Range(0, environmentElementsList.Count)];
            if (environmentElement.CompareTag("Tree"))
            {
                Instantiate(environmentElement, new Vector3(xPosition, yPosition + 1, 0), Quaternion.identity);
            }
            else
            {
                Instantiate(environmentElement, new Vector3(xPosition, yPosition - 1, 0), Quaternion.identity);
            }
        }
    }

    private void SetRandomCloud(float xPosition, float yPosition, int cloudMinSize, int cloudMaxSize, int cloudMinHeight, int cloudMaxHeight)
    {
        int randomSize = Random.Range(cloudMinSize, cloudMaxSize);
        int randomHeight = Random.Range(cloudMinHeight, cloudMaxHeight);
        GameObject randomCloud = cloudsList[Random.Range(0, cloudsList.Count)];
        randomCloud.transform.localScale = new Vector3(randomSize, randomSize, randomSize);
        Instantiate(randomCloud, new Vector3(xPosition, yPosition + randomHeight, 0), Quaternion.identity);
    }

    // ĐỔI TÊN: Phương thức cũ SetRandomSnowflake được thay bằng phương thức chung
    private void SetRandomCollectible(float xPosition, float yPosition, float collectibleChance, float powerUpRatio)
    {
        // 1. Kiểm tra tổng xác suất spawn vật phẩm thu thập (Star hoặc Power-up)
        if (Random.value > collectibleChance - (currentLevelIndex * 0.05f))
        {
            GameObject prefabToSpawn;

            // 2. Kiểm tra xem vật phẩm là Power-up hay Star
            if (Random.value < powerUpRatio)
            {
                // Là Power-up: Chọn ngẫu nhiên giữa SpeedBoost và Invincibility
                if (Random.value < 0.5f)
                {
                    prefabToSpawn = speedBoostPrefab;
                }
                else
                {
                    prefabToSpawn = invincibilityPrefab;
                }

                // Kiểm tra xem đã gán prefab Power-up chưa
                if (prefabToSpawn == null)
                {
                    Debug.LogWarning("PowerUp Prefab is missing, falling back to Star.");
                    prefabToSpawn = starPrefabs;
                }
            }
            else
            {
                // Là Star
                prefabToSpawn = starPrefabs;
            }

            // 3. Tạo vật phẩm
            if (prefabToSpawn != null)
            {
                // Spawn vật phẩm cao hơn địa hình 2f
                Instantiate(prefabToSpawn, new Vector3(xPosition, yPosition + 2f, 0), Quaternion.identity);
            }
            else
            {
                // Trường hợp starPrefabs bị null
                Debug.LogError("Collectible prefab (Star/PowerUp) is null!");
            }
        }
    }
}