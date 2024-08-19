# Unity Package Manager Continuous Integration Baseline
Provides a baseline project for the development and continous integration of several UPM packages in a single project.\
\
Each package is automatically deployed in its own branch whenever it is updated in the main branch. Packages can be added as git repositories with the url structure:\
`https://github.com/PixelsHub/pixelshub-coreunitypackages.git#{package name}`.
Example package:
```
https://github.com/PixelsHub/pixelshub-coreunitypackages.git#packageexample
```
## Unity Package Git Dependency handling
Add the script [Assets/Editor/PackagesGitDependencyAssetImporter.cs](Assets/Editor/PackagesGitDependencyAssetImporter.cs) inside any project to automatically try to download git package dependencies when specified in a `.upgd` file that is imported into Unity. These files should contain a line for each dependency with the structure `{package_name}@{git_url}` such as:
```
com.organization.mypackage@https://github.com/Organization/mypackage.git
```
> [!WARNING]  
> Unity caches data from imported assets that might prevent re-imports of `.upgd` files if a package is removed and added again in the same Unity session. It may be required to force Reimport on the dependency file to trigger package download.
## How to setup new packages for continuous integration
Commit project to main with newly created package inside Unity project's Packages folder.\
Add a new command line at the end of CI GitHub Action's *ci.yml* file: 
```
split_and_push {package name}
```
