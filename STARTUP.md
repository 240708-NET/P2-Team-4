# Team 4's Project 2
## Start up procedures
### Installation
1. install Node Package Manager (npm)
2. install NodeJS
3. npm install
4. dotnet tool install --global dotnet-ef

### Setting up database for first time
1. Start up Docker or other container program
2. Create ConnectionString file in Project2.Data (Project2Data/Project2.Data)
 - 2b. Point to new local database
3. Delete migrations folder
4. Navigate to Project2Data/Project2.Data
5. Create new migration Initial (dotnet ef migrations add Initial)
6. Update the database (dotnet ef database update)

### Populating database with initial data
1. Navigate to Project2Data/Project2.API
2. Start the API (dotnet run)
3. Open ManagerGame.cs (Project2App/Project2.App/ManagerGame.cs)
4. Change Port variable to the API port
5. Start the App (dotnet run) [This has to be done in a separate terminal, both the API and App must be running concurrently]
 - 5b. Once you reach the initial intro text, you can exit the program

### Starting the program
1. Navigate to project2-react
2. Start the react app (npm start) [This has to be done in a separate terminal, both the API and site must be running concurrently]