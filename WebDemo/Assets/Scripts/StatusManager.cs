using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatusManager : MonoBehaviour
{
    public string GetJson()
    {
        lock (statusLock)
        {
            return currentJson;
        }
    }

    private readonly object statusLock = new object();
    private string currentJson = "{}";
    private int sequence;

    public Transform trackedObject;

    public float updateInterval = 0.25f;

    public bool writeStatusFile;

    private float timer;

    [Serializable]
    public class StatusData
    {
        public ApplicationData application;
        public SessionData session;
        public TrackedObjectData trackedObject;
    }

    [Serializable]
    public class ApplicationData
    {
        public string timestamp;
        public string updatedAtUtc;
        public string scene;
        public string status;
        public int schemaVersion;
        public string source;
        public int sequence;
    }

    [Serializable]
    public class SessionData
    {
        public string scenario;
        public float elapsedSeconds;
    }

    [Serializable]
    public class TrackedObjectData
    {
        public string id;
        public string name;
        public bool active;
        public Vector3 position;
        public Vector3 rotation;
    }

    private string filePath;

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "status.json");

        if (writeStatusFile)
            Debug.Log($"Status JSON Path: {filePath}");

        WriteStatus();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= updateInterval)
        {
            timer = 0f;
            WriteStatus();
        }
    }

    void WriteStatus()
    {
        if (!trackedObject)
            return;

        int currentSequence = ++sequence;
        StatusData currentStatus = new StatusData
        {
            application = new ApplicationData
            {
                timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                updatedAtUtc = DateTime.UtcNow.ToString("O"),
                scene = SceneManager.GetActiveScene().name,
                status = "Running",
                schemaVersion = 1,
                source = "Unity",
                sequence = currentSequence,
            },

            session = new SessionData { scenario = "Demo01", elapsedSeconds = Time.time },

            trackedObject = new TrackedObjectData
            {
                id = trackedObject.name,
                name = trackedObject.name,
                active = trackedObject.gameObject.activeInHierarchy,
                position = trackedObject.position,
                rotation = trackedObject.eulerAngles,
            },
        };

        string json = JsonUtility.ToJson(currentStatus, true);
        lock (statusLock)
        {
            currentJson = json;
        }

        if (writeStatusFile)
            File.WriteAllText(filePath, json);
    }
}
