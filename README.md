# TeacherStatistic
A web application showing statistics about the teachers from a faculty and can be useful to use it in universities in order to improve the training of teachers, the way of courses are delivered and the way of teaching a course.

What exactly does the app offer?


The application sends students from a college specialty, by e-mail, surveys on:
-	The way of teaching the courses by a teacher;
-	Clear presentation of course objectives and criteria;
-	Teacher’s respect for his students;
-	The teacher’s receptivity to students reactions and needs;
-	Teacher availability for additional consultations;
-	Quality of courses;
-	Providing the teaching staff;
-	The students interest in the subject taught by the respective teacher;
-	The knowledge and skills acquired by the student in the discipline taught by the respective teacher;
-	Attendance at courses/seminars;

Students access the surveys within the application, and after evaluating the teacher, the corresponding statistics will be displayed by getting a qualifier (unsatisfactory, satisfactory, good and very good).
The charts will only be displayed for the administrators group on the first page of the application.

INSTALL THE APPLICATION

When you first run the app, the web page opened in your browser will have a button "Install".
In order to populate the database with certain predefined data, you will press the Install button, which will create a user named Administrator, with the Admin user name and password 123456, which will be changed later, three groups of categories, administrator, student and teachers, a set of questions.
After completing the operation, the user will be redirected to the login page.

HOW WE USE THE APPLICATION?

AS ADMINISTRATOR

The application will open in the browser with a login page, where you will need to fill in the username and password fields.
If the user has forgotten his password, then he can recover it by visiting the recovery page by clicking on the Forgot password link.
On the password recovery page, the user will have to fill in the user name and email address fields as in the figure below, then click on the Recover password button, the data will be processed and the client will receive a response.
If the data entered is found in the database, then the server will analyze the next step, if not, the user will receive an alert with the message that it does not have an account.
The server will process the user's request and send a reply via an email with the forgotten password.
Once successfully logged in, it will then be redirected to the main page, where teachers' charts will be displayed by looking for teachers by the name of the faculty where they teach.
In the top menu of the app, there is an icon with a Change password tooltip, whereby the administrator can change his predefined password with a new password for a more secure account by accessing that page with a click.
In the password change page, it is necessary to fill in the username fields, the old password and the new password you want.
Then click the Change button to send the requested information to the server.
It will process the request and make the appropriate changes to the database.
The user will be informed by an alert whether the change has been made successfully or not.
In the browser, after a user logs in to the app, a welcome message will be displayed to the current user, which will open a small menu where you will see that the user is logged in, as well as an exit button from the application.

In the left menu of the app there are:
-	the name of the application, Statistics 
-	current user name
-	whether or not connected
-	 the possibility for admin to change his profile image 
-	as well as a list of menus and submenus such as:
-	Settings, submenus for settings for universities, questions and groups.
-	Users with submenus for creating and listing users according to the group they belong to, administrators, teachers and student.
-	Questionnaires, with submenus for creating and listing teachers whose questionnaires were answered by the students
By clicking on the university submenus, the faculty list page will be displayed.

The administrator may:
-	adds a new faculty
-	rename a faculty
-	delete a faculty

By clicking on the questions submenu, the page with two sets of questions for each questionnaire will be displayed. 

The Admin:
-	 can create a new question for each set
-	 can delete a question 
-	can edit a question

By clicking on the group submenu the page with the name of the existing groups in the database will be displayed.

The Admin:
-	 can delete a group
-	can create a new group

By clicking the Add new submenu in the Users menu, admin will create a new user with the following properties: first name, last name, the group to which it belongs (if the user is from the student group, then the studio year will be selected), email address, phone number, and faculty to which it belongs.

When a new user is logged in the database, the newly created user will receive an email address for his access to the application, as well as the data he can log in: the username and password generated randomly.

By clicking on the Admins submenu on the Users menu, a page will appear showing all users in the admin group. This will be possible after the Get All button is pressed or an admin can be searched for by name and surname.

The Admin:
-	 can delete a user
-	can edit a user

By clicking the Teachers submenu on the Users menu, a page will appear showing all users in the teacher group. This will be possible after the Get All button is pressed, or a search by a name, surname or university can be made.
The Admin:
-	can delete a teacher from the database
-	can edit a teacher from the database
-	can see the statistics 
-	can export statistics to an excel file

By clicking on the Students submenu in the Users menu, a page will appear showing all students in the student group. This will be possible after the Get All button is pressed or a student can search by name, surname, year of study, or university.
The Admin:
-	can delete a student from the database
-	can edit a student from the database

By clicking on the Add New submenu in the Questionnaires menu, a page appears where the admin can create a survey about a professor, choosing the students to answer, the type of survey and the questions specific to each type of survey.
By clicking on the First type questionnaires submenu in the Questionnaires menu, a page will appear showing all the teachers who have created the polls about a first type of question categories and answered by the students. 
The Admin can delete the survey.

By clicking on the Second type questionnaires submenu in the Questionnaires menu, a page will appear showing all the teachers who have created the polls about a first type of question categories and answered by the students. 
The Admin can delete the survey.
B. AS A TEACHER

The teachers:
-	can log in the application just like admins
-	and can recover your password if it's forgotten by email
-	can change their password
-	upload a profile image
-	they can see the survey page
-	they can see the survey page where they are involved and the number of students who answered the questionnaire
Also, each professor will receive your username and password when I have an application account created by the admin.

C. AS A STUDENT

The students:
-	they can login to the application just like admins and teachers
-	they can recover their password if they are forgotten by email
-	can change their password
-	upload a profile image
-	they can see the survey page
-	they can respond to the polls they attend to refer to a particular teacher
 Also, each student will receive the username and password on the email address when an admin account is created by the admin, as well as an email to access the link with that survey when it is created.
