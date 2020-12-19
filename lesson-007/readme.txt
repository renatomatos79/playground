Hello dockers,

In our last lesson, we connected our API with the Redis server.
But if you realized, it was nothing fashion, because we had to take the Redis IP Address in order to connect the containers.
In the real world, we don't intend to do that. So! Let's use the Docker Compose approach to make 
the dependency connection between our connection more beautiful and easy to handle :)

Love it!

So, let's start using Docker Compose

Attention!
The compose file is available at this URL
https://github.com/renatomatos79/playground/blob/master/lesson-007/docker-compose.yaml


1. Open a new docker playground browse instance 
   https://labs.play-with-docker.com/
   
2. Create a directory for our docker-compose file 
   $ mkdir dk-compose
   $ cd dk-compose
   $ curl https://raw.githubusercontent.com/renatomatos79/playground/master/lesson-007/docker-compose.yaml >> docker-compose.yaml

3. Let's check if the docker-compose.yaml file has a content according to the code below
   $ cat docker-compose.yaml 
	version: '3'
	networks:
	  ntw_redis:
	services:
		redissrv:
			image: redis:latest
			ports:
			  - 6379:6379
			networks:
			  - ntw_redis
		api:
			image: renatomatos79/apis:core-docker-api-1.1
			ports:
			  - 8001:80
			environment:
			  - REDIS_HOST=redissrv:6379 
			networks:
			  - ntw_redis
			depends_on:
			  - redissrv	

4. Start a compose in background mode:
   $ docker-compose up -d
   
	Creating network "dk-compose_ntw_redis" with the default driver
	Pulling redissrv (redis:latest)...
	latest: Pulling from library/redis
	6ec7b7d162b2: Pull complete
	1f81a70aa4c8: Pull complete
	968aa38ff012: Pull complete
	884c313d5b0b: Pull complete
	6e858785fea5: Pull complete
	78bcc34f027b: Pull complete
	Digest: sha256:0f724af268d0d3f5fb1d6b33fc22127ba5cbca2d58523b286ed3122db0dc5381
	Status: Downloaded newer image for redis:latest
	Pulling api (renatomatos79/apis:core-docker-api-1.1)...
	core-docker-api-1.1: Pulling from renatomatos79/apis
	6ec7b7d162b2: Already exists
	f48adbf33222: Pull complete
	0caf687f11cc: Pull complete
	31f7e18202e6: Pull complete
	b2b381c9c354: Pull complete
	aab386788893: Pull complete
	f032c6cfda71: Pull complete
	Digest: sha256:935d4ede95b4446c3bdf844055cd3a1aa1d756184edb723240773d7c50b1d77d
	Status: Downloaded newer image for renatomatos79/apis:core-docker-api-1.1
	Creating dk-compose_redissrv_1 ... done
	Creating dk-compose_api_1      ... done

5. We just need to check the instances 

   $ docker ps
   
	CONTAINER ID   IMAGE                                    COMMAND                  CREATED              STATUS              PORTS                    NAMES
	9d8056613b15   renatomatos79/apis:core-docker-api-1.1   "dotnet CoreDockerAp…"   About a minute ago   Up About a minute   0.0.0.0:8001->80/tcp     dk-compose_api_1
	451cee4678be   redis:latest                             "docker-entrypoint.s…"   About a minute ago   Up About a minute   0.0.0.0:6379->6379/tcp   dk-compose_redissrv_1
	
6. Let'ts play with our API

	$ curl http://localhost:8001/products
	
	[{"id":1,"name":"Eggs (box with 12 units)","price":1.5},{"id":2,"name":"Chocolate","price":1.99},{"id":3,"name":"Butter (President)","price":2.25},{"id":4,"name":"Codfish (Pascoal)","price":12.25},{"id":5,"name":"Cheese 500g (Flamingo)","price":2.4},{"id":6,"name":"Yogurt Danone (6 units)","price":1.45},{"id":7,"name":"Bread (6 units)","price":0.45}][node1] (local) root@192.168.0.8 ~/dk-compose
	
7. Update the Redis cache "our route is /products/{cache-item}"

	$ curl -X PUT http://localhost:8001/products/products -H "content-type: application/json" -d "[{\"Id\":1,\"Name\":\"From cache\",\"Price\":1.5}]"
	
8. Check the API response once more

	$ curl http://localhost:8001/products
	
	[{"id":1,"name":"From cache","price":1.5}][node1] (local) root@192.168.0.8 ~/dk-compose
	
9. In order to turn off the compose service 

    $ docker-compose down
	
	Stopping dk-compose_api_1      ... done
	Stopping dk-compose_redissrv_1 ... done
	Removing dk-compose_api_1      ... done
	Removing dk-compose_redissrv_1 ... done
	Removing network dk-compose_ntw_redis


Everything is working properly