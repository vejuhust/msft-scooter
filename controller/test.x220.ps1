#$wrkDir = $(get-location).Path
$pkgDir = 'c:\tmp\DistanceMeasurement'
pushd $pkgDir
& $pkgDir\CS_DEMO.exe | %{$idx = 0}{
if ($_ -match "fd")
{
	$idx ++;
	if ($idx -le 0 -or $idx -gt 6)
	{
	#"cs,fd" 
	#& $pkgDir\zhiling-fd.wav
	$idx = 1;
	}
}elseif ($_ -match "st")
{
	"cs,lt"
	sleep 1;
	"cs,bk"
	sleep 1;
	# & $pkgDir\zhiling-ask4way.wav
}
}

