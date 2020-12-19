Hello dockers,

In the lesson 003 we built and also create our first .net core image 
For this lesson, we are gonna send our image to

- a remote repository using docker hub
- install our docker image from the docker hub into the docker playground 

Are you ready?


1. Create a free accoun on docker hub
   https://hub.docker.com/
   
2. Go to the repository page and create a Repository named apis with public visibility
   https://hub.docker.com/repositories   
   
3. Let's tag our image core-docker-api:1.0 created in the previous lesson with a new name

C:\Temp\docker-k8s>docker tag core-docker-api:1.0 renatomatos79/apis:core-docker-api-1.0

Attention! 
Replace the name renatomatos79 using your docker account

C:\Temp\docker-k8s>docker image ls | grep core-docker-api
core-docker-api                         1.0                                              829531959642   25 minutes ago   208MB
renatomatos79/apis                      core-docker-api-1.0                              829531959642   25 minutes ago   208MB

4. Use docker login to authenticate with our remote repository 

C:\Temp\docker-k8s>docker login
Authenticating with existing credentials...
Login Succeeded

5. Send the tagged image to the remote repository 

C:\Temp\docker-k8s>docker image push renatomatos79/apis:core-docker-api-1.0
The push refers to repository [docker.io/renatomatos79/apis]
964626942c97: Pushed
f56e906af74c: Pushed
7e9226471ff7: Pushed
7f99acd124a6: Pushed
ade80c04318a: Pushed
0916aa79e133: Pushed
87c8a1d8f54f: Pushed
core-docker-api-1.0: digest: sha256:24223753e1f3ad4b203b419c065e3f78e459298ea5565945de0e13e9f83aea25 size: 1791

6. Let's check our built image in our remote repository
   
   https://hub.docker.com/repository/docker/renatomatos79/apis/tags?page=1&ordering=last_updated
   
7. Go to the docker playground and start a new session 

   https://labs.play-with-docker.com
   
8. Run the command to create a container using your public image from docker hub

$ docker run -d -p 8081:80 --name core_docker_api renatomatos79/apis:core-docker-api-1.0
Unable to find image 'renatomatos79/apis:core-docker-api-1.0' locally
core-docker-api-1.0: Pulling from renatomatos79/apis
6ec7b7d162b2: Pull complete 
f48adbf33222: Pull complete 
0caf687f11cc: Pull complete 
31f7e18202e6: Pull complete 
b2b381c9c354: Pull complete 
aab386788893: Pull complete 
4c249820a204: Pull complete 
Digest: sha256:24223753e1f3ad4b203b419c065e3f78e459298ea5565945de0e13e9f83aea25
Status: Downloaded newer image for renatomatos79/apis:core-docker-api-1.0
f084693c5e385cdd02cb1226d007dca4be0be5bceba97c4425cdd6b5bbf439ea

$ docker ps
CONTAINER ID   IMAGE                                    COMMAND                  CREATED          STATUS          PORTS                  NAMES
f084693c5e38   renatomatos79/apis:core-docker-api-1.0   "dotnet CoreDockerAp…"   32 seconds ago   Up 31 seconds   0.0.0.0:8081->80/tcp   core_docker_api

$ curl http://localhost:8081/products
[{"id":1,"name":"Eggs (box with 12 units)","price":1.5},{"id":2,"name":"Chocolate","price":1.99}]

Congratulations!