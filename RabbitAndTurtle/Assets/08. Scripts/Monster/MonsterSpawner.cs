using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private Tilemap tileMap;
    [SerializeField]
    private List<GameObject> enemyPrefabs; // 여러 프리팹을 리스트로 받
    [SerializeField]
    private int enemyCount = 10;
    [SerializeField]
    private Transform target;


    private Vector3 offset = new Vector3(0.5f, 0.5f, 0);
    private List<Vector3> possibleTiles = new List<Vector3>();

    [System.Serializable]
    private struct WayPointData
    {
        public GameObject[] wayPoints;
    }

    [SerializeField]
    private WayPointData[] wayPointData;

    private void Awake()
    {
        //tileMap의 bounds 재설정
        tileMap.CompressBounds();
        //타일맵 모든 타일 대상으로 적 배치 가능한 타일 계산
        CalculatePossibleTiles();
        //임의 타일에 enemyCount 숫자만큼 적 생성

        for (int i = 0; i < enemyCount; i++)
        {
            int tileIndex = Random.Range(0, possibleTiles.Count);
            int wayIndex = Random.Range(0, wayPointData.Length);
            int prefabIndex = Random.Range(0, enemyPrefabs.Count); // 랜덤 프리팹 선택

            GameObject prefab = enemyPrefabs[prefabIndex];
            GameObject clone = Instantiate(prefab, possibleTiles[tileIndex], Quaternion.identity);
            clone.GetComponent<EnemyFSM>().Setup(target, wayPointData[wayIndex].wayPoints);
        }
    }

    private void CalculatePossibleTiles()
    {
        BoundsInt  bounds = tileMap.cellBounds;
        //타일맵 내부 모든 타일의 정보를 불러와 alltiles 배열에 저장
        TileBase[] allTiles = tileMap.GetTilesBlock(bounds);

        //외곽 라인 제외 모든 타일 검사
        for(int y = 1; y < bounds.size.y-1; y++)
        {
            for(int x = 1; x < bounds.size.x-1; x++)
            {
                //[y * bounds.size.x + x] 번째 방의 타일 정보를 불러옴
                TileBase tile = allTiles[y * bounds.size.x + x];
                // 해당타일이 비어있지 않으면 적 배치 가능 타일로 판단
                if (tile != null)
                {
                    Vector3Int localPosition = bounds.position + new Vector3Int(x, y);
                    Vector3 position = tileMap.CellToWorld(localPosition) + offset;
                    position.z = 0;
                    possibleTiles.Add(position);
                }
            }
        }
    }
}
