# Rancher Desktop Customizations
A set of YAML files to provide a customized kubernetes setup in rancher desktop.

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

## Storage (Coming Soon)