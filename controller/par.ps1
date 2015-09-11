$start = Get-Date

# get all hotfixes
$task1 = {& C:\tmp\DistanceMeasurement\test.ps1 }

# get all scripts in your profile
$task2 = { & C:\tmp\DistanceMeasurement\autoNavigation.ps1 }

# run 2 tasks in the background, and 1 in the foreground task
Start-Job -ScriptBlock $task1 
Start-Job -ScriptBlock $task2 
Get-Job | Wait-Job | Receive-Job | Out-File -Append -FilePath C:\tmp\DistanceMeasurement\jobs.log


# discard the jobs
Remove-Job -Job $job1, $job2

$end = Get-Date
Write-Host -ForegroundColor Red ($end - $start).TotalSeconds


