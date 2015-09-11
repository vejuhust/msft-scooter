$VerbosePreference = 'Continue';
$rtMap = @{};
$ltMap = @{};
$bkMap = @{};
(479, 544)|% {$rtMap[$_]++}
(539)|% {$ltMap[$_]++}
(565)|% {$bkMap[$_]++}
(0..500)| %{
.\locate.exe | sc .\loc.csv
$c = import-csv .\loc.csv
$c | %{
if ([double]$_.targetId -gt 190) {
write-verbose "Turn WARNING,st, $_";
"rt"
write-verbose "st";
}
if ([double]$_.angle2Neareast -gt 0.1) {
write-verbose "lt, $_";
"lt";sleep 0.3; 
"st"
write-verbose "st";
}
elseif([double]$_.angle2Neareast -lt -0.1){
write-verbose "rt, $_";
"rt";sleep 0.3; "st"
write-verbose "st";
}
else{
write-verbose "fd, $_";
"fd";
sleep 0.3; 
write-verbose "st";
"st"
}
}
} 