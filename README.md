# ShoppingList
a simple shopping list webapp


## Database Setup
The database can be easily created using the SL.Migrations project.

To run the database migrations, you have a few options:

### 1. Build and run executable
- build the solution
- run an instance of microsoft SQL Server (a docker compose file, *DevEnv.yml* is included, which contains SQL Server, RabbitMQ, and Elasticsearch local instances to run as needed)
- run the SL.Migrations executable with the command line argument for the connectionstring
  - SLConnString
- wait for database to be created and run any missing migrations

### 2. Run the SL.Migrations project from visual studio
- build the solution
- run an instance of microsoft SQL Server
- add env var  to launchsettings of SL.Migrations
  - SLConnString
- wait for database to be created and run any missing migrations

## Running the Application
The Application can be run locally with the following steps:
- build the solution
- open the Angular project directory (SL.App/ClientApp)
- npm install
- 'ng s' or 'npm run start'
- Add the SQl Server connection string under env var SLConnString into SL.App launchsettings
- once the angular project is being served, run the SL.App project
- the application should load to https://localhost:5001

## API Documentation
- The application's API is configured with Swagger UI, and is documented based on the comments on each controller.
- Swagger can be accessed when launching the application at https://localhost:5001/swagger/index.html

## Using the ShoppingList App
- The application uses a rudimentary user management system based on email address.  Name is also stored to give a more personalized launch page after logging in.
- On load, the app's NGXS state management will download the existing item list from the database and store it in the redux state for later user in autocomplete.
- When first running the app, it will check for a cookie, *SL-User*, which is a guid generated on first login, and stored until user logs out or cookie expires.

![image](https://user-images.githubusercontent.com/16948173/167541892-e0bbe07d-df3e-4477-9212-44377a892c89.png)

- After logging in, the ShoppingList will be loaded from the database, empty if new, or loading the saved state if an existing user.  The header will differ if user is new, or returning.

![image](https://user-images.githubusercontent.com/16948173/167542268-fa95a708-02ae-4358-9a7f-633969d1db7a.png)

![image](https://user-images.githubusercontent.com/16948173/167542334-7d92f26c-c7ec-4195-b643-13f452825f8b.png)

- When adding new items, entered text will bring up a filtered autocomplete list based on the input.  This list may be empty initially, but will grow as more items are added.  *TODO: Seed a list of common grocery items for initial autocomplete*
- Clicking the X button will clear any entered input, as the checkmark will submit your item.
  - if an item exists, the system will retrieve the itemId and add it to the list.
  - if the item does not already exist, it will be added
    - after adding any new items, the saved listItems array in state is refreshed to update the autocomplete with the new entries
- Once an item is added to the list, the completion slider is in the false position.
- The item can either be completed by clicking or dragging the slider, or deleted by clicking the trashcan
- When done with the app, or to switch users, you can logout in the top-right corner of the header
  - logging out will clear the application state, except for the autocomplete list, clear the saved cookie, and reload the login modal



