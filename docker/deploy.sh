script that will create and run all docker containers and make all needed changes to configurations:
* validate deployment-configuration file
* run etcd
* run BE containers with one argument of ETCD IP
* run HAProxy for all BE
* run FE containers with one argument of BE IP
