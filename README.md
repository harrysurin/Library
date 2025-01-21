# Readme

Library web api app made on .Net 9.0.

## How to run & test app

1.	Update the connection string in the appsettings.json.
2.	Run the following command to navigate to the LibraryRepository directory: 
`cd .\LibraryRepository\`
3.	Run the following command to update the database: 
`dotnet ef database update --startup-project ..\Library\`
4.	Start the Library project
5.	Once the application is running, navigate to the Swagger UI at: http://localhost:5045/swagger/index.html
6.	Get AdminCredentials from appsettings.json. These are the credentials for the default admin user created at the first run of the app. Or you can create a new user using `/register` endpoint.
7.	Make request to the `/login` endpoint. Provide only the email and password fields in the request body. Set “Use Cookies”, “Use Session Cookies” to false or --.
8.	After logging in successfully, copy the accessToken from the response.
9.	Go to the "Authorize" button in Swagger.
10.	Paste the token into the provided field and press the "Authorize" button.
