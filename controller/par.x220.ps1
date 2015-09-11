$start = Get-Date

$wrkDir = 'c:\tmp\DistanceMeasurement'

# get all hotfixes
$task1 = {& c:\tmp\DistanceMeasurement\test.ps1 }

# get all scripts in your profile
$task2 = { & c:\tmp\DistanceMeasurement\autoNavigation.ps1 }

$task3 = { & c:\tmp\DistanceMeasurement\cortana.ps1}

# run 2 tasks in the background, and 1 in the foreground task
Start-Job -ScriptBlock $task1 | out-null
#Sleep 5;
Start-Job -ScriptBlock $task2 | out-null
Start-Job -ScriptBlock $task3 | out-null

$limit = 0;
while($limit -lt 60)
{
Get-Job | Receive-Job |%{
if($_ -match "cs,(.*)")
{
	$matches[1];
	sleep 2;
}else
{
	$_;
}
}
$curTime = Get-Date

$limit = ($curTime - $start).TotalSeconds

}
"exit"
$end = Get-Date

#Write-Host -ForegroundColor Red ($end - $start).TotalSeconds

#Get-Job | Stop-Job
#Get-Job | Remove-job
