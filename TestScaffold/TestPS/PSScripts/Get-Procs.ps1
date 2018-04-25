Function Get-Procs{
	Param
	(
	$processName
	)

	Get-Process -Name $processName | Select ProcessName, Handles
}