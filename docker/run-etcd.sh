#!/bin/bash

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