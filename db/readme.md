# Configuring the Database
## Step 1. Importing Database Dump
- Make sure you have MySQL and MySQL Workbench installed and set up.
- download the two .sql files in this `/db` folder
- Open up MySQL Workbench and go to Server > Data Import
- Select “Import from Self-Contained File” and choose the database schema dump file (`schema.sql` file in this directory).
- Click "new..." to create a new database with this imported schema 
- To add the sample data, with your newly imported database schema open, do Server > Data Import again, and choose the name of your new database as the target schema to import from `test_data.sql`
## Step 2. Add Database Credentials to Connection String 
- switch to this branch (`git checkout -b database_setup origin/database_setup`)
-  `AttendanceAppProject.ApiService/appsettings.json` will house the connection string which requires your credentials to connect to the database. Since each of us will have a different password, we need to remove this from git so we don't overwrite each other's passwords.
- In this branch, `AttendanceAppProject.ApiService/appsettings.json` has been already been added to `.gitignore`, meaning any new commits won't include changes to `AttendanceAppProject.ApiService`.
- However, `AttendanceAppProject.ApiService/appsettings.json` must first be removed your local git repository as well in order for this to work for any future commits from your end. So run `cd AttendanceAppProject.ApiService` then `git rm --cached appsettings.json`.
- Now, go to `AttendanceAppProject.ApiService/appsettings.json`, and add the following with your credentials:
```
"ConnectionStrings": {
      "DefaultConnection": "server=YOUR_SERVER_HERE;port=YOUR_PORT_HERE;database=YOUR_DB_NAME_HERE;user=YOUR_USER_HERE;password=YOUR_PASSWORD_HERE;"
    }`
```
  - (you can double check this in MySQL Workbench, but your server is likely localhost, port 3306, user root, password is what you set when configuring mySQL, db name is name of the schema you just created.)
## Step 3. Connecting Database to Blazor App
- In order to connect C# to MySQL, and be able to run LINQ queries without having to use manual SQL queries, we need to use EFCore (Entity Framework Core) which acts as an ORM (object-relational mapper) between C# and the MySQL database.
- We will need 2 packages - `Pomelo.EntityFrameworkCore.MySql` and `Microsoft.EntityFrameworkCore.Design`. They should already be installed after you pull this branch, but in case they aren't for some reason, run ` dotnet add package Pomelo.EntityFrameworkCore.MySql --version 8.0.3` and `dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.13`.
- In order to run the project properly, we need both the API and Web projects to be running at the same time, as the API communicates with the database through EFCore and runs LINQ queries, while the front-end fetches the data from the API.
  1. Go to Solution Explorer > Right-click on the Solution > Select Configure Startup Projects.
  2. Choose “Multiple startup projects”.
  3. Set both ApiService and Web to “Start”. 
- This will ensure both are running at the same time so the API can query the data and the front-end can send a HTTP GET request to fetch the data from the API.
- Now press start to run both the Api and Web projects, and once it is running add `/dbtest` to the URL. You should see the sample student data on that page from the database. You can also try going to `http://localhost:7530/api/student` to confirm that the API is running, as there should be a JSON response with the same data.
- This confirms a connection to the database


### Understanding How it Works
Other than minor changes to `Program.cs` and `appsettings.json` in both the ApiService and Web projects, there are 3 main changes in this branch:
1. The added `AttendanceAppProject.Shared` project
2. Controllers and Data folders in ApiService project
3. `DbTest.razor` page added under `AttendanceAppProject.web/Components/Pages`

- The `AttendanceAppProject.Shared` project is a class library that houses the data models, which were scaffolded (automatically created) by EFCore after connecting to the MySQL database. There is one model for each relation in the database, you can explore how they correspond. I put these in a class library because both the Api and Web projects need to access this data, so I added references to this project in those other 2 projects (right click > add > project reference) so the data can be shared with both.
- The Controllers folder houses API controllers which handle HTTP requests by using LINQ queries to interact with the data, we can create one for each model / table in the database and allow it to do different things, like get students by ID, delete student with a certain id, etc. See the 2nd link at the bottom for more examples. You can take a look at how `StudentController.cs` is currently set up with just a simple GET request so far.
- The Data folder houses `ApplicationDbContext.cs`, which was also automatically generated by EFCore containing information about how EFcore interacts with the database
- The DbTest.razor page sends a request to `api/student` (configured by StudentController.cs which uses LINQ to query the database) and return student information. This is simply created to test that the connection to the database works, from the Razor component web page -> API using LINQ to query data -> MySQL Database

Here are some resources for more info:
- https://learn.microsoft.com/en-us/ef/core/
- https://medium.com/@sandunissts/implementing-net-core-web-api-with-entity-framework-1914167ba058
- https://learn.microsoft.com/en-us/ef/core/managing-schemas/scaffolding/?tabs=dotnet-core-cli
- https://www.learnentityframeworkcore.com/walkthroughs/existing-database
- https://dotnettutorials.net/lesson/controllers-in-asp-net-core-web-api/
- https://learn.microsoft.com/en-us/dotnet/csharp/linq/get-started/features-that-support-linq
