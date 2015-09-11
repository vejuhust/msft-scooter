$wrkDir = $(get-location).Path
$pkgDir = 'c:\tmp\DistanceMeasurement\autonomous'
pushd $pkgDir
$VerbosePreference = 'Continue';
$interval2turn = 1.5
$it = (3 * $interval2turn);
$rtMap = @{};
$ltMap = @{};
$bkMap = @{};
$stMap = @{};
#(5..244)|% {$rtMap[$_]++}
(250)|% {$ltMap[$_]++}
(251..600)|% {$bkMap[$_]++}
(601..999)|% {$stMap[$_]++}
#(532)|% {$stMap[$_]++}


(0..10)| %{$lastCmd = "fd"}{
$ret = & $pkgDir\locate.exe
if(-not $ret) {
write-verbose "locate failed";
$lastCmd
sleep $interval2turn; 
"st"
return;
}
$ret | sc $wrkDir\loc.csv
$c = import-csv $wrkDir\loc.csv
$c | %{
$key = [int]$_.targetId;
if ($rtMap[$key]) {
	write-verbose "Turn WARNING,rt, $_";
	"rt"
	sleep $interval2turn; 
	$lastCmd
	#$lastCmd = "rt"
	sleep $interval2turn; 
	write-verbose "st";
}
elseif ($ltMap[$key]) { 
write-verbose "Turn WARNING,lt, $_";
"lt";
sleep $interval2turn; 
$lastCmd
#$lastCmd = "lt"
sleep $interval2turn; 
#"st"
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
sleep $it; 
write-verbose "st";
"st"
}
}
} 

"exit"
