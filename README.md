## LOCAL REDIS setup

Instructions on how to set up Redis on Windows workstations

## Getting Started

These instructions will create a local instance of Redis running in a container

### Prerequisites
1. Make sure your ms_id is a part of the ASAM_Developer_Elevation group

### Steps
1. Install Chocolatey from app store using Appstore App Details - E2T Intake Portal![image](https://github.com/user-attachments/assets/15caa1db-746b-4a4d-9277-5b358b20c110)
2. Run powershell as Admin
3. Go to the location of the powershell.exe file, right click and run with Adminstrator rights
4. ![image](https://github.com/user-attachments/assets/749248de-c41b-4826-b03b-1a74b6a3097b)
5. After Chocolatey is installed install Podman-Cli
6. Type command into powershell prompt: choco install podman-cli
7. Reference: https://community.chocolatey.org/packages/podman-cli
8. Install Podman-desktop
9. Type command into powershell prompt: choco install podman-desktop
10. Reference: https://community.chocolatey.org/packages/podman-desktop
11. **Very Important SetUp Hyper-V**
12. If you haven't enabled hyper-v before you may be prompted for a restart!
13. Type command into powershell prompt: Enable-WindowsOptionalFeature -Online -FeatureName Microsoft-Hyper-V -All
14. **This command needs to be ran before opening Podman-Desktop**
15. Reference: https://learn.microsoft.com/en-us/virtualization/hyper-v-on-windows/quick-start/enable-hyper-v
16. Open Podman-Desktop with admin privileges
17. ![image](https://github.com/user-attachments/assets/6a4b6f46-714d-4088-95e9-5033edc15ab3)
18. Update Podman to 5.3.1
19. **Choose Hyper-V option**
20. Go to the bottom of home screen
21. press Initialize and Setup
22. Click on the cog at the bottom left side of the screen ![Screenshot 2025-01-10 at 2 20 22 PM](https://github.com/user-attachments/assets/e4a667aa-48e5-4dcc-ad6e-ab518fa2b983)
23. Control machine from here start, stop, create a new one and even open a terminal ![image](https://github.com/user-attachments/assets/fe765fba-98d6-46a7-ba71-6269d4bc4a21)
24. Pull redis 6 from Optum Artifactory
25. Use the left navigation bar to navigate to images tab
26. Hit the pull button and type docker.repo1.uhc.com/redis:6
27. Start Container by hitting the play button on the newly downloaded image
28. ![image](https://github.com/user-attachments/assets/d196b6a7-1d40-4989-8191-b3dc5f7bb79f)
29. In PIMS application update Redis Cache Connection to localhost:6379 and run!
30. On the container tab of Podman-Desktop you can exec into the container and run redis-cli commands to view keys
31. Troubleshoot: If your podman doesn't automatically initialize; check if you have image running under Images section.

## Troubleshoot
If you get following issue after installation and when you try to create podman machine

    **Error: Command execution failed with exit code 125 Command execution failed with exit code 125 Error: pinging container registry quay.io: Get "https://quay.io:443/v2/": Bad Request**
    
Please go to Proxy under setting of podman, and DISABLE it.




   
 







