# MicroService Project

This project demonstrates a microservice architecture using .NET Core. It consists of two main services: PlatformService and CommandsService. The PlatformService is responsible for managing platforms, while the CommandsService is responsible for managing commands related to those platforms. The services communicate with each other using HTTP and gRPC.

## Prerequisites

- .NET Core SDK
- Docker
- Kubernetes
- RabbitMQ
- SQL Server

## Setup

1. Clone the repository:

```bash
git clone https://github.com/chunglinyun/MicroService.git
cd MicroService
```

2. Install the required .NET tools:

```bash
dotnet tool install --global dotnet-ef
```

3. Set up the Kubernetes environment:

```bash
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.5.1/deploy/static/provider/cloud/deploy.yaml
kubectl get pods --namespace=ingress-nginx
kubectl apply -f K8S/local-pvc.yaml
kubectl create secret generic mssql --from-literal=SA_PASSWORD="pa55w0rd!"
```

4. Build and push the Docker images:

```bash
docker build -t DockerHubUsername/platformservice PlatformService/
docker push DockerHubUsername/platformservice
docker build -t DockerHubUsername/commandsservice CommandsService/
docker push DockerHubUsername/commandsservice
```

5. Apply the Kubernetes deployments:

```bash
kubectl apply -f K8S/mssql-plat-depl.yaml
kubectl apply -f K8S/rabbitmq-depl.yaml
kubectl apply -f K8S/platforms-depl.yaml
kubectl apply -f K8S/commands-depl.yaml
```

## Running the Project

1. Check the status of the deployments and services:

```bash
kubectl get deployments
kubectl get services
kubectl get pods
kubectl get namespace
kubectl get pvc
```

2. Restart a deployment if needed:

```bash
kubectl rollout restart deployment <deployment-name>
```

3. To delete a deployment:

```bash
kubectl delete deployment <deployment-name>
```

4. For local development, comment out the production-related settings in the PlatformService `Startup.cs` file and run the following commands:

```bash
dotnet ef migrations add initialmigration
dotnet ef database update -- --environment Production
```

## Additional Information

- The PlatformService communicates with the CommandsService using HTTP for synchronous communication and RabbitMQ for asynchronous communication.
- The services use gRPC for internal communication.
- The project uses In-Memory databases for development and SQL Server for production.

