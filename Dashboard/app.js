let requestInFlight = false;
let wasConnected = false;

async function updateStatus() {
  if (requestInFlight) return;

  requestInFlight = true;
  const controller = new AbortController();
  const timeout = setTimeout(() => controller.abort(), 1000);

  try {
    const response = await fetch("http://localhost:8989/", {
      cache: "no-store",
      signal: controller.signal,
    });

    if (!response.ok) throw new Error(`HTTP ${response.status}`);

    const data = await response.json();
    const receivedAt = new Date();

    document.getElementById("statusValue").textContent =
      data.application.status;

    document.getElementById("sceneValue").textContent = data.application.scene;

    document.getElementById("timeValue").textContent =
      data.application.updatedAtUtc || data.application.timestamp;

    document.getElementById("receivedValue").textContent =
      receivedAt.toLocaleTimeString();

    document.getElementById("objectNameValue").textContent =
      data.trackedObject.name;

    document.getElementById("xValue").textContent =
      data.trackedObject.position.x.toFixed(2);

    document.getElementById("yValue").textContent =
      data.trackedObject.position.y.toFixed(2);

    document.getElementById("zValue").textContent =
      data.trackedObject.position.z.toFixed(2);

    document.getElementById("rotationValue").textContent =
      `${data.trackedObject.rotation.x.toFixed(1)}, ${data.trackedObject.rotation.y.toFixed(1)}, ${data.trackedObject.rotation.z.toFixed(1)}`;

    document.getElementById("rawJson").textContent = JSON.stringify(
      data,
      null,
      2,
    );

    document.getElementById("connectionIndicator").classList.add("connected");
    wasConnected = true;
  } catch (error) {
    document.getElementById("statusValue").textContent = "Disconnected";

    document
      .getElementById("connectionIndicator")
      .classList.remove("connected");

    if (wasConnected) console.warn("Unity status connection lost", error);
    wasConnected = false;
  } finally {
    clearTimeout(timeout);
    requestInFlight = false;
  }
}

updateStatus();
setInterval(updateStatus, 250);
