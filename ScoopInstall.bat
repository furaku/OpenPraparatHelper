winget install Microsoft.PowerShell
pwsh -ep RemoteSigned -Command "irm get.scoop.sh | iex"
pwsh -Command "scoop bucket add nodoka https://github.com/nodokaha/myscoop"
pwsh -Command "scoop install OpenPraparatHelper"
