using System.Collections.Generic;
using UnityEngine;

// Pre-builds a fixed number of lasers through LaserFactory at scene start, then hands out and
// reclaims from that fixed set instead of the game ever calling Instantiate/Destroy on a laser
// again.
public class LaserPoolManager : MonoBehaviour
{
    public static LaserPoolManager Instance { get; private set; }

    public GameObject projectilePrefab;

    [SerializeField] private int quantity = 5;

    private List<ProjectileLaser> pooledLasers;
    private LaserFactory factory;

    void Awake()
    {
        if (Instance != null)
        {
            GameLog.Warning(LogCategory.Projectile, "A second LaserPoolManager exists, the newer one will be ignored");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        factory = new LaserFactory();
        pooledLasers = new List<ProjectileLaser>();
    }

    void Start()
    {
        for (int i = 0; i < quantity; i++)
            CreatePooledLaser();
    }

    private void CreatePooledLaser()
    {
        if (projectilePrefab == null)
        {
            GameLog.Warning(LogCategory.Projectile, "No projectile prefab assigned, the laser pool will stay empty");
            return;
        }

        ProjectileLaser laser = factory.CreateLaser(projectilePrefab);
        if (laser == null)
            return;

        // Parented directly under the pool itself rather than under World - World's children
        // all get deleted and recreated on every level build, which would wipe the pool out too.
        laser.transform.SetParent(transform);
        laser.gameObject.SetActive(false);
        pooledLasers.Add(laser);
    }

    public ProjectileLaser GetPooledLaser()
    {
        foreach (ProjectileLaser laser in pooledLasers)
        {
            if (!laser.gameObject.activeInHierarchy)
            {
                GameLog.Info(LogCategory.Projectile, "Laser taken from pool");
                return laser;
            }
        }

        return null;
    }
}
