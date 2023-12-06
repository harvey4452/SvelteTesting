# SvelteTesting
A basic project including a svelte frontend project and a C# backend using net core web Api. Connects locally to a local MySql database

Before running project:
1: make sure you have visual studio code installed.
2: make sure you have visual studio installed with net 6.0 available.
3: make sure you have nodeJS installed (https://nodejs.org/en/download) choose window installer .msi 64-bit (not arm).

To run project:

1: Run Sql script in my sql workbench (make sure it's on world connection with the username and password inputted).
2: run the C# project, leave the browser tab open and keep the project running.
3: open the svelte project file in visual studio code.
4: open a terminal in visual studio code and run the command 'npm install' and wait for the installation to finish.
5: open a terminal in visual studio code and run the command 'npm run dev' and the website URL should pop up in the terminal (should be localhost 8080).
6: put in any credentials and it should return false unless the email: newEmail@hull.ac.uk and the password: password1 are put in, where it will return true instead.

