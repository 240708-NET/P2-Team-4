# Team 4's Project 2
The project allows the user to create multiple characters, fight enemies, save their progress, and compare their efforts to other characters. 
Both the user and their characters are then stored in the database as well as a number of pre-generated enemies. This project is a full stack 
web application built on the Next.js React framework. Database, backend logic, and API connectivity were built using .NET C# code and utilizing 
Entity Framework's code-first migrations.

## User Stories
- I will be able to create a new login
- I will be able to "login" to a previous character and continue the game
- I will be able to check the leaderboard against other players
- I will be able to create a new, generated character
- I will be able to explore the game logic, exploring and fighting enemies

## ERD (Entity-Relationship Diagram)
- 1 player to 1 login (Username)
- 1 player to many characters
- 1 character to many items
- 1 combat to 1 character and 1 enemy
- 1 character to 1 inventoryID to many items
- Enemies to drop rates for items (Many items to many enemies)

## Presentation
Each member describes what they did for the project and then we run through an example player creating a character

## Website
### Login Page (Players can have multiple characters)
Central box with a field for username
Below that are two buttons: login and create
- Login logs the player in if they are in the database
- Create adds the player to the database if they aren't in it

### Character select/create page (Takes to character creation or the saved game state)
List of characters attached to player
Two buttons below: select character and create new character
- Select character puts player at last saved point for that character
- Create new character sends player to character creation page

### Character Creation Page
Field for the name
Radio buttons for the rolling methods, button to roll attributes
- This shows a "table" of radio buttons to assing the rolls to attributes
Select class radio button
Create character button

### Game Logic Page
Stats sidebar (shows player stats, etc.)
Central box showing the current event

### Leaderboard Page
Off the main page
Simple list that shows the player and character, tracked by X value

## Database
- Enemies (implemented)
- Items
- Players
- Characters (Player to many, Items to characters)
- Characters to game state

## API
### Players
- Retrieve all players.
- Retrieve a specific player by ID.
- Create a new player.
- Update an existing player.
- Delete a player.
(Repeat for characters)
 
### Enemies
- Retrieve all enemies.
- Retrieve a specific enemy by ID.
- Create a new enemy.
- Update existing enemy.
- Delete enemy.
 
### Combat
- Start a combat session.
- Perform  attack in a combat session.
- End a combat session.

Team member's responsibilities
- Connor: Team Lead
- Solomon: Database
- Omar: API
- Zachary: Website