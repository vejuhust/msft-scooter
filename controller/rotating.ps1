c:\tiltwebcam\WebcamController.exe l 50
c:\tiltwebcam\WebcamController.exe r 19
$step = [int]3;
$turnTag = [int]0;
$LeftOrRight = $false;
while($true) {
write-verbose "locate failed";
if ($LeftOrRight) {
	c:\tiltwebcam\WebcamController.exe r $step
	sleep 0.5
	$turnTag += $step;
	if($turnTag -ge 15)
	{
		$LeftOrRight = $false;		
	}
}else{
	c:\tiltwebcam\WebcamController.exe l $step
	sleep 0.5
	$turnTag -= $step;
	if($turnTag -le -15)
	{
		$LeftOrRight = $true;		
	}
}
}