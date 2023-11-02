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

## Eventing (Coming Soon)
Eventing comes with two options depending on what you want to use:
* [AzureEventGridSimulator](https://github.com/pmcilreavy/AzureEventGridSimulator)
* [KNative Eventing](https://knative.dev/docs/eventing/)

### KNative Eventing
[KNative Eventing](https://knative.dev/docs/eventing/) is an implementation of the [CloudEvents](https://cloudevents.io/) specification.
While there are multiple options available for a more advanced setup, I've included only the most basic implementation.
Installation order matters here, so follow the commands closely:

```shell
kubectl apply -f eventing/knative/crds.yaml
kubectl apply -f eventing/knative/core.yaml
kubectl apply -f eventing/knative/channel.yaml
kubectl apply -f eventing/knative/broker.yaml
```

## Storage (Coming Soon)
There are multiple options availble for storage systems to play with:
* [Minio](https://min.io/) - an S3 compatible database
* [Azurite](https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=docker-hub) - an emulator for azure storage accounts
* [Cosmos Emulator](https://learn.microsoft.com/en-us/azure/cosmos-db/how-to-develop-emulator?tabs=docker-linux%2Ccsharp&pivots=api-nosql) - an emulator for cosmos db
* [MSSQL](https://learn.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver16&pivots=cs1-bash) - an MSSQL Server instance running in a linux container