$start = Get-Date

$wrkDir = 'D:\work\scp'

# get all hotfixes
$task1 = {& D:\work\scp\test.ps1 }

# get all scripts in your profile
$task2 = { & D:\work\code\autonomous\Release\autoNavigation.ps1 }

# run 2 tasks in the background, and 1 in the foreground task
Start-Job -ScriptBlock $task1 | out-null
Start-Job -ScriptBlock $task2 | out-null
while($true)
{
Get-Job | Receive-Job 
}

$end = Get-Date
Write-Host -ForegroundColor Red ($end - $start).TotalSeconds

