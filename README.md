# CodingChallenge

This application was built using VS Code with C# for vscode extension. Boilerplate code was generated using dotnet cli hello world examples.

CodingChallenge is the UI side of the project. Hosted on github: https://github.com/Lativado/CodingChallenge.git
CodingChallengeAPI is the API side of the project. Hosted on github: https://github.com/Lativado/CodingChallengeApi.git

This app has been dockerized using the vscode docker extension.

The images can be built from the dockerfiles in both projects.
The images are also hosted in dockerhub: lativado/codingchallenge and lativado/codingchallengeapi

Since a network is required to connect to the api, the docker commands should be run in this order:

docker network create --driver bridge notification-net  
docker run -dit -p 5196:5196 --name codingchallengeapi --network notification-net codingchallengeapi
docker run -dit -p 5003:5003 --name codingchallenge --network notification-net codingchallenge
//Optional Step - to verify the containers are on the network
docker network inspect notification-net  
//To see the output after submit is selected
docker network inspect notification-net  

After running the above, open the codingchallenge container in the browser, fill out the form, submit, and optionally look at the api output in the console if attached above.