SELECT *
FROM msdb.dbo.sysjobs JOB WITH( NOLOCK)
INNER JOIN msdb. dbo.sysjobsteps STP WITH(NOLOCK )
ON STP .job_id = JOB .job_id