# ToDo Task - simple RESTAPI with few endpoints

A minimal structure for managing ToDo tasks. It allows you to create, update, and manage tasks with the following essential fields and operations.
 - ✅ .net >= 8.0
 - ✅ Web API
 - ✅ data persisted in db using ORM (mariadb)
 - ✅ xunit for unit/integration tests
 - ✅ commented code
 - ✅ ready to compile and run
 - ✅ without any external scripts
 - ✅ validated data
 - ✅ Code running in docker
 - ✅ Repository Pattern
## ToDo Structure

Each ToDo item contains the following fields:

- **Date and Time of Expiry**: Specifies when the task is due.
- **Title**: A short and descriptive title for the task.
- **Description**: A more detailed description of the task.
- **% Complete**: Indicates the percentage of the task that is completed.
##ToDo Task Setup Instructions

### 1. Visual Studio 2022

1. Clone this repository and open `ToDo.csproj`.
2. Set up MariaDB on Docker using the following command:
   ```bash
   docker run --name mariadbtest -e MYSQL_ROOT_PASSWORD=mypass -p 3306:3306 -d docker.io/library/mariadb:latest
   ```
   - **Default Credentials:**
     - **User**: `root`
     - **Password**: `mypass`
3. Change config file `appsettings.json`
   from
    ```bash
    "ConnectionStrings": {
      "DbConnection1": "server=mariadbtest;user=root;password=mypass;database=Todo"
    }
    ```
    into
   ```bash
    "ConnectionStrings": {
      "DbConnection1": "server=localhost;user=root;password=mypass;database=Todo"
    }
    ```

### 2. Docker

1. Create a network for containers using the command below:
   ```bash
   docker network create my_custom_network
   ```
2. Set up MariaDB using the command:
   ```bash
   docker run --name mariadbtest --network my_custom_network -e MYSQL_ROOT_PASSWORD=mypass -p 3306:3306 -d docker.io/library/mariadb:latest
   ```   
3. Create an image for the Web API using the Dockerfile:
   ```bash
   docker build --rm -t dotnetbackenddeveloper/todotaskwithtests:latest .
   ```
4. Create a container using the command:
   ```bash
   docker run --name my_todo_app_container --network my_custom_network -p 5041:5041 -e ASPNETCORE_URLS="http://+:5041" dotnetbackenddeveloper/todotaskwithtests
   ```
5. Open browser and type this url:
   ```bash
   http://localhost:5041/swagger/index.html
   ```
## Required Operations

My task supports the following operations:

### 1. **Get All Todos**
   - Retrieve a list of all the ToDo items.
   
### 2. **Get Specific Todo**
   - Fetch details of a specific ToDo item by its ID.

### 3. **Get Incoming Todos**
   - Retrieve all ToDo items that are due:
     - **Today**
     - **Tomorrow**
     - **This week**

### 4. **Create Todo**
   - Add a new ToDo item to the list with title, description, expiry date/time and percent complete.

### 5. **Update Todo**
   - Update an existing ToDo item’s details, such as title, description, or expiry date.

### 6. **Set Todo Percent Complete**
   - Adjust the completion percentage of a specific ToDo item only if new value is upper than old.

### 7. **Delete Todo**
   - Remove a specific ToDo item from the list by id.

### 8. **Mark Todo as Done**
   - Mark a specific ToDo item as 100% complete. This operation set 100%, if you will choose item by id.
