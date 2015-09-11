C:\exp\DistanceMeasurement\CS_DEMO.exe | %{$idx = 0; $fdOrSt = $true}{
if($idx -gt 20)
{
"exit"
}
if ($_ -match "fd")
{
	$idx ++;
	
	if($fdOrSt) {"fd" }else {"bk"}
	C:\exp\DistanceMeasurement\zhiling-fd.wav
	
}elseif ($_ -match "st")
{
	"st"
	C:\exp\DistanceMeasurement\zhiling-ask4way.wav
	$fdOrSt = -not $fdOrSt;
}
}

