$VerbosePreference = 'Continue';
$interval2turn = 1
$it = (3 * $interval2turn);
$rtMap = @{};
$ltMap = @{};
$bkMap = @{};
$stMap = @{};
(479, 544)|% {$rtMap[$_]++}
(532..542)|% {$ltMap[$_]++}
(120)|% {$bkMap[$_]++}
(565..573)|% {$stMap[$_]++}

(0..5)| %{$lastCmd = "fd"}{
.\locate.exe | sc .\loc.csv
$c = import-csv .\loc.csv
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
