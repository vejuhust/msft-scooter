$wrkDir = $(get-location).Path
$pkgDir = 'c:\tmp\DistanceMeasurement\autonomous'
pushd $pkgDir
$VerbosePreference = 'Continue';
$interval2turn = 0.5
$it = (3 * $interval2turn);
$fdMap = @{};
$rtMap = @{};
$ltMap = @{};
$bkMap = @{};
$stMap = @{};
(0..81)|% {$fdMap[$_]++}
(212..216)|% {$fdMap[$_]++}
(367..460)|% {$fdMap[$_]++}
#(82..211)|% {$rtMap[$_]++}
#(466..713)|% {$rtMap[$_]++}
(657..713)|% {$rtMap[$_]++}
(220..366)|% {$ltMap[$_]++}
#(251..600)|% {$bkMap[$_]++}
(720..999)|% {$stMap[$_]++}
#(532)|% {$stMap[$_]++}

c:\tiltwebcam\WebcamController.exe l 50
c:\tiltwebcam\WebcamController.exe r 19

$step = [int]3;
(0..120)| %{$lastCmd = "fd"}{
$ret = & $pkgDir\locate.exe
$turnTag = [int]0;
$LeftOrRight = $false; #Left
while(-not $ret) {
write-verbose "locate failed";
if ($LeftOrRight) {
	c:\tiltwebcam\WebcamController.exe r $step
	$turnTag += $step;
	if($turnTag -ge 15)
	{
		$LeftOrRight = $false;
		$turnTag = 0;
	}
}else{
	c:\tiltwebcam\WebcamController.exe l $step
	$turnTag -= $step;
	if($turnTag -le -15)
	{
		$LeftOrRight = $true;
		$turnTag = 0;
	}
}
$ret = & $pkgDir\locate.exe
}

$ret | sc $wrkDir\loc.csv
$c = import-csv $wrkDir\loc.csv
$c | %{
$key = [int]$_.targetId;
if ($rtMap[$key]) {
	write-verbose "Turn WARNING,rt, $_, with $turnTag";
	#if($turnTag -ge 3 -or [double]$_.distance2Neareast -gt 0)
	if($turnTag -le -3)
	{
		"fd"
		sleep $interval2turn; 
		$lastCmd
		sleep $interval2turn; 
	}else
	{
		"rt"
		sleep $interval2turn; 
		$lastCmd
		sleep $it;
		"st" 
		sleep $interval2turn;
		"lt"
		sleep $interval2turn; 
	}	
	
	sleep $interval2turn; 
	write-verbose "st";
}
elseif ($ltMap[$key]) { 
write-verbose "Turn WARNING,lt, $_,  with $turnTag";
#if($turnTag -le -3 -or [double]$_.distance2Neareast -gt 0)
if($turnTag -ge 3)
	{
		"fd"
		sleep $interval2turn; 
		$lastCmd
		sleep $interval2turn; 
	}else
	{
		"lt"
		sleep $interval2turn; 
		$lastCmd
		sleep $interval2turn; 
		"lt"
		sleep $interval2turn; 
	}
	sleep $interval2turn; 
	"st"
	write-verbose "st";
}
elseif ($bkMap[$key]) {
write-verbose "bk, $_";
$lastCmd = "bk"
"bk";
sleep $interval2turn; 
"st"
write-verbose "st";
}
elseif ($stMap[$key]) {
write-verbose "st, $_";
$lastCmd = "st"
"st";
}
else{
write-verbose "$lastCmd, $_";
$lastCmd
sleep $interval2turn; 
write-verbose "st";
"st"
}
}
} 

"exit"