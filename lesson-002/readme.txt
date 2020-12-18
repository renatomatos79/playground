Hello dockers,

Please open a new browser and type:
https://labs.play-with-docker.com/

Click on the Login button and then, after the login process click on the start button
After that, on the Instances page click on "+ Add new Instance" button in order 
to start a new playground

=================================================================================

At this point, we will be no more a simple docker cli operator.
We are gonna run our html files inside the Nginx application server
So, let's create our first HTML file using the template available in the template folder

$ mkdir html 
$ cd html
$ curl https://raw.githubusercontent.com/renatomatos79/playground/master/templates/blankpage.html >> index.html

Use the cat command to inspect the html file

$ cat index.html 
<!doctype html>
<html lang="en">
  <head>
    <!-- Required meta tags -->
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-giJF6kkoqNQ00vy+HMDP7azOuL0xtbfIcaT9wjKHr8RbDVddVHyTfAAsrekwKmP1" crossorigin="anonymous">

    <title>Hello, world!</title>
  </head>
  <body>
    <h1>Hello, world!</h1>

    <!-- Optional JavaScript; choose one of the two! -->

    <!-- Option 1: Bootstrap Bundle with Popper -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta1/dist/js/bootstrap.bundle.min.js" integrity="sha384-ygbV9kiqUc6oa4msXn9868pTtWMgiQaeYH7/t7LECLbyPA2x65Kgf80OJFdroafW" crossorigin="anonymous"></script>

    <!-- Option 2: Separate Popper and Bootstrap JS -->
    <!--
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.5.4/dist/umd/popper.min.js" integrity="sha384-q2kxQ16AaE6UbzuKqyBE9/u/KzioAlnx2maXQHiDX9d4/zp8Ok3f+M7DPm+Ib6IU" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta1/dist/js/bootstrap.min.js" integrity="sha384-pQQkAEnwaBkjpqZ8RU1fF1AKtTcHJwFl3pblpTlHXybJjHpMYo79HY3hIi4NKxyj" crossorigin="anonymous"></script>
    -->
  </body>

What are we gonna do?
Mapping the host html folder "~/html" into the container folder "/usr/share/nginx/html"
What does it mean?
If we do anything into the host "~/html" folder the changes will be reflected in the mapped container folder 
-> Creating a file in the host, the file will be available in the container 
-> Deleting a file in the host, the file will be also deleted in the container
-> Creating a file in the container, the file will be available in the host 
-> Deleting a file in the container, the file will be also deleted in the host

$ docker container run -d --name nginxsrv -p 8080:80 -v ~/html:/usr/share/nginx/html nginx

Check if our container is running...

$ docker ps
CONTAINER ID   IMAGE     COMMAND                  CREATED              STATUS              PORTS                  NAMES
f1d5a8d9615d   nginx     "/docker-entrypoint.…"   About a minute ago   Up About a minute   0.0.0.0:8080->80/tcp   nginxsrv

Let's access the Nginx logs

$ docker container logs nginxsrv
/docker-entrypoint.sh: /docker-entrypoint.d/ is not empty, will attempt to perform configuration
/docker-entrypoint.sh: Looking for shell scripts in /docker-entrypoint.d/
/docker-entrypoint.sh: Launching /docker-entrypoint.d/10-listen-on-ipv6-by-default.sh
10-listen-on-ipv6-by-default.sh: info: Getting the checksum of /etc/nginx/conf.d/default.conf
10-listen-on-ipv6-by-default.sh: info: Enabled listen on IPv6 in /etc/nginx/conf.d/default.conf
/docker-entrypoint.sh: Launching /docker-entrypoint.d/20-envsubst-on-templates.sh
/docker-entrypoint.sh: Configuration complete; ready for start up
172.17.0.1 - - [18/Dec/2020:20:57:14 +0000] "GET /index.html HTTP/1.1" 200 1371 "-" "curl/7.69.1" "-"

$ curl http://localhost:8080/index.html
<!doctype html>
<html lang="en">
  <head>
    <!-- Required meta tags -->
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-giJF6kkoqNQ00vy+HMDP7azOuL0xtbfIcaT9wjKHr8RbDVddVHyTfAAsrekwKmP1" crossorigin="anonymous">

    <title>Hello, world!</title>
  </head>
  <body>
    <h1>Hello, world!</h1>

    <!-- Optional JavaScript; choose one of the two! -->

    <!-- Option 1: Bootstrap Bundle with Popper -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta1/dist/js/bootstrap.bundle.min.js" integrity="sha384-ygbV9kiqUc6oa4msXn9868pTtWMgiQaeYH7/t7LECLbyPA2x65Kgf80OJFdroafW" crossorigin="anonymous"></script>

    <!-- Option 2: Separate Popper and Bootstrap JS -->
    <!--
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.5.4/dist/umd/popper.min.js" integrity="sha384-q2kxQ16AaE6UbzuKqyBE9/u/KzioAlnx2maXQHiDX9d4/zp8Ok3f+M7DPm+Ib6IU" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta1/dist/js/bootstrap.min.js" integrity="sha384-pQQkAEnwaBkjpqZ8RU1fF1AKtTcHJwFl3pblpTlHXybJjHpMYo79HY3hIi4NKxyj" crossorigin="anonymous"></script>
    -->
  </body>
  
=================================================================================

Let's see another approach, using a Dockerfile, to host our page into the Nginx application server
1. In the first line we specify the base image and version 
2. In the second line we supply some label information (the image maintainer)
3. In the third line the working folder will be the base Nginx folder
4. In the last line we copy from the host server, the file index.html to the container 

Attention!
The .Dockerfile for this example must be in the same path of the index.html

FROM nginx:latest
LABEL maintainer 'Renato Matos'
WORKDIR /usr/share/nginx/html
COPY ./index.html .

$ mkdir html 
$ cd html 
$ curl https://raw.githubusercontent.com/renatomatos79/playground/master/templates/DockerfileBlankPage.yaml >> Dockerfile
$ cat Dockerfile 
FROM nginx:latest
LABEL maintainer 'Renato Matos'
WORKDIR /usr/share/nginx/html

Let's build and tag our image 

$ docker image build -t blankpage:1.0.1 .
Sending build context to Docker daemon  99.84kB
Step 1/4 : FROM nginx:latest
 ---> ae2feff98a0c
Step 2/4 : LABEL maintainer 'Renato Matos'
 ---> Running in c8e7c517bea8
Removing intermediate container c8e7c517bea8
 ---> a78c74447f4e
Step 3/4 : WORKDIR /usr/share/nginx/html
 ---> Running in c99bdd9d40d7
Removing intermediate container c99bdd9d40d7
 ---> 0e3b3d28bdbe
Step 4/4 : COPY ./index.html .
 ---> f6c8a61815a5
Successfully built f6c8a61815a5
Successfully tagged blankpage:1.0.1

Let's run our new NGinx container with our inde BlankPage

$ docker container run -d --name nginxsrv2 -p 8081:80 blankpage:1.0.1
$ curl http://localhost:8081
<!doctype html>
<html lang="en">
  <head>
    <!-- Required meta tags -->
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-giJF6kkoqNQ00vy+HMDP7azOuL0xtbfIcaT9wjKHr8RbDVddVHyTfAAsrekwKmP1" crossorigin="anonymous">

    <title>Hello, world!</title>
  </head>
  <body>
    <h1>Hello, world!</h1>

    <!-- Optional JavaScript; choose one of the two! -->

    <!-- Option 1: Bootstrap Bundle with Popper -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta1/dist/js/bootstrap.bundle.min.js" integrity="sha384-ygbV9kiqUc6oa4msXn9868pTtWMgiQaeYH7/t7LECLbyPA2x65Kgf80OJFdroafW" crossorigin="anonymous"></script>

    <!-- Option 2: Separate Popper and Bootstrap JS -->
    <!--
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.5.4/dist/umd/popper.min.js" integrity="sha384-q2kxQ16AaE6UbzuKqyBE9/u/KzioAlnx2maXQHiDX9d4/zp8Ok3f+M7DPm+Ib6IU" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta1/dist/js/bootstrap.min.js" integrity="sha384-pQQkAEnwaBkjpqZ8RU1fF1AKtTcHJwFl3pblpTlHXybJjHpMYo79HY3hIi4NKxyj" crossorigin="anonymous"></script>
    -->
  </body>

