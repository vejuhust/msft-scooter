$wrkDir = $(get-location).Path
$pkgDir = 'd:\ps\club\garage\DistanceMeasurement'
pushd $pkgDir
& $pkgDir\CS.bat | %{$idx = 0}{
if ($_ -match "fd")
{
	$idx ++;
	if ($idx -le 0 -or $idx -gt 6)
	{
	"fd" 
	& $pkgDir\zhiling-fd.wav
	$idx = 1;
	}
}elseif ($_ -match "st")
{
	"st"
	& $pkgDir\zhiling-ask4way.wav
}
}

