FOR /D %%D in ("Materialise.*") DO (
	@if exist "%%D\project.json" (
		echo %%D
 	 	dotnet build "%%D"
 	)
)
