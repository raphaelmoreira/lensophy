# Updating the Documentation
After making changes to the `Lensophy` library, whether in the code or in its documentation, make sure to perform the 
`build` in `release`. This ensures that the `Lensophy.Doc.Lensophy.xml` is generated correctly.

Next, assuming you are in the project's root folder, run the following command:

```
docfx tool/docfx_project/docfx.json --serve
```

This will compile and publish the updated version of the documentation website on `localhost`, so you can check if 
everything is in order. After the <b>commit/push</b>, the deployment will take care of the rest.

<i>Note: As soon as possible, I will automate this process.</i>

# New Documentation Project
This solution is set up to generate documentation for all libraries, although for now, I have chosen to generate 
documentation only for `Lensophy`. If you need to generate a completely new structure, follow these steps.

Ensure you are using the latest version of DocFx:
```
dotnet tool update -g docfx
```

Create a new structure called "docfx_project" at the root. In the case of this project, I have chosen to place it
within the "tool" directory.
```
docfx init --quiet
```

Inside this folder, open the `docfx.json` file and configure the location of the projects in `metadata:src`. Finally, 
execute the following command and check the result:

```
docfx tool/docfx_project/docfx.json --serve
```