Hello dockers,

Please open a new browser and type:
https://labs.play-with-docker.com/

Click on Login button and then, after the login process click on start button
After that, in the Instances page click on + Add new Instance button in order 
to start a new playground

=================================================================================

showing the docker version

$ docker --version
Docker version 20.10.0, build 7287ab3

=================================================================================

downloading and running our first container

$ docker run hello-world
Unable to find image 'hello-world:latest' locally
latest: Pulling from library/hello-world
0e03bdcc26d7: Pull complete 
Digest: sha256:1a523af650137b8accdaed439c17d684df61ee4d74feac151b5b337bd29e7eec
Status: Downloaded newer image for hello-world:latest

Hello from Docker!
This message shows that your installation appears to be working correctly.

=================================================================================

running our first container, as we can see it was downloaded in the step before
 
$ docker run hello-world
Hello from Docker!
This message shows that your installation appears to be working correctly.

=================================================================================

downloading and running the alpine container in interactive mode... after the 
container running will see ourselves into the containers' bash

$ docker container run -it alpine
Unable to find image 'alpine:latest' locally
latest: Pulling from library/alpine
801bfaa63ef2: Pull complete 
Digest: sha256:3c7497bf0c7af93428242d6176e8f7905f2201d8fc5861f45be7a346b5f23436
Status: Downloaded newer image for alpine:latest
/ # ls
bin    etc    lib    mnt    proc   run    srv    tmp    var
dev    home   media  opt    root   sbin   sys    usr
/ # 

=================================================================================

showing all containers, the defalt is only running but using -a we can take all

$ docker container ls -a
CONTAINER ID   IMAGE         COMMAND     CREATED         STATUS                          PORTS     NAMES
a7519b3f192f   alpine        "/bin/sh"   5 minutes ago   Exited (0) About a minute ago             funny_brattain
5a8f3268454a   hello-world   "/hello"    7 minutes ago   Exited (0) 7 minutes ago                  quizzical_sanderson
fccf03fefe7b   hello-world   "/hello"    9 minutes ago   Exited (0) 9 minutes ago                  silly_euclid

=================================================================================

starting and attaching with our alpine container using its name

$ docker container start -ai funny_brattain
/ # ls
bin    etc    lib    mnt    proc   run    srv    tmp    var
dev    home   media  opt    root   sbin   sys    usr

=================================================================================

creating a new alpine image, but this name specifing a container name

$ docker container run --name myalpine -it alpine
/ # 

=================================================================================

let's try to run our myalpine container running the same command again.
As we can see it is impossible for more than one container using the same name

$ docker container run --name myalpine -it alpine
docker: Error response from daemon: Conflict. The container name "/myalpine" is already in use by container "3d58655dc68c2caa313d29318c9abbf274489eff7fd0c8e772157cc05d8e8a2e". You have to remove (or rename) that container to be able to reuse that name.
See 'docker run --help'.

=================================================================================

let's do something more ambitious.. running the nginx container exposing the internal
nginx door 80 using the door number 8080. At the end PRESS CTRL+C to unlock the bash

$ docker container run -p 8080:80 nginx
Unable to find image 'nginx:latest' locally
latest: Pulling from library/nginx
6ec7b7d162b2: Pull complete 
cb420a90068e: Pull complete 
2766c0bf2b07: Pull complete 
e05167b6a99d: Pull complete 
70ac9d795e79: Pull complete 
Digest: sha256:4cf620a5c81390ee209398ecc18e5fb9dd0f5155cd82adcbae532fec94006fb9
Status: Downloaded newer image for nginx:latest
/docker-entrypoint.sh: /docker-entrypoint.d/ is not empty, will attempt to perform configuration
/docker-entrypoint.sh: Looking for shell scripts in /docker-entrypoint.d/
/docker-entrypoint.sh: Launching /docker-entrypoint.d/10-listen-on-ipv6-by-default.sh
10-listen-on-ipv6-by-default.sh: info: Getting the checksum of /etc/nginx/conf.d/default.conf
10-listen-on-ipv6-by-default.sh: info: Enabled listen on IPv6 in /etc/nginx/conf.d/default.conf
/docker-entrypoint.sh: Launching /docker-entrypoint.d/20-envsubst-on-templates.sh
/docker-entrypoint.sh: Configuration complete; ready for start up

=================================================================================

Let's take the nginx container name.
In the list below the nginx image has a container with zealous_lichterman name

$ docker container ls -a
CONTAINER ID   IMAGE         COMMAND                  CREATED          STATUS                      PORTS                  NAMES
fe6f76edf010   nginx         "/docker-entrypoint.…"   25 minutes ago   Up 23 minutes               0.0.0.0:8080->80/tcp   zealous_lichterman
3d58655dc68c   alpine        "/bin/sh"                28 minutes ago   Exited (0) 27 minutes ago                          myalpine
a7519b3f192f   alpine        "/bin/sh"                42 minutes ago   Exited (0) 31 minutes ago                          funny_brattain
5a8f3268454a   hello-world   "/hello"                 45 minutes ago   Exited (0) 45 minutes ago                          quizzical_sanderson
fccf03fefe7b   hello-world   "/hello"                 46 minutes ago   Exited (0) 46 minutes ago                          silly_euclid

=================================================================================

Another approach to find the container, it's sending the output list above to the
grep using | like the sintaxe below

$ docker container ls -a | grep nginx
fe6f76edf010   nginx         "/docker-entrypoint.…"   27 minutes ago   Up 25 minutes               0.0.0.0:8080->80/tcp   zealous_lichterman

=================================================================================

In our previous command we could identify nginx running and listening the door 8080
Let's print the nginx output using CURL according to the sintaxy below

$ curl get http://localhost:8080
curl: (6) Could not resolve host: get
<!DOCTYPE html>
<html>
<head>
<title>Welcome to nginx!</title>
<style>
    body {
        width: 35em;
        margin: 0 auto;
        font-family: Tahoma, Verdana, Arial, sans-serif;
    }
</style>
</head>
<body>
<h1>Welcome to nginx!</h1>
<p>If you see this page, the nginx web server is successfully installed and
working. Further configuration is required.</p>

<p>For online documentation and support please refer to
<a href="http://nginx.org/">nginx.org</a>.<br/>
Commercial support is available at
<a href="http://nginx.com/">nginx.com</a>.</p>

<p><em>Thank you for using nginx.</em></p>
</body>
</html>
