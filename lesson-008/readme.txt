Hello dockers,

It's time to start our Swarm and look for high availability during the requests for our microservices. In this lesson 
we are gonna learn how to run our containers in many nodes of our cluster.

Of course! We are gonna build our cluster before! :)

Let's start open a new browser in order to create three instances of our swarm 
https://labs.play-with-docker.com/

In fact, in the playground, you must add three new instances. 

Pick one of the three instances, paste the code below to start a swarm cluster. Attention!
Replace the IP 192.168.0.28 with yours

$ docker swarm init --advertise-addr 192.168.0.28

You could realize, after init the cluster a token has been printed in the browser!
Copy the whole command to add workers for our cluster. The command is pretty similar to the template below. Pay attention! You must replace the Token and the final IP Address
if you do prefer to use the code below, but I strongly recommend you to use the code 
raised after initializing the cluster.

$ docker swarm join --token SWMTKN-1-4d87uxcng5ovb1ambt90neprz6x1ecguaj7pj822vbarqidiiy-4t084f59xwo9j00g9k8c46dwr 192.168.0.28:2377

After adding the worker nodes, use the command below to list node members 

$ docker node ls

Attention! Only manager nodes are able to control the cluster

Let's update the second and third nodes with the role Manager. For that, go to the first node 
and type

$ docker node update --role manager node2
$ docker node update --role manager node3

Let's create an overlay network in order to allow communication among our services 

$ docker network create ntw_redis --driver=overlay
lywtkutq1azc9ccpcwd760gnd

$  docker network ls
NETWORK ID     NAME              DRIVER    SCOPE
2a8597b8232e   bridge            bridge    local
7928635e372c   docker_gwbridge   bridge    local
66aa1fc440ec   host              host      local
x1f77xlpngju   ingress           overlay   swarm
6403b4629fd1   none              null      local
lywtkutq1azc   ntw_redis         overlay   swarm

With the cluster turned on, let's add a Redis service using the ntw_redis network 

$ docker service create -d  --name redisserver --network ntw_redis  -p 6379:6379 redis
x56y284sjgm31kf04rbvrsrtj

$ docker service ls
ID             NAME          MODE         REPLICAS   IMAGE          PORTS
x56y284sjgm3   redisserver   replicated   1/1        redis:latest   *:6379->6379/tcp

$ docker service ps redisserver
ID             NAME            IMAGE          NODE      DESIRED STATE   CURRENT STATE            ERROR     PORTS
bzbtsw8xxagm   redisserver.1   redis:latest   node1     Running         Running 54 seconds ago             

At this point, we have the Redis as our first service. So, let's add three replicas of our previous API

$ docker service create --replicas 3 --name dck-api -p 8001:80 -d --network ntw_redis -e REDIS_HOST=redisserver:6379 renatomatos79/apis:core-docker-api-1.1
fzxfjnmk8ten00w888yrh11bj

$ docker service ls
ID             NAME          MODE         REPLICAS   IMAGE                                    PORTS
fzxfjnmk8ten   dck-api       replicated   3/3        renatomatos79/apis:core-docker-api-1.1   *:8001->80/tcp
x56y284sjgm3   redisserver   replicated   1/1        redis:latest                             *:6379->6379/tcp

To remove the service, just type "service rm"! But don't do it! This is just a code to bear in mind! :)

$ docker service rm dck-api

Let's discover where our API is running on 

$ docker service ps dck-api
ID             NAME        IMAGE                                    NODE      DESIRED STATE   CURRENT STATE                ERROR     PORTS
knpbhvimllvg   dck-api.1   renatomatos79/apis:core-docker-api-1.1   node3     Running         Running about a minute ago             
vguf1q2ej5ue   dck-api.2   renatomatos79/apis:core-docker-api-1.1   node2     Running         Running about a minute ago             
49futogxxc3x   dck-api.3   renatomatos79/apis:core-docker-api-1.1   node1     Running         Running about a minute ago             

$ curl http://localhost:8001/products
[{"id":1,"name":"Eggs (box with 12 units)","price":1.5},{"id":2,"name":"Chocolate","price":1.99},{"id":3,"name":"Butter (President)","price":2.25},{"id":4,"name":"Codfish (Pascoal)","price":12.25},{"id":5,"name":"Cheese 500g (Flamingo)","price":2.4},{"id":6,"name":"Yogurt Danone (6 units)","price":1.45},{"id":7,"name":"Bread (6 units)","price":0.45}][node1] (local) root@192.168.0.28 ~

Go to the node number 3 and type 

$ curl http://localhost:8001/products
[{"id":1,"name":"Eggs (box with 12 units)","price":1.5},{"id":2,"name":"Chocolate","price":1.99},{"id":3,"name":"Butter (President)","price":2.25},{"id":4,"name":"Codfish (Pascoal)","price":12.25},{"id":5,"name":"Cheese 500g (Flamingo)","price":2.4},{"id":6,"name":"Yogurt Danone (6 units)","price":1.45},{"id":7,"name":"Bread (6 units)","price":0.45}][node3] (local) root@192.168.0.26 ~

Let's update the cache items 

$ curl -X PUT http://localhost:8001/products/products -H "content-type: application/json" -d "[{\"Id\":1,\"Name\":\"From cache\",\"Price\":1.5}]"
[node3] (local) root@192.168.0.26 ~

$ curl http://localhost:8001/products
[{"id":1,"name":"From cache","price":1.5}][node3] (local) root@192.168.0.26 ~

Awesome! Everything is working into our cluster