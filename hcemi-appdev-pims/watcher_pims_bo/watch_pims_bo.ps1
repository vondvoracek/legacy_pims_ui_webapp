Function Register-Watcher {
    param ($folder)
    $filter = "MI.PIMS.BO.dll" #MI.PIMS.BO.dll
    $watcher = New-Object IO.FileSystemWatcher $folder, $filter -Property @{ 
        IncludeSubdirectories = $false
        EnableRaisingEvents = $true
    }
    $watcher.path = "C:\github\ghec\hcemi-appdev-pims\MI.PIMS.MT\MI.PIMS.BO\bin\Release\net6.0\"
    $destination = "C:\github\ghec\hcemi-appdev-pims\MI.PIMS.UI\MI.PIMS.UI\Libs\"

    $changeAction = {
        $path = $event.SourceEventArgs.FullPath
        $name = $event.SourceEventArgs.Name
        $changetype = $event.SourceEventArgs.ChangeType
        Write-Host "File $name at path $path was $changetype at $(get-date)"
        Copy-Item -Path C:\github\ghec\hcemi-appdev-pims\MI.PIMS.MT\MI.PIMS.BO\bin\Release\net6.0\\MI.PIMS.BO.dll -Destination "C:\github\ghec\hcemi-appdev-pims\MI.PIMS.UI\MI.PIMS.UI\Libs\" 
    }    

    Register-ObjectEvent $Watcher -EventName "Changed" -Action $changeAction
}

Register-Watcher "C:\github\ghec\hcemi-appdev-pims\MI.PIMS.MT\MI.PIMS.BO\bin\Release\net6.0\"