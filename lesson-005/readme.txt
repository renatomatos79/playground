Hello dockers,

In lesson 004 we learn how to build our docker image,
deploy it to the docker hub and finally create a container 
using docker playground just to simulate a production env.

At this moment I intend to show you how to use Redis into a docker 
container and prepare the path to:
- add new cache objects to Redis 
- remove items from Redis cache 
- update the cache timeout

Are you ready?

1. Go to the Docker Playground
   https://labs.play-with-docker.com/
   
2. Create a new terminal using Add new instance button
   
3. Run the code below to run a Redis container 

$ docker run --name redisserver -d -p 6379:6379 redis
Unable to find image 'redis:latest' locally
latest: Pulling from library/redis
6ec7b7d162b2: Pull complete 
1f81a70aa4c8: Pull complete 
968aa38ff012: Pull complete 
884c313d5b0b: Pull complete 
6e858785fea5: Pull complete 
78bcc34f027b: Pull complete 
Digest: sha256:0f724af268d0d3f5fb1d6b33fc22127ba5cbca2d58523b286ed3122db0dc5381
Status: Downloaded newer image for redis:latest
87bb052ce6d21b08e8a505df079227240ce9f90f78946ced0aeef6fe7d347feb

4. Let's attach in the Redis container and start a redis-cli

$ docker exec -it redisserver bash
root@87bb052ce6d2:/data# 

5. Go to the /usr/local/bin folder

$ root@87bb052ce6d2:/# cd /usr/local/bin

6. Run the redis client using ./redis-cli

root@87bb052ce6d2:/usr/local/bin# ./redis-cli
127.0.0.1:6379> 

7. Create a cache item named list using "mset list 'a, b, c, d, e'"

root@87bb052ce6d2:/usr/local/bin# ./redis-cli
127.0.0.1:6379> mset list "a, b, c, d, e"
OK

8. Print the TTL available, before expires a specific cache item using "ttl list"

127.0.0.1:6379> ttl list
(integer) -1

Attention! When the TTL is -1 the item will never expire

9. Let's print the cache value using "mget list"

127.0.0.1:6379> mget list
1) "a, b, c, d, e"

10. Let's see how to define a new TTL in seconds for our cache item 

27.0.0.1:6379> expire list 20
(integer) 1

11. After 20 seconds, try to get the cache value again

127.0.0.1:6379> mget list
1) (nil)
