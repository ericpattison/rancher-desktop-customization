# Rancher Desktop Customizations
A set of YAML files to provide a customized kubernetes setup in rancher desktop.
While this repo is primarily used for my personal rancher desktop, it should be usable with any non-production kubernetes.

## Ingress Notes
All ingress resources defined in this setup are defined expecting the use of the lvh.me domain name. For example: (http://grafana.lvh.me)

## Core
To install all the core components you can run the following command:
```shell
kubectl apply -f core/
```

If you want, you can install each of the components separately to further customize your set up.

### Ingress
I personally prefer [Ingress-Nginx](https://kubernetes.github.io/ingress-nginx/) over Traefik, which comes out of the box with Rancher Desktop.
If you prefer to use Traefik, you will need to update all the ingress resources accordingly. Otherwise you should disable Traefik in Rancher Desktop.

I have included a YAML file to install ingress-nginx, or you can follow the instructions for installing into [Rancher Desktop](https://kubernetes.github.io/ingress-nginx/deploy/#rancher-desktop)

To install from the included YAML run:
```shell
kubectl apply -f core/ingress-nginx.yaml
```

## Monitoring
I've provided a simple monitoring setup built around the following services:
* [OpenTelemetry](https://opentelemetry.io/)
* [Prometheus](https://prometheus.io/)
* [Jaeger](https://www.jaegertracing.io/)
* [Grafana](https://grafana.com)

You can fully control the order of installation:
```shell
kubectl create ns monitoring
kubectl apply -f monitoring/grafana.yaml
kubectl apply -f monitoring/jaeger.yaml
kubectl apply -f monitoring/otel.yaml
kubectl apply -f monitoring/prom.yaml
kubectl apply -f monitoring/elasticsearch.yaml
```

Or just do a full install of the entire suite:
```shell
kubectl create ns monitoring
kubectl apply -f monitoring/
```

### Elasticsearch considerations
Elasticearch may have issues starting up by default and may require some adjustment to the rancher-desktop 

## Eventing
Eventing comes with two options depending on what you want to use:
* [Kafka](https://kafka.apache.org/) via [Strimzi](https://strimzi.io)
* [KNative Eventing](https://knative.dev/docs/eventing/)

### Kafka
We use [Strimzi](https://strimzi.io) to help manage [Kafka](https://kafka.apache.org/) in kubernetes. We include a "Single Node" reference yaml to set up a basic cluster once Strimzi is installed.
Other more complex examples may be included later.

While you can use Kafka alone, I prefer using it as the basis for a KNative Eventing setup. You can read more about that in the KNative section.

```shell
kubectl apply -f eventing/kafka/crds.yaml
kubectl apply -f eventing/kafka/core.yaml
```

### KNative Eventing
[KNative Eventing](https://knative.dev/docs/eventing/) is an implementation of the [CloudEvents](https://cloudevents.io/) specification.
There are multiple ways to use knative eventing, and we'll walk through a number of possible scenarios you can use.

To start, you need to install the [KNative eventing](https://knative.dev/docs/eventing) base setup.

```shell
kubectl apply -f eventing/knative/crds.yaml
kubectl apply -f eventing/knative/core.yaml
```

[KNative Eventing](https://knative.dev/docs/eventing/) has three mechanisms of communication:
1. Source to Sink
2. Channels and Subscriptions
3. Brokers and Triggers

#### Source To Sink
Source to sink is a simple way to source events, likely from an external source, and sink them to an internal service. They are very simple, and are one way communication method. Think Fire-and-Forget.
They also only support a one-to-one communcation system. There are currently no examples of this for this repo.

#### Channels and Subscriptions
Channels act as an event forwarding and persistence layer. Subscriptions connect a target service to the channel as a subscriber.

There are multiple channel implementations available two of which are supported in our setup.
1. In-Memory - no persistence, should only be used for development purposes
2. Kafka - uses Apache Kafka Topics to provide an ordered consumer delivery guarantee.

to install the In-Memory channel you can run:
```shell
kubectl apply -f eventing/knative/in-memory-channel.yaml
```

to install the Kafka Channel you can run:
```shell
kubectl apply -f eventing/knative/kafka-controller.yaml
kubectl apply -f eventing/knative/kafka-channel.yaml
```

## Storage (Coming Soon)
There are multiple options availble for storage systems to play with:
* [Minio](https://min.io/) - an S3 compatible database
* [Azurite](https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=docker-hub) - an emulator for azure storage accounts
* [Cosmos Emulator](https://learn.microsoft.com/en-us/azure/cosmos-db/how-to-develop-emulator?tabs=docker-linux%2Ccsharp&pivots=api-nosql) - an emulator for cosmos db
* [MSSQL](https://learn.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver16&pivots=cs1-bash) - an MSSQL Server instance running in a linux container