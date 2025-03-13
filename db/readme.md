## Connecting to the Database

One of the core functionalities of EFcore is scaffolding (converting) an existing database to C#. Once we have scaffolded the database, we already have models automatically generated for every relation in the DB (`Api/Data/Models`) as well as the context (`Api/Data/ApplicationDbContext.cs`), and we cannot connect that to any other database other than the database from which it was scaffolded as it was created based on that original database. 

So in order for everyone to be able to connect to the database locally, the correct way is to pull the latest changes from this branch, including all the EFcore generated context for the database, and then **from those models, creating a local MySQL database that will be connected to your project**. This is done by running a simple command in the terminal and should create a database that is the exactly the same.

So the structure is like this: my initial MySQL Database -> EFcore scaffold -> EFcore Data Models / Context in C# -> your local git repository + Data Models / Context -> your local MySQL database generated from EFcore context

## Steps
### Setup
- Make sure you have MySQL and MySQL Workbench installed and a password set up already.
- switch to this branch (`git checkout -b database_setup origin/database_setup`) or pull the latest changes if you already have the branch locally. Make sure the project `AttendanceAppProject.Shared` is not there, as it shouldn't be after pulling the latest changes. This was removed in the latest commit since it is no longer needed and was from a previous version.
- We will need 2 packages - `Pomelo.EntityFrameworkCore.MySql` and `Microsoft.EntityFrameworkCore.Design`. They should already be installed after you pull this branch, but in case they aren't for some reason, run ` dotnet add package Pomelo.EntityFrameworkCore.MySql --version 8.0.3` and `dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.13`.

### Database Connection String
- In this branch, `AttendanceAppProject.ApiService/appsettings.json` has been already been added to `.gitignore`, meaning any new commits won't include changes to `AttendanceAppProject.ApiService/appsettings.json`. This is because this contains your connection string and credentials, so we need to make sure future commits won't overlap each others credentials.
- However, `AttendanceAppProject.ApiService/appsettings.json` must first be **removed your local git repository** as well in order for this to work for any future commits from your end. You may already have this file in your local git repository so we must remove it from being tracked by git in that case. So run `cd AttendanceAppProject.ApiService` then `git rm --cached appsettings.json`.
- If you don't see `AttendanceAppProject.ApiService/appsettings.json`, and instead you see something like `AttendanceAppProject.ApiService/appsettings.Development.json`, simply create a new file called `appsettings.json` in `AttendanceAppProject.ApiService` (right click the API project > add new item), make sure it is removed from git (`git rm --cached appsettings.json`) and add the following to it:
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }, "AllowedHosts": "*"
}
```
- Now, go to `AttendanceAppProject.ApiService/appsettings.json`, and add the following with your credentials:
```
"ConnectionStrings": {
      "DefaultConnection": "server=YOUR_SERVER_HERE;port=YOUR_PORT_HERE;database=YOUR_DB_NAME_HERE;user=YOUR_USER_HERE;password=YOUR_PASSWORD_HERE;"
    }`
```
  - Make sure the name you give your database is **unique**, meaning you don't already have a local database with that same name. If you have already imported the database schema I created previously, **make sure you give the database a different name here**, because we need to create brand new database on your end that will directly connect to the EFcore models. So you won't be using any imported or already existing databases.
  - (your server is likely localhost, port 3306, user root, password is what you set when first configuring mySQL, db name is name of the schema.)
  - Your `AttendanceAppProject.ApiService/appsettings.json` should now look something like this:
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
      "DefaultConnection": "server=localhost;port=3306;database=attendance_system;user=root;password=1234;"
    }
}
```
  - Now, open a PowerShell terminal, (`view > terminal` at the top) and run `dotnet ef database update`. This will create a new MySQL database from the EFcore models. Go ahead and open MySQL Workbench and verify a new schema exists with the name you defined in the connection string.
    - If for some reason you don't see it, or you get an error, make sure the password you put is the same one you set when first installing MySQL, make sure the server and port you are using is correct (you can verify this on the homepage of MySQL Workbench or in server>server status in workbench), and make sure there are no typos or formatting issues with the JSON. 
### Running the Project
In order to run the project properly, we need both the API and Web projects to be running at the same time, as the API interacts with the data, while the front-end sends HTTP requests to fetch the data from the API.
  1. Go to Solution Explorer > Right-click on the Solution > Select Configure Startup Projects.
  2. Choose “Multiple startup projects”.
  3. Set both ApiService and Web to “Start”. 
- This will ensure both are running at the same time so the API can query the data and the front-end can send a HTTP GET request to fetch the data from the API.
- Now press start to run both the Api and Web projects, and once it is running add `/dbtest` to the URL. You should see the sample student data on that page from the database. It should look like this:
<img width="990" alt="image" src="https://github.com/user-attachments/assets/73a80d2f-33fa-4e33-985f-af91f3278552" />
- You can also try going to `http://localhost:7530/api/student` to see the JSON response from the API with the same data:
<img width="980" alt="image" src="https://github.com/user-attachments/assets/49288d91-c14c-4118-b305-740ab3a6059e" />
- (If it doesn't show up at first or throws an error, try refreshing a couple of times)
- This confirms a connection to the database

## Explaining the new stuff
The overall structure is using EFcore to act as a bridge between the database and C#, as it maps each relation of the database to a class in C# (these are called models) so we can easily interact with the data using LINQ. The API will directly interact with the database, and for each model / table in the db, we will have an API controller which handles HTTP requests and exposes API endpoints for different operations we want to do on the data. The Blazor frontend sends HTTP requests to the API and encapsulates the API's JSON response into its own classes (called DTOs or data transfer objects). For example the Blazor can send an HTTP GET request to api/students to get info on all students, or api/students/{utdId} to get info on a student with a specific id, etc.
- In the API project, you will see a directory called `Data`, this houses the models and `ApplicationDbContext.cs` which are auto-generated by EFcore. You will also see a `Controllers` directory, here we will create an API controller for each model exposing API endpoints and handling HTTP requests. While we can auto generate the controllers as well using EFcore, it may be better to just manually define them as our queries will likely get pretty complex and specific (i.e. students who were absent for 3 days in a row)
- A couple of changes were made in `Program.cs` of both the API project to connect to the database, and of the Web project to connect to the API.
- In the Web project, you will also see a directory called `Models`, housing the DTOs (data transfer objects) or classes that we use to encapsulate only the relevant data from the API's JSON response.
- In the Web project, under Components > Pages, you will see the `DbTest.razor` page, which has the code for displaying student information after making an HTTP request to the API endpoint we had defined in the `StudentController.cs` in the API project.
