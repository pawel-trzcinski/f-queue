#!/bin/bash

# script that will create and run all docker containers and make all needed changes to configurations

# validate deployment-configuration file

# * run etcd
export HostIP="192.168.0.206"

docker run \
  -p 2379:2379 \
  -p 2380:2380 \
  --rm \
  -d \
  --name fqueue-etcd \
  gcr.io/etcd-development/etcd:v3.4.14 \
  usr/local/bin/etcd \
  --name s1 \
  --data-dir /etcd-data \
  --listen-client-urls http://0.0.0.0:2379 \
  --advertise-client-urls http://0.0.0.0:2379 \
  --listen-peer-urls http://0.0.0.0:2380 \
  --initial-advertise-peer-urls http://0.0.0.0:2380 \
  --initial-cluster s1=http://0.0.0.0:2380 \
  --initial-cluster-token tkn \
  --initial-cluster-state new \
  --log-level info \
  --logger zap \
  --log-outputs stderr

#docker run -d --rm -p 4001:4001 -p 2379:2379 --name fqueue-etcd quay.io/coreos/etcd:v3.4.14 usr/local/bin/etcd
  
 # -name etcd0 \
 # -advertise-client-urls http://${HostIP}:2379,http://${HostIP}:4001 \
 #  \
 # -initial-advertise-peer-urls http://${HostIP}:2380 \
 # -listen-peer-urls http://0.0.0.0:2380 \
 # -initial-cluster-token etcd-cluster-1 \
 # -initial-cluster etcd0=http://${HostIP}:2380 \
 # -initial-cluster-state new

# * run BE containers with one argument of ETCD IP
# * run HAProxy for all BE
# * run FE containers with one argument of BE IP
#curl -L http://127.0.0.1:2379/v3/kv/range -X POST -d '{"key": "leader-election", "range_end": "leader-election"}'