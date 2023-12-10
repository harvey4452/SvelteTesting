# SvelteTesting
A basic project including a svelte frontend project and a C# backend using net core web Api. Connects locally to a local MySql database.

Before running project:
1: make sure you have visual studio code installed.
2: make sure you have visual studio installed with net 6.0 available.
3: make sure you have nodeJS installed (https://nodejs.org/en/download) choose window installer .msi 64-bit (not arm).

To run project:

1: Run Sql script in my sql workbench (make sure it's on world connection with the username: root and password: L3tM31n).
2: run the C# project, leave the browser tab open and keep the project running.
3: open the svelte project file in visual studio code.
4: open a terminal in visual studio code and run the command 'npm install' and wait for the installation to finish.
5: run the command 'npm i svelte-chartjs' and wait for the installation to finish.
6: run the command 'npm run dev' and the website URL should pop up in the terminal (should be localhost 8080).

From here you can use any of the websites implemented functionality!

Current features:

-working login page that leads to 2 different homepages based on whether an account is a customers or an admins.
-password hashing using Argon2 implemented into account creation and identity verification.
-admin homepage allows the user to create and delete accounts
-customer homepage displays pie chart of the users different exercises based on the amount of reps they do.
-customer homepage allows the user to increment the amount of reps they have done for a selected exercise displayed via dropdown box.


