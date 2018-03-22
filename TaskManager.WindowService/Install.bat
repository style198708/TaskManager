
C:\Windows\Microsoft.NET\Framework\v4.0.30319\installutil.exe TaskManager.WindowService.exe
Net Start TaskService
sc config TaskService start= auto