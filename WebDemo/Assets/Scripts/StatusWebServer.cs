using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

public class StatusWebServer : MonoBehaviour
{
    public StatusManager statusManager;
    public int port = 8989;

    private HttpListener listener;
    private Thread serverThread;
    private volatile bool shuttingDown;

    void Start()
    {
        try
        {
            listener = new HttpListener();
            listener.Prefixes.Add($"http://localhost:{port}/");
            listener.Start();

            serverThread = new Thread(Listen) { IsBackground = true };
            serverThread.Start();

            Debug.Log($"Status server running: http://localhost:{port}/");
        }
        catch (HttpListenerException exception)
        {
            Debug.LogError($"Status server could not start on port {port}: {exception.Message}");
            listener?.Close();
            listener = null;
        }
    }

    void Listen()
    {
        while (!shuttingDown && listener != null && listener.IsListening)
        {
            HttpListenerContext context;
            try
            {
                context = listener.GetContext();
            }
            catch (HttpListenerException)
            {
                if (shuttingDown)
                    break;

                continue;
            }

            string path = context.Request.Url.AbsolutePath.TrimEnd('/');
            string json = path == "/health"
                ? "{\"status\":\"ok\",\"service\":\"UnityStatusWebServer\",\"schemaVersion\":1}"
                : statusManager != null ? statusManager.GetJson() : "{\"status\":\"unavailable\"}";

            byte[] buffer = Encoding.UTF8.GetBytes(json);

            context.Response.ContentType = "application/json";
            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            context.Response.Headers.Add("Cache-Control", "no-store");
            context.Response.ContentLength64 = buffer.Length;

            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
        }
    }

    void OnApplicationQuit()
    {
        shuttingDown = true;
        listener?.Stop();
        listener?.Close();

        if (serverThread != null && serverThread.IsAlive && !ReferenceEquals(Thread.CurrentThread, serverThread))
            serverThread.Join(1000);
    }
}
