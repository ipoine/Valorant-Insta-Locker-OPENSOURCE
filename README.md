# Valorant-Insta-Locker
C# code that will automatically lock/pick you favorite agent in valorant.

-How code works?-
there is a folder named agents,program gets the name of every png file inside of this folder, when you select one of the agents and press the start button, it starts scannig your screen for the selected png. when it finds the selected photo on screen it gets the cordinates of photo. After that it scans for the lock.PNG(which is in as same path as your exe file). when it finds the agent and lock button on your screen it clicks on both of them and that means that you locked agent.

POSSIBLE ERRORS/PROBLEMS

- When i tried this app on my friends pc it didnt worked (idk why) when i press the start button app stops for a few second (which is normal while scannig) an after that it doesnt clicks anywhere.

- Since this app scans for photos, photos can be effected by your screen resolution, so if you use your pc on different resolution you have to change the photos in file. (all of the photos in agent file and also the lock.PNG)

- i took the lock.png photo from the corner of the Agent Select button so it wont be effected by the text which is can be changed by language.
- while taking the agents photo make sure to not take the background on photo. not sure but this can be a problem while scanning screen.
take the photos like the ones that i taked in agents folder. (also lock.png)

Note: lock.PNG must be while lock button is not activated otherwise code wont work.
