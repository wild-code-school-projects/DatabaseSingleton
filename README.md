# DatabaseSingleton
Challenge: Database Connection and Singleton Design Pattern

## Task
Implement two classes that require a connection to a database. Write a third class responsible for establishing a database connection and executing SQL queries through it. Ensure that the database connection class follows the Singleton design pattern.

## Validation Criteria

- Two classes utilizing a database are implemented.
- These two classes have a `Database` attribute.
- A `DatabaseSingleton` class implements the Singleton design pattern.
- `DatabaseSingleton` is lazily instantiated and used in the two other classes requiring a connection.
- `DatabaseSingleton` has no public constructor.
- A static property or method of `DatabaseSingleton` returns an instance of itself.
