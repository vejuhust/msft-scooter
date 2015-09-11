push﻿d c:\tmp\DistanceMeasurement
dir |%{ 
$_.name
$ret = .\ActOnImage.ps1 c:\tmp\DistanceMeasurement\$($_.name)
if($ret -imatch "Stop") {
& .\zhiling-bk.wav
}elseif($ret -imatch "Left") {
& .\zhiling-left.wav
}elseif($ret -imatch "Right") {
& .\zhiling-rgt.wav
}else{
& .\zhiling-fd.wav
}
}
