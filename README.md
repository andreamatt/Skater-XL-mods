# Collection of SXL Mods

## Getting started
### 1. Add References
Create a References folder and copy all contents of SkaterXL\SkaterXL_Data\Managed\ to there

### 2. Create post-build scripts
In order to have the built dlls in the game folder, create post-build scripts for each project you want to build. Follow the **"PostBuildExample.bat"** example.
If you want to edit XLGraphicsUI's Unity project, you need to copy XLGraphicsUI.dll to XLGraphicsUI\Resources\XLGraphicsUI\Assets\DLL\

**Post-build scripts should be like *{ProjectName}\PostBuild.bat***
