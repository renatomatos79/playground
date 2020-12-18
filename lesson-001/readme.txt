Hello dockers,

Please open a new browser and type:
https://labs.play-with-docker.com/

Click on the Login button and then, after the login process click on the start button
After that, on the Instances page click on "+ Add new Instance" button in order 
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

running our first container again... as we can see it was already downloaded in the step before
 
$ docker run hello-world
Hello from Docker!
This message shows that your installation appears to be working correctly.

=================================================================================

downloading and running the alpine container in interactive mode... when the container be ready the container bash will be shown

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

showing all containers, the default shows only running but using -a we can take all

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

creating a new alpine image, but this time, we are gonna run a new container specifying its name

$ docker container run --name myalpine -it alpine
/ # 

=================================================================================

let's try to run our myalpine container running the same command again.
As we can see! it is impossible to run more than one container using the same name

$ docker container run --name myalpine -it alpine
docker: Error response from daemon: Conflict. The container name "/myalpine" is already in use by container "3d58655dc68c2caa313d29318c9abbf274489eff7fd0c8e772157cc05d8e8a2e". You have to remove (or rename) that container to be able to reuse that name.
See 'docker run --help'.

=================================================================================

let's do something more ambitious.. running the Nginx container exposing its internal door "80" using another external and mapped door "8080". 
At the end PRESS CTRL+C to unlock the bash

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

Let's take the Nginx container name.
In the list below the Nginx, image is identified by the name zealous_lichterman name

$ docker container ls -a
CONTAINER ID   IMAGE         COMMAND                  CREATED          STATUS                      PORTS                  NAMES
fe6f76edf010   nginx         "/docker-entrypoint.…"   25 minutes ago   Up 23 minutes               0.0.0.0:8080->80/tcp   zealous_lichterman
3d58655dc68c   alpine        "/bin/sh"                28 minutes ago   Exited (0) 27 minutes ago                          myalpine
a7519b3f192f   alpine        "/bin/sh"                42 minutes ago   Exited (0) 31 minutes ago                          funny_brattain
5a8f3268454a   hello-world   "/hello"                 45 minutes ago   Exited (0) 45 minutes ago                          quizzical_sanderson
fccf03fefe7b   hello-world   "/hello"                 46 minutes ago   Exited (0) 46 minutes ago                          silly_euclid

=================================================================================

Another approach to find our container, it's sending the output list above to the
grep command using the operator "|" in the syntax below

$ docker container ls -a | grep nginx
fe6f76edf010   nginx         "/docker-entrypoint.…"   27 minutes ago   Up 25 minutes               0.0.0.0:8080->80/tcp   zealous_lichterman

=================================================================================

In our previous command, we could identify the Nginx container which was running and listening to door number 8080
Let's print the Nginx output using CURL according to the syntax below

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

=================================================================================

How much memory and CPU Nginx is consuming?
We can analyze these statistics using "stats" parameter 
Use CTRL+C to leave the terminal

$ docker container stats zealous_lichterman
CONTAINER ID   NAME                 CPU %     MEM USAGE / LIMIT   MEM %     NET I/O         BLOCK I/O   PIDS
fe6f76edf010   zealous_lichterman   0.00%     5.68MiB / 31.4GiB   0.02%     632B / 1.27kB   0B / 0B     2

=================================================================================

Let's inspect the container content using the "inspect" parameter 
For that, we are gonna keep inspecting the Nginx container

$ docker container inspect zealous_lichterman
[
    {
        "Id": "fe6f76edf0104e898d64564f5914bec5613461251c0dfcd150ed9add26cea7c0",
        "Created": "2020-12-18T18:39:16.846822282Z",
        "Path": "/docker-entrypoint.sh",
        "Args": [
            "nginx",
            "-g",
            "daemon off;"
        ],
        "State": {
            "Status": "running",
            "Running": true,
            "Paused": false,
            "Restarting": false,
            "OOMKilled": false,
            "Dead": false,
            "Pid": 13826,
            "ExitCode": 0,
            "Error": "",
            "StartedAt": "2020-12-18T18:41:58.898896456Z",
            "FinishedAt": "2020-12-18T18:40:50.589896964Z"
        },
        "Image": "sha256:ae2feff98a0cc5095d97c6c283dcd33090770c76d63877caa99aefbbe4343bdd",
        "ResolvConfPath": "/var/lib/docker/containers/fe6f76edf0104e898d64564f5914bec5613461251c0dfcd150ed9add26cea7c0/resolv.conf",
        "HostnamePath": "/var/lib/docker/containers/fe6f76edf0104e898d64564f5914bec5613461251c0dfcd150ed9add26cea7c0/hostname",
        "HostsPath": "/var/lib/docker/containers/fe6f76edf0104e898d64564f5914bec5613461251c0dfcd150ed9add26cea7c0/hosts",
        "LogPath": "/var/lib/docker/containers/fe6f76edf0104e898d64564f5914bec5613461251c0dfcd150ed9add26cea7c0/fe6f76edf0104e898d64564f5914bec5613461251c0dfcd150ed9add26cea7c0-json.log",
        "Name": "/zealous_lichterman",
        "RestartCount": 0,
        "Driver": "overlay2",
        "Platform": "linux",
        "MountLabel": "",
        "ProcessLabel": "",
        "AppArmorProfile": "docker-default",
        "ExecIDs": null,
        "HostConfig": {
            "Binds": null,
            "ContainerIDFile": "",
            "LogConfig": {
                "Type": "json-file",
                "Config": {}
            },
            "NetworkMode": "default",
            "PortBindings": {
                "80/tcp": [
                    {
                        "HostIp": "",
                        "HostPort": "8080"
                    }
                ]
            },
            "RestartPolicy": {
                "Name": "no",
                "MaximumRetryCount": 0
            },
            "AutoRemove": false,
            "VolumeDriver": "",
            "VolumesFrom": null,
            "CapAdd": null,
            "CapDrop": null,
            "CgroupnsMode": "host",
            "Dns": [],
            "DnsOptions": [],
            "DnsSearch": [],
            "ExtraHosts": null,
            "GroupAdd": null,
            "IpcMode": "private",
            "Cgroup": "",
            "Links": null,
            "OomScoreAdj": 0,
            "PidMode": "",
            "Privileged": false,
            "PublishAllPorts": false,
            "ReadonlyRootfs": false,
            "SecurityOpt": null,
            "UTSMode": "",
            "UsernsMode": "",
            "ShmSize": 67108864,
            "Runtime": "runc",
            "ConsoleSize": [
                0,
                0
            ],
            "Isolation": "",
            "CpuShares": 0,
            "Memory": 0,
            "NanoCpus": 0,
            "CgroupParent": "",
            "BlkioWeight": 0,
            "BlkioWeightDevice": [],
            "BlkioDeviceReadBps": null,
            "BlkioDeviceWriteBps": null,
            "BlkioDeviceReadIOps": null,
            "BlkioDeviceWriteIOps": null,
            "CpuPeriod": 0,
            "CpuQuota": 0,
            "CpuRealtimePeriod": 0,
            "CpuRealtimeRuntime": 0,
            "CpusetCpus": "",
            "CpusetMems": "",
            "Devices": [],
            "DeviceCgroupRules": null,
            "DeviceRequests": null,
            "KernelMemory": 0,
            "KernelMemoryTCP": 0,
            "MemoryReservation": 0,
            "MemorySwap": 0,
            "MemorySwappiness": null,
            "OomKillDisable": false,
            "PidsLimit": null,
            "Ulimits": null,
            "CpuCount": 0,
            "CpuPercent": 0,
            "IOMaximumIOps": 0,
            "IOMaximumBandwidth": 0,
            "MaskedPaths": [
                "/proc/asound",
                "/proc/acpi",
                "/proc/kcore",
                "/proc/keys",
                "/proc/latency_stats",
                "/proc/timer_list",
                "/proc/timer_stats",
                "/proc/sched_debug",
                "/proc/scsi",
                "/sys/firmware"
            ],
            "ReadonlyPaths": [
                "/proc/bus",
                "/proc/fs",
                "/proc/irq",
                "/proc/sys",
                "/proc/sysrq-trigger"
            ]
        },
        "GraphDriver": {
            "Data": {
                "LowerDir": "/var/lib/docker/overlay2/b05d2a2ef7cdbbc986a1c30b6dcca79dae91d6563474dca22171785ef93040f6-init/diff:/var/lib/docker/overlay2/80d348c6895411e6aa8dd22763e464f3f2e30940ce3f3739e0ba5ec7c96167de/diff:/var/lib/docker/overlay2/424f55ac666fc596c6a6e32bc7c9adf39036992cb000f6cdffcd06cb22e2845a/diff:/var/lib/docker/overlay2/a549db559ba858434f2b823969742f8710ae5d502bf717e999fe573d75b9bb51/diff:/var/lib/docker/overlay2/96428769d7203bf2addad1963f2c3f5994867e090bc9376740cc662fbb5c44a3/diff:/var/lib/docker/overlay2/2e43e27292d4d6c5e2a4304b32b9f14aaad9a5730aa1bc4ceb9a719d3bf03d20/diff",
                "MergedDir": "/var/lib/docker/overlay2/b05d2a2ef7cdbbc986a1c30b6dcca79dae91d6563474dca22171785ef93040f6/merged",
                "UpperDir": "/var/lib/docker/overlay2/b05d2a2ef7cdbbc986a1c30b6dcca79dae91d6563474dca22171785ef93040f6/diff",
                "WorkDir": "/var/lib/docker/overlay2/b05d2a2ef7cdbbc986a1c30b6dcca79dae91d6563474dca22171785ef93040f6/work"
            },
            "Name": "overlay2"
        },
        "Mounts": [],
        "Config": {
            "Hostname": "fe6f76edf010",
            "Domainname": "",
            "User": "",
            "AttachStdin": false,
            "AttachStdout": true,
            "AttachStderr": true,
            "ExposedPorts": {
                "80/tcp": {}
            },
            "Tty": false,
            "OpenStdin": false,
            "StdinOnce": false,
            "Env": [
                "PATH=/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin",
                "NGINX_VERSION=1.19.6",
                "NJS_VERSION=0.5.0",
                "PKG_RELEASE=1~buster"
            ],
            "Cmd": [
                "nginx",
                "-g",
                "daemon off;"
            ],
            "Image": "nginx",
            "Volumes": null,
            "WorkingDir": "",
            "Entrypoint": [
                "/docker-entrypoint.sh"
            ],
            "OnBuild": null,
            "Labels": {
                "maintainer": "NGINX Docker Maintainers <docker-maint@nginx.com>"
            },
            "StopSignal": "SIGQUIT"
        },
        "NetworkSettings": {
            "Bridge": "",
            "SandboxID": "b21a50c26219f5e490e3641e58438060970a8d0121e74fecfdcebd065183dbfb",
            "HairpinMode": false,
            "LinkLocalIPv6Address": "",
            "LinkLocalIPv6PrefixLen": 0,
            "Ports": {
                "80/tcp": [
                    {
                        "HostIp": "0.0.0.0",
                        "HostPort": "8080"
                    }
                ]
            },
            "SandboxKey": "/var/run/docker/netns/b21a50c26219",
            "SecondaryIPAddresses": null,
            "SecondaryIPv6Addresses": null,
            "EndpointID": "b1023efdceea3b4967ef531a89eafcceaf0f342a7d40e8924892a50bc7f272ce",
            "Gateway": "172.17.0.1",
            "GlobalIPv6Address": "",
            "GlobalIPv6PrefixLen": 0,
            "IPAddress": "172.17.0.2",
            "IPPrefixLen": 16,
            "IPv6Gateway": "",
            "MacAddress": "02:42:ac:11:00:02",
            "Networks": {
                "bridge": {
                    "IPAMConfig": null,
                    "Links": null,
                    "Aliases": null,
                    "NetworkID": "67ec9d7843d099f78800724cf12b6a2c0f240158a177a1971786038fdd66b3e5",
                    "EndpointID": "b1023efdceea3b4967ef531a89eafcceaf0f342a7d40e8924892a50bc7f272ce",
                    "Gateway": "172.17.0.1",
                    "IPAddress": "172.17.0.2",
                    "IPPrefixLen": 16,
                    "IPv6Gateway": "",
                    "GlobalIPv6Address": "",
                    "GlobalIPv6PrefixLen": 0,
                    "MacAddress": "02:42:ac:11:00:02",
                    "DriverOpts": null
                }
            }
        }
    }
]

=================================================================================

Once more, using the grep command line we can filter the output content. 
For instance, let's identify the IP address of the container 

$ docker container inspect zealous_lichterman | grep IP
            "LinkLocalIPv6Address": "",
            "LinkLocalIPv6PrefixLen": 0,
            "SecondaryIPAddresses": null,
            "SecondaryIPv6Addresses": null,
            "GlobalIPv6Address": "",
            "GlobalIPv6PrefixLen": 0,
            "IPAddress": "172.17.0.2",
            "IPPrefixLen": 16,
            "IPv6Gateway": "",
                    "IPAMConfig": null,
                    "IPAddress": "172.17.0.2",
                    "IPPrefixLen": 16,
                    "IPv6Gateway": "",
                    "GlobalIPv6Address": "",
                    "GlobalIPv6PrefixLen": 0,

Here we go again, using CURL against the Nginx! But this time over the nginx IP Address
$ curl get http://172.17.0.2:8080
curl: (6) Could not resolve host: get
curl: (7) Failed to connect to 172.17.0.2 port 8080: Connection refused

The connection was refused! And the reason is that the IP address 172.17.0.2 is internal 
In order to access the nginx page, we must use our HOST IP address 

$ ifconfig | grep "inet addr"
          inet addr:172.17.0.1  Bcast:172.17.255.255  Mask:255.255.0.0
          inet addr:192.168.0.28  Bcast:0.0.0.0  Mask:255.255.254.0
          inet addr:172.18.0.21  Bcast:0.0.0.0  Mask:255.255.0.0
          inet addr:127.0.0.1  Mask:255.0.0.0
          
$ curl get http://192.168.0.28:8080
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

=================================================================================

Let's identify the background running process

$ docker ps
CONTAINER ID   IMAGE     COMMAND                  CREATED          STATUS          PORTS                  NAMES
fe6f76edf010   nginx     "/docker-entrypoint.…"   47 minutes ago   Up 44 minutes   0.0.0.0:8080->80/tcp   zealous_lichterman

Compare with the "docker container ls" output!
Remember! "docker container ls" only lists the running containers 

$ docker container ls
CONTAINER ID   IMAGE     COMMAND                  CREATED          STATUS          PORTS                  NAMES
fe6f76edf010   nginx     "/docker-entrypoint.…"   47 minutes ago   Up 44 minutes   0.0.0.0:8080->80/tcp   zealous_lichterman

=================================================================================

How many downloaded images do we have?

$ docker image ls --no-trunc
REPOSITORY    TAG       IMAGE ID                                                                  CREATED         SIZE
alpine        latest    sha256:389fef7118515c70fd6c0e0d50bb75669942ea722ccb976507d7b087e54d5a23   43 hours ago    5.58MB
nginx         latest    sha256:ae2feff98a0cc5095d97c6c283dcd33090770c76d63877caa99aefbbe4343bdd   2 days ago      133MB
hello-world   latest    sha256:bf756fb1ae65adf866bd8c456593cd24beb6a0a061dedf42b26a993176745f6b   11 months ago   13.3kB

=================================================================================

How can we remove these images?
But before remove them, take a look at this short output image list 

$ docker image ls
REPOSITORY    TAG       IMAGE ID       CREATED         SIZE
alpine        latest    389fef711851   43 hours ago    5.58MB
nginx         latest    ae2feff98a0c   2 days ago      133MB
hello-world   latest    bf756fb1ae65   11 months ago   13.3kB

Ok! In fact, is pretty better copy and paste the truncated Image ID
So, let's remove the image bf756fb1ae65 (hellow-world)

$ docker image rm bf756fb1ae65
Error response from daemon: conflict: unable to delete bf756fb1ae65 (must be forced) - image is being used by stopped container fccf03fefe7b

$ docker image rm -f bf756fb1ae65
Untagged: hello-world:latest
Untagged: hello-world@sha256:1a523af650137b8accdaed439c17d684df61ee4d74feac151b5b337bd29e7eec
Deleted: sha256:bf756fb1ae65adf866bd8c456593cd24beb6a0a061dedf42b26a993176745f6b

As we can see the container fccf03fefe7b is still there, but its image was Untagged

$ docker container ls -a 
CONTAINER ID      IMAGE          COMMAND                  CREATED             STATUS                         PORTS                  NAMES
fe6f76edf010      nginx          "/docker-entrypoint.…"   About an hour ago   Up 57 minutes                  0.0.0.0:8080->80/tcp   zealous_lichterman
3d58655dc68c      alpine         "/bin/sh"                About an hour ago   Exited (0) About an hour ago                          myalpine
a7519b3f192f      alpine         "/bin/sh"                About an hour ago   Exited (0) About an hour ago                          funny_brattain
=> 5a8f3268454a   bf756fb1ae65   "/hello"                 About an hour ago   Exited (0) About an hour ago                          quizzical_sanderson
=> fccf03fefe7b   bf756fb1ae65   "/hello"                 About an hour ago   Exited (0) About an hour ago                          silly_euclid

Let's start the Untagged container! It is still working 

$ docker container start -ai fccf03fefe7b

Hello from Docker!
This message shows that your installation appears to be working correctly.

=================================================================================

Using the command below we can list all containers IDs and then, using the operator $( )
we are able to remove all containers 

$ docker container ls -a -q
fe6f76edf010
3d58655dc68c
a7519b3f192f
5a8f3268454a
fccf03fefe7b

Finally, matching the previous result with the delete container command we are gonna build a powerful command 

$ docker container rm -f $(docker container ls -a -q)
fe6f76edf010
3d58655dc68c
a7519b3f192f
5a8f3268454a
fccf03fefe7b

$ docker container ls -a
CONTAINER ID   IMAGE     COMMAND   CREATED   STATUS    PORTS     NAMES

Again, let's repeat the previous command, but for now, removing all images

$ docker image rm -f $(docker image ls -q)
Untagged: alpine:latest
Untagged: alpine@sha256:3c7497bf0c7af93428242d6176e8f7905f2201d8fc5861f45be7a346b5f23436
Deleted: sha256:389fef7118515c70fd6c0e0d50bb75669942ea722ccb976507d7b087e54d5a23
Deleted: sha256:777b2c648970480f50f5b4d0af8f9a8ea798eea43dbcf40ce4a8c7118736bdcf
Untagged: nginx:latest
Untagged: nginx@sha256:4cf620a5c81390ee209398ecc18e5fb9dd0f5155cd82adcbae532fec94006fb9
Deleted: sha256:ae2feff98a0cc5095d97c6c283dcd33090770c76d63877caa99aefbbe4343bdd
Deleted: sha256:782ae030602867e568a53a99643844e8b06702a851c4b0a09c817deae2520b28
Deleted: sha256:8b5b86a154fd4e4098f3f55cd5b71204560cef2e9f50e18e84ada5cb8fb3ae03
Deleted: sha256:528e7c6bece2def770f60aa8722648031a17de5e2df10e776acf955ef8ec90d0
Deleted: sha256:ffb8d6c7eb6938709ca6d1f39f58971ccc5f10372ec3e37e72c7cbc065bbfb57
Deleted: sha256:87c8a1d8f54f3aa4e05569e8919397b65056aa71cdf48b7f061432c98475eee9

$ docker image ls -a
REPOSITORY   TAG       IMAGE ID   CREATED   SIZE

Thanks
