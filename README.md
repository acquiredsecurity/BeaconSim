██████  ███████  █████   ██████  ██████  ███    ██ ███████ ██ ███    ███ 
██   ██ ██      ██   ██ ██      ██    ██ ████   ██ ██      ██ ████  ████ 
██████  █████   ███████ ██      ██    ██ ██ ██  ██ ███████ ██ ██ ████ ██ 
██   ██ ██      ██   ██ ██      ██    ██ ██  ██ ██      ██ ██ ██  ██  ██ 
██████  ███████ ██   ██  ██████  ██████  ██   ████ ███████ ██ ██      ██ 

Simulate Malware activity configure connections via different ports and protocls with Yaml



PS C:\BeaconSim\> .\BeaconSim.exe --help
BeaconSim - C2 Beacon Simulator
---------------------------------
Usage:
  BeaconSim.exe [--config=path] [--minInterval=N] [--maxInterval=N] [--loopCount=N]

Options:
  --config=path        Path to YAML config file (default: targets.yaml)
  --minInterval=N      Minimum delay in seconds between beacons (default: 60)
  --maxInterval=N      Maximum delay in seconds between beacons (default: 300)
  --loopCount=N        Number of beacon attempts (-1 = infinite)
  --help, -h           Show this help menu and exit

Example:
  BeaconSim.exe --config=mytargets.yaml --minInterval=30 --maxInterval=120 --loopCount=10
