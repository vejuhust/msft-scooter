param (
$path="C:\Users\yimlin\Pictures\hackathon\tmp.jpg",
$url = ""
)
if ($url) 
{
	. d:\tmp\download.ps1 $url $path
}
$ret =identify $path
$width = [int]($ret.Split()[2].split('x')[0]);
$halfWidth = $width / 2
$height = $ret.Split()[2].split('x')[1];
convert -crop "$($halfWidth)x$height+0+0"  $path "$path.lft.jpg"
convert -crop "$($halfWidth)x$height+$halfWidth+0"  $path "$path.rgt.jpg"


$ret = curl "http://localhost:30629/Home/AnalyzeRoad?isTest=False&faceUrl=$path.lft.jpg"
if($ret.Content -imatch "true") {$isLftRoad = $true;}

$ret = curl "http://localhost:30629/Home/AnalyzeRoad?isTest=False&faceUrl=$path.rgt.jpg"
if($ret.Content -imatch "true") {$isRgtRoad = $true;}

if($isLftRoad -and $isRgtRoad) {"FD"}elseif($isLftRoad) {"Left"}elseif($isRgtRoad) {"Right"}else {"Stop"}

