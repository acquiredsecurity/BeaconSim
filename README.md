## BeaconSim - C2 Beacon Simulator
Simulate Malware activity. Configure connections via different ports and protocls with Yaml.
---


<img width="611" alt="image" src="https://github.com/user-attachments/assets/f8c162a8-d24b-4732-99b5-42d4cd0a4686" />


Great for testing TI from your malware lab, to verify detections and searching logic in EDR and SIEMs!


PS C:\BeaconSim\> .\BeaconSim.exe --help
BeaconSim - C2 Beacon Simulator
---------------------------------

```
Usage:
  BeaconSim.exe [--config=path] [--minInterval=N] [--maxInterval=N] [--loopCount=N]

Options:
  --config=path        Path to YAML config file (default: targets.yaml)
  --minInterval=N      Minimum delay in seconds between beacons (default: 60)
  --maxInterval=N      Maximum delay in seconds between beacons (default: 300)
  --loopCount=N        Number of beacon attempts (-1 = infinite)
  --help, -h           Show this help menu and exit
```

---

### Example:

```
.\BeaconSim.exe --config=targets.yaml --minInterval=30 --maxInterval=120 --loopCount=10
```

🌐 Supported Protocols
You can specify one or more protocols per target in your targets.yaml config file. BeaconSim will simulate connection attempts using the selected protocols and ports.

| Protocol    | Description                                      | Notes                           |
| ----------- | ------------------------------------------------ | ------------------------------- |
| `ping`      | ICMP echo request (simulated)                    | Uses DNS resolution to simulate |
| `http`      | HTTP GET request                                 | Port 80 by default              |
| `https`     | HTTPS GET request (SSL/TLS)                      | Port 443 by default             |
| `dns`       | DNS resolution via `Dns.GetHostAddressesAsync()` | Uses system resolver            |
| `tcp`       | Raw TCP socket connection                        | Requires port(s) to be defined  |
| `ftp`       | TCP connection on port 21                        | Simulates basic connection only |
| `ssh`       | TCP connection on port 22                        | No authentication performed     |
| `websocket` | WebSocket handshake (if implemented)             | Fallbacks to HTTP(s) if needed  |
